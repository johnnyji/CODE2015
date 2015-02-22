using MongoDB.Bson;

namespace TemplateApp.DAO
{
    public class IndustryJob
    {
        public ObjectId Id { get; set; }
        public int Ref_Date { get; set; }
        public string GEOGRAPHY { get; set; }
        public string INDUSTRY { get; set; }
        public string Vector { get; set; }
        public string Coordinate { get; set; }
        public double Value { get; set; }
    }
}