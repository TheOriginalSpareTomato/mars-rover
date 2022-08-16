using System.Runtime.Serialization;

namespace MarsRover.Models.Exceptions
{
    [Serializable]
    public class CommandBufferException : Exception
    {
        public CommandBufferException()
        {
        }

        public CommandBufferException(string? message) : base(message)
        {
        }

        public CommandBufferException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CommandBufferException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}