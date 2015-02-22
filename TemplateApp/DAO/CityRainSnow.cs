using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateApp.DAO
{
    public class CityRainSnow
    {
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public double SnowFallCM { get; set; }
        public double PrecipitationMM { get; set; }
        public double WetDaysCount { get; set; }
    }
}
