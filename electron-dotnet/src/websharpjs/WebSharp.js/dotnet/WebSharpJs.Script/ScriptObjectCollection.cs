using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace WebSharpJs.Script
{

    public sealed class ScriptObjectCollection<T> : ScriptObject,
    IEnumerable<T>, IEnumerable, IDisposable
    {
        IEnumerable<T> enumeratorImplementation;

        public IEnumerator<T> GetEnumerator()
        {
            return enumeratorImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return enumeratorImplementation.GetEnumerator();
        }

        private ScriptObjectCollection() { }

        internal ScriptObjectCollection(Array array)
        {
            enumeratorImplementation = (IEnumerable<T>)array;
        }

        public int Count
        {
            get
            {
                // Array implements ICollection<T> so let's give that a try first.
                ICollection<T> c = enumeratorImplementation as ICollection<T>;
                if (c != null)
                    return c.Count;

                int count = 0;
                using (IEnumerator<T> enumerator = enumeratorImplementation.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                        count++;
                }
                
                return count;
            }

        }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                // Array implements IList<T> so let's give that a try first.
                IList<T> c = enumeratorImplementation as IList<T>;
                if (c != null)
                    return c[index];

                int count = 0;
                using (IEnumerator<T> enumerator = enumeratorImplementation.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (count == index)
                            return enumerator.Current;
                    }
                }

                return default(T);
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    enumeratorImplementation = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
