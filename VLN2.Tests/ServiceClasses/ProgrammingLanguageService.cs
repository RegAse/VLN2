using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLN2.Models;

namespace VLN2.Tests.ServiceClasses
{
    public interface IAppDataContext
    {
        IDbSet<ApplicationUser> Users { get; set; }
        IDbSet<Project> Projects { get; set; }
        IDbSet<UserHasProject> UserHasProject { get; set; }
        IDbSet<ProjectFile> ProjectFiles { get; set; }
        IDbSet<ProjectRole> ProjectRole { get; set; }
        IDbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        int SaveChanges();
    }
    public class ProgrammingLanguageService
    {
        private IAppDataContext _db;

        public ProgrammingLanguageService(IAppDataContext context)
        {
            _db = context;
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
