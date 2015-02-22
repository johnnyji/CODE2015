using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TemplateApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            // name: "Controller",
            // url: "{controller}/{action}/{id}",
            // defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
            // constraints: new { controller = new HtmlFinderConstraint() });

            routes.MapRoute(
         name: "ControllerV2",
         url: "{action}/{id}",
         defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
         constraints: new { controller = new HtmlFinderConstraint() });

            //routes.MapPageRoute("HomePage", "", "~/Default");
            routes.MapPageRoute("RemoveDotHtml", "{*file}", "~/{file}.html", true);
        }

        public class HtmlFinderConstraint : IRouteConstraint, IDisposable
        {
            private readonly string _objectKey = Guid.NewGuid().ToString();

            private readonly ThreadLocal<bool> _reentry;
            private IReadOnlyDictionary<string, bool> _cacheRoute;
            public bool CloseByReentrancy
            {
                get 
                {
                    var value = CallContext.LogicalGetData(_objectKey);
                    if (value == null)
                        return _reentry.Value;

                    return (bool)value || _reentry.Value;
                }
                set 
                {
                    CallContext.LogicalSetData(_objectKey, value);
                    _reentry.Value = true; 
                }
            }


            public HtmlFinderConstraint()
            {
                this._cacheRoute = new ReadOnlyDictionary<string, bool>(new Dictionary<string, bool>());
                this._reentry = new ThreadLocal<bool>();
            }

            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                object parameterValue;
                string controllerText;
                if (!values.TryGetValue(parameterName, out parameterValue))
                    return false;
           
                controllerText = parameterValue as string;

                if (controllerText == null)
                    return false;

                bool res;
                if (_cacheRoute.TryGetValue(controllerText, out res))
                    return res;

                if (CloseByReentrancy)
                    return true;

                CloseByReentrancy = true;
                try
                {
                    var factory = ControllerBuilder.Current.GetControllerFactory();
                    var routeData = route.GetRouteData(httpContext);

                    bool result = false;
                    if (routeData != null)
                    {
                        try
                        {
                            var controller = factory.CreateController(new RequestContext(httpContext, routeData), controllerText);

                            var disp = controller as IDisposable;
                            if (disp != null)
                                disp.Dispose();

                            result = true;
                        }
                        catch (HttpException ex)
                        {
                            if (ex.HResult != -2147467259) // controller not found
                                throw;
                        }
                    }

                    if (_cacheRoute.TryGetValue(controllerText, out res))
                        return res;

                    while (true)
                    {
                        var old = _cacheRoute;
                        var newDict = old.ToDictionary(a => a.Key, b => b.Value);
                        newDict[controllerText] = result;
                        var readOnly = new ReadOnlyDictionary<string, bool>(newDict);
                        var current = Interlocked.CompareExchange(ref _cacheRoute, readOnly, old);

                        if (current == old)
                            break;
                    }

                    return result;
                }
                finally
                {
                    CloseByReentrancy = false;
                }


                //var acc = this._servicesContainer.GetAssembliesResolver().GetAssemblies();
                //var found = acc.SelectMany(a => a.GetTypes())
                //                .Where(type => typeof(Controller).IsAssignableFrom(type))
                //                .Any(a => a.Name.Equals(controllerText, StringComparison.OrdinalIgnoreCase));


                //var controller = selector.SelectController(message);
                // var action = values["action"] as string;
                //var controller = values["controller"] as string;

                //return found;

                //var controllerFullName = string.Format("MvcApplication1.Controllers.{0}Controller", controller);
                //var cont = Assembly.GetExecutingAssembly().GetType(controllerFullName);
                //return cont != null && cont.GetMethod(action) != null;
            }

            public void Dispose()
            {
                _reentry.Dispose();
            }
        }
    }
}