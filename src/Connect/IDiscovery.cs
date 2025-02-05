﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PipServices3.Components.Auth;

namespace PipServices3.Components.Connect
{
    /// <summary>
    /// Interface for discovery services which are used to store and resolve connection parameters
    /// to connect to external services.
    /// </summary>
    /// See <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-components-dotnet/master/doc/api/class_pip_services_1_1_components_1_1_connect_1_1_connection_params.html">ConnectionParams</a>, 
    /// <a href="https://rawgit.com/pip-services3-dotnet/pip-services3-components-dotnet/master/doc/api/class_pip_services_1_1_components_1_1_auth_1_1_credential_params.html">CredentialParams</a>
    public interface IDiscovery
    {
        /// <summary>
        /// Registers connection parameters into the discovery service.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="key">a key to uniquely identify the connection parameters.</param>
        /// <param name="connection">a connection to be registered.</param>
        Task RegisterAsync(string correlationId, string key, ConnectionParams connection);

        /// <summary>
        /// Resolves a single connection parameters by its key.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="key">a key to uniquely identify the connection.</param>
        /// <returns>a resolved connection.</returns>
        Task<ConnectionParams> ResolveOneAsync(string correlationId, string key);

        /// <summary>
        /// Resolves all connection parameters by their key.
        /// </summary>
        /// <param name="correlationId">(optional) transaction id to trace execution through call chain.</param>
        /// <param name="key">a key to uniquely identify the connection.</param>
        /// <returns>a list with resolved connections.</returns>
        Task<List<ConnectionParams>> ResolveAllAsync(string correlationId, string key);
    }
}
