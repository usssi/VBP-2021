using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtensionMethods
{
    public static class MyExtensionMehods
    {
        public static void ToList<T>(this T[] arr, List<T> list)
        {
            foreach (T element in arr)
                list.Add(element);
        }
    }
}
