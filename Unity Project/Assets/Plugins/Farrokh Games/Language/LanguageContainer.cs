using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FarrokhGames.Language
{
    /// <summary>
    /// Class for storing information about a specific language
    /// </summary>
    public class LanguageContainer
    {
        /// <summary>
        /// Returns the language of this container
        /// </summary>
        public SystemLanguage Language { get; private set; }

        /// <summary>
        /// Returns a list of characters used by this language
        /// </summary>
        public Char[] UsedCharacters { get; private set; }

        /// <summary>
        /// Returns the number of entries for this language
        /// </summary>
        public int Count { get { return _dictionary.Count; } }

        private Dictionary<string, string> _dictionary;
        private Dictionary<string, int> _highestParam;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="language">The Language of this container</param>
        /// <param name="file">The language file to parse</param>
        public LanguageContainer(SystemLanguage language, string file)
        {
            Create(language, file);
        }

        /*
        Loop through the language file and store texts along with their id's in the dictionary
        */
        private void Create(SystemLanguage language, string file)
        {
            Language = language;
            _dictionary = new Dictionary<string, string>();
            _highestParam = new Dictionary<string, int>();
            var charList = new List<Char>();
            if (!string.IsNullOrEmpty(file))
            {
                using (var reader = new System.IO.StringReader(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length > 0 && line[0] != '#')
                        {
                            var bits = line.Split(new char[] { '=' }, 2, StringSplitOptions.None);
                            if (bits != null && bits.Length == 2)
                            {
                                string id = bits[0];
                                string txt = bits[1];

                                // Look for ID duplicates
                                if (_dictionary.ContainsKey(id))
                                {
                                    throw new FormatException("Error while building language (" + Language.ToString() + ") the id " + id + " was found multiple times. This is now allowed!");
                                }

                                _dictionary.Add(id, txt.Replace("\\n", "\n").Replace("\\t", "\t"));

                                // Count params
                                Match match = Regex.Matches(txt,
                                @"(?<!\{)\{(?<number>[0-9]+).*?\}(?!\})")
                                .Cast<Match>()
                                .OrderBy(m => m.Groups["number"].Value)
                                .LastOrDefault();
                                var highestParam = -1;
                                if (match != null)
                                {
                                    highestParam = int.Parse(match.Groups["number"].Value);
                                    _highestParam.Add(id, highestParam);
                                }

                                // Look for unique characters
                                var charArray = txt.ToCharArray();
                                for (var i = 0; i < charArray.Length; i++)
                                {
                                    var c = charArray[i];
                                    if (!charList.Contains(c))
                                    {
                                        charList.Add(c);
                                    }
                                }
                            }
                        }
                    }
                }
                UsedCharacters = charList.ToArray();
            }
        }

        /// <summary>
        /// Returns true if given identifier exists for this language
        /// </summary>
        /// <param name="id">Identifier</param>
        public bool Contains(string id)
        {
            return _dictionary.ContainsKey(id);
        }

        /// <summary>
        /// Returns text from given identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Localized string</returns>
        public string Get(string id)
        {
            return Contains(id) ? _dictionary[id] : string.Format("**!{0}!**", id);
        }

        /// <summary>
        /// Returns text from given identifier, and replace {0}-placeholders with given paramters
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="parameters">Parameters to replace placeholders</param>
        /// <returns>Localized string</returns>
        public string Get(string id, params object[] parameters)
        {
            return string.Format(Get(id), parameters);
        }

        /// <summary>
        /// Returns true if the text of the given identifier contains parameters
        /// </summary>
        /// <param name="id">Identifier</param>
        public bool HasParameter(string id)
        {
            if (_highestParam.ContainsKey(id))
            {
                return _highestParam[id] >= 0;
            }
            return false;
        }

        /// <summary>
        /// Returns the highest parameter number contained in the text of the given identifier. Returns -1 if no parameter was found.
        /// </summary>
        /// <param name="id">Identifier</param>
        public int HighestParameter(string id)
        {
            if (HasParameter(id))
            {
                return _highestParam[id];
            }
            return -1;
        }
    }
}