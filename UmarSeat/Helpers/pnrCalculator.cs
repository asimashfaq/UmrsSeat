using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using UmarSeat.Models;

namespace UmarSeat.Helpers
{
    public  class pnrCalculator
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
                    if (st == null)
                    {
                        StockTransfer skt = db.StockTransfer.Where(x => x.pnrNumber == pnr && x.recevingBranch == branchName).FirstOrDefault();
                        if (skt != null)
                        {
                            st = db.SeatConfirmation.Where(x => (x.pnrNumber == pnr || x.newPnrNumber == pnr) && x.recevingBranch == skt.transferingBranch).FirstOrDefault();
                            st.recevingBranch = skt.recevingBranch;
                            st.noOfSeats = skt.noOfSeats;
                            st.cost = skt.sellingPrice;
                            
                           
                        }
                    }

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


        public static pnrLog caluclateStats(string pnr, string branch, int idSuscription)
        {
            pnrLog pl = new pnrLog();
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                int days = 0;

             
                pl.pnrNumber = pnr;

                SeatConfirmation seatconfirmations = db.SeatConfirmation.Where(x => (x.pnrNumber == pnr && x.newPnrNumber == null) && x.id_Subscription == idSuscription && x.recevingBranch == branch).FirstOrDefault();
                if (seatconfirmations == null)
                {
                    seatconfirmations = db.SeatConfirmation.Where(x => (x.newPnrNumber == pnr) && x.id_Subscription == idSuscription && x.recevingBranch == branch).FirstOrDefault();
                }
                else
                {
                    days = (seatconfirmations.timeLimit - DateTime.Now).Days;
                }
                if (seatconfirmations == null)
                {
                   StockTransfer skt = db.StockTransfer.Where(x => x.pnrNumber == pnr && x.recevingBranch == branch  && x.id_Subscription == idSuscription).FirstOrDefault();
                   if (skt != null)
                    {
                       seatconfirmations = db.SeatConfirmation.Where(x => (x.pnrNumber == pnr || x.newPnrNumber == pnr) && x.recevingBranch == skt.transferingBranch && x.id_Subscription == idSuscription).FirstOrDefault();
                       
                         days = (seatconfirmations.timeLimit - DateTime.Now).Days;



                    }
                    seatconfirmations = new SeatConfirmation();
                }
                else
                {
                    days = (seatconfirmations.timeLimit - DateTime.Now).Days;
                }


                List<StockTransfer> recevingSeats = db.StockTransfer.Where(st => st.pnrNumber == pnr && st.id_Subscription == idSuscription && st.recevingBranch == branch).ToList();
                if (recevingSeats.Count > 0)
                {
                    recevingSeats.ForEach(xs =>
                    {
                        pl.avaliableSeats = pl.avaliableSeats + xs.noOfSeats;
                        pl.receiveSeats = pl.receiveSeats + xs.noOfSeats;
                        //   pl.totalSeats = pl.totalSeats + xs.noOfSeats;
                    });
                }

                if (seatconfirmations.newPnrNumber == null)
                {
                    pl.totalSeats = pl.totalSeats + seatconfirmations.noOfSeats;
                    pl.avaliableSeats = pl.avaliableSeats + pl.totalSeats;
                }
                else
                {
                    pl.totalSeats = pl.totalSeats + seatconfirmations.noOfSeats;
                    pl.avaliableSeats = pl.avaliableSeats + pl.totalSeats;
                }



                List<SeatConfirmation> gslist = db.SeatConfirmation.Where(gs => gs.pnrNumber == pnr && gs.id_Subscription == idSuscription && gs.recevingBranch == branch && gs.newPnrNumber != null).ToList();
                gslist.ForEach(gs =>
                {
                    pl.groupSplit = pl.groupSplit + gs.noOfSeats;
                    pl.avaliableSeats = pl.avaliableSeats - gs.noOfSeats;

                });
                List<StockTransfer> sellSeats = db.StockTransfer.Where(st => st.pnrNumber == pnr && st.id_Subscription == idSuscription && st.sellingBranch == branch).ToList();
                sellSeats.ForEach(xs =>
                {
                    pl.avaliableSeats = pl.avaliableSeats - xs.noOfSeats;
                    pl.sellSeats = pl.sellSeats + xs.noOfSeats;
                });
                List<StockTransfer> transferseats = db.StockTransfer.Where(st => st.pnrNumber == pnr && st.id_Subscription == idSuscription && st.transferingBranch == branch).ToList();
                transferseats.ForEach(xs =>
                {
                    pl.avaliableSeats = pl.avaliableSeats - xs.noOfSeats;
                    pl.transferSeats = pl.transferSeats + xs.noOfSeats;
                });


                pl.branchName = branch;

                pl.idSubscription = idSuscription;
                if (pl.avaliableSeats > 0)
                {
                    pl.pnrStatus = "Avaliable";
                    pl.pnrLock = "";
                   
                }
                else
                {
                    pl.pnrStatus = "Sold";
                    pl.pnrLock = "Locked";
                }
                if (days < 0)
                {
                    pl.pnrStatus = "Expired";
                    pl.pnrLock = "";
                }


                using (TransactionScope scope = new TransactionScope())
                {
                    var plf = db.pnrLogs.Where(y => y.pnrNumber == pl.pnrNumber && y.branchName == pl.branchName && y.idSubscription == idSuscription).FirstOrDefault();
                    if (plf == null)
                    {
                        db.pnrLogs.Add(pl);

                    }
                    else
                    {
                        plf.totalSeats = pl.totalSeats;
                        plf.avaliableSeats = pl.avaliableSeats;
                        plf.groupSplit = pl.groupSplit;
                        plf.sellSeats = pl.sellSeats;
                        plf.transferSeats = pl.transferSeats;
                        plf.receiveSeats = pl.receiveSeats;
                        plf.branchName = pl.branchName;
                        plf.pnrStatus = pl.pnrStatus;
                        plf.idSubscription = pl.idSubscription;
                        db.Entry(plf).OriginalValues["RowVersion"] = plf.RowVersion;



                    }
                    try
                    {
                        db.SaveChanges();
                        scope.Complete();
                    }

                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.Single();
                        var clientValues = (pnrLog)entry.Entity;
                        var databaseEntry = entry.GetDatabaseValues();
                        if (databaseEntry == null)
                        {
                            string a = "Unable to save changes. The department was deleted by another user.";
                        }
                        else
                        {
                            var databaseValues = (pnrLog)databaseEntry.ToObject();

                            if (databaseValues.receiveSeats != clientValues.receiveSeats)
                            {

                            }


                        }
                    }
                    catch (DbUpdateException ex1)
                    {
                        string a = "Unable to save changes. The department was deleted by another user.";
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {

                    }
                    catch (TransactionAbortedException ex)
                    {

                    }

                }


                if (pl.pnrStatus == "Sold")
                {
                    SeatConfirmation sc = db.SeatConfirmation.Where(scc => (scc.pnrNumber == pl.pnrNumber && scc.recevingBranch == pl.branchName) && scc.id_Subscription == idSuscription).FirstOrDefault();
                    if (sc != null)
                    {
                        sc.pnrStatus = "Sold";
                        sc.pnrStatus1 = "Sold";
                        db.Entry(sc).OriginalValues["RowVersion"] = sc.RowVersion;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {


                        }
                        catch (DbUpdateException ex)
                        {

                        }
                    }

                }
                else
                {
                    try
                    {
                        SeatConfirmation sc = db.SeatConfirmation.Where(scc => (scc.pnrNumber == pl.pnrNumber && scc.recevingBranch == pl.branchName && scc.newPnrNumber == null) && scc.id_Subscription == idSuscription).FirstOrDefault();
                        if(sc == null)
                        {
                            sc = db.SeatConfirmation.Where(scc => (scc.newPnrNumber == pl.pnrNumber && scc.recevingBranch == pl.branchName ) && scc.id_Subscription == idSuscription).FirstOrDefault();
                        }
                        if (sc != null)
                        {
                            sc.pnrStatus = "";
                            sc.pnrStatus1 = "";
              
                            db.Entry(sc).OriginalValues["RowVersion"] = sc.RowVersion;
                            db.SaveChanges();

                        }

                    }
                    catch (DbUpdateConcurrencyException ex)
                    {


                    }
                    catch (EntityCommandExecutionException ex)
                    {

                    }

                }
            }
            catch (Exception ex)
            {

                
            }
            return pl;
        }
    }
}