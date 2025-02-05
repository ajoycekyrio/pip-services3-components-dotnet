﻿using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using System;
using System.Linq;
using Xunit;

namespace PipServices3.Components.Auth
{
    //[TestClass]
    public sealed class CredentialResolverTest
    {
        private static readonly ConfigParams RestConfig = ConfigParams.FromTuples(
            "credential.username", "Negrienko",
            "credential.password", "qwerty",
            "credential.access_key", "key",
            "credential.store_key", "store key"
        );

        [Fact]
        public void TestConfigure()
        {
            var credentialResolver = new CredentialResolver(RestConfig);
            var config = credentialResolver.GetAll().FirstOrDefault();

            Assert.Equal(config["username"], "Negrienko");
            Assert.Equal(config["password"], "qwerty");
            Assert.Equal(config["access_key"], "key");
            Assert.Equal(config["store_key"], "store key");
        }

        [Fact]
        public void TestLookup()
        {
            var credentialResolver = new CredentialResolver();
            var credential = credentialResolver.LookupAsync("correlationId").Result;
            Assert.Null(credential);

            var restConfigWithoutStoreKey = ConfigParams.FromTuples(
                "credential.username", "Negrienko",
                "credential.password", "qwerty",
                "credential.access_key", "key"
            );
            credentialResolver = new CredentialResolver(restConfigWithoutStoreKey);
            credential = credentialResolver.LookupAsync("correlationId").Result;

            Assert.Equal(credential.Get("username"), "Negrienko");
            Assert.Equal(credential.Get("password"), "qwerty");
            Assert.Equal(credential.Get("access_key"), "key");
            Assert.Null(credential.Get("store_key"));

            credentialResolver = new CredentialResolver(RestConfig);
            credentialResolver.SetReferences(new References());
            try
            {
                credential = credentialResolver.LookupAsync("correlationId").Result;
            }
            catch (Exception)
            {
                //Assert.IsType<ReferenceException>(ex);
            }
        }
    }
}
