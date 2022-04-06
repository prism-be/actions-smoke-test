// -----------------------------------------------------------------------
//  <copyright file="SmokeTester.cs" company="Prism">
//  Copyright (c) Prism. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Xml;
using Microsoft.Extensions.Logging;

namespace Prism.GithubActions.SmokeTest;

public class SmokeTester
{
    private readonly ILogger<SmokeTester> logger;
    private readonly HttpClient httpClient;

    public SmokeTester(HttpClient httpClient, ILogger<SmokeTester> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task Run(ActionInputs inputs)
    {
        logger.LogInformation("Start processing the sitemap : {sitemap}", inputs.Sitemap);
        var sitemapContent = await httpClient.GetStringAsync(inputs.Sitemap);

        var document = new XmlDocument();
        document.LoadXml(sitemapContent);

        var nodes = document.SelectNodes("//*[local-name()='loc']");

        if (nodes == null)
        {
            logger.LogCritical("No url found in sitemap");
            return;
        }

        foreach (XmlNode node in nodes)
        {
            await Test(node.InnerText);
        }
    }

    private async Task Test(string url)
    {
        logger.LogInformation("Smoke Testing Url : {url}", url);
        var pageContent = await httpClient.GetStringAsync(url);
        logger.LogInformation("Smoke Tested Url : {url} - Content Length : {contentLength}", url, pageContent.Length);
    }
}