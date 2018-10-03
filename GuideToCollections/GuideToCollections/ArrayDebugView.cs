using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DevGuideToCollections
{
    /// <summary>
    /// Proxy for displaying the class in the debugger.
    /// </summary>
    internal class ArrayDebugView
    {
        object m_obj;

        public ArrayDebugView(object obj)
        {
            m_obj = obj;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public object[] Items
        {
            get
            {
                if (m_obj == null)
                {
                    return new object[0];
                }

                // You can use reflection to get the elements until the chapter of enumerations.
                var method = m_obj.GetType().GetMethod("ToArray");

                if (method == null)
                {
                    return new object[0];
                }

                Array array = method.Invoke(m_obj, null) as Array;

                object []retval = new object[array.Length];

                for (int i = 0; i < array.Length; ++i)
                {
                    retval[i] = array.GetValue(i);
                }

                return  retval;
            }
        }
    }
}
