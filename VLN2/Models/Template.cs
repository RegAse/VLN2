using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Models
{
    public class Template
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public virtual ProgrammingLanguage ProgrammingLanguage { get; set; }
    }
}