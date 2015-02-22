using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;

namespace TemplateApp.Service
{
    public class LookingForJob : YesOrNoProcessor
    {
        public LookingForJob(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }

        protected override IEnumerable<string> OptionASelected(object arg)
        {
            var context = ApplicationContext.Create();

            return context.EmploymentNumbers
                .AsQueryable()
                .OrderByDescending(a => a.Value)
                .Select(a => a.GEO)
                .Take(ResultLimit);
        }

        protected override IEnumerable<string> OptionBSelected(object arg)
        {
            yield break;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().EmploymentNumbers.AsQueryable()
       .Select(a => a.GEO)
       .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                                            .Select(a => a.City).ToArray();

            var unhandled = toHandle.Except(metric, StringComparer.OrdinalIgnoreCase);

            var exceed =
                metric.Except(toHandle, StringComparer.OrdinalIgnoreCase);

            return unhandled.Union(exceed).Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class WalkTransitProcessor : YesOrNoProcessor
    {
        private bool? _optionASelected;
        public WalkTransitProcessor(string value, int resultLimit) : base(value, resultLimit)
        {
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            if (!_optionASelected.HasValue)
                throw new NullReferenceException("_optionASelected");

            var metric = ApplicationContext.Create().WalkScores.AsQueryable()
.ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                                            .Select(a => a.City).ToArray();

            if (_optionASelected.Value)
            {
                var unhandledStandard = toHandle.Except(metric.Select(a => a.City), StringComparer.OrdinalIgnoreCase);

                var exceedStandard =
                    metric.Select(a => a.City).Except(toHandle, StringComparer.OrdinalIgnoreCase);

                return unhandledStandard.Union(exceedStandard).Distinct(StringComparer.OrdinalIgnoreCase);
            }

            var custom = metric.Where(a => !a.TransitScore.HasValue).Select(a => a.City).ToArray();
            var exUnhandled = toHandle.Except(custom, StringComparer.OrdinalIgnoreCase);

            var exExceed =
               custom.Except(toHandle, StringComparer.OrdinalIgnoreCase);

            return exUnhandled.Union(exExceed).Distinct(StringComparer.OrdinalIgnoreCase);
        }

        protected override IEnumerable<string> OptionASelected(object arg)
        {
            this._optionASelected = true;

            return ApplicationContext.Create()
                .WalkScores
                .AsQueryable()
                .OrderByDescending(a => a.Value)
                .Select(a => a.City)
                .Take(ResultLimit);

        }

        protected override IEnumerable<string> OptionBSelected(object arg)
        {
            this._optionASelected = false;
            return ApplicationContext.Create()
    .WalkScores
    .AsQueryable()
    .Where(a => a.TransitScore.HasValue)
    .OrderByDescending(a => a.TransitScore)
    .Select(a => a.City)
    .Take(ResultLimit);

        }
    }

    public class ActivitiesProcessor : YesOrNoProcessor
    {
        public ActivitiesProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }

        protected override IEnumerable<string> OptionASelected(object arg)
        {
            var context = ApplicationContext.Create();

            return context.Activities
                .AsQueryable()
                .OrderByDescending(a => a.Percent)
                .Select(a => a.City)
                .Take(ResultLimit);
        }

        protected override IEnumerable<string> OptionBSelected(object arg)
        {
            yield break;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().Activities.AsQueryable()
       .Select(a => a.City)
       .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                                            .Select(a => a.City).ToArray();

            var unhandled = toHandle.Except(metric, StringComparer.OrdinalIgnoreCase);

            var exceed =
                metric.Except(toHandle, StringComparer.OrdinalIgnoreCase);

            return unhandled.Union(exceed).Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class OutgoingProcessor : YesOrNoProcessor
    {
        public OutgoingProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }

        protected override IEnumerable<string> OptionASelected(object arg)
        {
            var context = ApplicationContext.Create();

            var all = context.Culture.FindAll().ToArray();

            return all.Select((i, o) => new {i, o})
                .Where(a => a.i.Museums.HasValue || a.i.Performing.HasValue)
                .Select(a => a.i.Museums.HasValue && a.i.Performing.HasValue
                    ? new {a.i, a.o, Avg = (a.i.Museums + a.i.Performing)/2}
                    : new {a.i, a.o, Avg = a.i.Museums ?? a.i.Performing})
                .OrderByDescending(a => a.Avg)
                .Select(a => a.i.City)
                .Take(ResultLimit);
        }

        protected override IEnumerable<string> OptionBSelected(object arg)
        {
            yield break;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().Culture.AsQueryable()
                .Where(a => a.Museums.HasValue || a.Performing.HasValue)
                .Select(a => a.City)
                .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                .Select(a => a.City).ToArray();

            var unhandled = toHandle.Except(metric, StringComparer.OrdinalIgnoreCase);

            var exceed =
                metric.Except(toHandle, StringComparer.OrdinalIgnoreCase);

            return unhandled.Union(exceed).Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class StudentProcessor : YesOrNoProcessor
    {
        public StudentProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }

        protected override IEnumerable<string> OptionASelected(object arg)
        {
            var context = ApplicationContext.Create();

            return context.Students
                .AsQueryable()
                .OrderByDescending(a => a.Rank)
                .Select(a => a.City)
                .Take(ResultLimit);
        }

        protected override IEnumerable<string> OptionBSelected(object arg)
        {
            yield break;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().Students.AsQueryable()
                .Select(a => a.City)
                .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                .Select(a => a.City).ToArray();

            var unhandled = toHandle.Except(metric, StringComparer.OrdinalIgnoreCase);

            var exceed =
                metric.Except(toHandle, StringComparer.OrdinalIgnoreCase);

            return unhandled.Union(exceed).Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class EntrepreneusProcessor : YesOrNoProcessor
    {
        public EntrepreneusProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }

        protected override IEnumerable<string> OptionASelected(object arg)
        {
            var context = ApplicationContext.Create();

            return context.Entrepreneurs
                .AsQueryable()
                .OrderByDescending(a => a.Score)
                .Select(a => a.City)
                .Take(ResultLimit);
        }

        protected override IEnumerable<string> OptionBSelected(object arg)
        {
            yield break;
        }
        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().Entrepreneurs.AsQueryable()
       .Select(a => a.City)
       .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                                            .Select(a => a.City).ToArray();

            var unhandled = toHandle.Except(metric, StringComparer.OrdinalIgnoreCase);

            var exceed =
                metric.Except(toHandle, StringComparer.OrdinalIgnoreCase);

            return unhandled.Union(exceed).Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }

    public class FamilyProcessor : YesOrNoProcessor
    {
        public FamilyProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }

        protected override IEnumerable<string> OptionASelected(object arg)
        {
            var context = ApplicationContext.Create();

            return context.Families
                .AsQueryable()
                .OrderByDescending(a => a.Rank)
                .Select(a => a.City)
                .Take(ResultLimit);
        }

        protected override IEnumerable<string> OptionBSelected(object arg)
        {
            yield break;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            var metric = ApplicationContext.Create().Families.AsQueryable()
      .Select(a => a.City)
      .ToArray();

            var toHandle = ApplicationContext.Create().Cities.AsQueryable()
                                            .Select(a => a.City).ToArray();

            var unhandled = toHandle.Except(metric, StringComparer.OrdinalIgnoreCase);
            var exceed =
                metric.Except(toHandle, StringComparer.OrdinalIgnoreCase);

            return unhandled.Union(exceed).Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }

    public abstract class YesOrNoProcessor : AppProcessorBase
    {
        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("a", OptionASelected);
            dict.Add("b", OptionBSelected);
            return dict;
        }

        protected abstract IEnumerable<string> OptionASelected(object arg);
        protected abstract IEnumerable<string> OptionBSelected(object arg);


        public YesOrNoProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }
    }
}