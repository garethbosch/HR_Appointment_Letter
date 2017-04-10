using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/**
 * File         Appointment.cs
 * 
 * Function     Model that defines a new employee appointment letter
 * 
 * Authors      Gareth Bosch    gbosch@alaska.edu
 *              Tevin Gladden   tjgldden@alaska.edu
 *              Rocco Haro      rharo@alaska.edu
 *              
 * Date         Fall 2016
 *              
 * */

namespace UAA.HR.Forms.WebUI.Models
{
    // An Appointment contains all new employee details that are needed
    //    to determine the correct appointment letter type for that employee.
    // An Appointment contains all details that need to be appended to the 
    //    letter .docx file.
    public class Appointment
    {
        public int AppointmentID { get; set; }

        [DisplayName("Employee Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Employee Working Title")]
        [Required]
        public string Working_Title { get; set; }

        [DisplayName("Employee Job Title")]
        [Required]
        public string Job_Title { get; set; }

        [DisplayName("Supervisor's Name")]
        [Required]
        public string Supervisor_Name { get; set; }

        [DisplayName("Employee Address")]
        [Required]
        public string Employee_Address { get; set; }

        [DisplayName("Position Number")]
        [Required]
        public string Position_Number { get; set; }

        [DisplayName("Full Time or Part Time")]
        public string Full_Time { get; set; }

        [DisplayName("Exempt or Non-Exempt")]
        public string Exemption { get; set; }

        [DisplayName("Bi-weekly Salary Amount")]
        public decimal? Biweekly_Salary { get; set; }

        [DisplayName("Hourly Rate Amount")]
        public decimal? Hourly_Rate { get; set; }

        [DisplayName("Type of Employee")]
        [Required]
        public EmployeeType Employee_Type { get; set; }

        [DisplayName("Union Status")]
        [Required]
        public Union Union_Status { get; set; }

        [DisplayName("Type of Staff")]
        [Required]
        public StaffType Staff_Type { get; set; }

        [DisplayName("Type of Temporary Staff")]
        [Required]
        public TemporaryType Temporary_Type { get; set; }

        [DisplayName("Beginning Date")]
        public string Beginning_Date { get; set; }

        [DisplayName("Ending Date")]
        public string Ending_Date { get; set; }

        public virtual Letter Letter { get; set; }

        
    }

    // Enums used for selection lists in the View

    // Current version only generates a letter for employee type Staff
    public enum EmployeeType
    {
        Staff,
        //Student,
        //Faculty
    }

    // Current version only generates a letter for union type Nonrepresented
    public enum Union
    {
        //NA,
        //Represented,
        Nonrepresented
    }

    // Current version only generates a letter for staff type Regular Term
    public enum StaffType
    {
        //NA,
        //Regular,
        Regular_Term,
        //Temporary
    }

    // Current version only generates a letter for non-Temporary Type
    public enum TemporaryType
    {
        NA,
        //Extended,
        //Variable,
        //Casual
    }
}