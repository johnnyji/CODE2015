using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class CityState
    {
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }
}