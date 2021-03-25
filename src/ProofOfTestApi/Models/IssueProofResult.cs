﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System.Text.Json.Serialization;
using NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Web.Models;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.ProofOfTestApi.Models
{
    public class IssueProofResult
    {
        [JsonPropertyName("ism")] public IssueSignatureMessage Ism { get; set; }
        [JsonPropertyName("attributes")] public string[] Attributes { get; set; }
    }
}
