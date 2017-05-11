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

        public static string RemoveFromTo(string original, int rowStart, int columnStart, int rowEnd, int columnEnd)
        {
            string result = "";

            int toIndexFirst = 0;
            if (rowStart != 0)
            {
                toIndexFirst = IndexOfOccurence(original, "\n", rowStart) + 1;
            }
            result += original.Substring(0, toIndexFirst + columnStart);

            int fromIndex = 0;
            if (rowEnd != 0)
            {
                fromIndex = IndexOfOccurence(original, "\n", rowEnd) + 1;
            }

            result += original.Substring(fromIndex + columnEnd);

            return result;
        }

        public static string InsertIntoStringAt(string original, string value, int row, int column)
        {
            string result = "";

            int toIndexFirst = 0;
            if (row != 0)
            {
                toIndexFirst = IndexOfOccurence(original, "\n", row) + 1;
            }
            int ind = toIndexFirst + column;
            if (ind != 0 && toIndexFirst + column > original.Length - 1)
            {
                result += original + value;
            }
            else
            {
                result += original.Substring(0, ind);
                result += value;
                result += original.Substring(ind);
            }

            return result;
        }

        public static int IndexOfOccurence(string s, string match, int occurence)
        {
            int i = 1, index = 0;

            while (i <= occurence && (index = s.IndexOf(match, index + 1)) != -1)
            {
                if (i == occurence)
                {
                    return index;
                }

                i++;
            }

            return -1;
        }

    }
}