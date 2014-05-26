using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{

    /// <summary>
    /// Set of extension methods that can be used anywhere in MSSolve
    /// </summary>
    /// <remarks>
    /// This class should ONLY contain extension methods for non-MSSolve objects
    /// because this DLL should not reference anything MSSolve specific and should
    /// be reusable across other projects and applications.
    /// </remarks>
    /// <RevisionHistory>
    /// 	<Revision Author="shitals" Date="8/25/2008 3:26 PM">Created</Revision>
    /// </RevisionHistory>
    public static class ExtentionMethods
    {
        /// <summary>
        /// Loop through all elements of specified collection/array and execute the 
        /// action that is provided
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list through which all elements needs to be looped</param>
        /// <param name="action">The code that should be executed for each element of the collection</param>
        /// <example>myCollection.ForEach((element) => Debug.WriteLine(element.ToString()));</example>
        /// <RevisionHistory>
        /// 	<Revision Author="shitals" Date="8/25/2008 3:28 PM">Created</Revision>
        /// </RevisionHistory>
        public static void ForEach<T>(this IEnumerable<T> list,
            Action<T> action)
        {
            foreach (T listItem in list)
            {
                action(listItem);
            }
        }


        /// <summary>
        /// Same as string.Format. Formats string for specified arguments.
        /// </summary>
        /// <param name="format">String which contains tokens to be replaced</param>
        /// <param name="args">Values to replace tokens</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is created because the currently available method Format doesn't work on constant strings, i.e., "My string is {0}".Format("big!") doesn't work
        /// </remarks>
        /// <example>"This is {0} string".FormatEx("Example");</example>
        /// <RevisionHistory>
        /// 	<Revision Author="shitals" Date="8/25/2008 3:32 PM">Created</Revision>
        /// </RevisionHistory>
        public static string FormatEx(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> list, string delimiter)
        {
            return ToDelimitedString(list, delimiter, null, true);
        }

        /// <summary>
        /// Converts any list in to delimited string value
        /// </summary>
        /// <typeparam name="T">Type of list element</typeparam>
        /// <param name="list">Instance of list</param>
        /// <param name="delimiter">Delimiter to be used to seperate values</param>
        /// <param name="toStringConverter">User defined function that provides string value for each list item</param>
        /// <param name="includeNullOrEmptyValues">If this is true and if the string for list element is null or empty then it's included in output otherwise its not</param>
        /// <returns>Delimited string</returns>
        /// <example>"myCollection.ToDelimitedString(",", (myObject) => myObject.MainProperty, true);</example>
        /// <RevisionHistory>
        /// 	<Revision Author="shitals" Date="6/2/2009 5:53 PM">Created</Revision>
        /// </RevisionHistory>
        public static string ToDelimitedString<T>(this IEnumerable<T> list, string delimiter, Func<T, string> toStringConverter, bool includeNullOrEmptyValues)
        {
            //Build the delimited string
            StringBuilder resultString = new StringBuilder();

            //If list has any elements
            if (list != null)
            {
                //walk through each element
                foreach (T listElement in list)
                {
                    //Convert element to string
                    string listElementAsString = (toStringConverter == null ? (listElement != null ? listElement.ToString() : null) : toStringConverter(listElement)) ?? string.Empty;
                    
                    //Add this element in delimited string
                    if (((listElementAsString.Length == 0) && includeNullOrEmptyValues) || (listElementAsString.Length > 0))
                    {
                        //Add delimiter if the result already contains some value
                        if (resultString.Length > 0)
                            resultString.Append(delimiter);

                        resultString.Append(listElementAsString);
                    }
                }
            }
            //else we'll return empty string

            return resultString.ToString();
        }

        /// <summary>
        /// Truncate string to specified value
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(this string s, int maxLength)
        {
            if (string.IsNullOrEmpty(s) || maxLength <= 0)
                return string.Empty;
            else if (s.Length > maxLength)
                return s.Substring(0, maxLength) + "...";
            else
                return s;
        }


    }
}
