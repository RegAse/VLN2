using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Models
{
    public class ProjectRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserHasProject> UserHasProjects { get; set; }
    }
}