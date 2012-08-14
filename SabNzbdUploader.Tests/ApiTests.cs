using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Arasoft.SabNzdbUploader.Core.Api;

namespace SabNzbdUploader.Tests
{
    [TestFixture]
    public class ApiTests
    {
        [Test]
        public void GetCategories()
        {
            api_returns = "{\"categories\":[\"*\",\"movie\"]}";

            List<string> categories = api.GetCategories();
            Assert.AreEqual(2, categories.Count);
            Assert.AreEqual("*", categories[0]);
            Assert.AreEqual("movie", categories[1]);
        }

        [Test]
        public void Should_get_categories_from_correct_url()
        {
            api_returns = "{\"categories\":[\"*\",\"movie\"]}";

            api.GetCategories();

            Assert.AreEqual("http://localhost:8080/api?mode=get_cats&output=json&apikey=theapikey", called_url);
        }

        [SetUp]
        public void Setup()
        {
            api = new SabNzbdApi();

            api.HttpGet = (url) =>
            {
                called_url = url;
                return api_returns;
            };
        }

        string called_url;
        string api_returns;
        SabNzbdApi api;
    }
}
