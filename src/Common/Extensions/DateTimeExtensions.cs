﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    ///     Returns TRUE if [comparisionDate] - [nHours] &lt; [date] &lt; [comparisionDate]
    /// </summary>
    public static bool LessThanNHoursBefore(this DateTime date, int nHours, DateTime comparisionDate)
    {
        var from = comparisionDate.AddHours(-1 * nHours);

        return date >= from && date <= comparisionDate;
    }

    /// <summary>
    ///     Returns TRUE if the date-time precision is to the hour
    /// </summary>
    public static bool IsHourPrecision(this DateTime date)
    {
        return date.Minute == 0 && date.Second == 0 && date.Millisecond == 00;
    }

    /// <summary>
    ///     Returns a new DateTime with hour precision using FLOOR()
    /// </summary>
    public static DateTime ToHourPrecision(this DateTime date)
    {
        return date.Date.AddHours(date.Hour);
    }

    /// <summary>
    ///     Converts an UTC Date into the UnixTime.
    /// </summary>
    public static long ToUnixTime(this DateTime value)
    {
        return Convert.ToInt64(Math.Floor((value - DateTimeOffset.UnixEpoch).TotalSeconds));
    }
}
