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

            api_returns = "{\"categories\":[\"*\",\"movie\"]}";

            List<string> categories = api.GetCategories();
            Assert.AreEqual(2, categories.Count);
            Assert.AreEqual("*", categories[0]);
            Assert.AreEqual("movie", categories[1]);
        }

        [Test]
        public void Should_get_categories_from_correct_url()
        {
            string called_url = "";
            var api = new Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi()
            {
                HttpGet = (url) =>
                {
                    called_url = url;
                    return string.Empty;
                }
            };

        }

        [SetUp]
        public void Setup()
        {
            var api = new Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi();

            api.HttpGet = (url) =>
            {
                called_url = url;
                return api_returns;
            };
        }

        string called_url;
        string api_returns;
        Arasoft.SabNzdbUploader.Core.Api.SabNzbdApi api;
    }
}
