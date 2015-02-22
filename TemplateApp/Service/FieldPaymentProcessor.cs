using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using TemplateApp.DAO;

namespace TemplateApp.Service
{
    public class FieldPaymentProcessor : AppProcessorBase
    {
        public FieldPaymentProcessor(string value, int resultLimit)
            : base(value, resultLimit)
        {
        }
        //protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        //{
        //    var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
        //    dict.Add("NL", a => GeneralPreference(a, "NL"));
        //    dict.Add("PE", a => GeneralPreference(a, "PE"));
        //    dict.Add("NS", a => GeneralPreference(a, "NS"));
        //    dict.Add("NB", a => GeneralPreference(a, "NB"));
        //    dict.Add("QC", a => GeneralPreference(a, "QC"));
        //    dict.Add("ON", a => GeneralPreference(a, "ON"));
        //    dict.Add("MB", a => GeneralPreference(a, "MB"));
        //    dict.Add("SK", a => GeneralPreference(a, "SK"));
        //    dict.Add("AB", a => GeneralPreference(a, "AB"));
        //    dict.Add("BC", a => GeneralPreference(a, "BC"));
        //    dict.Add("YT", a => GeneralPreference(a, "YT"));
        //    dict.Add("NT", a => GeneralPreference(a, "NT"));
        //    dict.Add("NU", a => GeneralPreference(a, "NU"));
        //    return dict;
        //}

        protected override Dictionary<string, Func<object, IEnumerable<string>>> ValueFactory()
        {
            var dict = new Dictionary<string, Func<object, IEnumerable<string>>>();
            dict.Add("1", AcommodationAndFoodPreference);
            dict.Add("2", AgriculturePreference);
            dict.Add("3", BusinessBuildingSupportServicesPreference);
            dict.Add("4", ConstructionPreference);
            dict.Add("5", EducationServicePreference);
            dict.Add("6", FinanceInsuranceRealStatePreference);
            dict.Add("7", ForestryFishingMiningQuarryingOilGasPreference);
            dict.Add("8", GoodsProducingSectorPreference);
            dict.Add("9", HeathCareSocialPreference);
            dict.Add("10", InformationCultureRecreationPreference);
            dict.Add("11", ManufactoringPreference);
            dict.Add("12", OtherServicesPeference);
            dict.Add("13", ProfissionScientificTechnicalPreference);
            dict.Add("14", PublicAdministrationPreference);
            dict.Add("15", ServiceProducingPreference);
            dict.Add("16", TradePreference);
            dict.Add("17", TransportationWarehousingPreference);
            dict.Add("18", UtilitiesPreference);
            
            return dict;
        }

        public override IEnumerable<string> GetNonParticipatingCities()
        {
            yield break;
        }

        private IEnumerable<string> UtilitiesPreference(object arg)
        {
            return GeneralPreference(arg, "Utilities");
        }

        private IEnumerable<string> TransportationWarehousingPreference(object arg)
        {
            return GeneralPreference(arg, "Transportation and warehousing");
        }

        private IEnumerable<string> TradePreference(object arg)
        {
            return GeneralPreference(arg, "Trade");
        }

        private IEnumerable<string> ServiceProducingPreference(object arg)
        {
            return GeneralPreference(arg, "Services-producing sector");
        }

        private IEnumerable<string> PublicAdministrationPreference(object arg)
        {
            return GeneralPreference(arg, "Public administration");
        }

        private IEnumerable<string> ProfissionScientificTechnicalPreference(object arg)
        {
            return GeneralPreference(arg, "Professional, scientific and technical services");
        }

        private IEnumerable<string> OtherServicesPeference(object arg)
        {
            return GeneralPreference(arg, "Other services");
        }

        private IEnumerable<string> ManufactoringPreference(object arg)
        {
            return GeneralPreference(arg, "Manufacturing");
        }

        private IEnumerable<string> InformationCultureRecreationPreference(object arg)
        {
            return GeneralPreference(arg, "Information, culture and recreation");
        }

        private IEnumerable<string> HeathCareSocialPreference(object arg)
        {
            return GeneralPreference(arg, "Health care and social assistance");
        }

        private IEnumerable<string> GoodsProducingSectorPreference(object arg)
        {
            return GeneralPreference(arg, "Goods-producing sector");
        }

        private IEnumerable<string> ForestryFishingMiningQuarryingOilGasPreference(object arg)
        {
            return GeneralPreference(arg, "Forestry, fishing, mining, quarrying, oil and gas");
        }

        private IEnumerable<string> FinanceInsuranceRealStatePreference(object arg)
        {
            return GeneralPreference(arg, "Finance, insurance, real estate and leasing");
        }

        private IEnumerable<string> EducationServicePreference(object arg)
        {
            return GeneralPreference(arg, "Educational services");
        }

        private IEnumerable<string> ConstructionPreference(object arg)
        {
            return GeneralPreference(arg, "Construction");
        }

        private IEnumerable<string> BusinessBuildingSupportServicesPreference(object arg)
        {
            return GeneralPreference(arg, "Business, building and other support services");
        }

        private IEnumerable<string> AgriculturePreference(object arg)
        {
            return GeneralPreference(arg, "Agriculture");
        }

        private IEnumerable<string> AcommodationAndFoodPreference(object arg)
        {
            return GeneralPreference(arg, "Accommodation and food services");
        }

        private IEnumerable<string> GeneralPreference(object extraArg, string pref)
        {
            var ctx = ApplicationContext.Create();

            return ctx.IndustryJobs.AsQueryable()
                                    .Where(a => a.INDUSTRY == pref)
                                    .OrderByDescending(a => a.Value)
                                    .Select(a => a.GEOGRAPHY);
        }

        //private IEnumerable<string> GeneralPreference(object extraArg, string pref)
        //{
        //    var ctx = ApplicationContext.Create();
        //    var provName =
        //    ctx.Provinces.AsQueryable().FirstOrDefault(a => a.ProvinceCode == pref);

        //    if (provName ==null)
        //        throw new ArgumentNullException("provName");

        //    ctx.IndustryJobs.AsQueryable()
                
        //}

    }
}