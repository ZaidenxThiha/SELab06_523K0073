using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETWebAppMVCStudentApp.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
