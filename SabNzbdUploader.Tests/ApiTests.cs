using System;
using System.Collections.Generic;
using NUnit.Framework;
using Arasoft.SabNzdbUploader.Core.Api;
using System.IO;

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

            Assert.AreEqual("http://the_url/api?mode=get_cats&output=json&apikey=the_key", called_url);
        }

        [Test]
        public void Upload_file()
        {
            api_returns = "{\"status\":true}";
            api.UploadFile(new FileInfo(@"c:\folder\the_file"), "the_category");

            Assert.AreEqual("http://the_url/api?mode=addfile&cat=the_category&output=json&apikey=the_key", called_url);
            Assert.AreEqual(@"c:\folder\the_file", uploaded_file);
        }

        [Test]
        public void Upload_file_returns_true()
        {
            api_returns = "{\"status\":true}";
            var result = api.UploadFile(new FileInfo(@"c:\folder\the_file"), "the_category");

            Assert.AreEqual(true, result);
        }

        [SetUp]
        public void Setup()
        {
            api = new SabNzbdApi("http://the_url/", "the_key");

            api.HttpGet = (url) =>
            {
                called_url = url;
                return api_returns;
            };

            api.HttpSendFile = (url, file) =>
            {
                called_url = url;
                uploaded_file = file;
                return api_returns;
            };

            api_returns = "";
        }

        string called_url;
        string api_returns;
        SabNzbdApi api;
        string uploaded_file;
    }
}
