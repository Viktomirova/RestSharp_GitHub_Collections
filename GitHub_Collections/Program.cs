using GitHub_Collections;

using RestSharp;
using RestSharp.Authenticators;

using System;
using System.Collections.Generic;
using System.Text.Json;

UserData user = new UserData();
var client = new RestClient("http://api.github.com");
client.Authenticator = new HttpBasicAuthenticator(user.UserName, user.Password);

string repo_url = "/users/{user}/repos";
var repoRequest = new RestRequest(repo_url);
repoRequest.AddUrlSegment("user", user.UserName);
var repoResponse = await client.ExecuteAsync(repoRequest);
List<Repo> repos = JsonSerializer.Deserialize<List<Repo>>(repoResponse.Content);
Console.WriteLine($"StatusCode: {repoResponse.StatusCode}");
Console.WriteLine($"Body: {repoResponse.Content}");
foreach (var item in repos)
{
    Console.WriteLine($"Repo Id: {item.id}");
    Console.WriteLine($"Repo Name: {item.name}");
    Console.WriteLine($"Repo Name: {item.html_url}");
}

string issue_url = "/repos/{user}/Postman/issues";
var issueRequest = new RestRequest(issue_url);
issueRequest.AddUrlSegment("user", user.UserName);
issueRequest.AddBody(new { title = "Issue From RestSharp" });
var issueResponse = await client.ExecuteAsync(issueRequest, Method.Post);
Console.WriteLine("Status Code " + issueResponse.ResponseStatus);
List<Issue> issues = JsonSerializer.Deserialize<List<Issue>>(issueResponse.Content);

foreach (var issue in issues)
{
    Console.WriteLine($"Issue Number: {issue.number}");
    Console.WriteLine($"Issue Name: {issue.title}");
    Console.WriteLine($"Issue ID: {issue.id}");
}
Console.WriteLine($"Issues Total Count: {issues.Count}");