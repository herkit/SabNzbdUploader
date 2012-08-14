using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Dynamic.Json;
using Dynamic.Duck;
using System.Configuration;

namespace Arasoft.SabNzdbUploader.Core.Api
{
    public class SabNzbdApi
    {
        public Func<string, string> HttpGet = (url) =>
        {
            return string.Empty;
        };

        public List<string> GetCategories()
        {
            var d = JObject.Parse(HttpGet(BuildUrl("api?mode=get_cats&output=json")));
            
            dynamic data = d.AsDynamic();


            List<string> categories = new List<string>();

            foreach (var category in data.categories)
            {
                categories.Add(category);
            }

            return categories;
        }

        private string BuildUrl(string resource)
        {
            var rooturl = ConfigurationManager.AppSettings["sabRootUrl"];
            var apikey = ConfigurationManager.AppSettings["sabApiKey"];

            return String.Format("{0}{1}&apikey={2}", rooturl, resource, apikey);
        }

    }
}
