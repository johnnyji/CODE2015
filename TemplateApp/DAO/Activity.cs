using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class Activity
    {
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public double Percent { get; set; }
    }
}