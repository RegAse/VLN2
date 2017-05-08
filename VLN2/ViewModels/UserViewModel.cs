using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.ViewModels
{
    public class UserViewModel
    {
        public string Displayname { get; set; }

        public string Description { get; set; }

        public UserViewModel(string displayname, string description)
        {
            Displayname = displayname;

            Description = description;

        }
    }
}