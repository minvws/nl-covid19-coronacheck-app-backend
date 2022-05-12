﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Config;

[Serializable]
public class MissingConfigurationValueException : Exception
{
    public MissingConfigurationValueException(string name) : base($"Missing value: {name}.")
    {
    }
}
