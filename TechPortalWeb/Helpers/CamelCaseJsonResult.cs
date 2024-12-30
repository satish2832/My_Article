using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechPortalWeb.Helpers
{
    public class CamelCaseJsonResult : JsonResult
    {
        public CamelCaseJsonResult()
        {
            // Set default JSON settings
            SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
        }

        public JsonSerializerSettings SerializerSettings { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            if (Data != null)
            {
                response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }

                var serializedData = JsonConvert.SerializeObject(Data, SerializerSettings);
                response.Write(serializedData);
            }
        }
    }
}