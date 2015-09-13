using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Models;

namespace UmarSeat.jobs
{
    [DisallowConcurrentExecution]
    public class pnrStatsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                 ApplicationDbContext db = new ApplicationDbContext();
            List<Task> tasks = new List<Task>();
            List<SeatConfirmation> seatconfirmations = db.SeatConfirmation.GroupBy(x => new { x.pnrNumber,x.recevingBranch,x.newPnrNumber}).Select(x => x.FirstOrDefault()).ToList();
            List<string> pnrsList = new List<string>();
            seatconfirmations.ForEach(x => {
               
                if(!string.IsNullOrEmpty(x.newPnrNumber))
                {
                    pnrsList.Add(x.newPnrNumber);
                }
                else
                {
                        pnrsList.Add(x.pnrNumber);
                    
                }
            });
            pnrsList.ForEach(pnr => {
               tasks.Add(Task.Factory.StartNew(() => {
                   db = new ApplicationDbContext();
                   seatconfirmations = db.SeatConfirmation.Where(x => x.pnrNumber == pnr).GroupBy(x => new { x.recevingBranch }).Select(x => x.FirstOrDefault()).ToList();
                   if (seatconfirmations.Count > 0)
                   {
                       seatconfirmations.ForEach(x =>
                       {




                           pnrLog pl = new pnrLog();
                           pl.pnrNumber = pnr;


                           if (x.newPnrNumber == null)
                           {
                               pl.totalSeats = pl.totalSeats + x.noOfSeats;
                               pl.avaliableSeats = pl.totalSeats;
                           }
                           List<StockTransfer> recevingSeats = db.StockTransfer.Where(st => st.pnrNumber == x.pnrNumber && st.recevingBranch == x.recevingBranch).ToList();
                           if (recevingSeats.Count > 0)
                           {
                               recevingSeats.ForEach(xs =>
                               {
                                   pl.avaliableSeats = pl.avaliableSeats + xs.noOfSeats;
                                   pl.receiveSeats = pl.receiveSeats + xs.noOfSeats;
                                  //   pl.totalSeats = pl.totalSeats + xs.noOfSeats;
                               });
                           }
                           
                           else
                           {
                               pl.totalSeats = pl.totalSeats + x.noOfSeats;
                               pl.avaliableSeats = pl.avaliableSeats +pl.totalSeats;
                           }



                           List<SeatConfirmation> gslist = db.SeatConfirmation.Where(gs => gs.pnrNumber == x.pnrNumber && gs.recevingBranch == x.recevingBranch && gs.newPnrNumber != null).ToList();
                           gslist.ForEach(gs =>
                           {
                               pl.groupSplit = pl.groupSplit + gs.noOfSeats;
                               pl.avaliableSeats = pl.avaliableSeats - gs.noOfSeats;

                           });
                           List<StockTransfer> sellSeats = db.StockTransfer.Where(st => st.pnrNumber == x.pnrNumber && st.sellingBranch == x.recevingBranch).ToList();
                           sellSeats.ForEach(xs =>
                           {
                               pl.avaliableSeats = pl.avaliableSeats - xs.noOfSeats;
                               pl.sellSeats = pl.sellSeats + xs.noOfSeats;
                           });
                           List<StockTransfer> transferseats = db.StockTransfer.Where(st => st.pnrNumber == x.pnrNumber && st.transferingBranch == x.recevingBranch).ToList();
                           transferseats.ForEach(xs =>
                           {
                               pl.avaliableSeats = pl.avaliableSeats - xs.noOfSeats;
                               pl.transferSeats = pl.transferSeats + xs.noOfSeats;
                           });


                           pl.branchName = x.recevingBranch;

                           pl.idSubscription = x.id_Subscription;
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
                           
                           using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
                           {
                               var plf = db.pnrLogs.Where(y => y.pnrNumber == pl.pnrNumber && y.branchName == pl.branchName).FirstOrDefault();
                               if (plf == null)
                               {
                                   db.pnrLogs.Add(pl);
                                   db.SaveChanges();
                               }
                               else
                               {
                                   plf.totalSeats = pl.totalSeats;
                                   plf.avaliableSeats = pl.avaliableSeats;
                                   plf.groupSplit = pl.groupSplit;
                                   plf.sellSeats = pl.sellSeats + 10;
                                   plf.transferSeats = pl.transferSeats;
                                   plf.receiveSeats = pl.receiveSeats;
                                   plf.branchName = pl.branchName;
                                   plf.pnrStatus = pl.pnrStatus;
                                   db.Entry(plf).OriginalValues["RowVersion"] = plf.RowVersion;

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
                                   catch(TransactionAbortedException ex)
                                   {

                                   }

                               }
                               
                           }
                           

                           if (pl.pnrStatus == "Sold")
                           {
                               SeatConfirmation sc = db.SeatConfirmation.Where(scc => (scc.pnrNumber == pl.pnrNumber && scc.recevingBranch == pl.branchName)).FirstOrDefault();
                               if (sc != null)
                               {
                                   sc.pnrStatus = "Sold";
                                   sc.pnrStatus1 = "Sold";
                                   db.Entry(sc).State = System.Data.Entity.EntityState.Modified;
                                   db.SaveChanges();
                               }

                           }
                           else
                           {
                               SeatConfirmation sc = db.SeatConfirmation.Where(scc => (scc.pnrNumber == pl.pnrNumber && scc.recevingBranch == pl.branchName)).FirstOrDefault();
                               if (sc != null)
                               {
                                   sc.pnrStatus = "";
                                   sc.pnrStatus1 = "";
                                   db.Entry(sc).State = System.Data.Entity.EntityState.Modified;
                                   db.SaveChanges();
                               }
                           }

                       });
                   }
                   else
                   {
                       seatconfirmations = db.SeatConfirmation.Where(x => x.newPnrNumber == pnr).GroupBy(x => new { x.recevingBranch, }).Select(x => x.FirstOrDefault()).ToList();
                       seatconfirmations.ForEach(x =>
                       {

                           pnrLog pl = new pnrLog();
                           pl.pnrNumber = pnr;
                           List<StockTransfer> recevingSeats = db.StockTransfer.Where(st => st.pnrNumber == x.newPnrNumber && st.recevingBranch == x.recevingBranch).ToList();
                           if (recevingSeats.Count > 0)
                           {
                               recevingSeats.ForEach(xs =>
                               {
                                   pl.avaliableSeats = pl.avaliableSeats + xs.noOfSeats;
                                   pl.receiveSeats = pl.receiveSeats + xs.noOfSeats;
                           
                               });
                           }
                           else
                           {
                               pl.totalSeats = pl.totalSeats + x.noOfSeats;
                               pl.avaliableSeats = pl.avaliableSeats + pl.totalSeats;
                           }

                           if (x.newPnrNumber == null)
                           {
                               pl.totalSeats = pl.totalSeats + x.noOfSeats;
                               pl.avaliableSeats = pl.totalSeats;
                           }

                          
                           List<SeatConfirmation> gslist = db.SeatConfirmation.Where(gs => gs.pnrNumber == x.newPnrNumber && gs.recevingBranch == x.recevingBranch && gs.newPnrNumber != null).ToList();
                           gslist.ForEach(gs =>
                           {
                               pl.groupSplit = pl.groupSplit + gs.noOfSeats;
                               pl.avaliableSeats = pl.avaliableSeats - gs.noOfSeats;

                           });

                           List<StockTransfer> sellSeats = db.StockTransfer.Where(st => st.pnrNumber == x.newPnrNumber && st.sellingBranch == x.recevingBranch).ToList();
                           sellSeats.ForEach(xs =>
                           {
                               pl.avaliableSeats = pl.avaliableSeats - xs.noOfSeats;
                               pl.sellSeats = pl.sellSeats + xs.noOfSeats;
                           });
                           List<StockTransfer> transferseats = db.StockTransfer.Where(st => st.pnrNumber == x.newPnrNumber && st.transferingBranch == x.recevingBranch).ToList();
                           transferseats.ForEach(xs =>
                           {
                               pl.avaliableSeats = pl.avaliableSeats - xs.noOfSeats;
                               pl.transferSeats = pl.transferSeats + xs.noOfSeats;
                           });

                           pl.branchName = x.recevingBranch;
                           pl.idSubscription = x.id_Subscription;
                           if (pl.avaliableSeats > 0)
                           {
                               pl.pnrStatus = "Avaliable";
                               pl.pnrLock = "";
                           }
                           else
                           {
                               pl.pnrStatus = "Sold";
                               pl.pnrLock = "Locled";
                           }
                           var plf = db.pnrLogs.Where(y => y.pnrNumber == pl.pnrNumber && y.branchName == pl.branchName).FirstOrDefault();
                           if (plf == null)
                           {
                               db.pnrLogs.Add(pl);
                               db.SaveChanges();
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

                               db.Entry(plf).State = System.Data.Entity.EntityState.Modified;
                               db.SaveChanges();
                           }
                           if (pl.pnrStatus == "Sold")
                           {
                               SeatConfirmation sc = db.SeatConfirmation.Where(scc => (scc.newPnrNumber == pl.pnrNumber && scc.recevingBranch == pl.branchName)).FirstOrDefault();
                               if (sc != null)
                               {
                                   sc.pnrStatus = "Sold";
                                   sc.pnrStatus1 = "Sold";
                                   db.Entry(sc).State = System.Data.Entity.EntityState.Modified;
                                   db.SaveChanges();
                               }

                           }

                       });
                   }
                }));
               Task.WaitAll(tasks.ToArray());
            });
           

         
            }
            catch (Exception ex)
            {
                
                throw;
            }   
        }
    }
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            
            IJobDetail job = JobBuilder.Create<pnrStatsJob>().WithIdentity("asim1","group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x .WithIntervalInSeconds(10).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);

          
        }
        public void pause()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler ss = schedFact.GetScheduler();
            ss.PauseJob(new JobKey("asim1", "group1"));
        }
        public void resume()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler ss = schedFact.GetScheduler();
            ss.ResumeJob(new JobKey("asim1", "group1"));
        }

    }
}