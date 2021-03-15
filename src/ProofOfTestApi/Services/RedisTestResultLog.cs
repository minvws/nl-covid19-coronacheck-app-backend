﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using NL.Rijksoverheid.CoronaTester.BackEnd.ProofOfTestApi.Config;
using StackExchange.Redis;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NL.Rijksoverheid.CoronaTester.BackEnd.ProofOfTestApi.Services
{
    public class RedisTestResultLog : ITestResultLog, IDisposable
    {
        private readonly IRedisTestResultLogConfig _config;
        private readonly ConnectionMultiplexer _redis;

        public RedisTestResultLog(IRedisTestResultLogConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            // The multiplexor is designed to be a long-life object, it's expensive to build but thread-safe,
            // for now I've tied the lifetime of the instance to this instance.
            // TODO: can we move the LCM to the DI container?
            _redis = ConnectionMultiplexer.Connect(_config.Configuration);
        }

        public async Task<bool> Add(string unique, string providerId)
        {
            var key = CreateUniqueKey(unique, providerId);

            var db = _redis.GetDatabase();

            // Execute the ADD in a transaction; if a key already exists then
            // the transaction will rollback and won't be committed. This
            // ensures that the operation is atomic and thus test results
            // cannot be issued twice.
            var tran = db.CreateTransaction();
            tran.AddCondition(Condition.KeyNotExists(key));
            await tran.StringSetAsync(key, key, TimeSpan.FromHours(_config.Duration));

            return  await tran.ExecuteAsync();
        }

        public async Task<bool> Contains(string unique, string providerId)
        {
            var key = CreateUniqueKey(unique, providerId);
            var db = _redis.GetDatabase();

            var value = await db.StringGetAsync(key);

            // IsNull is TRUE when the key was not found; the documentation on StringGetAsync refers
            // to a special `nil` value but that appears to be mixing of lingo.
            // Source: https://github.com/StackExchange/StackExchange.Redis/blob/main/docs/KeysValues.md
            return !value.IsNull;
        }

        private string CreateUniqueKey(string unique, string providerId)
        {
            var key = $"{unique}.{providerId}";

            var hmacKeyBytes = Encoding.UTF8.GetBytes(_config.Salt);
            var valueBytes = Encoding.UTF8.GetBytes(key);

            using var hmac = new HMACSHA256(hmacKeyBytes);

            var hashBytes = hmac.ComputeHash(valueBytes);

            return Encoding.UTF8.GetString(hashBytes);
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}