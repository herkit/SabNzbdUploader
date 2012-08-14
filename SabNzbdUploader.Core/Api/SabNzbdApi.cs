using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Dynamic.Json;
using Dynamic.Duck;

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
            var d = JObject.Parse(HttpGet(""));
            
            dynamic data = d.AsDynamic();


            List<string> categories = new List<string>();

            foreach (var category in data.categories)
            {
                categories.Add(category);
            }

            return categories;
        }


    }
}
