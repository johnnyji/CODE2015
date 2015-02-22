using System;
using System.Collections.Generic;
using System.Linq;
using TemplateApp.DAO;
using TemplateApp.Service;

namespace TemplateApp.Controllers
{
    public class EmploymentTotalProcessor : AppProcessorBase
    {
        public EmploymentTotalProcessor(string choice, int resultLimit)
            : base(choice, resultLimit)
        {
        }

        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("a", ProcessEmploymentMoreNumber);
            return dict;
        }

        private IEnumerable<string> ProcessEmploymentMoreNumber(object arg)
        {
            var ctx = ApplicationContext.Create();

            var query = (from i in ctx.EmploymentNumbers.FindAll()
                group i by i.GEO
                into g
                select g).ToDictionary(a => a.Key, b => b.OrderByDescending(a => a.Value).Average(c => c.Value));

            var best = query
                .OrderByDescending(a => a.Value)
                .Select(a => a.Key)
                .Take(ResultLimit);

            return best;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            yield break;
        }
    }
}