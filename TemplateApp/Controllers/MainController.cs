using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Management;
using TemplateApp.Models;
using TemplateApp.Properties;
using TemplateApp.Service;

namespace TemplateApp.Controllers
{
    public class MainController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ShowResult([FromUri]  string[] p)
        {
            if (p == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            if (p.Length % 2 != 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var s = new ScoreStore();

            var dict = s.Split(p);

            foreach (var item in GetProcessors(dict))
            {
                var process = item.Item1;
                var selectedValue = item.Item2;

                if (selectedValue.IsNothing())
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var res = process.GetResult().ToArray();

                if (res.Length == 0)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                AddAnswer(s, process.GetType(), res, process.GetNonParticipatingCities());
            }

            var final = s.GetResult();
            return Request.CreateResponse(final);
        }

        private void AddAnswer(ScoreStore store, Type type, string[] res, IEnumerable<string> nonParticipating)
        {
            if (type == typeof(WeatherProcessor))
            {
                store.AddWeatherWinners(res, nonParticipating);
            }
            else if (type == typeof(WalkTransitProcessor))
            {
                store.AddWalkScoreWinners(res, nonParticipating);
            }
            else if (type == typeof(FamilyProcessor))
            {
                store.AddFamilyWinners(res, nonParticipating);
            }
            else if (type == typeof(EntrepreneusProcessor))
            {
                store.AddEntrepreneusWinners(res, nonParticipating);
            }
            else if (type == typeof(ActivitiesProcessor))
            {
                store.AddActivitiesWinners(res, nonParticipating);
            }
            else if (type == typeof(StudentProcessor))
            {
                store.AddStudentWinners(res, nonParticipating);
            }
            else if (type == typeof(OutgoingProcessor))
            {
                store.AddOutGoingWinners(res, nonParticipating);
            }
            else if (type == typeof(FieldPaymentProcessor))
            {
                store.AddFieldIndustryPaymentWinners(res, nonParticipating);
            }
            else if (type == typeof(LookingForJob))
            {
                store.AddEmploymentOpportunitiesWinners(res, nonParticipating);
            }
            else
            {
                throw new InvalidOperationException("unexpected processor type");
            }
        }

        private IEnumerable<Tuple<AppProcessorBase, IMaybe<string>>> GetProcessors(Dictionary<string, string> dict)
        {
            var selectedValue = GetWeatherSelectedValue(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            AppProcessorBase process = new WeatherProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            selectedValue = GetFamilySelectedValue(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            process = new FamilyProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            selectedValue = GetTransitWalkSelectedValue(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            process = new WalkTransitProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            selectedValue = GetEntrepreusSelectedValue(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            process = new EntrepreneusProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            selectedValue = GetActivitiesSelectedValue(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            process = new ActivitiesProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            selectedValue = GetStudentSelectedValue(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            process = new StudentProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            selectedValue = GetCultureSelectedValue(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            process = new OutgoingProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            selectedValue = GetLookingForJobSelected(dict);
            if (selectedValue.IsNothing())
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            process = new FieldPaymentProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
            yield return Tuple.Create(process, selectedValue);

            if (selectedValue.Ret() == "b")
            {
                selectedValue = GetFieldIndustrySelectedValue(dict);
                if (selectedValue.IsNothing())
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                process = new FieldPaymentProcessor(selectedValue.Ret(), Settings.Default.SearchResultLimit);
                yield return Tuple.Create(process, selectedValue);
            }
        }

        private IMaybe<string> GetFieldIndustrySelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("fi", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }

        private IMaybe<string> GetLookingForJobSelected(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("lo", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }

        private IMaybe<string> GetCultureSelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("ci", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }

        private IMaybe<string> GetStudentSelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("st", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }

        private IMaybe<string> GetActivitiesSelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("ac", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }

        private IMaybe<string> GetEntrepreusSelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("en", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }


        private IMaybe<string> GetTransitWalkSelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("ws", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }

        private IMaybe<string> GetFamilySelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("fa", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }

        //public HttpResponseMessage ResultForTransitScoreChoice(string choice)
        //{
        //    var process = new TransitScoreProcessor(choice, Settings.Default.SearchResultLimit);
        //    var res = process.GetResult();
        //    var evaluatedResult = res.ToArray();

        //    if (evaluatedResult.Length == 0)
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);

        //    return Request.CreateResponse(evaluatedResult);
        //}


        //public HttpResponseMessage ResultForWalkScoreChoice(string choice)
        //{
        //    var process = new WalkScoreProcessor(choice, Settings.Default.SearchResultLimit);
        //    var res = process.GetResult();
        //    var evaluatedResult = res.ToArray();

        //    if (evaluatedResult.Length == 0)
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);

        //    return Request.CreateResponse(evaluatedResult);
        //}


        public HttpResponseMessage ResultForWeatherChoice(string choice)
        {
            var process = new WeatherProcessor(choice, Settings.Default.SearchResultLimit);
            var res = process.GetResult();

            var evaluatedResult = res.ToArray();

            if (evaluatedResult.Length == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            return Request.CreateResponse(evaluatedResult);
        }

        public HttpResponseMessage ResultForJobEarning(string choice)
        {
            var proc = new EarningProcessor(choice, Settings.Default.SearchResultLimit);

            var res = proc.GetResult();

            var evaluatedResult = res.ToArray();

            if (evaluatedResult.Length == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            return Request.CreateResponse(evaluatedResult);
        }

        public HttpResponseMessage ResultForTotalEmployment(string choice)
        {
            var proc = new EmploymentTotalProcessor(choice, Settings.Default.SearchResultLimit);

            var res = proc.GetResult();

            var evaluatedResult = res.ToArray();

            if (evaluatedResult.Length == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            return Request.CreateResponse(evaluatedResult);
        }


        public HttpResponseMessage ResultForIndustryJobChoice(string choice)
        {
            var process = new FieldPaymentProcessor(choice, Settings.Default.SearchResultLimit);
            var res = process.GetResult();

            var evaluatedResult = res.ToArray();

            if (evaluatedResult.Length == 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            return Request.CreateResponse(evaluatedResult);
        }


        private IMaybe<string> GetWeatherSelectedValue(Dictionary<string, string> dict)
        {
            string value;
            if (!dict.TryGetValue("we", out value))
                return Nothing<string>.Default;

            return new Just<string>(value);
        }
    }
}
