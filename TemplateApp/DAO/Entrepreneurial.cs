using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class Entrepreneurial
    {
        public ObjectId Id { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public int Score { get; set; }
    }
}