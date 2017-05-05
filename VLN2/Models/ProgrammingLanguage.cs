using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Models
{
    public class ProgrammingLanguage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FileExtension { get; set; }

        public virtual ICollection<Template> Templates { get; set; }
    }
}