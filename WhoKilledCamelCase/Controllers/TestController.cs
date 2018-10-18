using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace WhoKilledCamelCase.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("config")]
        public object Config()
        {
            return Content(HttpStatusCode.OK, new
            {
                GlobalConfiguration.Configuration.Formatters.JsonFormatter,
                GlobalConfiguration.Configuration.Formatters.XmlFormatter,
                GlobalConfiguration.Configuration.Formatters.FormUrlEncodedFormatter,
                GlobalConfiguration.Configuration.Formatters,
            }, new JsonMediaTypeFormatter()
            {
                SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                }
            });
        }

        private object GetData()
        {
            return new
            {
                var1 = "value",
                Var2 = "other value",
                Var3 = new
                {
                    childVar = "another value",
                    ChildVar2 = new object[]
                    {
                        new { x = "y" },
                        new { X = "z" },
                    },
                },
            };
        }

        [HttpGet]
        [Route("fromConfig")]
        public object FromConfig()
        {
            return GetData();
        }

        [HttpGet]
        [Route("asCamel")]
        public object AsCamel()
        {
            return Content(HttpStatusCode.OK, GetData(), new JsonMediaTypeFormatter()
            {
                SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                }
            });
        }

        [HttpGet]
        [Route("asDefault")]
        public object AsDefault()
        {
            return Content(HttpStatusCode.OK, GetData(), new JsonMediaTypeFormatter()
            {
                SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver(),
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                }
            });
        }

        [HttpGet]
        [Route("setCamel")]
        public object SetCamel()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return Content(HttpStatusCode.OK, "OK");
        }

        [HttpGet]
        [Route("setDefault")]
        public object SetDefault()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver();
            return Content(HttpStatusCode.OK, "OK");
        }
    }
}
