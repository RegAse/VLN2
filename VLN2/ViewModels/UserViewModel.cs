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
        public int UserId { get; set; }

        public string Displayname { get; set; }

        public string Description { get; set; }

        public string Username { get; set; }

        public ICollection<ApplicationUser> Followers { get; set; }

        public ICollection<ApplicationUser> Following { get; set; }

        public UserViewModel(int userid, string displayname, string description, string username, ICollection<ApplicationUser> followers, ICollection<ApplicationUser> following)
        {
            UserId = userid;

            Displayname = displayname;

            Description = description;

            Username = username;

            Followers = followers;

            Following = following;
        }
    }
}