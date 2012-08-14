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
            using (var httpclient = new System.Net.WebClient())
            {
                return httpclient.DownloadString(url);
            }
        };

        public Func<string, string, string> HttpSendFile = (url, file) =>
        {
            using (var httpclient = new System.Net.WebClient())
            {
                return System.Text.Encoding.ASCII.GetString(httpclient.UploadFile(url, file));
            }
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

        public void UploadFile(System.IO.FileInfo file, string category)
        {
            var resource = BuildUrl(String.Format("api?mode=addfile&name={0}&cat={1}", file.Name, category));
            HttpSendFile(resource, file.FullName);
        }

        private string BuildUrl(string resource)
        {
            return String.Format("{0}{1}&apikey={2}", SabRootUrl, resource, SabApiKey);
        }

        private string SabRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["sabRootUrl"];
            }
        }

        private string SabApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["sabApiKey"];
            }
        }


    }
}
