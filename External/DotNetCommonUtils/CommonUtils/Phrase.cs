
namespace CommonUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Phrase
    {
        public readonly string Text;
        public readonly string[] Terms;

        public Phrase(string text)
        {
            this.Text = text;
            this.Terms = GetTerms(text);
        }

        public static string[] GetTerms(string text)
        {
            return text.Split(Utils.SpaceDelimiter, StringSplitOptions.RemoveEmptyEntries);
        }

        public static Phrase TextToPhrase(string text)
        {
            return new Phrase(text);
        }

        public static IEnumerable<Phrase> GetPhrases(string phraseTextLines)
        {
            return GetPhrases(phraseTextLines, Phrase.TextToPhrase);
        }

        public static IEnumerable<TPhrase> GetPhrases<TPhrase>(string phraseTextLines, Func<string, TPhrase> createPhrase) where TPhrase:Phrase
        {
            return Utils.GetLines(phraseTextLines)
                .Select(line => createPhrase(line));
        }
    }
}
