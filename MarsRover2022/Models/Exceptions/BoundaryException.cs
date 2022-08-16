using System.Runtime.Serialization;

namespace MarsRover.Models.Exceptions
{
    [Serializable]
    public class BoundaryException : Exception
    {
        public BoundaryException()
        {
        }

        public BoundaryException(string? message) : base(message)
        {
        }

        public BoundaryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BoundaryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}