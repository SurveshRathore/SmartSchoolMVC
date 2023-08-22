using BusinessLayer.Interface;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace SmartSchoolMVCApp.Controllers
{
    public class TicketController : Controller
    {
        private readonly IBus ibus;
        private readonly IUserBL userBL;

        public TicketController(IBus ibus, IUserBL userBL)
        {
            this.ibus = ibus;
            this.userBL = userBL;
        }

        public async Task<IActionResult> CreateTicketForPassword(string regNo,string email)
        {
            try
            {
                var token = userBL.StudentLogin(regNo, email, HttpContext);
                string data = "hello";
                //if(!string.IsNullOrEmpty(token))
                if (!string.IsNullOrEmpty(data))
                {
                    var ticket = userBL.CreateTicketForPassword(regNo, email);
                    Uri uri = new Uri("rabbitmq");
                    var endpoint = await ibus.GetSendEndpoint(uri);
                    await endpoint.Send(ticket);
                    return Ok(new { Sucess = true, Message = "Mail send successfully" });

                }
                else
                {
                    return BadRequest(new { Sucess = false, Message = "Email not found" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
