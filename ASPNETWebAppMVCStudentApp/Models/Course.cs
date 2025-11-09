using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPNETWebAppMVCStudentApp.Models
{
    public class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int CourseID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Range(0, 10)]
        public int Credits { get; set; }

        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
