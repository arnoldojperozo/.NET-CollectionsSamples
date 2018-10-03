using DevGuideToCollections;
using System.Text;
using System;

namespace Driver
{
    class MyArraySample
    {
        public ArrayEx<int> Array { get;}
        private ArrayEx<int> _array;

        public MyArraySample()
        {
            Random rnd = new Random();
            ArrayEx<int> _array = new ArrayEx<int>();
            for (int i = 0; i < 20; ++i)
            {
                _array.Add(rnd.Next(100));
            }
            Array = _array;
        }

        public void ArraySample()
        {
            _array = Array;
            Console.WriteLine("Sorting the following list");
            Console.WriteLine(ArrayToString(_array.ToArray()));

            for (int i = 0; i < _array.Count; ++i)
            {
                for (int j = i + 1; j < _array.Count; ++j)
                {
                    if (_array[i] > _array[j])
                    {
                        int tmp = _array[j];
                        _array[j] = _array[i];
                        _array[i] = tmp;
                    }
                }
            }

            Console.WriteLine("The sorted list is");
            Console.WriteLine(ArrayToString(_array.ToArray()));
            Console.ReadLine();
        }

        public string ArrayToString(Array array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (array.Length > 0)
            {
                sb.Append(array.GetValue(0));
            }
            for (int i = 1; i < array.Length; ++i)
            {
                sb.AppendFormat(",{0}", array.GetValue(i));
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
