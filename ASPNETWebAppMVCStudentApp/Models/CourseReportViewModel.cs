using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ASPNETWebAppMVCStudentApp.Models
{
    public class CourseReportViewModel
    {
        [Display(Name = "Course")]
        [Required(ErrorMessage = "Please select a course.")]
        public int? SelectedCourseId { get; set; }

        public IEnumerable<SelectListItem> Courses { get; set; }
    }
}
