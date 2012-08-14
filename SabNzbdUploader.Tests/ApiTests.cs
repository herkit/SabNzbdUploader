using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SabNzbdUploader.Tests
{
    [TestFixture]
    public class ApiTests
    {
        [Test]
        public void GetCategories()
        {
            var api = new Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi();

            api.HttpGet = (url) =>
            {
                return "{\"categories\":[\"*\",\"movie\"]}";
            };
            List<string> categories = api.GetCategories();

        }
    }
}
