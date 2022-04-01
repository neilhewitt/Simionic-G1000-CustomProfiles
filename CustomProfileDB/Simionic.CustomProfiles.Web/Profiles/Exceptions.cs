using System;
using System.Net.Http;

namespace Simionic.CustomProfiles.Web
{
    public class ProfileStoreException : Exception
    {
        public ProfileStoreException() : base() { }
        public ProfileStoreException(string message) : base(message) { }
        public ProfileStoreException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ProfileUpdateException : ProfileStoreException
    {
        public HttpResponseMessage ResponseMessage { get; init; }

        public ProfileUpdateException() : base() { }
        public ProfileUpdateException(string message) : base(message) { }
        public ProfileUpdateException(string message, Exception innerException) : base(message, innerException) { }
        public ProfileUpdateException(string message, HttpResponseMessage response) : base(message)
        { 
            ResponseMessage = response;
        }
    }

    public class ProfileDeleteException : ProfileStoreException
    {
        public HttpResponseMessage ResponseMessage { get; init; }

        public ProfileDeleteException() : base() { }
        public ProfileDeleteException(string message) : base(message) { }
        public ProfileDeleteException(string message, Exception innerException) : base(message, innerException) { }
        public ProfileDeleteException(string message, HttpResponseMessage response) : base(message)
        {
            ResponseMessage = response;
        }
    }
}
