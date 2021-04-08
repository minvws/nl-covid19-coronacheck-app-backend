﻿// Copyright 2020 De Staat der Nederlanden, Ministerie van Volksgezondheid, Welzijn en Sport.
// Licensed under the EUROPEAN UNION PUBLIC LICENCE v. 1.2
// SPDX-License-Identifier: EUPL-1.2

using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NL.Rijksoverheid.CoronaCheck.BackEnd.Common;
using NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Config;
using NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Services;
using NL.Rijksoverheid.CoronaCheck.BackEnd.Common.Web.Builders;
using NL.Rijksoverheid.CoronaCheck.BackEnd.IssuerApi.Models;

namespace NL.Rijksoverheid.CoronaCheck.BackEnd.IssuerApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("proof")]
    public class IssuerController : ControllerBase
    {
        private readonly IApiSigningConfig _apiSigningConfig;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<IssuerController> _logger;
        private readonly IProofOfTestService _potService;
        private readonly ISignedDataResponseBuilder _srb;

        public IssuerController(
            ILogger<IssuerController> logger,
            IProofOfTestService potService,
            IJsonSerializer jsonSerializer,
            ISignedDataResponseBuilder signedDataResponseBuilder,
            IApiSigningConfig apiSigningConfig)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _potService = potService ?? throw new ArgumentNullException(nameof(potService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _srb = signedDataResponseBuilder ?? throw new ArgumentNullException(nameof(signedDataResponseBuilder));
            _apiSigningConfig = apiSigningConfig ?? throw new ArgumentNullException(nameof(apiSigningConfig));
        }

        /// <summary>
        ///     Issues the proof of (negative) test
        /// </summary>
        [HttpPost]
        [Route("issue")]
        [ProducesResponseType(typeof(IssueProofResult), 200)]
        public IActionResult IssueProof(IssueProofRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("IssueProof: Invalid model state.");

                return new BadRequestResult();
            }

            try
            {
                var commitmentsJson = Base64.DecodeAsUtf8String(request.Commitments!);

                var attributes = new ProofOfTestAttributes(
                    request.Attributes!.SampleTime,
                    request.Attributes.TestType!,
                    request.Attributes.FirstNameInitial!,
                    request.Attributes.LastNameInitial!,
                    request.Attributes.BirthDay!,
                    request.Attributes.BirthMonth!,
                    false, // Always set to false for non-static
                    request.Attributes.IsSpecimen
                );

                var proofResult =
                    _potService.GetProofOfTest(attributes, request.Nonce!, commitmentsJson);

                var issueProofResult = _jsonSerializer.Deserialize<IssueProofResult>(proofResult);

                return _apiSigningConfig.WrapAndSignResult
                    ? Ok(_srb.Build(issueProofResult))
                    : Ok(issueProofResult);
            }
            catch (FormatException e)
            {
                _logger.LogError("IssueProof: Error decoding either commitments or issuer message.", e);

                return new BadRequestResult();
            }
            catch (Exception e)
            {
                _logger.LogError("IssueProof: Unexpected exception.", e);

                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        ///     Generates and returns a random nonce
        /// </summary>
        [HttpPost]
        [Route("nonce")]
        [ProducesResponseType(typeof(GenerateNonceResult), 200)]
        public IActionResult GenerateNonce()
        {
            try
            {
                var nonce = _potService.GenerateNonce();

                var result = new GenerateNonceResult {Nonce = nonce};

                return _apiSigningConfig.WrapAndSignResult
                    ? Ok(_srb.Build(result))
                    : Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError("IssueProof: Error generating nonce.", e);

                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        ///     Issues a static proof, returning QR in PNG format, base64 encoded in a string
        /// </summary>
        [HttpPost]
        [Route("issue-static")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult IssueStaticProof(IssueStaticProofRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("IssueProof: Invalid model state.");

                return new BadRequestResult();
            }

            try
            {
                var attributes = new ProofOfTestAttributes(
                    request.Attributes!.SampleTime,
                    request.Attributes.TestType!,
                    request.Attributes.FirstNameInitial!,
                    request.Attributes.LastNameInitial!,
                    request.Attributes.BirthDay!,
                    request.Attributes.BirthMonth!,
                    true, // Always set to true for static!
                    request.Attributes.IsSpecimen);

                var qr = _potService.GetStaticProofQr(attributes);

                return _apiSigningConfig.WrapAndSignResult
                    ? Ok(_srb.Build(qr))
                    : Ok(qr);
            }
            catch (FormatException e)
            {
                _logger.LogError("IssueProof: Error decoding either commitments or issuer message.", e);

                return new BadRequestResult();
            }
            catch (Exception e)
            {
                _logger.LogError("IssueProof: Error issuing proof.", e);

                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}
