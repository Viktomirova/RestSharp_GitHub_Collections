using GitHub_Collections;

using NUnit.Framework;

using RestSharp;
using RestSharp.Authenticators;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test_GitHubApi
{
    public class Tests
    {
        private RestClient client;
        private RestRequest request;
        private RestResponse response;
        private UserData user;

        [SetUp]
        public void Setup()
        {
            client = new RestClient("http://api.github.com");
            string url = "/repos/Viktomirova/Postman/issues";
            request = new RestRequest(url);
            response = new RestResponse();
            user = new UserData();
            client.Authenticator = new HttpBasicAuthenticator(user.UserName, user.Password);

        }

        //static async Task<Issue> CreateIssue(string title, string body)
        //{
        //    UserData user = new UserData();
        //    var client = new RestClient("http://api.github.com");
        //    client.Authenticator = new HttpBasicAuthenticator(user.UserName, user.Password);
        //    string issue_url = "/repos/{user}/Postman/issues";
        //    var issueRequest = new RestRequest(issue_url);
        //    issueRequest.AddUrlSegment("user", user.UserName);
        //    var issueResponse = await client.ExecuteAsync(issueRequest, Method.Post);
        //    var issue = JsonSerializer.Deserialize<Issue>(issueResponse.Content);
        //    return issue;
        //}

        public async Task<Issue> CreateIssue(string title, string body)
        {
            string issue_url = "/repos/{user}/Postman/issues";
            var issueRequest = new RestRequest(issue_url);
            //issueRequest.AddUrlSegment("user", user.UserName);
            issueRequest.AddBody(new { title, body });
            var issueResponse = await client.ExecuteAsync(issueRequest, Method.Post);
            Console.WriteLine("Status Code " + issueResponse.ResponseStatus);
            var issue = JsonSerializer.Deserialize<Issue>(issueResponse.Content);
            return issue;
        }

        [Test]
        public async Task Test_Get_Issue()
        {
            var response = await client.ExecuteAsync(request);

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            // var issues = new SystemTextJsonSerializer().Deserialize<List<Issue>>(response);
            // var issues = JsonConvert.DeserializeObject<List<Issue>>(response.Content);

            foreach (var issue in issues)
            {
                Assert.IsNotNull(issue.html_url);
                Assert.IsNotNull(issue.id, "Issue id must not be null");
            }

            Assert.IsNotNull(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Test_Get_AllIssues()
        {
            response = await client.ExecuteAsync(request);
            Assert.IsNotNull(response.Content);
            List<Issue> issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);
            Assert.That(issues.Count > 1);
            foreach (Issue issue in issues)
            {
                Assert.Greater(issue.id, 0);
                Assert.Greater(issue.number, 0);
                Assert.IsNotEmpty(issue.title);
            }
        }


        [Test]
        public async Task Test_Create_NewIssue()
        {
            response = await client.ExecuteAsync(request);
            string title = "New Issue from RestSharp";
            string body = "Some question";
            var newIssue = await CreateIssue(title, body);

            Assert.IsNotEmpty(newIssue.title);

        }
    }
}