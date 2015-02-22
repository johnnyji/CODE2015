using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class Crime
    {
        public ObjectId Id { get; set; }
        public int Rank { get; set; }
        public string City { get; set; }
        public double Percentage { get; set; }
    }
}