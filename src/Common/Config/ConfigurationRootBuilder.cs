﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Config
{
    public static class ConfigurationRootBuilder
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public static IConfigurationRoot Build()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrWhiteSpace(environmentName))
                environmentName = "Development";

            var baseDirectory = Directory.GetParent(AppContext.BaseDirectory);

            if (baseDirectory == null)
                throw new InvalidOperationException($"No parent directory for {AppContext.BaseDirectory}");

            return new ConfigurationBuilder()
                  .SetBasePath(baseDirectory.FullName)
                  .AddJsonFile("appsettings.json", false)
                  .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                  .Build();
        }
    }
}
