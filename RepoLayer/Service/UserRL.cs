using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Model;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace RepoLayer.Service
{
    public class UserRL:IUserRL
    {
        private readonly IConfiguration iconfiguration1;
        private string ConnectionString;
        private readonly SqlConnection sqlConnection = new SqlConnection();


        public UserRL(IConfiguration iconfiguration) {
            iconfiguration1 = iconfiguration;
            this.ConnectionString = iconfiguration.GetConnectionString("SmartSchlDB");
            sqlConnection.ConnectionString = this.ConnectionString;
        }

        public string RegisterNewStudent (StudentModel studentModel)
        {
            string regNo = "";
            try
            {
                using (sqlConnection)
                {
                    
                    SqlCommand sqlCommand = new SqlCommand("spStudentRegistration", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@FirstName", studentModel.FirstName );
                    sqlCommand.Parameters.AddWithValue("@LastName", studentModel.LastName );
                    sqlCommand.Parameters.AddWithValue("@EmailId", studentModel.EmailID );

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.HasRows)
                    {
                        while(sqlDataReader.Read())
                        {
                            regNo = sqlDataReader.IsDBNull("RegistrationNumber") ? String.Empty : sqlDataReader.GetString("RegistrationNumber");
                        }
                        return regNo;
                    }
                    
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                if(sqlConnection.State == ConnectionState.Open) 
                { 
                    sqlConnection.Close(); 
                }
            }
        }

        public StudentModel UpdateStudent (StudentModel studentModel) {

            try
            {
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("spUpdateStudentDetail", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@stdID", studentModel.Id);
                    sqlCommand.Parameters.AddWithValue("@FirstName", studentModel.FirstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", studentModel.LastName);
                    sqlCommand.Parameters.AddWithValue("@EmailId", studentModel.EmailID);
                    sqlCommand.Parameters.AddWithValue("@Class", studentModel.StdClass);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", studentModel.PhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@StdAddress", studentModel.Address);
                    sqlCommand.Parameters.AddWithValue("@DOB", studentModel.DOB);

                    sqlConnection.Open();

                    int result = sqlCommand.ExecuteNonQuery();

                    if(result >= 1)
                    {
                        return studentModel;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();
            }

        }

        public string StudentLogin (string email, string regNo, HttpContext httpContext)
        {
            try
            {
                using (sqlConnection)
                {
                    //var id = 0;
                    SqlCommand sqlCommand = new SqlCommand("spStudentLogin", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    
                    sqlCommand.Parameters.AddWithValue("@RegNum", regNo);
                    sqlCommand.Parameters.AddWithValue("@EmailId", email);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();


                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            int id = sqlDataReader.IsDBNull(0) ? 0 : sqlDataReader.GetInt32(0);
                            //HttpContext httpContext = new HttpContext();
                            httpContext.Session.SetInt32("stdId", id);
                            httpContext.Session.SetString("email", email);
                            httpContext.Session.SetString("regNo", regNo);

                            var token = GenerateJwtToken(email, id);
                            return token;
                        }
                        //return 0;
                    }
                    return null;

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();
                
            }
        }

        public StudentModel StudentDetail(int? id)
        {
            try
            {
                using(sqlConnection)
                {
                    StudentModel model = new StudentModel();
                    //string query = "select * from studentTable where EmailId =@Id";
                    string query = "select * from studentTable where stdID =" + id;
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.CommandType = CommandType.Text;

                    //sqlCommand.Parameters.AddWithValue("@Id", id);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.HasRows)
                    {
                        while(sqlDataReader.Read())
                        {
                            
                            model.Id =  sqlDataReader.IsDBNull(0) ? 0 : sqlDataReader.GetInt32(0);
                            model.RegNo = sqlDataReader.IsDBNull(1) ? string.Empty : sqlDataReader.GetString(1);
                            model.EmailID = sqlDataReader.IsDBNull(2) ? string.Empty : sqlDataReader.GetString(2);
                            model.FirstName = sqlDataReader.IsDBNull(3) ? string.Empty : sqlDataReader.GetString(3);
                            model.LastName = sqlDataReader.IsDBNull(4) ? string.Empty : sqlDataReader.GetString(4);
                            model.StdClass = sqlDataReader.IsDBNull(5) ? 0 : sqlDataReader.GetInt32(5);
                            model.PhoneNumber = sqlDataReader.IsDBNull(6) ? 0 : sqlDataReader.GetInt64(6);
                            model.Address = sqlDataReader.IsDBNull(7) ? string.Empty : sqlDataReader.GetString(7);
                            model.DOB = sqlDataReader.IsDBNull(8) ? DateTime.Now : sqlDataReader.GetDateTime(8);
                            model.CreatedAt = sqlDataReader.IsDBNull(9) ? DateTime.Now : sqlDataReader.GetDateTime(9);
                            model.LastUpdatedAt = sqlDataReader.IsDBNull(10) ? DateTime.Now : sqlDataReader.GetDateTime(10);
                            model.Gender = sqlDataReader.IsDBNull(13) ? string.Empty : sqlDataReader.GetString(13);
                        }

                        return model;
                    }
                    return null;

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlConnection.Close();

            }
        }

        public IEnumerable<StudentModel> GetAllStudent ()
        {
            try
            {
                using(sqlConnection)
                {
                    string query = "Select * from StudentTable";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.CommandType = CommandType.Text;

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.HasRows)
                    {
                        List <StudentModel> studentsList = new List<StudentModel>();
                        while(sqlDataReader.Read())
                        {
                            StudentModel studentModel = new StudentModel();
                            studentModel.Id = sqlDataReader.IsDBNull(0)? 0 : sqlDataReader.GetInt32(0);
                            studentModel.RegistrationNumber = sqlDataReader.IsDBNull(1) ? String.Empty : sqlDataReader.GetString(1);
                            studentModel.FirstName = sqlDataReader.IsDBNull(3) ? String.Empty : sqlDataReader.GetString(3);
                            studentModel.LastName = sqlDataReader.IsDBNull(4) ? String.Empty : sqlDataReader.GetString(4);
                            studentModel.Gender = sqlDataReader.IsDBNull(13) ? String.Empty : sqlDataReader.GetString(13);
                            studentModel.StdClass = sqlDataReader.IsDBNull(5) ? 0 : sqlDataReader.GetInt32(5);
                            studentModel.EmailID = sqlDataReader.IsDBNull(2) ? String.Empty : sqlDataReader.GetString(2);
                            studentModel.PhoneNumber = sqlDataReader.IsDBNull(6) ? 0 : sqlDataReader.GetInt32(6);
                            studentModel.Address = sqlDataReader.IsDBNull(7) ? String.Empty : sqlDataReader.GetString(7);
                            studentModel.DOB = sqlDataReader.IsDBNull(8) ? DateTime.Now : sqlDataReader.GetDateTime(8);
                            studentModel.CreatedAt = sqlDataReader.IsDBNull(9) ? DateTime.Now : sqlDataReader.GetDateTime(9);
                            studentModel.LastUpdatedAt = sqlDataReader.IsDBNull(10) ? DateTime.Now : sqlDataReader.GetDateTime(10);
                            studentsList.Add(studentModel);
                        }
                        return studentsList;
                    }

                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StudentTicketModel CreateTicketForPassword (string Email, string Token)
        {
            try
            {
                StudentTicketModel studentTicketModel = new StudentTicketModel
                {
                    FirstName = "Survesh",
                    LastName = "Rathore",
                    EmailId = "surveshrathore98@gmail.com",
                    Token = Token,
                    IssueAt = DateTime.Now
                };
                return studentTicketModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public String GenerateJwtToken(string email, long userId)
        {
            try
            {
                //var tokenHandler = new JwtSecurityTokenHandler();
                var userKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.iconfiguration1["JWT:key"]));
                var crediential = new SigningCredentials(userKey, SecurityAlgorithms.HmacSha256Signature);

                var claims = new[]
                {
                    new Claim("Email", email),
                    new Claim ("UserId", userId.ToString())
                };

                var token = new JwtSecurityToken(

                    issuer: iconfiguration1["JWT:Issuer"],
                    audience: iconfiguration1["JWT:Audience"],
                    claims,
                    expires:DateTime.Now.AddHours(2),
                    signingCredentials: crediential
                    );
               
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public string UploadImage (ImageModel imageModel, int studentId, IFormFile image)
        public string UploadImage(ImageModel imageModel)
        {
            try
            {
                using(sqlConnection)
                {
                    //string query = "select * from StudentTable where stdID = " + studentId;

                    Account account = new Account(
                        iconfiguration1["CloudinaryDetails:Name"],
                        iconfiguration1["CloudinaryDetails:APIKey"],
                        iconfiguration1["CloudinaryDetails:APISecret"]
                        );

                    Cloudinary cloudinary = new Cloudinary(account);

                    var uploadImage = new ImageUploadParams()
                    {
                        File = new FileDescription(imageModel.image.FileName, imageModel.image.OpenReadStream()),
                    };

                    var imageUploadResult =  cloudinary.Upload(uploadImage);
                    string imagePath = imageUploadResult.Url.ToString();

                    string query = "insert into studentImage(studentImage, studentID) values(\'" + imagePath + "\'," + imageModel.studentId + ")";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    //sqlCommand.CommandType = CommandType.Text;

                    sqlConnection.Open();
                    int result = sqlCommand.ExecuteNonQuery();

                    if(result >= 1)
                    {
                        return "Image uploaded successfully";
                    }
                    else
                    {
                        return "Failed to upload image";
                    }

                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
