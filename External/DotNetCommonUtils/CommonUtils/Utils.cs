using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

/**
 * CommonUtils contains domain-general static utility functions.
 * Dependencies should only include those in .Net runtime.
 */
namespace CommonUtils
{
    using System.Web;

    public static class Utils
    {
        private static readonly double LogE2 = Math.Log(2);

        public static bool IsNullOrWhiteSpaceDotNet35(string input)
        {
            if (input == null)
                return true;
            else if (String.IsNullOrEmpty(input))
                return true;
            else if (input.Trim().Length == 0)
                return true;
            else
                return false;
        }

        public static string GetMD5HashForXml(string xml)
        {
            return GetMD5HashString(XElement.Parse(xml).ToString(SaveOptions.DisableFormatting));
        }

        public static void AppendLines(this StringBuilder sb, IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                sb.AppendLine(value);
            }
        }

        public static T CreateInstance<T>(string typeName)
        {
            var typeOfInstance = Type.GetType(typeName);
            if (typeOfInstance == null)
                throw new ArgumentException("Type '{0} cannot be found".FormatEx(typeName));
            var instance = (T)Activator.CreateInstance(typeOfInstance);

            return instance;
        }

        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower(CultureInfo.CurrentCulture));
        }

        public static string Append(this string s, string valueToAppend, string delimiter = null, bool applyDelimiterOnlyIfNotEmpty = true)
        {
            if (delimiter == null || (string.IsNullOrEmpty(s) && applyDelimiterOnlyIfNotEmpty))
                return string.Concat(s, valueToAppend);
            else
                return string.Concat(delimiter, s, valueToAppend);
        }

        public static LinkedListNode<T> AddItemFirst<T>(this LinkedList<T> list, T item)
        {
            var node = new LinkedListNode<T>(item);
            list.AddFirst(node);
            return node;
        }

        public static LinkedListNode<T> AddItemLast<T>(this LinkedList<T> list, T item)
        {
            var node = new LinkedListNode<T>(item);
            list.AddLast(node);
            return node;
        }

        public static double Log2(double x)
        {
            return Math.Log(x) / LogE2;
        }

        private readonly static Regex nonAlphaNumericRegex = new Regex(@"[\W]", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase); //remove all non-alpha chars
        public static string RemoveNonAlphaNumericChars(string s)
        {
            return nonAlphaNumericRegex.Replace(s, String.Empty);
        }

        public static IEnumerable<string> RemoveNullOrEmpty(this IEnumerable<string> source)
        {
            return source.Where(s => !String.IsNullOrEmpty(s));
        }

        public static IEnumerable<string> RemoveNullOrWhiteSpace(this IEnumerable<string> source)
        {
            return source.Where(s => !String.IsNullOrWhiteSpace(s));
        }

        public static string GetAttributeValue(this XElement element, string attributeName, bool mustExist)
        {
            var attribute = element.Attribute(attributeName);

            if (attribute == null)
            {
                if (mustExist)
                    throw new ArgumentOutOfRangeException("attributeName", "The attribute '{0}' must exist in XML element '{1}'".FormatEx(attributeName, element.Name));
                else
                    return null;
            }
            else
                return attribute.Value;
        }

        public static string ToCurrencyString(this decimal currency)
        {
            return currency.ToString("C");
        }
        public static string ToStringInvariant(this decimal currency)
        {
            return currency.ToString(CultureInfo.InvariantCulture);
        }
        public static decimal ParseDecimal(string value)
        {
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }
        public static string ToStringCurrentCulture(this int k)
        {
            return k.ToString(CultureInfo.CurrentCulture);
        }
        public static string ToStringInvariant(this int k)
        {
            return k.ToString(CultureInfo.InvariantCulture);
        }
        public static string ToStringInvariant(this int? k, string valueIfNull = IntMinValueAsString)
        {
            if (k == null)
                return valueIfNull;
            else
                return k.Value.ToString(CultureInfo.InvariantCulture);
        }
        public static string ToStringInvariant(this double k)
        {
            return k.ToString(CultureInfo.InvariantCulture);
        }
        public static string ToStringInvariant(this float k)
        {
            return k.ToString(CultureInfo.InvariantCulture);
        }
        public static string ToStringInvariant(this float? k)
        {
            if (k == null)
                return IntMinValueAsString;
            else
                return k.Value.ToString(CultureInfo.InvariantCulture);
        }
        public static string ToStringInvariant(this double? k)
        {
            if (k == null)
                return IntMinValueAsString;
            else
                return k.Value.ToString(CultureInfo.InvariantCulture);
        }
        public static void InsertAndExpand<T>(this List<T> list, int index, T item)
        {
            if (index < 0)
                throw new IndexOutOfRangeException("List index must be >= 0");
            else if (index < list.Count)
                list[index] = item;
            else if (index == list.Count)
                list.Add(item);
            else
            {
                list.AddRange(Enumerable.Repeat(default(T), index - list.Count + 1));
                list[index] = item;
            }
        }
        public static string ToStringInvariant(this double? k, bool allowNonNumericalValues)
        {
            if (k == null)
                return IntMinValueAsString;
            else if (!allowNonNumericalValues && (Double.IsNaN(k.Value) || Double.IsInfinity(k.Value)))
                throw new Exception("Double value is not expected to be NaN or Infinity");
            else
                return k.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        public static string GetMD5HashString(IEnumerable<object> objects, bool sort, bool hexStringOutput = false)
        {
            var strings = objects.Select(o => o.ToStringNullSafe());
            if (sort)
                strings = strings.OrderBy(s => s, StringComparer.Ordinal);

            var bytes = strings.SelectMany(s => Encoding.UTF8.GetBytes(s ?? string.Empty)).ToArray();

            return GetMD5HashString(bytes, hexStringOutput);
        }

        private enum TermType
        {
            UpperCaseLetters,
            LowercaseLetters,
            NumberOrOther,
            None
        };

        public static bool IsOneSubStringOfOther(string s1, string s2)
        {
            if (s1.IsNullOrEmpty() && s2.IsNullOrEmpty())
                return true;
            else if (s1.IsNullOrEmpty() || s2.IsNullOrEmpty())
                return false;
            else
                return s1.StartsWith(s2, StringComparison.Ordinal) || s2.StartsWith(s1, StringComparison.Ordinal);
        }

        public static Dictionary<TKey, int> ToValueIndexDictionary<TKey>(this IEnumerable<TKey> keys)
        {
            var dictionary = new Dictionary<TKey, int>();

            var index = 0;
            foreach (var key in keys)
            {
                dictionary.Add(key, index);
                index++;
            }

            return dictionary;
        }

        private readonly static Dictionary<Type, DataContractJsonSerializer> dictionaryDataContractJsonSerializers = 
            new Dictionary<Type, DataContractJsonSerializer>()
                {
                    {typeof(IEnumerable<KeyValuePair<string, string>>), new DataContractJsonSerializer(typeof(IEnumerable<KeyValuePair<string, string>>))},
                    {typeof(IEnumerable<KeyValuePair<string, double>>), new DataContractJsonSerializer(typeof(IEnumerable<KeyValuePair<string, double>>))}
                };
        public static string SerializeToJson<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
        {
            if (dictionary != null)
                return SerializeToJson(
                    dictionaryDataContractJsonSerializers[typeof(IEnumerable<KeyValuePair<TKey, TValue>>)]
                    , dictionary);
            else
                return null;
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> DeserializeDictionaryFromJson<TKey, TValue>(string jsonSerializedDisctionary)
        {
            if (String.IsNullOrEmpty(jsonSerializedDisctionary))
                return null;

            return DeserializeFromJson<IEnumerable<KeyValuePair<TKey, TValue>>>(
                dictionaryDataContractJsonSerializers[typeof(IEnumerable<KeyValuePair<TKey, TValue>>)]
                , jsonSerializedDisctionary);
        }

        public static string SerializeToXml<T>(T item)
        {
            using (var writer = new StringWriter())
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(writer, item);
                return writer.ToString();
            }
        }

        public static T DeserializeFromXml<T>(string xmlString)
        {
            using (var reader = new StringReader(xmlString))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T) serializer.Deserialize(reader);
            }
        }

        public static string ToStringNullSafe(this object o)
        {
            if (o == null)
                return null;
            else
                return o.ToString();
        }


        /// <summary>
        /// This method would break down string on boundry of letter or digits or multiple of upper/small case letters
        /// Examples:
        ///     Quest12 => {Quest, 12}
        ///     QUESTPolo23, Inc! => {QUEST, Polo, 23, Inc}
        ///     QuestPolo23 @ Corp => {Quest, Polo, 23, Corp}
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IEnumerable<string> WordBreak(this string content)
        {
            var term = new StringBuilder();
            var termType = TermType.None;
            foreach (var c in content)
            {
                var flushTermLength = -1;

                //Decide if we should return the accumulated term
                if (termType == TermType.UpperCaseLetters)
                {
                    if (Char.IsLower(c))
                    {
                        if (term.Length > 1)
                            flushTermLength = term.Length - 1;
                    }
                    else if (!Char.IsUpper(c))
                        flushTermLength = term.Length;
                }
                else if (termType == TermType.LowercaseLetters)
                {
                    if (Char.IsUpper(c))
                    {
                        if (term.Length > 1)
                            flushTermLength = term.Length - 1;
                    }
                    else if (!Char.IsLower(c))
                        flushTermLength = term.Length;
                }
                else if (termType == TermType.NumberOrOther)
                {
                    if (!Char.IsDigit(c))
                        flushTermLength = term.Length;
                }


                if (flushTermLength > -1)
                {
                    var termToReturn = term.ToString();
                    var termToAdd = String.Empty;
                    if (flushTermLength < termToReturn.Length)
                    {
                        termToAdd = termToReturn.Substring(flushTermLength);
                        termToReturn = termToReturn.Substring(0, flushTermLength);
                    }

                    if (termToReturn.Length > 0)
                    {
                        yield return termToReturn;
                        term.Length = 0;
                    }

                    if (termToAdd.Length > 0)
                        term.Append(termToAdd);
                }

                if (Char.IsLetterOrDigit(c))
                {
                    term.Append(c);

                    if (Char.IsUpper(c))
                        termType = TermType.UpperCaseLetters;
                    else if (Char.IsLower(c))
                        termType = TermType.LowercaseLetters;
                    else
                        termType = TermType.NumberOrOther;
                }
                else
                    termType = TermType.None;
            }
        }

        static readonly Regex wordBreaker = new Regex(@"[a-zA-Z]+|[0-9]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase); //remove all non-alpha chars, split on alpha num boundaries

        /// <summary>
        /// Returns terms without single letter buffering
        /// </summary>
        [Obsolete("This method would not work for non-English cultures. Use MatchUtils.BreakWords instead.")]
        public static string[] GetTerms(string content)
        {
            MatchCollection termMatches = GetTermMatches(content);
            var terms = new string[termMatches.Count];

            for (int termIndex = 0; termIndex < termMatches.Count; termIndex++)
            {
                terms[termIndex] = termMatches[termIndex].Value;
            }

            return terms;
        }

        public static string[] Split(string[] delimiters, string s, bool removeEmptyEntries)
        {
            if (!String.IsNullOrEmpty(s))
            {
                return s.Split(delimiters, removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
            }
            else
                return EmptyStringArray;
        }

        public static bool IsNumericalValue(this double value)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value);
        }

        public static IEnumerable<TElement> TryGetValues<TKey, TElement>(this ILookup<TKey, TElement> lookup, TKey key)
        {
            if (lookup == null || !lookup.Contains(key))
                return Enumerable.Empty<TElement>();

            return lookup[key];
        }

        public static double GetJaccardSimilarity<TKey, TValue>(IDictionary<TKey, TValue> value1Terms, IDictionary<TKey, TValue> value2Terms)
        {
            IDictionary<TKey, TValue> smallerValueTerms, largerValueTerms;
            if (value1Terms.Count < value2Terms.Count)
            {
                smallerValueTerms = value1Terms;
                largerValueTerms = value2Terms;
            }
            else
            {
                smallerValueTerms = value2Terms;
                largerValueTerms = value1Terms;
            }

            var intersectCount = smallerValueTerms.Keys.Count(smallerValueTerm => largerValueTerms.ContainsKey(smallerValueTerm));

            return intersectCount / (double)(value1Terms.Count + value2Terms.Count - intersectCount);
        }

        public static double GetJaccardSimilarity(HashSet<string> value1Terms, HashSet<string> value2Terms)
        {
            HashSet<string> smallerValueTerms, largerValueTerms;
            if (value1Terms.Count < value2Terms.Count)
            {
                smallerValueTerms = value1Terms;
                largerValueTerms = value2Terms;
            }
            else
            {
                smallerValueTerms = value2Terms;
                largerValueTerms = value1Terms;
            }

            var intersectCount = smallerValueTerms.Count(smallerValueTerm => largerValueTerms.Contains(smallerValueTerm));

            return intersectCount / (double) (value1Terms.Count + value2Terms.Count - intersectCount);
        }

        public static double GetDiceSimilarity(HashSet<string> value1Terms, HashSet<string> value2Terms)
        {
            HashSet<string> smallerValueTerms, largerValueTerms;
            if (value1Terms.Count < value2Terms.Count)
            {
                smallerValueTerms = value1Terms;
                largerValueTerms = value2Terms;
            }
            else
            {
                smallerValueTerms = value2Terms;
                largerValueTerms = value1Terms;
            }

            var intersectCount = smallerValueTerms.Count(smallerValueTerm => largerValueTerms.Contains(smallerValueTerm));

            return (intersectCount * 2.0) / (value1Terms.Count + value2Terms.Count);
        }

        public static double GetDiceSimilarity(string[] value1Terms, string[] value2Terms, double defaultForNullOrEmpty)
        {
            if (value1Terms.Length > 0 && value2Terms.Length > 0)
            {
                var value1TermsSet = value1Terms.ToHashSet();

                var totalTermCount = value1TermsSet.Count + value2Terms.Length;
                value1TermsSet.IntersectWith(value2Terms);
                var diceSimilarity = (2.0 * value1TermsSet.Count) / totalTermCount;

                return diceSimilarity;
            }
            else return defaultForNullOrEmpty;
        }

        public static IEnumerable<T> SelectRange<T>(this IList<T> source, int startIndex, int endIndex)
        {
            for (var sourceIndex = startIndex; sourceIndex <= endIndex; sourceIndex++)
                yield return source[sourceIndex];
        }

        public static IEnumerable<T> SelectRange<T>(this IList<T> source, int startIndex)
        {
            return SelectRange(source, startIndex, source.Count - 1);
        }

        public static IEnumerable<T> SelectReverse<T>(this IList<T> source)
        {
            for(var sourceIndex = source.Count - 1; sourceIndex >= 0; sourceIndex--)
                yield return source[sourceIndex];
        }

        public static IEnumerable<KeyValuePair<T, T>> SelectPairs<T>(this IList<T> source)
        {
            for (var sourceIndex1 = 0; sourceIndex1 < source.Count; sourceIndex1++)
                for (var sourceIndex2 = sourceIndex1 + 1; sourceIndex2 < source.Count; sourceIndex2++)
                    yield return new KeyValuePair<T, T>(source[sourceIndex1], source[sourceIndex2]);
        }

        public static void ToVoid<T>(this IEnumerable<T> sequence)
        {
            foreach (var x in sequence)
            {
            }
        }

        public static TResult IfNotNullValue<TInput, TResult>(this TInput? input, Func<TInput, TResult> resultSelector, TResult resultIfNull) where TInput : struct 
        {
            if (input == null)
                return resultIfNull;
            else
                return resultSelector(input.Value);
        }

        public static TResult IfNotNullValue<TInput, TResult>(this TInput? input, Func<TInput, TResult> resultSelector) where TInput : struct 
        {
            return IfNotNullValue(input, resultSelector, default(TResult));
        }

        public static TResult IfNotNull<TInput, TResult>(this TInput input, Func<TInput, TResult> resultSelector, TResult resultIfNull) where TInput : class
        {
            if (input == null)
                return resultIfNull;
            else
                return resultSelector(input);
        }

        public static TResult IfNotNull<TInput, TResult>(this TInput input, Func<TInput, TResult> resultSelector) where TInput : class
        {
            return IfNotNull(input, resultSelector, default(TResult));
        }

        public static TResult IfNotNullOrEmpty<TInput, TResult>(this IList<TInput> list, Func<IList<TInput>, TResult> fn, TResult defaultValue = default(TResult))
        {
            if (list == null || list.Count == 0)
                return defaultValue;
            else 
                return fn(list);
        }


        public static TV MostOccuring<T,TV>(this IEnumerable<T> sequenceElements, Func<T, TV> selector)
        {
            var frequencies =
                sequenceElements
                .GroupBy(sequenceElement => selector(sequenceElement))
                .Select(g => new { g.Key, Count = g.Count() }).ToArray();

            if (frequencies.Length == 0)
                return default(TV);

            var highestFrequency = frequencies.Max(frequency => frequency.Count);

            var valuesWithHighestFrequencies = frequencies.Where(frequency => frequency.Count == highestFrequency);

            return valuesWithHighestFrequencies.Select(frequency => frequency.Key).FirstOrDefault();
        }

        public static T MostOccuring<T>(this IEnumerable<T> sequenceElements)
        {
            return sequenceElements.MostOccuring(sequenceElement => sequenceElement);
        }

        /// <summary>
        /// Return terms with single letter buffering
        /// </summary>
        [Obsolete("This method would not work for non-English cultures. Use MatchUtils.BreakWords instead.")]
        public static IEnumerable<string> GetTerms(string content, HashSet<string> filterSet)
        {
            return GetTermsWithBufferedSingleLetters(content, filterSet).Select(termKvp => termKvp.Key);
        }

        /// <summary>
        /// Return terms with single letter buffering
        /// </summary>
        [Obsolete("This method would not work for non-English cultures. Use MatchUtils.BreakWords instead.")]
        public static Dictionary<string, int> GetTermsAsDictionary(string content, int positionMultiplier, HashSet<string> filterSet)
        {
            var terms = new Dictionary<string, int>();
            foreach (var termKvp in GetTermsWithBufferedSingleLetters(content, filterSet))
                terms[termKvp.Key] = termKvp.Value*positionMultiplier;

            return terms;
        }

        public static string CompactSerialize(double value)
        {
            return ByteArrayToBase64String(BitConverter.GetBytes(value));
        }
        public static Double CompactDeserializeDouble(string value)
        {
            return BitConverter.ToDouble(Base64StringToByteArray(value), 0);
        }

        /// <summary>
        /// This is enhanced version of GetTermMatches with the difference that multiple single letter terms are combined to 1.
        /// "abc xyz123~q!1." -> {"abc", "xyz", "123", "q", "1"}
        /// "abc xyz123~p q r ps!1." -> {"abc", "xyz", "123", "pqr", "ps", "1"}
        /// </summary>
        /// <param name="content">string to break</param>
        /// <param name="filterSet">These terms would not be processed (typically noise words)</param>
        private static IEnumerable<KeyValuePair<string, int>> GetTermsWithBufferedSingleLetters(string content, HashSet<string> filterSet)
        {
            //Break on any non-alpha chars
            var termMatches = GetTermMatches(content);
            var bufferedTerm = String.Empty;
            var bufferedTermIndex = -1;
            for (var termIndex = 0; termIndex < termMatches.Count; termIndex++)
            {
                var term = termMatches[termIndex].Value;

                if (term.Length == 1)
                {
                    bufferedTerm = String.Concat(bufferedTerm, term);
                    if (bufferedTermIndex == -1) bufferedTermIndex = termIndex;
                    continue;
                }
                else
                {
                    if (bufferedTerm.Length > 0)
                    {
                        if (filterSet==null || !filterSet.Contains(bufferedTerm))
                            yield return new KeyValuePair<string, int>(bufferedTerm, bufferedTermIndex);

                        bufferedTerm = String.Empty;
                        bufferedTermIndex = -1;
                    }

                    if (filterSet == null || !filterSet.Contains(term))
                        yield return new KeyValuePair<string, int>(term, termIndex);
                }
            }

            if (bufferedTerm.Length > 0)
            {
                if (filterSet == null || !filterSet.Contains(bufferedTerm))
                    yield return new KeyValuePair<string, int>(bufferedTerm, bufferedTermIndex);
            }
        }

        /// <summary>
        /// Cleans accented chars and breaks the string on any non-alpha chars. "abc xyz123~q!1." -> {"abc", "xyz", "123", "q", "1"}
        /// </summary>
        private static MatchCollection GetTermMatches(string content)
        {
            content = content ?? String.Empty;
            var asciiLowerCasedContent = ConvertCharsToASCII(content.Trim().ToLowerInvariant());

            var termMatches = wordBreaker.Matches(asciiLowerCasedContent);

            return termMatches;
        }

        private enum CsvParserState
        {
            ValueStart, UnquotedValue, QuotedValue, QuotedValueEndOrDoubleQuote, AfterQuotedValueEnd, Error
        }
        public static IEnumerable<string> ParseCsvLine(string line, char delimiter = ',', char quoteChar = '"')
        {
            if (!string.IsNullOrEmpty(line))
            {
                var columnValue = new StringBuilder();   
                var state = CsvParserState.ValueStart;
                var errorMessage = (string)null;
                var yieldValue = false;

                for (var charIndex = 0; charIndex < line.Length; charIndex++)
                {
                    var currentChar = line[charIndex];

                    switch (state)
                    {
                        case CsvParserState.ValueStart:
                            if (currentChar == delimiter)
                                yieldValue = true;
                            else if (currentChar == quoteChar)
                                state = CsvParserState.QuotedValue;
                            else if (char.IsWhiteSpace(currentChar))
                                columnValue.Append(currentChar);
                            else
                            {
                                columnValue.Append(currentChar);
                                state = CsvParserState.UnquotedValue;
                            }
                            break;
                        case CsvParserState.UnquotedValue:
                            if (currentChar == delimiter)
                                yieldValue = true;
                            else if (currentChar == quoteChar)
                            {
                                state = CsvParserState.Error;
                                errorMessage = "Quote char cannot appear in unquoted value. Quote char {2} is not expected at position {0} in line \"{1}\"".FormatEx(charIndex, line, quoteChar);
                            }
                            else
                                columnValue.Append(currentChar);
                            break;
                        case CsvParserState.QuotedValue:
                            if (currentChar == quoteChar)
                                state = CsvParserState.QuotedValueEndOrDoubleQuote;
                            else
                                columnValue.Append(currentChar);
                            break;
                        case CsvParserState.QuotedValueEndOrDoubleQuote:
                            if (currentChar == delimiter)
                                yieldValue = true;
                            else if (currentChar == quoteChar)
                            {
                                columnValue.Append(currentChar);
                                state = CsvParserState.QuotedValue;
                            }
                            else if (char.IsWhiteSpace(currentChar))
                            {
                                columnValue.Append(currentChar);
                                state = CsvParserState.AfterQuotedValueEnd;
                            }
                            else
                            {
                                state = CsvParserState.Error;
                                errorMessage = "Quote char must be followed by another quote char, space or delimiter. Char {2} is not expected at position {0} in line \"{1}\"".FormatEx(charIndex, line, currentChar);
                            }
                            break;
                        case CsvParserState.AfterQuotedValueEnd:
                            if (currentChar == delimiter)
                                yieldValue = true;
                            else
                            {
                                state = CsvParserState.Error;
                                errorMessage = "Quoted values must end with delimiter or end of line. Char {2} is not expected at position {0} in line \"{1}\"".FormatEx(charIndex, line, currentChar);
                            }
                            break;
                        default:
                            throw new Exception("State machine for Csv parser s in invalid state {0}".FormatEx(state));
                    }

                    var isLastChar = charIndex == line.Length - 1;
                    if (isLastChar)
                    {
                        switch (state)
                        {
                            case CsvParserState.ValueStart:
                            case CsvParserState.Error:
                                break;
                            case CsvParserState.UnquotedValue:
                            case CsvParserState.QuotedValueEndOrDoubleQuote:
                            case CsvParserState.AfterQuotedValueEnd:
                                yieldValue = true;
                                break;
                            case CsvParserState.QuotedValue:
                                state = CsvParserState.Error;
                                errorMessage = "Quoted value does not end with quote. Quote char is expected at the end of line \"{0}\"".FormatEx(line);
                                break;
                            default:
                                throw new Exception("State machine for Csv parser s in invalid state {0}".FormatEx(state));
                        }
                    }

                    if (state == CsvParserState.Error)
                        throw new InvalidDataException(errorMessage);

                    if (yieldValue)
                    {
                        yield return columnValue.ToString();

                        if (!isLastChar)
                        {
                            columnValue.Clear();
                            yieldValue = false;
                            state = CsvParserState.ValueStart;
                        }
                        else if (currentChar == delimiter)
                            yield return string.Empty;
                    }
                }
            }
        }

        public static readonly char[] TabDelimiter = new char[] { '\t' };
        public static readonly char[] UnderscoreDelimiter = new char[] { '_' };
        public static readonly char[] CommaDelimiter = new char[] { ',' };
        public static readonly char[] SemiColonDelimiter = new char[] { ';' };
        public static readonly char[] SpaceDelimiter = new char[] { ' ' };
        public static readonly char[] TildaDelimiter = new char[] { '~' };
        public static readonly char[] DotDelimiter = new char[] { '.' };
        public static readonly char[] PipeDelimiter = new char[] { '|' };
        public static readonly char[] ColonDelimiter = new char[] { ':' };
        public static readonly string CommaDelimiterString = ",";

        public static int IncrementCountInDictionary<TKey>(this IDictionary<TKey, int> dictionary, TKey key)
        {
            int existingCount = 0;
            if (dictionary.TryGetValue(key, out existingCount))
                dictionary[key] = existingCount + 1;
            else
                dictionary.Add(key, 1);

            return existingCount;
        }
        public static uint IncrementCountInDictionary<TKey>(this IDictionary<TKey, uint> dictionary, TKey key) 
        {
            uint existingCount = 0;
            if (dictionary.TryGetValue(key, out existingCount))
                dictionary[key] = existingCount + 1;
            else
                dictionary.Add(key, 1);

            return existingCount;
        }
        public static void IncrementCountInDictionary<TKey>(this IDictionary<TKey, float> dictionary, TKey key)
        {
            float existingCount;
            if (dictionary.TryGetValue(key, out existingCount))
                dictionary[key] = existingCount + 1;
            else
                dictionary.Add(key, 1);
        }

        public static Dictionary<string, string> Get2And3Grams(string[] tokens)
        {
            if (tokens != null && tokens.Length > 0)
            {
                var ngrams = new Dictionary<string, string>();

                string lastToken = tokens[0];
                string lastBiagram = null;
                for (var currentIndex = 1; currentIndex < tokens.Length; currentIndex++)
                {
                    var token = tokens[currentIndex];
                    var bigram = String.Concat(lastToken, token);
                    var fullForm = String.Concat(lastToken, " ", token);
                    ngrams[bigram] = fullForm;

                    if (lastBiagram != null)
                    {
                        var trigram = String.Concat(lastBiagram, token);
                        ngrams[trigram] = String.Concat(fullForm, " ", token);
                    }

                    lastToken = token;
                    lastBiagram = bigram;
                }

                return ngrams;
            }
            else return null;
        }

        public static string GetMD5HashString(string s, bool hexStringOutput = false)
        {
            var hash = GetMD5Hash(s);
            return hexStringOutput ? ByteArrayToHexString(hash) : ByteArrayToBase64String(hash);
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }

        public static UInt64 GetMD5HashUInt64(string s, MD5 md5Hasher)
        {
            return BitConverter.ToUInt64(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(s)), 0);
        }

        public static string GetMD5HashString(byte[] bytes, bool hexStringOutput = false)
        {
            var hash = GetMD5Hash(bytes);
            return hexStringOutput ? ByteArrayToHexString(hash) : ByteArrayToBase64String(hash);
        }

        public static byte[] GetMD5Hash(byte[] bytes)
        {
            // Note: the previous implementation caches md5Hasher as a static class variable,
            // which causes LDSEntity_Serialization_Test to fail and throw NullReferenceException,
            // when all tests are run.
            using (var md5Hasher = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                var data = md5Hasher.ComputeHash(bytes);
                return data;
            }
        }

        public static byte[] GetMD5Hash(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            return GetMD5Hash(bytes);
        }

        public static IEnumerable<T> AsEnumerable<T>(this T value)
        {
            return Enumerable.Repeat(value, 1);
        }
        public static T[] AsArray<T>(this T value)
        {
            return new T[] {value};
        }
        public static IEnumerable<T> AsEnumerable<T>(params T[] values)
        {
            if (values != null)
                foreach (var value in values)
                    yield return value;
        }
        public static IEnumerable<T> AsEnumerable<T>(bool ignoreNull, params T[] values) where T : class
        {
            return AsEnumerable(values).Where(value => !ignoreNull || (value != null));
        }

        public static bool Overlaps<T>(this IList<T> list1, IList<T> list2) where T:IEquatable<T>
        {
            if (list1.Count < 2 || list2.Count < 2 || (list1.Count * list2.Count < 16))
            {
                foreach (var element1 in list1)
                {
                    foreach (var element2 in list2)
                    {
                        if (EqualityComparer<T>.Default.Equals(element1, element2)) 
                            return true;
                    }
                }

                return false;
            }
            else
                return list1.Intersect(list2).Any();
        }

        public static string ToBase64String(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return ByteArrayToBase64String(Encoding.UTF8.GetBytes(s));
        }

        public static string FromBase64String(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return Encoding.UTF8.GetString(Base64StringToByteArray(s));
        }

        public static string ToBase64String(this Guid guid)
        {
            return ByteArrayToBase64String(guid.ToByteArray());
        }

        public static double GetLatitudeAtDistanceInMiles(double latitude, double distance)
        {
            return latitude + (distance / 69.1);
        }

        public static double GetLongitudeAtDistanceInMiles(double latitude, double longitude, double distance)
        {
            return longitude + (distance / (Math.Cos(latitude / 57.3) * 69.1));
        }

        public static double GetLatLongDistanceInMiles(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var x = 69.1 * (latitude2 - latitude1);
            var y = 69.1 * (longitude2 - longitude1) * Math.Cos(latitude1 / 57.3);

            return Math.Sqrt(x * x + y * y);            
        }

        public static bool IsTrueOrNull(this bool? bn)
        {
            return !bn.HasValue || bn.Value;
        }

        public static bool IsTrue(this bool? bn)
        {
            return bn.HasValue && bn.Value;
        }

        public static bool IsFalse(this bool? bn)
        {
            return bn.HasValue && !bn.Value;
        }

        public static bool IsFalseOrNull(this bool? bn)
        {
            return !bn.HasValue || !bn.Value;
        }

        public static bool IsNullOrValue(this int? numberToCompare, int value)
        {
            return numberToCompare == null || numberToCompare.Value == value;
        }

        public static bool IsNullOrNotValue(this int? numberToCompare, int value)
        {
            return numberToCompare == null || numberToCompare.Value != value;
        }

        public static double GetLatLongDistanceInMiles(double latitude1, double longitude1, double latitude2, double longitude2, out double latitudeDifference, out double longitudeDifference)
        {
            var avgLongitude = (longitude1 + longitude2) / 2;
            var avgLatitude = (latitude1 + latitude2) / 2;
            latitudeDifference = GetLatLongDistanceInMiles(latitude1, avgLongitude, latitude2, avgLongitude); ;
            longitudeDifference = GetLatLongDistanceInMiles(avgLatitude, longitude1, avgLatitude, longitude2); ;

            return GetLatLongDistanceInMiles(latitude1, longitude1, latitude2, longitude2);
        }


        public static IEnumerable<XElement> GetElementsByPath(this XElement startElement, params string[] elementNamesInPath)
        {
            var currentElement = startElement;
            for (var elementNameIndex = 0; elementNameIndex < elementNamesInPath.Length - 1; elementNameIndex++)
            {
                var elementName = elementNamesInPath[elementNameIndex];
                currentElement = currentElement.Element(elementName);
                if (currentElement == null)
                    break;
            }

            return currentElement.IfNotNull(e => e.Elements(elementNamesInPath.Last()), Enumerable.Empty<XElement>());
        }

        public static bool? EqualAndNotWhitespace(this string s1, string s2, StringComparison compareMode = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2))
                return null;
            else
                return string.Equals(s1, s2, compareMode);
        }

		public static IEnumerable<string> GetXElementText(this XElement element)
		{
			return element.Nodes().OfType<XText>().Select(t => t.Value);
		}
		public static string GetInnerXml(this XElement element)
		{
			using (var reader = element.CreateReader())
			{
				reader.MoveToContent();
				return reader.ReadInnerXml();
			}
		}
		public static string GetOuterXml(this XElement element)
		{
			using (var reader = element.CreateReader())
			{
				reader.MoveToContent();
				return reader.ReadOuterXml();
			}
		}

        /// <summary>
        /// Retrieve element of node specified by path
        /// </summary>
        /// <param name="xml">Xml to search for the path</param>
        /// <param name="attributeName">If value of attribute is required then set this parameter otherwise leave it to null in which case content of node is returned</param>
        /// <param name="elementNamesInPath">list of elements that makes up the path, Example: ["Entity", "NameGroup", "Name"]</param>
        /// <returns>If multiple nodes matching the path are found than their values is concated delimited by "~" char. If path is not found then empty string is returned.</returns>
        public static string GetPathNodeValue(string xml, string attributeName
            , params string[] elementNamesInPath)
        {
            var element = XElement.Parse(xml);
            return element.GetPathNodeValue(attributeName, elementNamesInPath);
        }

        /// <summary>
        /// Retrieve element of node specified by path
        /// </summary>
        /// <param name="startElement">Element to search for the path</param>
        /// <param name="attributeName">If value of attribute is required then set this parameter otherwise leave it to null in which case content of node is returned</param>
        /// <param name="elementNamesInPath">list of elements that makes up the path, Example: ["Entity", "NameGroup", "Name"]</param>
        /// <returns>If multiple nodes matching the path are found than their values is concated delimited by "~" char. If path is not found then empty string is returned.</returns>
        public static string GetPathNodeValue(this XElement startElement, string attributeName
            , params string[] elementNamesInPath)
        {
            var nodeValues = GetPathNodeValueRecursive(startElement, attributeName, elementNamesInPath, 0);

            var concatenatedNodeValue = Concat("~", nodeValues);

            return concatenatedNodeValue;
        }


        private static IEnumerable<string> GetPathNodeValueRecursive(XElement startElement, string attributeName
            , string[] elementNamesInPath, int startElementNamesInPath)
        {
            if (startElementNamesInPath < elementNamesInPath.Length)
            {
                var elementName = elementNamesInPath[startElementNamesInPath];
                var elements = startElement.Elements(elementName);
                foreach (var element in elements)
                {
                    var nodeValues = GetPathNodeValueRecursive(element, attributeName, elementNamesInPath, startElementNamesInPath + 1);
                    foreach (var nodeValue in nodeValues)
                        yield return nodeValue;
                }
            }
            else
            {
                if (attributeName == null)
                    yield return startElement.Value;
                else
                {
                    foreach (var attribute in startElement.Attributes(attributeName))
                    {
                        yield return attribute.Value;
                    }
                }
            }
        }

        public static string EmptyIfNull(this string value)
        {
            return value ?? string.Empty;
        }
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> value)
        {
            return value ?? Enumerable.Empty<T>();
        }
        public static string[] NullIfEmpty(this string[] value)
        {
            if (value == null || value.Length == 0)
                return null;
            else return value;
        }
        public static string NullIfEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else return value;
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> seqOfSeq)
        {
            foreach (var seq in seqOfSeq)
            {
                foreach (var element in seq)
                {
                    yield return element;
                }
            }
        }

        public static string[] GetLines(string text)
        {
            return (text ?? String.Empty).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static T[] IfNullOrEmpty<T>(this T[] values, Func<T[]> alternateValue = null)
        {
            return values.IsNullOrEmpty() ? (alternateValue != null ? alternateValue() : null) : values;
        }

        public static bool IsNullOrEmpty<T>(this T[] values)
        {
            return values == null || values.Length == 0;
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }

        public static string ConvertCharsToASCII(string input)
        {
            if (!String.IsNullOrEmpty(input))
                return UnicodeAsciiCharacterMap.GetMappedCharacters(input);
            else 
                return input;
        }


        private const int BigramLevel = 1, TrigramLevel = 2;
        /// <summary>
        /// ASUUMPTIONS: pased indexes are valid
        /// </summary>
        public static bool IsNGramMatch(string[] tokens1, string[] tokens2, ref int tokens1Index, ref int tokens2Index)
        {
            var currentToken1 = tokens1[tokens1Index];
            var currentToken2 = tokens2[tokens2Index];

            if (currentToken1 != currentToken2)
            {
                string token1Bigram;
                if (!DoesNGramLeftToRightMatch(tokens1, tokens2, ref tokens1Index, tokens2Index, BigramLevel, currentToken1, out token1Bigram))
                {
                    string token1Trigram;
                    if (!DoesNGramLeftToRightMatch(tokens1, tokens2, ref tokens1Index, tokens2Index, TrigramLevel, token1Bigram, out token1Trigram))
                    {
                        string token2Bigram;
                        if (!DoesNGramLeftToRightMatch(tokens2, tokens1, ref tokens2Index, tokens1Index, BigramLevel, currentToken2, out token2Bigram))
                        {
                            string token2Trigram;
                            return DoesNGramLeftToRightMatch(tokens2, tokens1, ref tokens2Index, tokens1Index, TrigramLevel, token2Bigram, out token2Trigram);
                        }
                        else return true;
                    }
                    else return true;
                }
                else return true;
            }
            else return true;
        }

        public static string Concat(string delimiter, bool ignoreNullOrEmptyValues, params string[] values)
        {
            return Concat(values, delimiter, ignoreNullOrEmptyValues);
        }

        public static string Concat(string[] values, string delimiter, bool ignoreNullOrEmptyValues)
        {
            return Concat(values, delimiter, ignoreNullOrEmptyValues, 0, null);
        }


        public static void DebugBreakIfEqual<T>(T value1Source, T value1Target) where T : IComparable<T>
        {
            if (value1Source.CompareTo(value1Target) == 0)
            {
                Console.Beep();
                Debugger.Break();
            }
        }

        public static void DebugBreakIfEqual<T>(T value1Source, T value1Target, T value2Source, T value2Target) where T : IComparable<T>
        {
            if (value1Source.CompareTo(value1Target) == 0 || value1Source.CompareTo(value2Target) == 0 
                || value2Source.CompareTo(value1Target) == 0 || value2Source.CompareTo(value2Target) == 0)
            {
                Console.Beep();
                Debugger.Break();
            }
        }

        public static T[] CreateInitializedArray<T>(int length, Func<int, T> elementFactory)
        {
            var array = new T[length];

            if (elementFactory != null)
            {
                for (var i = 0; i < length; i++)
                {
                    array[i] = elementFactory(i);
                }
            }

            return array;
        }

        public static T[][] CreateJaggedArray<T>(int m, int n)
        {
            var v = new T[m][];
            for (var i = 0; i < m; ++i) v[i] = new T[n];
            return v;
        }

        public static string Concat(string[] tokens, string delimiter, bool ignoreNullOrEmptyValues, int startIndex, Func<string, string> tokenProcessor)
        {
            if (tokens == null || tokens.Length == 0)
                return String.Empty;
            else
            {
                if (tokens.Length == 1)
                    return tokens[0] ?? String.Empty;
                else
                {
                    var concateResult = new StringBuilder();

                    for (var tokenIndex = startIndex; tokenIndex < tokens.Length; tokenIndex++)
                    {
                        var token = tokens[tokenIndex];

                        if (tokenProcessor != null)
                            token = tokenProcessor(token) ?? String.Empty;

                        if (ignoreNullOrEmptyValues && String.IsNullOrEmpty(token))
                            continue;

                        if (concateResult.Length > 0)
                            concateResult.Append(delimiter);

                        concateResult.Append(token);
                    }

                    return concateResult.ToString();
                }
            }
        }

        private static bool DoesNGramLeftToRightMatch(string[] tokens1, string[] tokens2, ref int tokens1Index, int tokens2Index, int ngramLevel, string previousNGram, out string ngram)
        {
            var tokens1NGramIndex = tokens1Index + ngramLevel;
            if (tokens1NGramIndex < tokens1.Length)
            {
                ngram = String.Concat(previousNGram, tokens1[tokens1NGramIndex]);
                var isMatch = (ngram == tokens2[tokens2Index]);
                if (isMatch)
                    tokens1Index = tokens1NGramIndex;

                return isMatch;
            }
            else
            {
                ngram = null;
                return false;
            }
        }

        public static void Swap<T>(ref T value1, ref T value2)
        {
            var temp = value1;
            value1 = value2;
            value2 = temp;
        }
        public static void Swap<T>(IList<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        public static bool SwapIfLeftIsLargerValue(ref string leftValue, ref string rightValue)
        {
            if (String.CompareOrdinal(leftValue, rightValue) > 0)
            {
                Swap(ref leftValue, ref rightValue);
                return true;
            }
            else return false;
        }

        public static bool SwapIfLeftIsLargerLength(ref string leftValue, ref string rightValue)
        {
            if (leftValue.Length > rightValue.Length || (leftValue.Length == rightValue.Length) && String.CompareOrdinal(leftValue, rightValue) > 0)
            {
                Swap(ref leftValue, ref rightValue);
                return true;
            }
            else return false;
        }

        public static void ReplaceBiTriGramsFromLeftSet(Dictionary<string, int> rightTokensSet, Dictionary<string, int> leftTokensSet)
        {
            var rightTokens = rightTokensSet.OrderBy(kvp => kvp.Value).Select(kvp => kvp.Key).ToArray();

            for(var rightTokenIndex = 1; rightTokenIndex < rightTokens.Length; rightTokenIndex++)
            {
                var bigram = String.Concat(rightTokens[rightTokenIndex - 1], rightTokens[rightTokenIndex]);
                string trigram = null;
                if (rightTokenIndex > 1)
                    trigram = String.Concat(rightTokens[rightTokenIndex - 2], bigram);

                int leftTokenIndex;
                var trigramFound = false;
                if (trigram != null)
                {
                    trigramFound = leftTokensSet.TryGetValue(trigram, out leftTokenIndex);
                    if (trigramFound)
                    {
                        leftTokensSet.Remove(trigram);
                        leftTokensSet[rightTokens[rightTokenIndex-2]] = leftTokenIndex;
                        leftTokensSet[rightTokens[rightTokenIndex-1]] = leftTokenIndex + 1;
                        leftTokensSet[rightTokens[rightTokenIndex]] = leftTokenIndex + 2;
                    }
                }
                    
                if (!trigramFound && leftTokensSet.TryGetValue(bigram, out leftTokenIndex))
                {
                    leftTokensSet.Remove(bigram);
                    leftTokensSet[rightTokens[rightTokenIndex-1]] = leftTokenIndex;
                    leftTokensSet[rightTokens[rightTokenIndex]] = leftTokenIndex + 1;
                }
            }
        }

        /// <summary>
        /// ASSUMPTIONS: Each array is not-null, both arrays are of same length
        /// </summary>
        public static bool IsStringArrayEqual(string[] array1, string[] array2, bool ignoreAnyNullOrEmptyValue)
        {
            for(var arrayIndex = 0; arrayIndex < array1.Length; arrayIndex++)
            {
                if (array1[arrayIndex] == array2[arrayIndex] ||
                    (ignoreAnyNullOrEmptyValue && (String.IsNullOrEmpty(array1[arrayIndex]) ||  String.IsNullOrEmpty(array2[arrayIndex])))
                   )
                    continue;
                else
                {
                    return false;
                }
            }

            return true;
        }


        public static bool Is1EditDistanceAway(string token1, string token2, int minTokenLengthFor1EditDistanceErrors, int minTokenLengthFor2EditDistanceErrors)
        {
            bool is1EditDistanceAway;
            bool is2EditDistanceAway;

            Is1EditDistanceAway(token1, token2, minTokenLengthFor1EditDistanceErrors, minTokenLengthFor2EditDistanceErrors, out is1EditDistanceAway, out is2EditDistanceAway);

            return is1EditDistanceAway || is2EditDistanceAway;
        }

        public static void Is1EditDistanceAway(string token1, string token2, int minTokenLengthFor1EditDistanceErrors, int minTokenLengthFor2EditDistanceErrors, out bool is1EditDistanceAway, out bool is2EditDistanceAway)
        {
            if (token1.Length == token2.Length)    //1 edit distance update scenarios
            {
                if (token1.Length > minTokenLengthFor1EditDistanceErrors)
                {
                    var charDiffCount = 0;
                    for (var tokenIndex = 0; tokenIndex < token1.Length; tokenIndex++)
                    {
                        if (token1[tokenIndex] != token2[tokenIndex])
                        {
                            charDiffCount++;
                            if (charDiffCount > 2) break;   //detect up to 2 errors
                        }
                    }
                    is1EditDistanceAway = (token1.Length > minTokenLengthFor1EditDistanceErrors && charDiffCount == 1);        //Allow 1 update error for specified min length
                    is2EditDistanceAway = (token1.Length > minTokenLengthFor2EditDistanceErrors && charDiffCount == 2);    //Allow 2 update error for twice of specified min length
                }
                else
                {
                    is1EditDistanceAway = false;
                    is2EditDistanceAway = false;
                }
            }
            else if (token1.Length + token2.Length > (minTokenLengthFor1EditDistanceErrors * 2) && Math.Abs(token1.Length - token2.Length) == 1)    //1 edit distance delete scenarios
            {
                string smallerToken, largerToken;
                if (token1.Length > token2.Length)
                {
                    smallerToken = token2;
                    largerToken = token1;
                }
                else
                {
                    smallerToken = token1;
                    largerToken = token2;
                }

                var charDiffCount = 0;
                var largerTokenIndex = 0;
                var smallerTokenIndex = 0;
                while (smallerTokenIndex < smallerToken.Length && largerTokenIndex < largerToken.Length && charDiffCount < 2)
                {
                    if (smallerToken[smallerTokenIndex] != largerToken[largerTokenIndex])
                    {
                        largerTokenIndex++;
                        charDiffCount++;
                    }
                    else
                    {
                        smallerTokenIndex++;
                        largerTokenIndex++;
                    }
                }
                if (charDiffCount == 0) charDiffCount = 1;  //for loop ended without char diff so count for last char

                is1EditDistanceAway = charDiffCount == 1;
                is2EditDistanceAway = false;
            }
            else
            {
                is1EditDistanceAway = false;
                is2EditDistanceAway = false;
            }
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> kvps, bool updateExistingValues)
        {
            foreach (var kvp in kvps)
            {
                if (!dictionary.ContainsKey(kvp.Key))
                {
                    dictionary.Add(kvp);
                }
                else if (updateExistingValues)
                {
                    dictionary[kvp.Key] = kvp.Value;
                }
            }
        }

        public static void SaveLines(string saveFilePath, IEnumerable<string> lines)
        {
            using (var textFileWriter = File.CreateText(saveFilePath))
            {
                foreach (var line in lines)
                    textFileWriter.WriteLine(line);
            }
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            if (keys == null)
                return;

            using (var valuesEnumerator = values.GetEnumerator())
            {
                foreach (var key in keys)
                {
                    var valueAvailable = valuesEnumerator.MoveNext();
                    if (!valueAvailable)
                        throw new IndexOutOfRangeException("values sequence is of less size than keys sequence");
                    dictionary.Add(key, valuesEnumerator.Current);
                }
            }
        }

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> elementsToAdd, bool allowDuplicates)
        {
            if (elementsToAdd == null)
                return;

            foreach (var element in elementsToAdd)
            {
                var isAdded = set.Add(element);
                if (!isAdded && !allowDuplicates)
                    throw new ArgumentException("Attempto add duplicate elements when duplicates were not expected");
            }            
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> elementsToAdd)
        {
            if (elementsToAdd == null)
                return;

            foreach (var element in elementsToAdd)
            {
                collection.Add(element);
            }
        }
        public static bool RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keysToRemove)
        {
            var isAnyRemoved = false;
            foreach (var key in keysToRemove)
            {
                var isRemoved = dictionary.Remove(key);
                isAnyRemoved = isAnyRemoved || isRemoved;
            }

            return isAnyRemoved;
        }

        public static bool RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> elementsToRemove)
        {
            if (elementsToRemove == null)
                return false;

            var isAnyRemoved = false;
            foreach (var element in elementsToRemove)
            {
                var isRemoved = collection.Remove(element);
                isAnyRemoved = isAnyRemoved || isRemoved;
            }

            return isAnyRemoved;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> sequence)
        {
            return ToHashSet(sequence, false, false);
        }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> sequence, bool returnNullIfEmpty, bool trimExcess)
        {
            HashSet<T> hashSet = returnNullIfEmpty ? null : new HashSet<T>(sequence);

            if (returnNullIfEmpty)
            {
                foreach (var item in sequence)
                {
                    if (hashSet == null) 
                        hashSet = new HashSet<T>();

                    hashSet.Add(item);
                }
            }

            if (trimExcess && hashSet != null)
                hashSet.TrimExcess(); 

            return hashSet;
        }
        public static Queue<T> ToQueue<T>(this IEnumerable<T> sequence)
        {
            var queue = new Queue<T>(sequence);
            //queue.TrimExcess();   //Commented because GC perf is not measured
            return queue;
        }
        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> sequence)
        {
            return new LinkedList<T>(sequence);
        }

        public const string IntMinValueAsString = "-2147483648";
        public const string IntMaxValueAsString =  "2147483647";

        public static SortedList<TKey, TValue> ToSortedList<TItem, TKey, TValue>(this IEnumerable<TItem> items, Func<TItem, TKey> getKey, Func<TItem, TValue> getValue, bool trimExcess)
        {
            var sortedList = new SortedList<TKey, TValue>();
            foreach (var item in items)
                sortedList.Add(getKey(item), getValue(item));

            if (trimExcess)
                sortedList.TrimExcess();

            return sortedList;
        }
        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TItem, TKey, TValue>(this IEnumerable<TItem> items, Func<TItem, TKey> getKey, Func<TItem, TValue> getValue)
        {
            var sortedDictionary = new SortedDictionary<TKey, TValue>();
            foreach (var item in items)
                sortedDictionary.Add(getKey(item), getValue(item));

            return sortedDictionary;
        }

        public static DateTime ParseJsonDate(string jsonDate)
        {
            return DateTime.ParseExact(jsonDate, @"yyyy-MM-ddTHH:mm:ssK", CultureInfo.InvariantCulture);
        }

        public static void AddToDictionarySet<TKeyType, TValueType, TCollectionType>(this IDictionary<TKeyType, TCollectionType> dictionary, TKeyType key, TValueType valueToAdd) where TCollectionType:ICollection<TValueType>, new()
        {
            TCollectionType existingValue;
            if (!dictionary.TryGetValue(key, out existingValue))
            {
                existingValue = new TCollectionType();
                dictionary.Add(key, existingValue);
            }

            existingValue.Add(valueToAdd);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvps)
        {
            var dict = new Dictionary<TKey, TValue>();
            dict.AddRange(kvps);
            return dict;
        }

        public static Dictionary<TKey, TValue> ToLastSetDictionary<T, TKey, TValue>(this IEnumerable<T> sequence, Func<T, TKey> getKeys, Func<T, TValue> getValues)
        {
            var dict = new Dictionary<TKey, TValue>();
            foreach (var item in sequence)
            {
                var key = getKeys(item);
                var value = getValues(item);

                dict[key] = value;
            }
            return dict;
        }

        public static string SerializeToJson<T>(DataContractJsonSerializer serializer, T objectToSerialize)
        {
            MemoryStream buffer = null;
            try
            {
                buffer = new MemoryStream();
                serializer.WriteObject(buffer, objectToSerialize);
                buffer.Flush();
                buffer.Position = 0;
                using (var stream = new StreamReader(buffer))
                {
                    buffer = null;
                    return stream.ReadToEnd();
                }
            }
            finally
            {
                if (buffer != null)
                    buffer.Close();
            }
        }

        public static bool Intersects(this Enum flag, Enum value)
        {
            if (Enum.GetUnderlyingType(value.GetType()) == typeof(ulong))
                return (Convert.ToUInt64(value) & Convert.ToUInt64(flag)) > 0;
            else
                return (Convert.ToInt64(value) & Convert.ToInt64(flag)) > 0;
        }

        public static string GetTimeZoneHoursFromAbbreviation(string timeZoneAbbreviation)
        {
            return TimeZones.AbbreviationsMap[timeZoneAbbreviation.ToUpperInvariant()];
        }

        public static T DeserializeFromJson<T>(DataContractJsonSerializer serializer, string jsonToDeserialize)  
        {
            if (String.IsNullOrEmpty(jsonToDeserialize))
                return default(T);
            MemoryStream buffer = null;
            try
            {
                buffer = new MemoryStream();
                StreamWriter stream = null;
                try
                {
                    stream = new StreamWriter(buffer);
                    stream.Write(jsonToDeserialize);
                    stream.Flush();
                    buffer.Flush();

                    buffer.Position = 0;
                    var deserializedObject = (T) serializer.ReadObject(buffer);

                    return deserializedObject;
                }
                finally
                {
                    if (stream != null)
                    {
                        buffer = null;
                        stream.Close();
                    }
                }
            }
            finally
            {
                if (buffer != null)
                    buffer.Close();
            }
        }

        public static readonly XmlSerializerNamespaces XmlEmptyNamespace = GetEmptyNameSpace();
        private static XmlSerializerNamespaces GetEmptyNameSpace()
        {
            var emptyNamespace = new XmlSerializerNamespaces();
            emptyNamespace.Add(String.Empty, String.Empty);
            return emptyNamespace;
        }


        /// <summary>
        /// Use carefully as there may be huge perf implication
        /// </summary>
        public static T[] AppendToArray<T>(T[] array, T valueToAppend)
        {
            if (array == null)
                array = new T[] { valueToAppend };
            else
            {
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = valueToAppend;
            }

            return array;
        }

        public static string RandomIfEmpty(string valueToCheck)
        {
            return String.IsNullOrEmpty(valueToCheck) ? Guid.NewGuid().ToString() : valueToCheck;
        }
        public static string RandomIfEmpty(string valueToCheck, string alternativeValue)
        {
            return String.IsNullOrEmpty(valueToCheck) ? RandomIfEmpty(alternativeValue) : valueToCheck;
        }

        public static string RemoveChars(this string stringValue, HashSet<char> charsToRemove)
        {
            return new string(stringValue.Where(c => !charsToRemove.Contains(c)).ToArray());
        }

        /// <summary>
        /// This method is highly optimized to avoid any dynamic memory allocation of number of values are less than 2.
        /// This method is expected to be called many times when number of values are less than 2.
        /// </summary>
        public static string Concat(string delimiter, IEnumerable<string> values)
        {
            StringBuilder buffer = null;
            var firstValue = String.Empty;
            var valueCount = 0;

            foreach (var value in values)
            {
                switch (valueCount)
                {
                    case 0:
                        firstValue = value;
                        break;
                    case 1:
                        buffer = new StringBuilder(firstValue);
                        buffer.Append(delimiter);
                        buffer.Append(value);
                        break;
                    default:
                        buffer.Append(delimiter);
                        buffer.Append(value);
                        break;
                }

                valueCount++;
            }

            switch (valueCount)
            {
                case 0:
                    return firstValue;  //Empty enumerator
                case 1:
                    return firstValue;
                default:
                    return buffer.ToString();
            }
        }

        public static bool ParseBool(string value, Func<bool> getDefaultValue)
        {
            var parsedNullableValue = ParseBoolNullable(value, () => getDefaultValue());
            if (parsedNullableValue == null)
            {
                if (getDefaultValue == null)
                    throw new ArgumentException(@"Cannot parse value '{0}' as bool".FormatEx(value));
                else
                    return getDefaultValue();
            }
            else
                return parsedNullableValue.Value;
        }

        public static bool ParseBool(string value, bool defaultValue)
        {
            return ParseBool(value, () => defaultValue);
        }

        public static IEnumerable<string> Split(this string value, char[] delimiter, bool trimParts = false, bool removeEmptyParts = false)
        {
            if (!string.IsNullOrEmpty(value))
                return
                    value.Split(delimiter)
                         .Select(s => trimParts ? s.Trim() : s)
                         .Where(s => !removeEmptyParts || !string.IsNullOrEmpty(s));
            else return Enumerable.Empty<string>();
        }

        public static bool? ParseBoolNullable(string value, Func<bool?> getDefaultValue)
        {
            if (String.IsNullOrEmpty(value))
                return null;
            else
            {
                if (String.Compare(value, Boolean.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
                else if (String.Compare(value, Boolean.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
                    return false;
                else
                {
                    int convertedInt;
                    if (Int32.TryParse(value, out convertedInt))
                        return convertedInt > 0;
                    else
                    {
                        if (String.Compare(value, "yes", StringComparison.OrdinalIgnoreCase) == 0
                            || String.Compare(value, "y", StringComparison.OrdinalIgnoreCase) == 0)
                            return true;
                        else if (String.Compare(value, "no", StringComparison.OrdinalIgnoreCase) == 0
                            || String.Compare(value, "n", StringComparison.OrdinalIgnoreCase) == 0)
                            return false;
                        else
                            return getDefaultValue();
                    }
                }
            }
        }

        public static bool IsNullOrEmpty<T>(ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
        public static bool IsNullOrEmpty<T>(LinkedList<T> linkedList)
        {
            return linkedList == null || linkedList.First == null;
        }

        public static int RemoveWhere<T>(this LinkedList<T> linkedList, Func<T, bool> predicate)
        {
            var linkedListNodeToCheck = linkedList.First;
            var removeCount = 0;

            while (linkedListNodeToCheck != null)
            {
                if (predicate(linkedListNodeToCheck.Value))
                {
                    linkedListNodeToCheck = linkedList.RemoveAndGetNext(linkedListNodeToCheck);
                    removeCount++;
                }
                else
                    linkedListNodeToCheck = linkedListNodeToCheck.Next;
            }

            return removeCount;
        }

        public static int RemoveWhere<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            var keysToRemove = dictionary.Where(predicate).Select(kvp => kvp.Key).ToList();
            return dictionary.RemoveKeys(keysToRemove);
        }

        public static int RemoveKeys<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IList<TKey> keysToRemove)
        {
            var removeCount = 0;
            foreach (var key in keysToRemove)
            {
                dictionary.Remove(key);
                removeCount++;
            }

            return removeCount;
        }

        public static int RemoveWhere<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, bool> predicate)
        {
            var keysToRemove = dictionary.Keys.Where(predicate).ToList();
            return dictionary.RemoveKeys(keysToRemove);
        }

        public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> linkedList)
        {
            var linkedListNode = linkedList.First;
            while (linkedListNode != null)
            {
                yield return linkedListNode;
                linkedListNode = linkedListNode.Next;
            }
        }

        public static IEnumerable<KeyValuePair<LinkedListNode<T>, int>> NodesAndIndex<T>(this LinkedList<T> linkedList)
        {
            var linkedListNode = linkedList.First;
            var index = 0;
            while (linkedListNode != null)
            {
                yield return new KeyValuePair<LinkedListNode<T>, int>(linkedListNode, index);
                linkedListNode = linkedListNode.Next;
                index++;
            }
        }

        public static LinkedListNode<T> RemoveAndGetNext<T>(this LinkedList<T> linkedList, LinkedListNode<T> nodeToRemove)
        {
            var tempNext = nodeToRemove.Next;
            linkedList.Remove(nodeToRemove);
            return tempNext;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue existingValue;
            if (dictionary.TryGetValue(key, out existingValue))
                return existingValue;
            else
                return defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return GetValueOrDefault(dictionary, key, default(TValue));
        }

        public static TValue AddOrGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> getValueIfNotExist)
        {
            TValue existingValue;
            if (!dictionary.TryGetValue(key, out existingValue))
            {
                existingValue = getValueIfNotExist();
                dictionary.Add(key, existingValue);
            }

            return existingValue;
        }


        public static int ToInt(this bool? booleanValue)
        {
            return booleanValue == null ? 0 : (booleanValue.Value ? 1 : 0);
        }
        public static int ToInt(this bool booleanValue)
        {
            return booleanValue ? 1 : 0;
        }
        public static bool ToBool(this int intValue)
        {
            if (intValue == 0)
                return false;
            else if (intValue == 1)
                return true;
            else
                throw new ArgumentOutOfRangeException(String.Format("intValue is expected to be either 0 or 1 but it's {0}", intValue));
        }

        public static bool AddIfNotExist<TKey,TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, bool updateExistingValue = false, IComparer<TValue> valueComparer = null)
        {
            TValue existingValue;
            var exist = dictionary.TryGetValue(key, out existingValue);

            if (!exist)
                dictionary.Add(key, value);
            else if (updateExistingValue)
            {
                var updateRequired = valueComparer == null || valueComparer.Compare(existingValue, value) != 0;
                if (updateRequired)
                    dictionary[key] = value;    
            }

            return exist;
        }

        public static TValueType AddOrGetFromDictionary<TKeyType, TValueType>(IDictionary<TKeyType, TValueType> dictionary, TKeyType key, Func<TKeyType, TValueType> createDictionaryValueForKey)
        {
            TValueType existingValue;

            if (!dictionary.TryGetValue(key, out existingValue))
            {
                existingValue = createDictionaryValueForKey(key);
                dictionary.Add(key, existingValue);
            }

            return existingValue;
        }

        public readonly static string[] EmptyStringArray = new string[]{};

        public static void Exchange<T>(IList<T> items, int index1, int index2)
        {
            if (index1 == index2)
                return;

            var temp = items[index1];
            items[index1] = items[index2];
            items[index2] = temp;
        }

        public static bool IsAnyNull(params object[] objects)
        {
            return objects.Any(o => o == null);
        }

        public static double Average(params double[] values)
        {
            return values.Average();
        }

        public static T[] ScalerToArray<T>(this T obj)
        {
            return new T[] {obj};
        }

        public static float Square(float number)
        {
            return number * number;
        }
        public static int Square(int number)
        {
            return number * number;
        }
        public static double Square(double number)
        {
            return number*number;
        }

        /// <summary>
        /// Walks through current call stack and returns the first frame from the top that is outside of 
        /// specified type/
        /// </summary>
        /// <param name="t">The type outside of which call stack frame is required.</param>
        /// <param name="stackFrameSearchStartIndex">The search of stack frame begins at this index. The 0 being the top most
        /// and will always point to this perticular method. The value 1 will be caller's stack frame. So usually 2 is the 
        /// starting frame outside caller (unless there are overloads). This parameter enhances performance by having to search through all 
        /// frames starting from 0 however if unsure then just pass 1.
        /// </param>
        /// <returns>MethodBase object that represents the method in call stack that was found out of type T</returns>
        /// <remarks>
        /// This function is perticularly useful when you want to get the name of the method, for example, to set in timer
        /// property or report in error log etc.
        /// </remarks>
        /// <RevisionHistory>
        /// 	<Revision Author="shitals" Date="8/25/2008 4:01 PM">Created</Revision>
        /// </RevisionHistory>
        public static StackFrame GetFirstMethodCallOutsideOfType(Type t, int stackFrameSearchStartIndex, StackTrace callStackToScan, out MethodBase userCodeMethodBase)
        {
            StackFrame userCodeStackFrame = null;   //By default we'll return null
            userCodeMethodBase = null;

            if (callStackToScan == null)
                callStackToScan = new StackTrace();    //Get current call stack

            //Loop through stack frames starting from specified search index
            for (var index = stackFrameSearchStartIndex; index < callStackToScan.FrameCount; index++)
            {
                var thisFrame = callStackToScan.GetFrame(index);
                var thisMethod = thisFrame.GetMethod();

                //Does this frame outside of specified type?
                if (thisMethod.DeclaringType != t)
                {
                    //yes return it
                    userCodeStackFrame = thisFrame;
                    userCodeMethodBase = thisMethod;
                    break;
                }
            }

            return userCodeStackFrame;
        }


        /// <summary>
        /// Converts specified base64 encoded string to byte array
        /// </summary>
        /// <param name="stringToConvert">Base64 encoded string</param>
        /// <returns></returns>
        public static byte[] Base64StringToByteArray(this string stringToConvert)
        {
            return Convert.FromBase64String(stringToConvert);
        }

        public static Guid Base64StringToGuid(this string valueToConvertFrom)
        {
            if (!String.IsNullOrEmpty(valueToConvertFrom))
                return new Guid(Base64StringToByteArray(valueToConvertFrom));
            else
                return Guid.Empty;
        }

        /// <summary>
        /// Converts specified byte array to base encoded string
        /// </summary>
        /// <param name="byteArrayToConvert">byte array to convert</param>
        /// <returns></returns>
        public static string ByteArrayToBase64String(byte[] byteArrayToConvert)
        {
            return Convert.ToBase64String(byteArrayToConvert);
        }

        /// <summary>
        /// Serialized entire object graph in to byte array
        /// </summary>
        /// <param name="anyObject">Object to serialize</param>
        /// <returns></returns>
        public static byte[] SerializeObject(object anyObject)
        {
            using (var buffer = new MemoryStream())
            {
                var serializationFormatter = new BinaryFormatter();
                serializationFormatter.Serialize(buffer, anyObject);
                return buffer.ToArray();
            }
        }

        /// <summary>
        /// Deserialized byte array back to original object graph
        /// </summary>
        /// <param name="serializedObjectGraph">Byte array to deserialized</param>
        /// <returns></returns>
        public static object DeserializeObject(byte[] serializedObjectGraph)
        {
            using (var buffer = new MemoryStream(serializedObjectGraph))
            {
                var serializationFormatter = new BinaryFormatter();
                return serializationFormatter.Deserialize(buffer);
            }
        }

        /// <summary>
        /// Converts specified byte array in to DataTable using the specified sceham for the data table.
        /// The byte array must have been formed by SerializeObject call on to array of DataRow.ItemArray
        /// of original data table. If schema or byte array is not specified then this call returns null.
        /// If byte array is empty but schema is specified then it returns empty data table.
        /// </summary>
        /// <param name="serializedTableData">Byte array of data table</param>
        /// <param name="tableSchema">Schema of original data table</param>
        /// <returns></returns>
        public static DataTable DeserializeDataTable(byte[] serializedTableData, string tableSchema)
        {
            DataTable table = null;
            if (tableSchema != null)
            {
                try
                {
                    table = new DataTable();
                    using (StringReader s = new StringReader(tableSchema))
                    {
                        table.ReadXmlSchema(s);
                        if (serializedTableData != null && serializedTableData.Length > 0)
                        {
                            object[][] itemArrayForRows = (object[][])DeserializeObject(serializedTableData);
                            for (int rowIndex = 0; rowIndex < itemArrayForRows.Length; rowIndex++)
                            {
                                table.Rows.Add(itemArrayForRows[rowIndex]);
                            }
                        }
                    }
                }
                catch
                {
                    if (table != null)
                        table.Dispose();
                    throw;
                }
                //else leave table empty
            }
            return table;
        }

        public static double ParseOrDefault(string numberAsString, double defaultValue, bool useDefaultValueIfNaN)
        {
            double result;
            if (!String.IsNullOrEmpty(numberAsString))
            {
                if (!Double.TryParse(numberAsString, out result))
                    result = defaultValue;
                else if (useDefaultValueIfNaN && Double.IsNaN(result))
                    result = defaultValue;
            }
            else
                result = defaultValue;

            return result;
        }

        public static long IntPairToLong(int left, int right)
        {
            //implicit conversion of left to a long
            long res = left;

            //shift the bits creating an empty space on the right
            // ex: 0x0000CFFF becomes 0xCFFF0000
            res = (res << 32);

            //combine the bits on the right with the previous value
            // ex: 0xCFFF0000 | 0x0000ABCD becomes 0xCFFFABCD
            res = res | (long)(uint)right; //uint first to prevent loss of signed bit

            //return the combined result
            return res;
        }

        public static string AddToNumberAsString(string numberAsString, double numberToAdd)
        {
            string result;
            if (numberAsString == null)
                result = numberToAdd.ToString(CultureInfo.InvariantCulture);
            else
                result = (Double.Parse(numberAsString) + numberToAdd).ToString(CultureInfo.InvariantCulture);

            return result;
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, int randomSeed)
        {
            var rnd = new Random(randomSeed);
            return source.OrderBy((item) => rnd.Next());
        }
        public static void Shuffle<T>(this IList<T> list, int randomSeed)
        {
            var rnd = new Random(randomSeed);
            for(var i = list.Count - 1; i > 0; i--)
                Swap(list, rnd.Next(i + 1), i);
        }

        public static IEnumerable<T> Slice<T>(this T[] source, int start, int end)
        {
            for(var index = start; index <= end && index < source.Length && start >= 0; index++)
                yield return source[index];
        }

        public static IEnumerable<T> Slice<T>(this T[] source, int start)
        {
            for (var index = start; index < source.Length && start >= 0; index++)
                yield return source[index];
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, params T[] items)
        {
            foreach (var sourceItem in source)
                yield return sourceItem;

            foreach (var item in items)
                yield return item;
        }

        static public float Sift3Distance(string s1, string s2, int maxOffset)
        {
            if (String.IsNullOrEmpty(s1))
                return
                String.IsNullOrEmpty(s2) ? 0 : s2.Length;
            if (String.IsNullOrEmpty(s2))
                return s1.Length;
            int c = 0;
            int offset1 = 0;
            int offset2 = 0;
            int lcs = 0;
            while ((c + offset1 < s1.Length) && (c + offset2 < s2.Length))
            {
                if (s1[c + offset1] == s2[c + offset2])
                {
                    lcs++;
                }
                else
                {
                    offset1 = 0;
                    offset2 = 0;
                    for (int i = 0; i < maxOffset; i++)
                    {
                        if ((c + i < s1.Length)
                        && (s1[c + i] == s2[c]))
                        {
                            offset1 = i;
                            break;
                        }
                        if ((c + i < s2.Length)
                        && (s1[c] == s2[c + i]))
                        {
                            offset2 = i;
                            break;
                        }
                    }
                }
                c++;
            }
            return (s1.Length + s2.Length) / 2 - lcs;
        }

        public static IEnumerable<T> IntersectFast<T>(this HashSet<T> set1, HashSet<T> set2)
        {
            if (set1.Count > set2.Count)
                return set2.Where(set1.Contains);
            else
                return set1.Where(set2.Contains);
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> IntersectKeys<TKey, TValue>(this IDictionary<TKey, TValue> set1, IDictionary<TKey, TValue> set2)
        {
            if (set1 == null || set2 == null)
                return Enumerable.Empty<KeyValuePair<TKey, TValue>>();

            if (set1.Count > set2.Count)
            {
                var temp = set1;
                set1 = set2;
                set2 = temp;
            }

            return set1.Where(kvp => set2.ContainsKey(kvp.Key));
        }

        public static bool DoesSetsOverlap<T>(this HashSet<T> set1, HashSet<T> set2)
        {
            if (set1.Count < set2.Count)
                return set2.Overlaps(set1);
            else
                return set1.Overlaps(set2);
        }

        //Purpose of this method is to prevent JIT to remove variables when debugging. So you can use this call instead of Debug.WriteLine.
        public static int DoNothing(object o)
        {
            return o == null ? 0 : 1;
        }

        public static double ParseDoubleInvariant(string doubleString)
        {
            return Double.Parse(doubleString, CultureInfo.InvariantCulture);
        }

        public static string ToIntStringInvariant(this bool b)
        {
            return b ? "1" : "0";
        }

        public static string ToIntStringInvariant(this bool? b)
        {
            if (b.HasValue)
                return ToIntStringInvariant(b.Value);
            else
                return IntMinValueAsString;
        }

        public static string GetDoubleAsString(double number, int maxDecimalPlaces)
        {
            //Error - If correct number of decimal places are not specified,
            //return the number as string
            if (maxDecimalPlaces < 0)
                return number.ToString(CultureInfo.InvariantCulture);

            var sb = new StringBuilder("0.");
            for (int i = 0; i < maxDecimalPlaces; i++)
                sb.Append("#");

            string format = sb.ToString();

            return number.ToString(format);
        }

        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-1 * x));
        }

        // We want to measure how much distribution of lat/longs of feed provider is similar to distribution of lat/longs of other feed providers in the same area
        // Kullback–Leibler divergence is a measure of similarity two Normal distributions: minimized when distributions are the same
        // http://www.allisons.org/ll/MML/KL/Normal/
        public static double KullbackLeiblerDivergenceConfidence(double expectationSelf, double expectationOther, double deviationSelf, double deviationOther)
        {
            double valExp = (expectationSelf - expectationOther);
            double deviation2 = deviationOther * deviationOther;
            double valueKullbackLeibler = Math.Abs(
                                           (valExp * valExp + (deviationSelf * deviationSelf - deviation2))
                                           /
                                           (2 * deviation2) + Math.Log(deviationOther / deviationSelf)
                                          );
            var groupConfidence = 0.5 + Sigmoid(-1 * valueKullbackLeibler); // will be close to 1.0 when distributions are the same, and will be decreasing once distributions are different
            return groupConfidence;
        }

        public static double GetArithmeticStandardMean(IEnumerable<double> values, int valuesCount)
        {
            double result = values.Sum();
            return result / valuesCount;
        }

        public static double GetArithmeticStandardDeviation(IEnumerable<double> values, double expectation, int valuesCount)
        {
            var sum = values.Sum(value => (value - expectation) * (value - expectation));
            return Math.Sqrt(sum / valuesCount);
        }

        public static double GetStandardGeometricMean(IEnumerable<double> values, int valuesCount)
        {
            var product = values.Aggregate(1.0, (total, value) => total * value);
            return Math.Pow(Math.Abs(product), 1.0 / valuesCount); // Math.Pow does not accept negative values
        }

        public static double GetStandardGeometricDeviation(IEnumerable<double> values, double expectationGeometric, int valuesCount)
        {
            double sum = values.Sum(value => Math.Pow(Math.Log(value / expectationGeometric), 2));
            return Math.Exp(Math.Sqrt(sum / valuesCount));
        }

        public static double CalculateZScoreGeometricConfidence(double expectationGeometric, double deviationGeometric, double x, int valuesCount)
        {
            double mlogRatio = Math.Log(x / expectationGeometric);
            double mlogDeviation = Math.Log(deviationGeometric);
            return Math.Abs((mlogRatio + 1) / (mlogDeviation + 1));
        }

        public static double CalculateZScoreConfidence(double expectation, double deviation, double x, int valuesCount)
        {
            double mRatio = Math.Abs(((expectation - x) + 1) / (deviation + 1));
            return mRatio;
        }

        public static string GetGoogleDirectionsUrl(string[] addresses)
        {
            var mapLink = new StringBuilder("http://maps.google.com/maps?saddr={0}&daddr={1}".FormatEx(HttpUtility.UrlEncode(addresses[0]), HttpUtility.UrlEncode(addresses.Length > 1 ? addresses[1] : string.Empty)));

            for (var i = 2; i < addresses.Length; i++)
                mapLink.Append(" to:{0}".FormatEx(HttpUtility.UrlEncode(addresses[i])));

            return mapLink.ToString();
        }

        public static void InitializeArrayElement<T>(T[,] vectors, T initialValue)
        {
            for (var i = 0; i < vectors.GetLength(0); i++)
                for (var j = 0; j < vectors.GetLength(1); j++)
                    vectors[i, j] = initialValue;
        }
        public static void InitializeArrayElement<T>(IList<T> vector, T initialValue)
        {
            for (var i = 0; i < vector.Count; i++)
                vector[i] = initialValue;
        }
    }
}

