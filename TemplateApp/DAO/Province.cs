using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class Province
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string ProvinceCode { get; set; }
    }
}