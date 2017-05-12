using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Hubs
{
    public class ProjectHubHelper
    {

        /// <summary>
        /// Concats the projectID and projectFileID.
        /// </summary>
        /// <param name="ProjectID">The ID of the project</param>
        /// <param name="ProjectFileID">The ID of the project file</param>
        /// <returns>The Concated string</returns>
        public static string GetLobbyName(int ProjectID, int ProjectFileID)
        {
            return ProjectID + "-" + ProjectFileID;
        }

    }
}