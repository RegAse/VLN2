using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Models
{
    public class ProjectFile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public virtual Project Project { get; set; }
    }
}