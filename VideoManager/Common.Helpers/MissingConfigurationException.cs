using System;
using System.Runtime.Serialization;

namespace VideoManager.Helpers
{
    /// <summary>
    /// Should be thrown by the Configuration Helper.
    /// </summary>
    [Serializable]
    public sealed class MissingConfigurationException : Exception
    {
        public MissingConfigurationException()
        {
        }

        public MissingConfigurationException(string message) : base(message)
        {
        }

        public MissingConfigurationException(string message, Exception e) : base(message, e)
        {
        }

        private MissingConfigurationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}