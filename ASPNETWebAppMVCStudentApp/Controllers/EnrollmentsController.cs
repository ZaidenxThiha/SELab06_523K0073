using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        // GET: Enrollments
        public ActionResult Index()
        {
            var enrollments = db.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .OrderBy(e => e.Course.Title)
                .ThenBy(e => e.Student.LastName)
                .ToList();
            return View(enrollments);
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enrollment = db.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefault(e => e.EnrollmentID == id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(enrollment.CourseID, enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }

            PopulateDropDowns(enrollment.CourseID, enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropDowns(enrollment.CourseID, enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enrollment = db.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefault(e => e.EnrollmentID == id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var enrollment = db.Enrollments.Find(id);
            if (enrollment != null)
            {
                db.Enrollments.Remove(enrollment);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private void PopulateDropDowns(object selectedCourse = null, object selectedStudent = null)
        {
            var courses = db.Courses
                .OrderBy(c => c.Title)
                .Select(c => new { c.CourseID, Display = c.Title })
                .ToList();
            var students = db.Students
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Select(s => new { s.StudentID, Display = s.FullName })
                .ToList();

            ViewBag.CourseID = new SelectList(courses, "CourseID", "Display", selectedCourse);
            ViewBag.StudentID = new SelectList(students, "StudentID", "Display", selectedStudent);
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
