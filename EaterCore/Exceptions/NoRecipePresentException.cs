using System;

namespace EaterCore.Exceptions
{
    [Serializable]
    public class NoRecipePresentException : Exception
    {
        public NoRecipePresentException()
        { }

        public NoRecipePresentException(string message)
            : base(message) { }
    }
}