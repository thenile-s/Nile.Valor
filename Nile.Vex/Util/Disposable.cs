using System;
using System.Collections.Generic;
using System.Text;

namespace Nile.Vex.Util
{
    /// <summary>
    /// Helps objects manage their state and throw an exception if they are disposed
    /// </summary>
    public abstract class Disposable
    {
        /// <summary>
        /// Indicates whether this object is disposed or not. <see cref="Disposed"/>
        /// </summary>
        public bool Disposed { get; protected set; }

        /// <summary>
        /// Throws an exception if this object is disposed
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (Disposed)
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Disposes of unmanaged resources used by this object, also making this object invalid for further use; sets <see cref="Disposed"/> to true. Can be called multiple times.
        /// </summary>
        public void Dispose()
        {
            if(!Disposed)
            {
                Disposed = true;
                DisposeInternal();
            }    
        }

        /// <summary>
        /// To be overriden by implementors to  dispose of resources
        /// </summary>
        protected abstract void DisposeInternal();
    }
}
