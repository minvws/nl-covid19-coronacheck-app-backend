﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System.Security.Cryptography.X509Certificates;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Certificates;

public interface ICertificateProvider
{
    X509Certificate2 GetCertificate();
}
