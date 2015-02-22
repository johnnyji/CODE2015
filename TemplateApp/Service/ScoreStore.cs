using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;

namespace TemplateApp.Service
{
    public class ScoreStore
    {
        private Dictionary<Tuple<string, string>, double> _score;
        public ScoreStore()
        {
            _score = new Dictionary<Tuple<string, string>, double>();

            var cur = ApplicationContext.Create().Cities.FindAll();

            foreach (var item in cur)
            {
                _score[new Tuple<string, string>(item.Province, item.City)] = 0;
            }
        }

        internal Dictionary<string, string> Split(string[] p)
        {
            var questions = p.Select((a, i) => new { a, i }).Where(obj => obj.i % 2 == 0).Select(a => a.a);
            var answers = p.Select((a, i) => new { a, i }).Where(obj => obj.i % 2 != 0).Select(a => a.a).ToArray();

            var dict = questions.Select((a, i) => new KeyValuePair<string, string>(a, answers[i])).ToDictionary(a => a.Key, b => b.Value);
            return dict;
        }

        private double weatherTopMark = 100;
        private Func<double, double> weatherThresold = a => a - 10;
        internal void AddWeatherWinners(string[] res, IEnumerable<string> nonParticipants)
        {
            foreach (var item in res)
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += weatherTopMark;
                weatherTopMark = weatherThresold(weatherTopMark);
            }

            if (nonParticipants.Any())
                throw new NotImplementedException("nonParticipants");
        }


        private double familyTopMark = 100;
        private Func<double, double> familyThresold = a => a - 10;
        //private bool familyImposeValueForBlanks = false;
        //private double familyBlankMarkValue = 0;
        internal void AddFamilyWinners(string[] res, IEnumerable<string> nonParticipants)
        {
            var exclude = nonParticipants.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += familyTopMark;
                familyTopMark = familyThresold(familyTopMark);
            }


            HandleNonParticipants(exclude, familyTopMark);
        }

        private double entrepreneusTopMark = 100;
        private Func<double, double> entrepreneusThresold = a => a - 10;
        //private bool entrepreneusImposeValueForBlanks = false;
        //private double blankMarkValue = 0;
        internal void AddEntrepreneusWinners(string[] res, IEnumerable<string> nonParticipants)
        {
            var exclude = nonParticipants.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += entrepreneusTopMark;
                entrepreneusTopMark = entrepreneusThresold(entrepreneusTopMark);
            }

            HandleNonParticipants(exclude, entrepreneusTopMark);
        }

        private void HandleNonParticipants(IBuffer<string> exclude, double specificValue)
        {
            if (exclude.Any())
            {
                foreach (var item in exclude)
                {
                    var province = MatchProvince(item);
                    var tuple = Tuple.Create(province, item);
                    if (_score.ContainsKey(tuple))
                    {
                        _score[new Tuple<string, string>(MatchProvince(item), item)] += specificValue;
                    }
                }
            }
        }

        private double walkScoreTopMark = 100;
        private Func<double, double> walkScoreThresold = a => a - 10;
        //private bool  walkScoreImposeValueForBlanks = false;
        //private double  walkScoreblankMarkValue = 0;
        public void AddWalkScoreWinners(string[] res, IEnumerable<string> nonParticipants)
        {
            var exclude = nonParticipants.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += walkScoreTopMark;
                walkScoreTopMark = walkScoreThresold(walkScoreTopMark);
            }

            HandleNonParticipants(exclude, walkScoreTopMark);
        }

        public string[] MatchCity(string province)
        {
            return
                ApplicationContext.Create()
                    .Cities.AsQueryable()
                    .Where(a => a.Province == province)
                    .Select(a => a.City)
                    .ToArray();
        }

        public string MatchProvince(string city)
        {
            return
                ApplicationContext.Create()
                    .Cities.AsQueryable()
                    .Where(a => a.City == city)
                    .Select(a => a.Province)
                    .FirstOrDefault();
        }

        public IEnumerable<Tuple<string, string>> GetResult()
        {
            return _score.OrderByDescending(a => a.Value).Select(a => a.Key);
        }

        private double activiesScoreTopMark = 100;
        private Func<double, double> activiesScoreThresold = a => a - 10;
        //private bool  activiesScoreImposeValueForBlanks = false;
        //private double  activiesScoreblankMarkValue = 0;
        internal void AddActivitiesWinners(string[] res, IEnumerable<string> nonParticipating)
        {
            var exclude = nonParticipating.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += activiesScoreTopMark;
                activiesScoreTopMark = activiesScoreThresold(activiesScoreTopMark);
            }

            HandleNonParticipants(exclude, activiesScoreTopMark);
        }

        private double studentScoreTopMark = 100;
        private Func<double, double> studentScoreThresold = a => a - 10;
        //private bool  studentScoreImposeValueForBlanks = false;
        //private double  studentScoreblankMarkValue = 0;
        internal void AddStudentWinners(string[] res, IEnumerable<string> nonParticipating)
        {
            var exclude = nonParticipating.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += studentScoreTopMark;
                studentScoreTopMark = studentScoreThresold(studentScoreTopMark);
            }

            HandleNonParticipants(exclude, studentScoreTopMark);
        }

        private double outgoingScoreTopMark = 100;
        private Func<double, double> outgoingScoreThresold = a => a - 10;
        //private bool  outgoingScoreImposeValueForBlanks = false;
        //private double  outgoingScoreblankMarkValue = 0;
        internal void AddOutGoingWinners(string[] res, IEnumerable<string> nonParticipating)
        {
            var exclude = nonParticipating.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += outgoingScoreTopMark;
                outgoingScoreTopMark = outgoingScoreThresold(outgoingScoreTopMark);
            }

            HandleNonParticipants(exclude, outgoingScoreTopMark);
        }

        private double fieldIndustryScoreTopMark = 100;
        private Func<double, double> fieldIndustryScoreThresold = a => a - 10;
        //private bool  fieldIndustryScoreImposeValueForBlanks = false;
        //private double  fieldIndustryScoreblankMarkValue = 0;
        internal void AddFieldIndustryPaymentWinners(string[] res, IEnumerable<string> nonParticipating)
        {
            var exclude = nonParticipating.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += fieldIndustryScoreTopMark;
                fieldIndustryScoreTopMark = fieldIndustryScoreThresold(fieldIndustryScoreTopMark);
            }

            HandleNonParticipants(exclude, fieldIndustryScoreTopMark);
        }

        private double employmentOpportScoreTopMark = 100;
        private Func<double, double> employmentOpportScoreThresold = a => a - 10;
        //private bool  employmentOpportScoreImposeValueForBlanks = false;
        //private double  employmentOpportScoreblankMarkValue = 0;
        internal void AddEmploymentOpportunitiesWinners(string[] res, IEnumerable<string> nonParticipating)
        {
            var exclude = nonParticipating.Memoize();
            foreach (var item in res.Except(exclude))
            {
                _score[new Tuple<string, string>(MatchProvince(item), item)] += employmentOpportScoreTopMark;
                employmentOpportScoreTopMark = employmentOpportScoreThresold(employmentOpportScoreTopMark);
            }

            HandleNonParticipants(exclude, employmentOpportScoreTopMark);
        }
    }
}