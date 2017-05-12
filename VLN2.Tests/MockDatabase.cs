using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

using VLN2.Models;
using FakeDbSet;
using VLN2.Tests;

namespace Example.BusinessLogicTest
{
    /// <summary>
    /// This is an example of how we'd create a fake database by implementing the 
    /// same interface that the BookeStoreEntities class implements.
    /// </summary>
    public class MockDatabase : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDatabase()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            this.Projects = new InMemoryDbSet<Project>();
            this.ProgrammingLanguages = new InMemoryDbSet<ProgrammingLanguage>();
            this.ProjectFiles = new InMemoryDbSet<ProjectFile>();
            this.Users = new InMemoryDbSet<ApplicationUser>();
            this.UserHasProject = new InMemoryDbSet<UserHasProject>();
            this.ProjectRole = new InMemoryDbSet<ProjectRole>();
        }

        public IDbSet<ApplicationUser> Users { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<UserHasProject> UserHasProject { get; set; }
        public IDbSet<ProjectFile> ProjectFiles { get; set; }
        public IDbSet<ProjectRole> ProjectRole { get; set; }
        public IDbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}
