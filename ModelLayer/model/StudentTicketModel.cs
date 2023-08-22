using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.Model
{
    public class StudentTicketModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Token { get; set; }
        public DateTime IssueAt { get; set; }
    }
}
