using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UmarSeat.Models;

namespace UmarSeat.Helpers
{
    public static class pnrCalculator
    {
       
        public static bool isPnrAvaliable(string pnr, string branchName, string status1 , string status2)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (!String.IsNullOrEmpty(pnr))
            {
                
                SeatConfirmation st = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber == null).FirstOrDefault();
                if (st == null)
                {
                    st = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr).FirstOrDefault();


                }
                
                st.avaliableSeats = st.noOfSeats;
                List<Task> tasks = new List<Task>();
                
              
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<SeatConfirmation> groupsplitlist = db1.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber != null).ToList();

                    db1.Dispose();
                    if (groupsplitlist.Count > 0)
                    {
                        groupsplitlist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats - x.noOfSeats;
                           
                        });
                
                    }


                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<StockTransfer> sellinglist = db1.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.pnrNumber == pnr ).ToList();
                    db1.Dispose();
                    if (sellinglist.Count > 0)
                    {
                        sellinglist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats - x.noOfSeats;
                          
                        });
                
                    }

                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<StockTransfer> transferlist = db1.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.pnrNumber == pnr && x.recevingBranch == branchName).ToList();
                    db1.Dispose();
                    if (transferlist.Count > 0)
                    {
                        transferlist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats + x.noOfSeats;
                          
                        });
                
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<StockTransfer> transferlist = db1.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.pnrNumber == pnr && (x.sellingBranch == branchName || x.transferingBranch == branchName)).ToList();
                    db1.Dispose();
                    if (transferlist.Count > 0)
                    {
                        transferlist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats - x.noOfSeats;

                        });

                    }
                }));

                Task.WaitAll(tasks.ToArray());
                
                if(st.avaliableSeats>0)
                {
                    st.pnrStatus = status1;
                    db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    st.pnrStatus = status2;
                    db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                
            }
            return false;
        }
        public static bool isPnrAvaliable1(string pnr, string branchName, string status1, string status2)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (!String.IsNullOrEmpty(pnr))
            {

                SeatConfirmation st = db.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber == null).FirstOrDefault();
                if (st == null)
                {
                    st = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr).FirstOrDefault();


                }

                st.avaliableSeats = st.noOfSeats;
                List<Task> tasks = new List<Task>();


                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<SeatConfirmation> groupsplitlist = db1.SeatConfirmation.Where(x => x.pnrNumber == pnr && x.newPnrNumber != null).ToList();

                    db1.Dispose();
                    if (groupsplitlist.Count > 0)
                    {
                        groupsplitlist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats - x.noOfSeats;

                        });

                    }


                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<StockTransfer> sellinglist = db1.StockTransfer.Where(x => x.advanceDate.HasValue == true && x.pnrNumber == pnr).ToList();
                    db1.Dispose();
                    if (sellinglist.Count > 0)
                    {
                        sellinglist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats - x.noOfSeats;

                        });

                    }

                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<StockTransfer> transferlist = db1.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.pnrNumber == pnr && x.recevingBranch == branchName).ToList();
                    db1.Dispose();
                    if (transferlist.Count > 0)
                    {
                        transferlist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats + x.noOfSeats;

                        });

                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    ApplicationDbContext db1 = new ApplicationDbContext();
                    List<StockTransfer> transferlist = db1.StockTransfer.Where(x => x.advanceDate.HasValue == false && x.pnrNumber == pnr && (x.sellingBranch == branchName || x.transferingBranch == branchName)).ToList();
                    db1.Dispose();
                    if (transferlist.Count > 0)
                    {
                        transferlist.ForEach(x =>
                        {
                            st.avaliableSeats = st.avaliableSeats - x.noOfSeats;

                        });

                    }
                }));

                Task.WaitAll(tasks.ToArray());

                if (st.avaliableSeats > 0)
                {
                    st.pnrStatus1 = status1;
                    db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    st.pnrStatus1 = status2;
                    db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
            return false;
        }
    }
}