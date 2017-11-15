using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarrokhGames.Language
{
    /// <summary>
    /// Keeps track of current language and enables fallback if an identifier is not found in current language.
    /// </summary>
    public class LanguageManager
    {
        /// <summary>
        /// Invoke when the current language is changed
        /// </summary>
        public Action OnCurrentLanguageChanged;

        private LanguageContainer _fallbackLanguage;
        private Dictionary<SystemLanguage, LanguageContainer> _containers;
        private SystemLanguage _currentLanguage = Application.systemLanguage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fallbackLanguageContainer">The container of the language to fallback to if an identifier is not found in current language</param>
        public LanguageManager(LanguageContainer fallbackLanguageContainer)
        {
            if (fallbackLanguageContainer == null) { throw new ArgumentNullException("fallbackLanguageContainer"); }
            _fallbackLanguage = fallbackLanguageContainer;
            _containers = new Dictionary<SystemLanguage, LanguageContainer>();
            AddLanguage(_fallbackLanguage);
        }

        /// <summary>
        /// Adds a language container to this manager
        /// </summary>
        /// <param name="container">The container to add</param>
        public void AddLanguage(LanguageContainer container)
        {
            if (_containers.ContainsKey(container.Language))
            {
                throw new InvalidOperationException("The language (" + container.Language.ToString() + ") have alread been added");
            }
            _containers.Add(container.Language, container);
        }

        /// <summary>
        /// Gets or sets the current language of this manager
        /// </summary>
        public SystemLanguage CurrentLanguage
        {
            get { return _containers.ContainsKey(_currentLanguage) ? _currentLanguage : _fallbackLanguage.Language; }
            set
            {
                if (_containers.ContainsKey(value))
                {
                    var isNew = value != _currentLanguage;
                    _currentLanguage = value;
                    if (isNew && OnCurrentLanguageChanged != null) { OnCurrentLanguageChanged(); }
                }
                else
                {
                    throw new InvalidOperationException("Trying to switch to language that does not exist (" + value.ToString() + ")");
                }
            }
        }

        /// <summary>
        /// Returns a list of all avaiable languages
        /// </summary>
        public SystemLanguage[] AllLanguages
        {
            get { return _containers.Keys.ToArray(); }
        }

        /// <summary>
        /// Returns true if given identifier exists for this language
        /// </summary>
        /// <param name="id">Identifier</param>
        public bool Contains(string id)
        {
            if (_containers[CurrentLanguage].Contains(id))
            {
                return true;
            }
            return _fallbackLanguage.Contains(id);
        }

        /// <summary>
        /// Returns text from given identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Localized string</returns>
        public string Get(string id)
        {
            if (_containers[CurrentLanguage].Contains(id))
            {
                return _containers[CurrentLanguage].Get(id);
            }
            return _fallbackLanguage.Get(id);
        }

        /// <summary>
        /// Returns text from given identifier, and replace {0}-placeholders with given paramters
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="parameters">Parameters to replace placeholders</param>
        /// <returns>Localized string</returns>
        public string Get(string id, params object[] parameters)
        {
            if (_containers[CurrentLanguage].Contains(id))
            {
                return _containers[CurrentLanguage].Get(id, parameters);
            }
            return _fallbackLanguage.Get(id, parameters);
        }
    }
}