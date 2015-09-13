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
using UmarSeat.Helpers;
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
                List<Dictionary<string, string>> pnrsList = new List<Dictionary<string, string>>();
                tasks.Add(Task.Factory.StartNew(()=> {
                    db = new ApplicationDbContext();
                    List<SeatConfirmation> seatconfirmations = new List<SeatConfirmation>();
                    try
                    {

                        seatconfirmations = db.SeatConfirmation.GroupBy(x => new { x.pnrNumber, x.recevingBranch, x.newPnrNumber, x.id_Subscription }).Select(x => x.FirstOrDefault()).ToList();
                    }
                    catch (Exception)
                    {

                        
                    }
                    seatconfirmations.ForEach(x => {
                      
                        Dictionary<string, string> pnrdic = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(x.newPnrNumber))
                        {
                            pnrdic["pnr"] = x.newPnrNumber;
                            pnrdic["branchName"] = x.recevingBranch;
                            pnrdic["subscription"] = x.id_Subscription.ToString();
                            pnrsList.Add(pnrdic);
                        }
                        else
                        {
                            pnrdic["pnr"] = x.pnrNumber;
                            pnrdic["branchName"] = x.recevingBranch;
                            pnrdic["subscription"] = x.id_Subscription.ToString();
                            pnrsList.Add(pnrdic);

                        }
                    });
                   
                }));
                tasks.Add(Task.Factory.StartNew(() => {
                    db = new ApplicationDbContext();
                    List<StockTransfer> stocktransferlist = db.StockTransfer.Where(x => x.recevingBranch != null).GroupBy(x => new { x.pnrNumber, x.recevingBranch, x.id_Subscription }).Select(x => x.FirstOrDefault()).ToList();
                    stocktransferlist.ForEach(x => {
                        Dictionary<string, string> pnrdic = new Dictionary<string, string>();

                        pnrdic["pnr"] = x.pnrNumber;
                        pnrdic["branchName"] = x.recevingBranch;
                        pnrdic["subscription"] = x.id_Subscription.ToString();
                        var isExist = pnrsList.Contains(pnrdic);
                        if (!isExist)
                        {
                            pnrsList.Add(pnrdic);
                        }
                    });
                  
                }));
                Task.WaitAll(tasks.ToArray());


                tasks = new List<Task>();
                int i = 0;
                pnrsList.ForEach(pnr => {

                    tasks.Add(Task.Factory.StartNew(() => {
                        pnrCalculator.caluclateStats(pnr["pnr"], pnr["branchName"], Convert.ToInt32(pnr["subscription"]));
                    }));
                    
                });
                Task.WaitAll(tasks.ToArray());


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