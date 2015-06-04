// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Claims;
using System.IdentityModel.Services;
using System.Linq;
using System.Web.Helpers;
using GeekQuiz.Utils;

namespace GeekQuiz
{
    // For more information on ASP.NET Identity, please visit http://go.microsoft.com/fwlink/?LinkId=301863
    public static class IdentityConfig
    {
        public static string AudienceUri { get; private set; }
        public static string Realm { get; private set; }

        public static void ConfigureIdentity()
        {
            RefreshValidationSettings();
            // Set the realm for the application
            Realm = ConfigurationManager.AppSettings["ida:realm"];

            // Set the audienceUri for the application
            AudienceUri = ConfigurationManager.AppSettings["ida:AudienceUri"];
            if (!String.IsNullOrEmpty(AudienceUri))
            {
                UpdateAudienceUri();
            }

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
        }

        public static void RefreshValidationSettings()
        {
            string metadataLocation = ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];
            DatabaseIssuerNameRegistry.RefreshKeys(metadataLocation);
        }

        public static void UpdateAudienceUri()
        {
            int count = FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                .AudienceRestriction.AllowedAudienceUris.Count(
                    uri => String.Equals(uri.OriginalString, AudienceUri, StringComparison.OrdinalIgnoreCase));
            if (count == 0)
            {
                FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                    .AudienceRestriction.AllowedAudienceUris.Add(new Uri(IdentityConfig.AudienceUri));
            }
        }
    }
}
