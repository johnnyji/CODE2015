using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class Sunny
    {
        public ObjectId Id { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int Days { get; set; }
    }
}