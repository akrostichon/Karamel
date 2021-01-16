using System;

namespace Karamel.Infrastructure.ExceptionHandling
{
    /// <summary>
    /// Exception inside the CdgLibrary
    /// </summary>
    public class CdgLibraryException : Exception
    {
        #region Constructors
        
        public CdgLibraryException()
        {
        }

        public CdgLibraryException(string message)
            : base(message)
        {
        }

        public CdgLibraryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        #endregion Constructors
    }
}
