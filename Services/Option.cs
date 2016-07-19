using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// This class represents a return type that may not have a value, e.g. being null.
    /// It is similar to the Nullable type that is used for Value types.
    /// I use it in my code as a return type to denote methods that may return null.
    /// </summary>
    /// <typeparam name="T">any class</typeparam>
    public class Option<T> where T : class
    {
        public T Value { get; private set; }

        public Option(T arg)
        {
            Value = arg;
        }

        public Option()
        {
            Value = default(T);
        }

        public bool IsEmpty { get { return Value == null; } }
        public bool HasValue { get { return Value != null; } }
    }
}
