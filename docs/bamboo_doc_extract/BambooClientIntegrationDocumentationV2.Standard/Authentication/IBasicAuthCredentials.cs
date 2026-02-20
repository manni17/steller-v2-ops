// <copyright file="IBasicAuthCredentials.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using System;

namespace BambooClientIntegrationDocumentationV2.Standard.Authentication
{
    /// <summary>
    /// Authentication configuration interface for BasicAuth.
    /// </summary>
    public interface IBasicAuthCredentials
    {
        /// <summary>
        /// Gets string value for username.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Gets string value for password.
        /// </summary>
        string Password { get; }

        /// <summary>
        ///  Returns true if credentials matched.
        /// </summary>
        /// <param name="username"> The string value for credentials.</param>
        /// <param name="password"> The string value for credentials.</param>
        /// <returns>True if credentials matched.</returns>
        bool Equals(string username, string password);
    }
}