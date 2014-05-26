using System;
using System.Collections.Generic;

namespace CommonUtils
{
    /// <summary>
    /// This class contains utility methods for functional programming
    /// </summary>
    /// <remarks>
    /// In general, lot of standard functional programming tools are missing from
    /// .Net 3.5. This class would contain these tools that can be used across different
    /// projects.
    /// </remarks>
    /// <RevisionHistory>
    /// 	<Revision Author="shitals" Date="8/25/2008 3:36 PM">Created</Revision>
    /// </RevisionHistory>
    public static class FunctionalUtilities
    {
        /// <summary>
        /// Takes a given method (i.e. function which returns void) and then converts it in to
        /// function that returns int.
        /// </summary>
        /// <param name="methodCode">The code that needs to be converted</param>
        /// <returns></returns>
        /// <remarks>
        /// This is also known as currieng in functional programming. However in C#
        /// we need this method because "void" is not considered as type. So for example,
        /// one can not write List<void> myList;. Due to this, C# has a concept of methods and functions
        /// (formar returns void and later returns some type). Consequently there are two flavors of 
        /// generic delegates Func and Action. So if you want to write common code you have to use
        /// Func but then you can overload method and ignore the return value. Below utility method
        /// allows to convert any code that doesn't return value in to a code that does return value.
        /// We choose int as return type because that's most efficint type but anything else can be used as well.
        /// </remarks>
        /// <RevisionHistory>
        /// 	<Revision Author="shitals" Date="8/25/2008 3:38 PM">Created</Revision>
        /// </RevisionHistory>
        public static Func<int> MethodToFunctionConverter(Action methodCode)
        {
            return () =>
            {
                methodCode();
                return 0;
            };
        }


        /// <summary>
        /// Returned the cached-version of given code block. The cached version of code block
        /// will first see if value already exist for given input and if it does then it will return
        /// cached values orther wise actual function would be executed.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="codeBlockToMemoize">The code block to memoize.</param>
        /// <returns></returns>
        /// <seealso href="http://diditwith.net/PermaLink,guid,7191176b-c72a-49db-986e-466845665fa1.aspx">Background</seealso>
        /// <RevisionHistory>
        /// 	<Revision Author="shitals" Date="8/29/2008 6:06 PM">Created</Revision>
        /// </RevisionHistory>
        public static Func<TKey, TResult> Memoize<TKey, TResult>(Func<TKey, TResult> codeBlockToMemoize)
        {
            Dictionary<TKey, TResult> memoizedValues = new Dictionary<TKey, TResult>();

            return (TKey key) =>
            {
                TResult value;
                if (memoizedValues.TryGetValue(key, out value))
                    return value;

                value = codeBlockToMemoize(key);
                memoizedValues.Add(key, value);
                return value;
            };
        }
    }
}
