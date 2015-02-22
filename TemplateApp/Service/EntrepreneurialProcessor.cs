using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;

namespace TemplateApp.Service
{
    public class EntrepreneurialProcessor : AppProcessorBase
    {
        public EntrepreneurialProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {

        }

        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("a", EntreprenurialPreference);
            return dict;
        }

        private IEnumerable<string> EntreprenurialPreference(object o)
        {
            return ApplicationContext.Create().Entrepreneurs.AsQueryable().OrderByDescending(a => a.Score)
                .Select(a => a.City)
                .Take(ResultLimit);
        }


        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().WalkScores.AsQueryable()
               .Select(a => a.City)
               .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                                            .Select(a => a.City).ToArray();

            return toHandle.Except(metric, StringComparer.OrdinalIgnoreCase).Distinct(StringComparer.OrdinalIgnoreCase);

        }


    }
}