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

        /// <summary>
        /// Get programming language by extension.
        /// </summary>
        /// <param name="extension">The extension of the file</param>
        /// <returns></returns>
        public ProgrammingLanguage GetProgrammingLanguageByExtension(string extension)
        {
            return _db.ProgrammingLanguages.SingleOrDefault(x => x.FileExtension == extension);
        }

    }
}