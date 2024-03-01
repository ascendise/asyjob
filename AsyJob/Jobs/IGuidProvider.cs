﻿namespace AsyJob.Jobs
{
    /// <summary>
    /// Provider for Guids
    /// </summary>
    public interface IGuidProvider
    {
        /// <summary>
        /// Generates a unique GUID
        /// </summary>
        /// <returns></returns>
        Guid GetGuid();
    }
}
