﻿// Copyright 2021 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System;
using System.Runtime.InteropServices;
using static NL.Rijksoverheid.CoronaCheck.BackEnd.Interop.Go.Helpers;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.HolderInterop
{
    public class Holder
    {
        private const int DefaultBufferSize = 65536; // 64kb

        /// <summary>
        ///     Creates the Issuer Commitment Message, this is sent to Issuer.IssueProof()
        /// </summary>
        /// <param name="holderSecretKey">Secret generated on the holder by GenerateHolderSecretKey()</param>
        /// <param name="nonceB64">Nonce generated by Issuer.GenerateNonce()</param>
        public string CreateCommitmentMessage(string holderSecretKey, string nonceB64)
        {
            if (string.IsNullOrWhiteSpace(holderSecretKey)) throw new ArgumentNullException(nameof(holderSecretKey));
            if (string.IsNullOrWhiteSpace(nonceB64)) throw new ArgumentNullException(nameof(nonceB64));

            var privateKeyGo = ToGoString(holderSecretKey);
            var nonceB64Go = ToGoString(nonceB64);

            var buffer = Marshal.AllocHGlobal(DefaultBufferSize);
            try
            {
                HolderInteropInterface.CreateCommitmentMessage(privateKeyGo, nonceB64Go, buffer, DefaultBufferSize, out var written, out var error);

                return Result(buffer, written, error);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        ///     Creates a secret key for the holder
        /// </summary>
        public string GenerateHolderSecretKey()
        {
            var buffer = Marshal.AllocHGlobal(DefaultBufferSize);
            try
            {
                HolderInteropInterface.GenerateHolderSk(buffer, DefaultBufferSize, out var written, out var error);

                return Result(buffer, written, error);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        ///     Loads the issuer keys
        /// </summary>
        public void LoadIssuerPks(string annotatedIssuerKeys)
        {
            if (string.IsNullOrWhiteSpace(annotatedIssuerKeys)) throw new ArgumentNullException(nameof(annotatedIssuerKeys));

            var annotatedIssuerKeysGo = ToGoString(annotatedIssuerKeys);

            var buffer = Marshal.AllocHGlobal(DefaultBufferSize);
            try
            {
                HolderInteropInterface.LoadIssuerPks(annotatedIssuerKeysGo, buffer, DefaultBufferSize, out var written, out var error);

                ResultVoid(buffer, written, error);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
