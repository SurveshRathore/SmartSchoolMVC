using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using ModelLayer.Model;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBL :IUserBL
    {
        private readonly IUserRL userRL;

        public UserBL(IUserRL userRL)
        { 
            this.userRL = userRL; 
        }
        public string RegisterNewStudent(StudentModel studentModel)
        {
            try
            {
                return this.userRL.RegisterNewStudent(studentModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StudentModel StudentDetail(int? id)
        {
            try
            {
                return this.userRL.StudentDetail(id);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string StudentLogin(string email, string regNo, HttpContext httpContext)
        {
            try
            {
                return this.userRL.StudentLogin(email, regNo, httpContext);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<StudentModel> GetAllStudent()
        {
            try
            {
                return this.userRL.GetAllStudent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StudentModel UpdateStudent(StudentModel studentModel)
        {
            try
            {
                return this.userRL.UpdateStudent(studentModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StudentTicketModel CreateTicketForPassword(string Email, string Token)
        {
            try
            {
                return this.userRL.CreateTicketForPassword(Email, Token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string UploadImage(ImageModel imageModel)
        {
            try
            {
                return this.userRL.UploadImage(imageModel );
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
