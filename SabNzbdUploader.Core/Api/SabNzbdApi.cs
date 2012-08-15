using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Dynamic.Json;
using Dynamic.Duck;
using System.Configuration;
using System.Web;
using System.IO;
using System.Globalization;

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
            using (var request = new System.Net.WebClient())
            {

                var fi = new FileInfo(file);

                StringBuilder sb = new StringBuilder();
                
                string boundary = string.Format("{0}", DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture));
                string b = string.Format("--------{0}", boundary);
                
                sb.AppendFormat("--{0}", b);
                sb.Append(Environment.NewLine);
                sb.AppendFormat("Content-Disposition: form-data; name=\"nzbfile\"; filename=\"{0}\"", Path.GetFileName(fi.Name));
                sb.Append(Environment.NewLine);
                sb.AppendFormat("Content-Type: {0}", "application/octet-stream");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                
                byte[] bufferHeader = Encoding.ASCII.GetBytes(sb.ToString());
                byte[] bufferFooter = Encoding.ASCII.GetBytes(
                    string.Format("\r\n--{0}--\r\n", b)
                );
                
                using (var ms = new MemoryStream())
                {
                    using (Stream fs = File.Open(fi.FullName, FileMode.Open))
                    {
                        ms.Write(bufferHeader, 0, bufferHeader.Length);
                        fs.CopyTo(ms);
                        ms.Write(bufferFooter, 0, bufferFooter.Length);
                    }
                    request.Headers.Add(string.Format("Content-Type: multipart/form-data; boundary={0}", b));
                    return Encoding.ASCII.GetString(request.UploadData(new Uri(url), "POST", ms.ToArray()));
                    
                }

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
            var resource = BuildUrl(String.Format("api?mode=addfile&cat={1}&output=json", Uri.EscapeUriString(file.Name), category));
            var result = HttpSendFile(resource, file.FullName);
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
