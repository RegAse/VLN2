using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VLN2.Models;

namespace VLN2.Services
{
    public class ProgrammingLanguageService
    {
        private ApplicationDbContext _db;

        public ProgrammingLanguageService()
        {
            _db = new ApplicationDbContext();
        }

        public ProgrammingLanguage GetProgrammingLanguageByExtension(string extension)
        {
            ProgrammingLanguage lang = _db.ProgrammingLanguages.SingleOrDefault(x => x.FileExtension == extension);

            return lang;
        }

    }
}