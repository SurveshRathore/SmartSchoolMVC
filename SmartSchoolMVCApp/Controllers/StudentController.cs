using BusinessLayer.Interface;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelLayer.Model;

namespace SmartSchoolMVCApp.Controllers
{
    public class StudentController : Controller
    {
        private IUserBL userBL;

        public StudentController (IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpGet]
        public IActionResult StudentReg()
        {
            return View();
        }
        

        [HttpPost]
        public IActionResult StudentReg(StudentModel studentModel)
        {
            try
            {
                userBL.RegisterNewStudent(studentModel);
                return View(studentModel);
            }
            catch (System.Exception)
            {

                throw;
            }
            return View();
        }
        [HttpGet]
        public IActionResult StdLogin()
        {
            TempData["StudentIsLogin"] = false;
            return View();
        }


        [HttpPost]
        public IActionResult StdLogin(StudentLoginModel studentLoginModel)
        {
            if (ModelState.IsValid)
            {
                var token = userBL.StudentLogin(studentLoginModel.EmailID, studentLoginModel.RegNo, HttpContext);

                if(token != null)
                {
                    HttpContext.Session.SetString("token", token);
                    //HttpContext.Session.SetInt32 ("stdId", id);

                    TempData["StudentIsLogin"] = true;

                    var emailSender = new EmailSender();
                    emailSender.SendEmailRequest("surveshrathore98@gmail.com", "Test Subject", "Test Body");
                    return RedirectToAction("StudentDetails");

                    var emailListener = new ConsumerProcessingEmail();
                    emailListener.StartListening();
                    
                }
                
            }
            return View(studentLoginModel);
        }
        //public IActionResult StdLogin( FormCollection form )
        //{
        //    try
        //    {
        //        string email = form["Email"];
        //        string regno = form["RegNo"];
        //        if (email == null || regno == null ) {
        //            return NotFound();
        //        }
        //        var id = userBL.StudentLogin(email, regno);
        //        if(id != 0)
        //        {
        //            HttpContext.Session.SetInt32 ("stdId", id);
        //            return RedirectToAction("StudentDetails");
        //        }
        //        return View(email, regno);
        //    }
        //    catch (System.Exception)
        //    {

        //        throw;
        //    }
        //}

        [HttpGet]
        //public IActionResult EditStudentDetails(int stdID)
        public IActionResult EditStudentDetails()
        {
            int stdID = (int)HttpContext.Session.GetInt32("stdId");
            var email = HttpContext.Session.GetString("email");
            ViewBag.StudentEmail = email;
            try
            {
                if(stdID == 0)
                {
                    return NotFound();
                }
                StudentModel studentModel = userBL.StudentDetail(stdID);
                return View(studentModel);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult EditStudentDetails(int id, StudentModel studentModel)
        {
            studentModel.Id = id;
            if (ModelState.IsValid)
            {
                userBL.UpdateStudent(studentModel);
                return RedirectToAction("StudentDetail");

            }
            return View(studentModel);
        }



        [HttpGet]
        public IActionResult StudentDetails()
        {
            int id = (int)HttpContext.Session.GetInt32("stdId");
            try
            {
                if(id == 0)
                {
                    return NotFound();
                }
                StudentModel studentModel = userBL.StudentDetail(id);
                if(studentModel == null)
                {
                    return NotFound();
                }
                return View(studentModel);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public IActionResult GetAllStudent()
        {
            try
            {
                var result = this.userBL.GetAllStudent();
                return View(result);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult UploadImage()
        {
            try
            {
                return View();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult UploadImage(ImageModel imageModel)
        //public IActionResult UploadImage(ImageModel imageModel)
        {
            try
            {
                var result = this.userBL.UploadImage(imageModel);
                return RedirectToAction("StudentDetail");
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        //[HttpPost]
        //public IActionResult UploadImage(int id, HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        Account account = new Account("dr6tafwcd", "579242561473469", "qjqbQQRloyPBxe5EdRvMRmY6XYk");

        //        Cloudinary cloudinary = new Cloudinary(account);

        //        var uploadParams = new ImageUploadParams()
        //        {
        //            File = new FileDescription(file.FileName, file.InputStream)
        //        };

        //        ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

        //        string imagePath = uploadResult.Url.ToString();

        //        var result = this.userBL.UploadImage(id, imagePath);
        //        return View(result);
        //    }
        //    catch (System.Exception)
        //    {

        //        throw;
        //    }
        //}

        public IActionResult Index()
        {
            return View();
        }
    }

    
}
