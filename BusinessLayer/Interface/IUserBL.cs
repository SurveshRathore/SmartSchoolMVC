using Microsoft.AspNetCore.Http;
using ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        //public string RegisterNewStudent(string firstName, string lastName, string emailId);

        public string RegisterNewStudent(StudentModel studentModel);

        public StudentModel StudentDetail(int? id);
        public string StudentLogin(string email, string regNo, HttpContext httpContext);

        public IEnumerable<StudentModel> GetAllStudent();

        public StudentModel UpdateStudent(StudentModel studentModel);

        public StudentTicketModel CreateTicketForPassword(string Email, string Token);

        public string UploadImage(ImageModel imageModel);
    }
}
