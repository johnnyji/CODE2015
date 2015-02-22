using System.Web.Mvc;

namespace TemplateApp
{
    public class FilterConfig
    {
        internal static void RegisterGlobalFilters(System.Web.Mvc.GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}