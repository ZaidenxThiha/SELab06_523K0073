using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASPNETWebAppMVCStudentApp.Models;
using Microsoft.Reporting.WebForms;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        [HttpGet]
        public ActionResult Index()
        {
            var model = new CourseReportViewModel
            {
                Courses = GetCourseSelectList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CourseReportViewModel model, string actionType)
        {
            model.Courses = GetCourseSelectList(model.SelectedCourseId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var course = db.Courses.FirstOrDefault(c => c.CourseID == model.SelectedCourseId);
            if (course == null)
            {
                ModelState.AddModelError("", "Selected course could not be found.");
                return View(model);
            }

            var reportBytes = GenerateReport(model.SelectedCourseId.Value, course.Title);
            var fileName = $"StudentReport_{course.Title}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            if (string.Equals(actionType, "Preview", StringComparison.OrdinalIgnoreCase))
            {
                return File(reportBytes, "application/pdf");
            }

            return File(reportBytes, "application/pdf", fileName);
        }

        private IEnumerable<SelectListItem> GetCourseSelectList(int? selectedCourseId = null)
        {
            return db.Courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.CourseID.ToString(),
                    Selected = (selectedCourseId != null && c.CourseID == selectedCourseId.Value)
                })
                .ToList();
        }

        private byte[] GenerateReport(int courseId, string courseTitle)
        {
            var reportData = db.Enrollments
                .Where(e => e.CourseID == courseId)
                .Select(e => new StudentReportItem
                {
                    StudentID = e.Student.StudentID,
                    FirstName = e.Student.FirstName,
                    LastName = e.Student.LastName,
                    Grade = e.Grade,
                    CourseTitle = e.Course.Title
                })
                .OrderBy(item => item.LastName)
                .ThenBy(item => item.FirstName)
                .ToList();

            var localReport = new LocalReport
            {
                ReportPath = Server.MapPath("~/Reports/StudentReport.rdlc")
            };

            var dataSource = new ReportDataSource("StudentReportDataSet", reportData);
            localReport.DataSources.Clear();
            localReport.DataSources.Add(dataSource);
            localReport.SetParameters(new ReportParameter("SelectedCourse", courseTitle));

            var bytes = localReport.Render(
                "PDF",
                null,
                out string mimeType,
                out string encoding,
                out string filenameExtension,
                out string[] streams,
                out Warning[] warnings);

            return bytes;
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
