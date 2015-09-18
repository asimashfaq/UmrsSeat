using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UmarSeat.Models;
using Newtonsoft.Json;
using UmarSeat.Helpers;

namespace UmarSeat.Controllers
{
    [Authorize]
    public class AgentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Agents/
        [CheckSessionOut]
        public async Task<ActionResult> Index()
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var agent = db.Agent.Include(a => a.Person).Where(x=> x.id_Subscription == idSubcription);
            var list = await agent.ToListAsync();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list = list.OrderByDescending(x => x.id_Agent).Skip(5 * (1 - 1)).Take(5).ToList();
            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            if (count > 0)
            {
                ViewBag.start = 1;
            }
            else
            {
                ViewBag.start = 0;
            }
            int end = 5;
            if (end >= count)
            {
                end = (int)Math.Ceiling(count);
            }
            else
            {

            }
            ViewBag.end = end;
            ViewBag.prev = 1;
            ViewBag.next = 2;
            ViewBag.current = 1;
            ViewBag.length = 5;
            return View(await agent.ToListAsync());
        }


        [HttpGet]
        [CheckSessionOut]
        public async Task<ActionResult> GetAgents(string length, string pageNum)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            var pageSize = int.Parse(length);
            var numPage = int.Parse(pageNum);
            var agent = db.Agent.Include(a => a.Person).Where(x => x.id_Subscription == idSubcription);
            var list = await agent.ToListAsync();
            decimal count = Convert.ToDecimal(list.Count.ToString());
            list =  list.OrderByDescending(x => x.id_Agent).Skip(pageSize * (numPage - 1)).Take(pageSize).ToList();
            
          
            decimal pages = count / 5;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = pageSize * (numPage - 1)+1;
            int end = 5;
            if (end >= count)
            {
                end = (int)Math.Ceiling(count);
            }
            else
            {

            }
            ViewBag.end = end;
            ViewBag.prev = numPage - 1;
            ViewBag.next = numPage + 1;
            ViewBag.length = length;
            ViewBag.current = numPage;
            return PartialView("_agentlist", list);
        }
        [HttpPost]
        [CheckSessionOut]
        public ActionResult advanceSearch(Agents agent)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            List<Agents> ss = filterdata(agent, db.Agent.Include(x => x.Person).Where(x => x.id_Subscription == idSubcription).ToList());
            decimal count = Convert.ToDecimal(ss.Count.ToString());
            decimal pages = 1;
            ViewBag.pages = (int)Math.Ceiling(pages); ;
            ViewBag.total = count;
            ViewBag.start = 1;
            ViewBag.end = count;
            ViewBag.prev = 1;
            ViewBag.next = 1;
            ViewBag.length = count;
            ViewBag.current = 1;

            return PartialView("_agentlist", ss);
        }

        private List<Agents> filterdata(Agents agent, List<Agents> ss)
        {

            if (agent.id_Agent >0)
            {
                ss = ss.Where(x => x.id_Agent == agent.id_Agent ).ToList();
            }
            if (!string.IsNullOrEmpty(agent.Person.firstName))
            {
                ss = ss.Where(x => x.Person.firstName.Contains(agent.Person.firstName)).ToList();
            }
            if (!string.IsNullOrEmpty(agent.Person.email))
            {
                ss = ss.Where(x => x.Person.email.Contains(agent.Person.email)).ToList();
            }
            
            
            return ss;
        }


      

        // GET: /Agents/Create
        public ActionResult Create()
        {
            ViewBag.id_Person = new SelectList(db.Persons, "id_Person", "userId");
            Agents agent = new Agents() { Person = new person() };
            return View(agent);
        }

        // POST: /Agents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CheckSessionOut]

        public async Task<string> Create( Agents agents)
        {
           

         

            ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {




                    if (agents.Person.email != null)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.Agent.Where(x => x.Person.email.ToLower() == agents.Person.email.ToLower() && idSubcription == x.id_Subscription).FirstOrDefault();
                        if (sc == null)
                        {
                          
                            agents.id_Subscription = idSubcription;
                            db.Agent.Add(agents);
                            await db.SaveChangesAsync();
                            rr.isSuccess = true;
                            rr.Message = "Insert Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.ErrorMessage= rr.Message = "Email already exists";
                            
                            errors.Add(rr);
                        }


                    }
                }
                catch (Exception ex)
                {

                    rr.isSuccess = false;
                    rr.Message = "Exception occur: " + ex.ToString() + " " + ex.InnerException.ToString();
                    errors.Add(rr);
                }

            }
            else
            {


            }

            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errors1 = query.ToList();
            if (errors1.Count > 0)
            {
                foreach (var e in errors1)
                {
                    errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                }
            }


            return JsonConvert.SerializeObject(errors); ;
        }
        [CheckSessionOut]
        // GET: /Agents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Agents agents =  await db.Agent.Include(x => x.Person).Where(x => x.id_Agent == id && x.id_Subscription == idSubcription).SingleOrDefaultAsync();
            if (agents == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Person = new SelectList(db.Persons, "id_Person", "userId", agents.id_Person);
            return View(agents);
        }

        // POST: /Agents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CheckSessionOut]
        public async Task<string> Edit(Agents agents)
        {
                      ResponseRequest rr = new ResponseRequest();
            List<ResponseRequest> errors = new List<ResponseRequest>();
            if (ModelState.IsValid)
            {
                try
                {

                    if (agents.Person.email != null)
                    {
                        int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
                        var sc = db.Agent.Include(x=> x.Person).Where(x => x.Person.email.ToLower() == agents.Person.email.ToLower() && x.id_Subscription == idSubcription).FirstOrDefault();
                        if (sc != null)
                        {

                            sc.Person.firstName = agents.Person.firstName;
                            sc.Person.lastName = agents.Person.lastName;

                            sc.Person.mobileNumber = agents.Person.mobileNumber;
                            db.Entry(sc).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                           
                            rr.isSuccess = true;
                            rr.Message = "Update Successfully";
                            errors.Add(rr);
                        }
                        else
                        {
                            rr.isSuccess = false;
                            rr.Message = "Agent Not found";
                            errors.Add(rr);
                        }


                    }
                }
                catch (Exception ex)
                {

                    rr.isSuccess = false;
                    rr.Message = "Exception occur: " + ex.ToString() + " " + ex.InnerException.ToString();
                    errors.Add(rr);
                }

            }
            else
            {


            }

            var query = from state in ModelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            var errors1 = query.ToList();
            if (errors1.Count > 0)
            {
                foreach (var e in errors1)
                {
                    errors.Add(new ResponseRequest() { isSuccess = false, ErrorMessage = e });
                }
            }


            return JsonConvert.SerializeObject(errors); ;
           
        }

        // GET: /Agents/Delete/5
        [CheckSessionOut]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Agents agents = await db.Agent.Include(x=> x.Person).Where( x=> x.id_Agent == id && x.id_Subscription == idSubcription).SingleOrDefaultAsync();
            if (agents == null)
            {
                return HttpNotFound();
            }
            return View(agents);
        }

        // POST: /Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [CheckSessionOut]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            int idSubcription = Convert.ToInt32(Session["idSubscription"].ToString());
            Agents agents = await db.Agent.Include(x => x.Person).Where(x => x.id_Agent == id && x.id_Subscription== idSubcription).SingleOrDefaultAsync();
            db.Persons.Remove(agents.Person);
            db.Agent.Remove(agents);
            await db.SaveChangesAsync();
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
