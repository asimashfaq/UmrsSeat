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
using UmarSeat.Models;

namespace UmarSeat.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ImportBooking()
        {
            return View();
        }

        public ActionResult SaveUploadedFile()
        {
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
                        //string vpath = string.Format("~/imports/{0}/bookingimport/{1}", Subscription, fileName1);
                        var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", path);

                        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                        //get the workbook
                        Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(path);

                        //get the first worksheet
                        Microsoft.Office.Interop.Excel.Worksheet wSheet = excelBook.Sheets.Item[1];

                        //Fill the DataSet by the Sheets.
                        var adapter = new OleDbDataAdapter("SELECT * FROM [" + wSheet.Name + "$]", connectionString);
                        foreach(var row in wSheet.Rows)
                        {

                        }


                        excelBook.Close(false);
                      
                        var ds = new DataSet();
                        List<string> colums = new List<string>();
                        DataTable loadDT = new DataTable();
                        List<string> erros = new List<string>();

                        adapter.Fill(ds, "SeatConfirmation");
                        DataTable data = ds.Tables["SeatConfirmation"];
                        using (TransactionScope ts = new TransactionScope())
                        {
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                if (data.Rows[i][2].ToString() != "" && erros.Count ==0)
                                {
                                    ApplicationDbContext db = new ApplicationDbContext();
                                    SeatConfirmation sc = new SeatConfirmation();
                                    sc.pnrNumber = data.Rows[i][2].ToString();
                                    sc.airLine = data.Rows[i][1].ToString();


                                    try
                                    {
                                        sc.outBoundDate = Convert.ToDateTime(data.Rows[i][5].ToString());
                                        sc.inBoundDate = Convert.ToDateTime(data.Rows[i][6].ToString());
                                    }
                                    catch (Exception)
                                    {

                                        //throw;
                                    }
                                    sc.outBoundSector = data.Rows[i][4].ToString();
                                    sc.inBoundSector = data.Rows[i][4].ToString();
                                    sc.noOfSeats = int.Parse(data.Rows[i][7].ToString());
                                    sc.cost = int.Parse(data.Rows[i][8].ToString());
                                
                                    sc.recevingBranch = data.Rows[i][3].ToString();
                                    sc.avaliableSeats = 999;

                                    if(sc.outBoundDate.Date<= DateTime.Now.Date)
                                    {
                                        sc.timeLimit = sc.CreatedAt = sc.UpdatedAt = sc.outBoundDate;
                                        
                                    }
                                    else
                                    {
                                        sc.timeLimit= sc.CreatedAt = sc.UpdatedAt = DateTime.Now;
                                    }
                                     
                                    int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                                    sc.id_Subscription = idSubcription;

                                    SeatConfirmation sc2 = db.SeatConfirmation.Where(x => x.pnrNumber.ToLower() == sc.pnrNumber.ToLower() && x.id_Subscription == idSubcription).FirstOrDefault();
                                    if (sc2 == null && erros.Count == 0)
                                    {
                                        List<Task> tasks = new List<Task>();

                                        City city = db.City.Where(x => sc.recevingBranch == x.city).FirstOrDefault();
                                        string fcountry = "";
                                        if(city != null)
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
                                                    
                                                }
                                                Sector _sector = db.Sector.Where(x => x.sectorName.ToLower() == sc.outBoundSector.ToLower() && x.id_Subscription == idSubcription).FirstOrDefault();
                                                if (_sector == null)
                                                {

                                                    _sector = new Sector() { sectorName = sc.outBoundSector, id_Subscription = idSubcription, country = sc.country, airline = sc.airLine, category = "Both" };
                                                    db.Sector.Add(_sector);
                                                   
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
                                                }
                                             
                                                branches _branch = db.Branch.Where(x => x.branchName.ToLower().Contains(b.ToLower()) && x.id_Subscription == idSubcription).FirstOrDefault();
                                                if (_branch == null)
                                                {

                                                    _branch = new branches() { branchName = b, CreatedAt = DateTime.Now, id_Subscription = idSubcription, branchCity = city.city, branchCountry = sc.country };
                                                     db.Branch.Add(_branch);
                                                  
                                                }
                                                if (erros.Count == 0)
                                                {
                                                    db.SeatConfirmation.Add(sc);
                                                    db.SaveChanges();
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                            
                                                erros.Add(ex.ToString());
                                                ts.Dispose();
                                            throw;
                                        }

                                    }
                                }
                            }

                            if(erros.Count==0)
                            ts.Complete();
                        }
                           

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
    }
}