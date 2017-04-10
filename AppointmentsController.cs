using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Novacode; //DocX API for manipulating .docx files
using UAA.HR.Forms.WebUI.Models;

/**
 * File         AppointmentsController.cs
 * 
 * Function     Controller that makes new employee Appointment letter
 *              Input:   Employee data posted from the web form
 *              Output:  A .docx file that is the Appointment letter
 *                          for this employee type
 *                       The letter is appended with all employee data
 *                          in the letter template blank fields
 *                       User is prompted to download the file
 *                       
 * Authors      Gareth Bosch    gbosch@alaska.edu
 *              Tevin Gladden   tjgldden@alaska.edu
 *              Rocco Haro      rharo@alaska.edu
 *              
 * Date         Fall 2016
 *              
 * */


namespace UAA.HR.Forms.WebUI.Controllers
{
    public class AppointmentsController : Controller
    {
        private AppointmentDbContext db = new AppointmentDbContext();

        // GET: Appointments
        public ActionResult Index()
        {
            return View(db.Appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            return View();
        }

        // Step through employee type tree to find correct type.
        // Currently only works for employee of type Staff, Non-Union Represented, Regular-Term.
        // All the logic exists to find employees of other types. Their respective CreateLetter()
        //    methods need to be implemented.
        private Appointment TreeLogic(Appointment currentAppointment)
        {
            // int values come from the enumerables
            //int current_type = (int)currentAppointment.Employee_Type;
            //int current_union = (int)currentAppointment.Union_Status;
            //int current_stype = (int)currentAppointment.Staff_Type;
            //int currrent_ttype = (int)currentAppointment.Temporary_Type;

            //if (current_type == -1)
            //{
            //    // no type selected
            //    return null;
            //}

            //if (current_type == 0)
            //{
            //    // Staff
            //    if (current_union == 0)
            //    {
            //        //union represented
            //        return null;
            //    }
            //    if (current_union == 0)
            //    {
            //        //non union represented
            //        if (current_stype == 0)
            //        {
            //            //staff, union represented, regular
            //            return null;
            //        }
            //        if (current_stype == 0)
            //        {
                        // staff, non-represented, regular term
                        CreateLetterStaffNonrepTerm(currentAppointment);
                        return currentAppointment;
            //        }
                    
            //        else
            //        {
            //            // non-represented, temporary
            //            if (currrent_ttype == 0)
            //            {
            //                //staff, union-represented, temporary, extended temporary
            //                return null;
            //            }
            //            else if (currrent_ttype == 1)
            //            {
            //                //staff, union-represented, temporary, variable hour temporary
            //                return null;
            //            }
            //            else
            //            {
            //                //staff, union-represented, temporary, casual
            //                return null;
            //            }
            //        }
            //    }
            //    else return null;
            //}
            //else if (current_type == 1)
            //{
            //    //Student
            //    // TODO: add student specific attributes (graduate/undergraguate, taxable/non-taxable) to Appointment

            //    if (current_union == 0) //undergraduate?
            //    {
            //        //Union-Represented
            //        if (current_stype == 0) //taxable?
            //        {
            //            //Student undergraduate taxable
            //            return null;
            //        }
            //        else if (current_stype == 1) //non-taxable?
            //        {
            //            //Student undergraduate non-taxable
            //            return null;
            //        }
            //        else return null;
            //    }
            //    else if (current_union == 1) //graduate?
            //    {
            //        if (current_stype == 0) //taxable?
            //        {
            //            //Student graduate taxable
            //            return null;
            //        }
            //        else if (current_stype == 1) //non-taxable?
            //        {
            //            //Student graduate non-taxable
            //            return null;
            //        }
            //        else return null;
            //    }
            //    else return null;
            //}
            //else
            //{
            //    // Faculty
            //    // TODO: add faculty specific attributes (administrative, adjunct credit, adjunct non-credit) to Appointment
            //    if (current_union == 0)
            //    {
            //        //union represented
            //        if (current_stype == 0)
            //        {
            //            //faculty, union represented, regular
            //            return null;
            //        }
            //        else if (current_stype == 1)
            //        {
            //            //Faculty, union represented, regular term
            //            return null;
            //        }
            //        else
            //        {
            //            //falculty, union-represented, adjunct (temporary)
            //            return null;
            //        }
            //    }
            //        else return null;
            //    }

            }

        // Generate an appointment letter for employee of type Staff, Non-Union Represented, Regular-Term.
        // Input:   Appointment object (employee information)
        // Output:  Microsoft .docx file appended with employee information, saved to Content folder.
        public void CreateLetterStaffNonrepTerm(Appointment appointmentForDoc)
        {            
            // create a DocX object with the letter template .docx file
            string staffLetterFileName = "NR-Staff-Term-Appt-Ltr_040716.docx";
            string staffFilePath = AppDomain.CurrentDomain.BaseDirectory + @"content\appointment_letters\" + staffLetterFileName;
            DocX letter = DocX.Load(staffFilePath);

            // Replace all the fields in the template with atrributes from the Appointment object
            letter.ReplaceText("<<CurrentDate>>", DateTime.Now.ToShortDateString());
            letter.ReplaceText("<<Name>>", appointmentForDoc.Name);
            letter.ReplaceText("<<Address>>", appointmentForDoc.Employee_Address);
            letter.ReplaceText("<<SupervisorName>>", appointmentForDoc.Supervisor_Name);
            letter.ReplaceText("<<WorkingTitle>>", appointmentForDoc.Working_Title);
            letter.ReplaceText("<<JobTitle>>", appointmentForDoc.Job_Title);
            letter.ReplaceText("<<PCN>>", appointmentForDoc.Position_Number);
            letter.ReplaceText("<<Status>>", "Regular Term, " + appointmentForDoc.Full_Time + ", " + appointmentForDoc.Exemption);
            if (appointmentForDoc.Biweekly_Salary.HasValue)
            {
                letter.ReplaceText("<<PayType>>", "Bi-weekly salary");
                letter.ReplaceText("<<PayAmount>>", appointmentForDoc.Biweekly_Salary.ToString() + " per pay period");
            }
            if (appointmentForDoc.Hourly_Rate.HasValue)
            {
                letter.ReplaceText("<<PayType>>", "Hourly rate");
                letter.ReplaceText("<<PayAmount>>", appointmentForDoc.Hourly_Rate.ToString() + " per hour");
            }
            letter.ReplaceText("<<BeginningDate>>", appointmentForDoc.Beginning_Date);
            letter.ReplaceText("<<EndingDate>>", appointmentForDoc.Ending_Date);

            // Save the letter as a temporary document for download
            letter.SaveAs(AppDomain.CurrentDomain.BaseDirectory + @"content\temp_files\TempAppointmentLetter.docx");
        }

        // POST:    Appointments/Create
        // Input:   Employee information posted to web form
        // Output:  A File download prompt to the user
        //          The file is the complete Employee Appointment letter
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentID,Name,Working_Title,Job_Title,Supervisor_Name,"
                    +"Employee_Address,Position_Number,Full_Time,Exemption,Biweekly_Salary,Hourly_Rate,Employee_Type,"
                    +"Union_Status,Staff_Type,Temporary_Type,Beginning_Date,Ending_Date")] Appointment appointment)
        {
            // determine correct employee type
            TreeLogic(appointment);

            // Prepare the temporary .docx file for download
            string employeeName = appointment.Name;
            HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory 
                + @"content\temp_files\TempAppointmentLetter.docx"), "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = "NR-Staff-Term-Appt-Ltr " + employeeName + ".docx"
            };

            // Prompts the user to download the completed appointment letter
            return result;
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentID,Name,Type,Union,SType,TType")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
