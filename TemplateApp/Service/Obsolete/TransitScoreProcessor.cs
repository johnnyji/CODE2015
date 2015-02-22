using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;

namespace TemplateApp.Service
{
    [Obsolete]
    public class TransitScoreProcessor : AppProcessorBase
    {

        private IEnumerable<string> CalculateTransitScorePreference(object arg)
        {
            // error
            return ApplicationContext.Create()
                .WalkScores
                .AsQueryable()
                .OrderByDescending(a => a.TransitScore)
                .Select(a => a.City)
                .Take(ResultLimit);
        }

        public TransitScoreProcessor(string value, int resultLimit) : base(value, resultLimit)
        {
        }

        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("a", CalculateTransitScorePreference);
            return dict;
        }


        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().WalkScores.AsQueryable()
                .Where(a => a.TransitScore.HasValue)
                .Select(a => a.City)
                .ToArray();

            var withoutValue = ApplicationContext.Create().WalkScores.AsQueryable()
                .Where(a => !a.TransitScore.HasValue)
                .Select(a => a.City)
                .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                .Select(a => a.City).ToArray();

            return toHandle.Except(metric, StringComparer.OrdinalIgnoreCase).Union(withoutValue).Distinct(StringComparer.OrdinalIgnoreCase);

        }
    }
}