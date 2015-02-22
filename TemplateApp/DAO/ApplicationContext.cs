using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using TemplateApp.Models;
using TemplateApp.Properties;

namespace TemplateApp.DAO
{
    public class ApplicationContext : IDisposable
    {
        private MongoDatabase _database;

        public ApplicationContext()
        {

        }

        public ApplicationContext(MongoDatabase database)
        {
            this._database = database;
        }

        public MongoCollection<CityRainSnow> CityRainSnow { get { return this._database.GetCollection<CityRainSnow>(typeof(CityRainSnow).Name); } }
        public MongoCollection<Province> Provinces { get { return this._database.GetCollection<Province>(typeof(Province).Name); } }
        public MongoCollection<Sunny> Sunniest { get { return this._database.GetCollection<Sunny>(typeof(Sunny).Name); } }
        public MongoCollection<IndustryJob> IndustryJobs { get { return this._database.GetCollection<IndustryJob>(typeof(IndustryJob).Name); } }
        public MongoCollection<CommonStaticalData> Earnings { get { return this._database.GetCollection<CommonStaticalData>("Earning"); } }
        public MongoCollection<CommonStaticalData> EmploymentNumbers { get { return this._database.GetCollection<CommonStaticalData>("EmploymentNumber"); } }
        public MongoCollection<WalkScore> WalkScores { get { return this._database.GetCollection<WalkScore>(typeof(WalkScore).Name); } }
        public MongoCollection<CityState> Cities { get { return this._database.GetCollection<CityState>(typeof(CityState).Name); } }
        public MongoCollection<Population> Population { get { return this._database.GetCollection<Population>(typeof(Population).Name); } }
        public MongoCollection<Entrepreneurial> Entrepreneurs { get { return this._database.GetCollection<Entrepreneurial>(typeof(Entrepreneurial).Name); } }
        public MongoCollection<Crime> Crimes { get { return this._database.GetCollection<Crime>(typeof(Crime).Name); } }
        public MongoCollection<Activity> Activities { get { return this._database.GetCollection<Activity>(typeof(Activity).Name); } }
        public MongoCollection<Family> Families { get { return this._database.GetCollection<Family>(typeof(Family).Name); } }
        public MongoCollection<Student> Students { get { return this._database.GetCollection<Student>(typeof(Student).Name); } }
        public MongoCollection<Culture> Culture { get { return this._database.GetCollection<Culture>(typeof(Culture).Name); } } 

        public MongoDatabase Database
        {
            get { return _database; }
        }

        public static ApplicationContext Create()
        {
            // todo add settings where appropriate to switch server & _database in your own application
            var client = new MongoClient(Settings.Default.MongoDBConnectionString);
            var database = client.GetServer().GetDatabase(Settings.Default.MongoDBName);

            return new ApplicationContext(database);
        }

        public void Dispose()
        {
        }
    }
}