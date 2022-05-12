﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Services;

public class StandardUtcDateTimeProvider : IUtcDateTimeProvider
{
    public StandardUtcDateTimeProvider()
    {
        Snapshot = DateTime.UtcNow;
    }

    /// <summary>
    ///     Time of start of transaction scope
    /// </summary>
    public DateTime Snapshot { get; }
}
