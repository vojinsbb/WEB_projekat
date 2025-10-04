using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_projekat.Models
{
    public class Employee : User
    {
        public bool IsBlocked { get; set; } = false;
    }
}