using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            throw new NotImplementedException();
        }


    }
}
