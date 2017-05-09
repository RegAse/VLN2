using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VLN2.Models
{
    public class Follower
    {
        public int ID { get; set; }
        public int FollowerID { get; set; }
        public int FollowedID { get; set; }
    }
}