// -----------------------------------------------------------------------
//  <copyright file="ActionInputs.cs" company="Prism">
//  Copyright (c) Prism. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using CommandLine;

namespace Prism.GithubActions.SmokeTest;

public class ActionInputs
{
    [Option('s', "sitemap",
        Required = true,
        HelpText = "The URL of the base sitemap")]
    public string Sitemap { get; set; } = null!;
}