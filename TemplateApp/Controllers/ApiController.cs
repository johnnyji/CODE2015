using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TemplateApp.Models;

namespace TemplateApp.Controllers
{
    public class ApiTestController : ApiController
    {
        [HttpGet]
        public int SimpleGetHitTest()
        {
            return 1;
        }

        public HttpResponseMessage SimplePostFoo(Foo f)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage SimplePostHitTest()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
