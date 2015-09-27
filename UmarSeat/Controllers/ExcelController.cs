using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using UmarSeat.Helpers;
using UmarSeat.Models;

namespace UmarSeat.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }
        [CheckSessionOut]
        public ActionResult ImportBooking()
        {
            return View();
        }
        [CheckSessionOut]
        public string ImportBookingStatus()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            response.Add("status", Session["status"]);
            response.Add("rowsadd", Session["rowsadd"]);

            response.Add("invaliddata", (List<string>)Session["invaliddata"]);

            return JsonConvert.SerializeObject(response);
        }
        [CheckSessionOut]
        public bool Savingbooking(string path)
        {
            statusdata = ("Processing File");
            Session["status"] = statusdata;
            //string vpath = string.Format("~/imports/{0}/bookingimport/{1}", Subscription, fileName1);
          
            Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Worksheet wSheet = new Worksheet();
            try
            {
                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", path);

                Workbook excelBook = xlApp.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                wSheet = excelBook.Sheets.Item[1];

                statusdata = ("Opening the file");
                Session["status"] = statusdata;

                int rowsadd = 0;
                try
                {
                    List<string> erros = new List<string>();


                    using (TransactionScope ts = new TransactionScope())
                    {
                        Range UsedRange = wSheet.UsedRange;
                        int lastUsedRow = UsedRange.Row + UsedRange.Rows.Count - 1;
                        statusdata = ("Validating  the File"); Session["status"] = statusdata;
                        for (int i = 1; i <= lastUsedRow; i++)
                        {
                            try
                            {
                                string p = wSheet.Cells[i, 3].Text.ToString();
                                if (p != "" && erros.Count == 0 && !p.ToLower().Contains("pnr"))
                                {
                                    ApplicationDbContext db = new ApplicationDbContext();
                                    SeatConfirmation sc = new SeatConfirmation();


                                    try
                                    {

                                        sc.pnrNumber = wSheet.Cells[i, 3].Value2.ToString();
                                        statusdata = ("Validating  the PNR" + sc.pnrNumber); Session["status"] = statusdata;
                                        sc.airLine = wSheet.Cells[i, 2].Value2.ToString();
                                        string ob = wSheet.Cells[i, 6].Value2.ToString();
                                        string ib = wSheet.Cells[i, 7].Value2.ToString();
                                        sc.outBoundDate = DateTime.FromOADate(double.Parse(ob)); ;
                                        sc.inBoundDate = DateTime.FromOADate(double.Parse(ib)); ;
                                        sc.outBoundSector = wSheet.Cells[i, 5].Value2.ToString();
                                        sc.inBoundSector = wSheet.Cells[i, 5].Value2.ToString();
                                        sc.noOfSeats = int.Parse(wSheet.Cells[i, 8].Value2.ToString());
                                        sc.cost = int.Parse(wSheet.Cells[i, 9].Value2.ToString());

                                        sc.recevingBranch = wSheet.Cells[i, 4].Value2.ToString();
                                        sc.avaliableSeats = 999;
                                        rowsadd++;
                                        Session["rowsadd"] = rowsadd;
                                    }
                                    catch (Exception)
                                    {

                                        invaliddata.Add("Invalid or courrpt data at " + sc.pnrNumber + " at row: " + i);
                                        Session["invaliddata"] = invaliddata;
                                    }


                                    if (sc.outBoundDate.Date <= DateTime.Now.Date)
                                    {
                                        sc.timeLimit = sc.CreatedAt = sc.UpdatedAt = sc.outBoundDate;

                                    }
                                    else
                                    {
                                        sc.CreatedAt = sc.UpdatedAt = DateTime.Now;
                                        sc.timeLimit = sc.outBoundDate;
                                    }

                                    int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                                    sc.id_Subscription = idSubcription;

                                    SeatConfirmation sc2 = db.SeatConfirmation.Where(x => x.pnrNumber.ToLower() == sc.pnrNumber.ToLower() && x.id_Subscription == idSubcription).FirstOrDefault();
                                    if (sc2 == null && erros.Count == 0)
                                    {


                                        City city = db.City.Where(x => sc.recevingBranch == x.city).FirstOrDefault();
                                        string fcountry = "";
                                        if (city != null)
                                        {
                                            sc.country = city.country;
                                            fcountry = city.countryFull;
                                        }

                                        try
                                        {

                                            airLine Air_line = db.Airline.Where(x => x.airlineName.ToLower() == sc.airLine.ToLower() && x.id_Subscription == idSubcription).FirstOrDefault();
                                            if (Air_line == null)
                                            {

                                                Air_line = new airLine() { airlineName = sc.airLine, id_Subscription = idSubcription, createdAt = DateTime.Now, Country = fcountry };
                                                db.Airline.Add(Air_line);
                                                statusdata = ("New Airline found " + sc.airLine); Session["status"] = statusdata;
                                            }
                                            Sector _sector = db.Sector.Where(x => x.sectorName.ToLower() == sc.outBoundSector.ToLower() && x.id_Subscription == idSubcription).FirstOrDefault();
                                            if (_sector == null)
                                            {

                                                _sector = new Sector() { sectorName = sc.outBoundSector, id_Subscription = idSubcription, country = sc.country, airline = sc.airLine, category = "Both" };
                                                db.Sector.Add(_sector);
                                                statusdata = ("New Sector found " + sc.outBoundSector); Session["status"] = statusdata;

                                            }
                                            string b = sc.recevingBranch;
                                            string ocity = "";
                                            if (b.ToLower().Contains("branch"))
                                            {

                                            }
                                            else
                                            {
                                                ocity = b;
                                                b = b + " Branch";
                                                sc.recevingBranch = b;
                                            }

                                            branches _branch = db.Branch.Where(x => x.branchName.ToLower().Contains(b.ToLower()) && x.id_Subscription == idSubcription).FirstOrDefault();
                                            if (_branch == null)
                                            {

                                                _branch = new branches() { branchName = b, CreatedAt = DateTime.Now, id_Subscription = idSubcription, branchCity = city.city, branchCountry = sc.country };
                                                db.Branch.Add(_branch);
                                                statusdata = ("New Branch found " + sc.outBoundSector); Session["status"] = statusdata;

                                            }
                                            if (erros.Count == 0)
                                            {
                                                db.SeatConfirmation.Add(sc);
                                                db.SaveChanges();
                                                statusdata = ("Saving Booking Record " + sc.pnrNumber);
                                                Session["status"] = statusdata;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            excelBook.Close(false);
                                            erros.Add(ex.ToString());
                                            ts.Dispose();
                                            invaliddata.Add("Some thing went wrong while adding 3rd party content");
                                            Session["invaliddata"] = invaliddata;
                                            statusdata = ("Some thing wrong " + ex.Message.ToString() + ex.StackTrace.ToString() + ex.InnerException); Session["status"] = statusdata;
                              
                                        }

                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                invaliddata.Add("Some thing went wrong1 "+ ex.Message.ToString() + ex.StackTrace.ToString());
                                Session["invaliddata"] = invaliddata;

                            }
                        }

                        if (erros.Count == 0)
                            ts.Complete();


                        excelBook.Close(false);

                    }
                }
                catch (Exception ex)
                {
                    excelBook.Close(false);

                    invaliddata.Add("Some thing went wrong2 " + ex.Message.ToString() + ex.StackTrace.ToString());
                    Session["invaliddata"] = invaliddata;
                    statusdata = ("Some thing wrong " + ex.Message.ToString() + ex.StackTrace.ToString() + ex.InnerException);
                    Session["status"] = statusdata;

                }

            }
            catch (Exception ex)
            {

                invaliddata.Add("Some thing went wrong3 " + ex.Message.ToString() + ex.StackTrace.ToString());
                Session["invaliddata"] = invaliddata;

            }
            
            Session["status"] = "Completed";
            Session["invaliddata"] = null;
           
         
            return true;
        }
        string statusdata ;
         List<string> invaliddata = new List<string>();

        [CheckSessionOut]

        public ActionResult SaveUploadedFile()
        {
            statusdata = "";
            invaliddata = null;
            Session["rowsadd"] = 0;
            Session["status"] = "";
            Session["invaliddata"] = null;
            bool isSavedSuccessfully = true;
            string fName = "";
            var path = "";
         
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
          
                    if (file != null && file.ContentLength > 0)
                    {
                        string Subscription = Session["idSubscription"].ToString();
                        var originalDirectory = new DirectoryInfo(string.Format("{0}\\imports\\{1}", Server.MapPath(@"\"),Subscription));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "bookingimport");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);
                 
                        path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);
             
                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
        
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName, filepath = path });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
    }
}