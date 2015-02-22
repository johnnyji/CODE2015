using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;

namespace TemplateApp.Service
{
       [Obsolete]
    public class WalkScoreProcessor : AppProcessorBase
    {
        public WalkScoreProcessor(string choice, int resultLimit) : base(choice, resultLimit)
        {
        }

        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("a", CalculateWalkScorePreference);
          
            return dict;
        }


        private IEnumerable<string> CalculateWalkScorePreference(object arg)
        {
            return ApplicationContext.Create()
        .WalkScores
        .AsQueryable()
        .OrderByDescending(a => a.Value)
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