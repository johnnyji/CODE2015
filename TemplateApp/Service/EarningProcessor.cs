using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;
using TemplateApp.Service;

namespace TemplateApp.Controllers
{
    public class EarningProcessor : AppProcessorBase
    {
        public EarningProcessor(string choice, int resultLimit)
            : base(choice, resultLimit)
        {
        }

        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("a", ProcessEarningBestOption);
            return dict;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            yield break;
        }


        private IEnumerable<string> ProcessEarningBestOption(object arg)
        {
            var ctx = ApplicationContext.Create();

            var query = (from i in ctx.Earnings.FindAll()
                group i by i.GEO
                into g
                select g).ToDictionary(a => a.Key, b => b.OrderByDescending(a => a.Value).Average(c => c.Value));

            var best = query
                .OrderByDescending(a => a.Value)
                .Select(a => a.Key)
                .Take(ResultLimit);

            //var res = 
            //    .GroupBy(a => a.GEO)
            //    .OrderByDescending(e => e.Key, e => e.OrderByDescending(a => a.Value))                
            //    .Take(ResultLimit)
            //    .Select(a => a.Key);

            return best;
        }
    }
}
