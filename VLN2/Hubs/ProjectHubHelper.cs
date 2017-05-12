using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Hubs
{
    public class ProjectHubHelper
    {

        public static string GetLobbyName(int ProjectID, int ProjectFileID)
        {
            return ProjectID + "-" + ProjectFileID;
        }

    }
}