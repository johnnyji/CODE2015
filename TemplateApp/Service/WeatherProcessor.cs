using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;
using TemplateApp.Models;

namespace TemplateApp.Service
{
    public class WeatherProcessor : AppProcessorBase
    {
        public WeatherProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }

        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("a", RainPreference);
            dict.Add("b", SnowPreference);
            dict.Add("c", SunnyPreference);
            return dict;
        }

        private IEnumerable<string> RainPreference(object arg)
        {
            return ApplicationContext.Create()
                .CityRainSnow.AsQueryable()
                .OrderByDescending(a => a.WetDaysCount)
                .Take(ResultLimit)
                .Select(a => a.City);
        }

        private IEnumerable<string> SnowPreference(object arg)
        {
            return ApplicationContext.Create()
           .CityRainSnow.AsQueryable()
           .OrderByDescending(a => a.SnowFallCM)
           .Take(ResultLimit)
           .Select(a => a.City);
        }

        private IEnumerable<string> LessRainPreference(object arg)
        {
            return ApplicationContext.Create()
        .CityRainSnow.AsQueryable()
        .OrderBy(a => a.PrecipitationMM)
        .Take(ResultLimit)
        .Select(a => a.City);
        }

        private IEnumerable<string> SunnyPreference(object arg)
        {
            return ApplicationContext.Create()
      .Sunniest.AsQueryable()
      .OrderByDescending(a => a.Days)
      .Take(ResultLimit)
      .Select(a => a.City);
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var rainMetric = ApplicationContext.Create().CityRainSnow.AsQueryable()
                .Select(a => a.City)
                .ToArray();

            var sunMetric = ApplicationContext.Create().Sunniest.AsQueryable()
            .Select(a => a.City)
            .ToArray();

            var nonLeft = rainMetric.Except(sunMetric);
            //var nonRight = sunMetric.Except(rainMetric);
            var toExclude = nonLeft
                            //.Union(nonRight)
                            .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                                            .Select(a => a.City).ToArray();


            var used = rainMetric.Join(sunMetric, a => a, b => b, (a,b) => a, StringComparer.OrdinalIgnoreCase);

            return toExclude.Union(toHandle.Except(used, StringComparer.OrdinalIgnoreCase)).Distinct(StringComparer.OrdinalIgnoreCase);

        }
    }
}