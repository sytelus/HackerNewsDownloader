
namespace CommonUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class PhraseSet<TPhrase> : IEnumerable<TPhrase>
        where TPhrase : Phrase
    {
        private readonly IDictionary<string, IList<TPhrase>> termToPhraseIndex = new Dictionary<string, IList<TPhrase>>();
        private readonly IDictionary<string, TPhrase> textToPhraseMap = new Dictionary<string, TPhrase>();

        public PhraseSet()
        {
        }

        public PhraseSet(IEnumerable<TPhrase> phrases) : base()
        {
            this.AddRange(phrases);
        }

        private static readonly IList<TPhrase> EmptyPhraseList = new List<TPhrase>(1);
        public bool ContainsAll(IEnumerable<string> terms, Func<TPhrase, bool> phrasePredicate)
        {
            return MatchingPhrases(terms, phrasePredicate).Any();
        }

        public IEnumerable<string> MatchingPhraseTerms(IEnumerable<string> terms, Func<TPhrase, bool> phrasePredicate)
        {
            return this.MatchingPhrases(terms, phrasePredicate).SelectMany(phrase => phrase.Terms);
        }

        public IEnumerable<TPhrase> MatchingPhrases(IEnumerable<string> phraseTerms, Func<TPhrase, bool> phrasePredicate)
        {
            IDictionary<TPhrase, uint> phraseTermCounts = null;
            foreach (var phraseTerm in phraseTerms)
            {
                var potentialPhrases = termToPhraseIndex.GetValueOrDefault(phraseTerm);
                if (potentialPhrases == null)
                    continue;

                foreach (var phrase in potentialPhrases)
                {
                    if (phrasePredicate == null || phrasePredicate(phrase))
                    {
                        if (phraseTermCounts == null)
                            phraseTermCounts = new Dictionary<TPhrase, uint>();

                        phraseTermCounts.IncrementCountInDictionary(phrase);
                    }
                }
            }

            if (phraseTermCounts == null)
                return Enumerable.Empty<TPhrase>();
            else
                return phraseTermCounts.Where(kvp => kvp.Key.Terms.Length == kvp.Value).Select(kvp => kvp.Key);

            /* 
            // shorter linq version but not as much performant
            return phraseTerms
                .SelectMany(phraseTerm => termToPhraseIndex.GetValueOrDefault(phraseTerm, EmptyPhraseList))
                .Where(phrase => phrasePredicate == null || phrasePredicate(phrase))
                .GroupBy(phrase => phrase)
                .Where(phraseGroup => phraseGroup.Count() == phraseGroup.Key.Terms.Length)
                .Select(phraseGroup => phraseGroup.Key);
             */
        }

        public int PhraseCount
        {
            get { return this.textToPhraseMap.Count; }
        }

        public void AddPhraseSets<TPhraseSet>(params TPhraseSet[] otherPhraseSets) where TPhraseSet : PhraseSet<TPhrase>
        {
            foreach (var otherPhraseSet in otherPhraseSets)
            {
                foreach (var otherPhraseSetKvp in otherPhraseSet.termToPhraseIndex)
                {
                    var existingValue = this.termToPhraseIndex.GetValueOrDefault(otherPhraseSetKvp.Key);

                    if (existingValue == null)
                        this.termToPhraseIndex.Add(otherPhraseSetKvp.Key, otherPhraseSetKvp.Value);
                    else
                        existingValue.AddRange(otherPhraseSetKvp.Value.Where(p => !this.termToPhraseIndex.ContainsKey(p.Text)));
                }

                this.textToPhraseMap.AddRange(otherPhraseSet.textToPhraseMap.Where(kvp => !this.textToPhraseMap.ContainsKey(kvp.Key)));
            }
        }

        public TPhraseSet Clone<TPhraseSet>(Func<TPhrase, bool> phrasePredicate) where TPhraseSet:PhraseSet<TPhrase>, new() 
        {
            var extractedPhrases = this.textToPhraseMap.Values.Where(phrase => phrasePredicate == null || phrasePredicate(phrase));
            var clonedSet = new TPhraseSet();
            clonedSet.AddRange(extractedPhrases);

            return clonedSet;
        }

        public void Add(TPhrase phrase)
        {
            if (!this.textToPhraseMap.ContainsKey(phrase.Text))
                this.AddInternal(phrase);
        }

        private void AddInternal(TPhrase phrase)
        {
            foreach (var phraseTerm in phrase.Terms)
            {
                var existingValue = this.termToPhraseIndex.GetValueOrDefault(phraseTerm);
                if (existingValue == null)
                {
                    existingValue = new List<TPhrase>();
                    this.termToPhraseIndex.Add(phraseTerm, existingValue);
                }

                existingValue.Add(phrase);
            }

            this.textToPhraseMap.Add(phrase.Text, phrase);
        }

        public string[] Add(string phraseText, Func<string, TPhrase> createPhrase)
        {
            var phrase = this.textToPhraseMap.GetValueOrDefault(phraseText);
            if (phrase == null)
            {
                phrase = createPhrase(phraseText);
                this.AddInternal(phrase);
            }

            return phrase.Terms;
        }

        public void AddRange(IEnumerable<TPhrase> phrases)
        {
            foreach (var phrase in phrases)
                this.Add(phrase);
        }


        public void Remove(IEnumerable<string> phraseTexts)
        {
            foreach (var phraseText in phraseTexts)
                this.Remove(phraseText);
        }

        public bool Remove(string phraseText)
        {
            var phrase = this.textToPhraseMap.GetValueOrDefault(phraseText);
            if (phrase == null) 
                return false;

            foreach (var phraseTerm in phrase.Terms)
            {
                this.termToPhraseIndex[phraseTerm].Remove(phrase);  //O(n) operation

                if (this.termToPhraseIndex[phraseTerm].Count == 0)
                    this.termToPhraseIndex.Remove(phraseTerm);
            }

            this.textToPhraseMap.Remove(phrase.Text);

            return true;
        }



        #region Implementation of IEnumerable

        IEnumerator<TPhrase> IEnumerable<TPhrase>.GetEnumerator()
        {
            return this.textToPhraseMap.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.textToPhraseMap.Values.GetEnumerator();
        }

        #endregion
    }


}
