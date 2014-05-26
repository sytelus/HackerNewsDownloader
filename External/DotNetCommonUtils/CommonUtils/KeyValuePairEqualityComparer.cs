using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
     public sealed class KeyValuePairEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>> 
         where TKey : IComparable 
     {
         public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
         {
            return (x.Key.CompareTo(y.Key) == 0);
         }
 
         public int GetHashCode(KeyValuePair<TKey, TValue> obj)
         {
             return obj.Key.GetHashCode();
         }
     }
}
