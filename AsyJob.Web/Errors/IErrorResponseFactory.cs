﻿using System.Net;

namespace AsyJob.Web.Errors
{
    internal interface IErrorResponseFactory
    {
        public IEnumerable<Type> SupportedTypes { get; }

        /// <summary>
        /// Returns a matching http status code for the passed exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>"
        HttpStatusCode Create(Exception ex);

        /// <summary>
        /// Returns if the factory is able to create an http status code from the error response
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool Supports(Exception exception);
    }
}

