using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ZapFitness.Models
{
    public class Member_Model
    {
        public int MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

    }
}