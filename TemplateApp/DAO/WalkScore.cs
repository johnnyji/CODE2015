using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateApp.DAO
{
    public class WalkScore
    {
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        [MongoDB.Bson.Serialization.Attributes.BsonElement("WalkScore")]
        public int Value { get; set; }
        [MongoDB.Bson.Serialization.Attributes.BsonIgnoreIfNull(true)]
        public int? TransitScore { get; set; }
    }
}