using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TermsAndDefinitions.Domain.Stemmers;
using System.Threading.Tasks;

namespace TermsAndDefinitions.Domain
{
    public static class Canonizator
    {
      
            public static IEnumerable<string> GetCanonizedTextWords(string text)
            {
                var rusStemmer = new RussianStemmer();
                var enStemmer = new EnglishStemmer();
                var reg1 = new Regex(@"[\s\p{P}№^\|<>`~$]");
                var words = reg1.Split(text.ToLower()).Where(s => s != string.Empty && !StopWordsFilter.Contains(s));
                foreach (var word in words)
                    if (IsNumbers(word))
                        yield return word;
                    else if (IsRussian(word))
                        yield return rusStemmer.Stem(word);
                    else yield return enStemmer.Stem(word);
            }

            private static List<string> StopWordsFilter
            {
                get
                {

                    var _textStreamReader = new System.IO.StreamReader("Resources\\RussianStopwords.txt", Encoding.ASCII);
                    string russianStopwords = _textStreamReader.ReadToEnd();
                    _textStreamReader.Close();
                    _textStreamReader = new StreamReader("Resources\\EnglishStopwords.txt", Encoding.ASCII);
                    string englishStopwords = _textStreamReader.ReadToEnd();
                    _textStreamReader.Close();
                    List<string> stopWords = new List<string>();
                    stopWords.AddRange(russianStopwords.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));
                    stopWords.AddRange(englishStopwords.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));
                    return stopWords;
                }
            }

            private static bool IsNumbers(string text)
            {
                return Regex.IsMatch(text, @"[\d]");
            }

            private static bool IsRussian(string text)
            {
                return Regex.IsMatch(text, "[а-яА-ЯеЁ]");
            }
        }
    }
