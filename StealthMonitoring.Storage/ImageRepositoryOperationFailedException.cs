using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace StealthMonitoring.Storage
{
    public class ImageRepositoryOperationFailedException : Exception
    {
        public ImageRepositoryOperationFailedException()
        {
        }

        protected ImageRepositoryOperationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ImageRepositoryOperationFailedException(string message) : base(message)
        {
        }

        public ImageRepositoryOperationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
