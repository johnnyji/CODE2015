using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class CommonStaticalData
    {
        public ObjectId Id { get; set; }
        public string Ref_Date { get; set; }
        public string GEO { get; set; }
        public string Vector { get; set; }
        public double Coordinate { get; set; }
        public double Value { get; set; }
    }
}