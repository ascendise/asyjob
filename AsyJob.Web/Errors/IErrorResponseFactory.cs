namespace AsyJob.Web.Errors
{
    internal interface IErrorResponseFactory
    {
        public IEnumerable<Type> SupportedTypes { get; }

        /// <summary>
        /// Returns a matching <see cref="ErrorResponse"/> for the passed exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>"
        ErrorResponse Create(Exception ex);

        /// <summary>
        /// Returns if the factory is able to create an error response from the error response
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool Supports(Exception exception);
    }
}

