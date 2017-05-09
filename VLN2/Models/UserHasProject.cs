using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VLN2.Models
{
    public class UserHasProject
    {
        public int ID { get; set; }

        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public int ProjectRoleID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Project Project { get; set; }
    }
}