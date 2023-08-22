using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.Model
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Gender { get; set; }
        public int StdClass { get; set; }   
        public string EmailID { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd-mm-yyyy}")]
        public DateTime DOB { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set;}

    }
}
