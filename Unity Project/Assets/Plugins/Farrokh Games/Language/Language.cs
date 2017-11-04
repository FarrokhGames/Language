using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarrokhGames.Language
{
    /// <summary>
    /// Static class for setting current language, and localizing text
    /// </summary>
    public static class Language
    {
        private const SystemLanguage FALLBACK_LANGUAGE = SystemLanguage.English; // You can change fallback language to anything you like.

        /// <summary>
        /// Invoke when the current language is changed
        /// </summary>
        public static Action OnLanguageChanged;

        private static bool _isLoaded = false;
        private static LanguageManager _manager;

        private static void Init()
        {
            if (!_isLoaded)
            {
                var fallback = Resources.Load<TextAsset>("Languages/" + FALLBACK_LANGUAGE.ToString());
                if (fallback == null)
                {
                    // Make sure the fallback language was not missing
                    throw new System.NullReferenceException("Fallback language (" + FALLBACK_LANGUAGE.ToString() + ") is missing!");
                }

                _manager = new LanguageManager(CreateContainer(fallback));

                _manager.OnCurrentLanguageChanged += () => { if (OnLanguageChanged != null) OnLanguageChanged(); };

                // Add languages
                var textAssets = Resources.LoadAll<TextAsset>("Languages/");
                foreach (var asset in textAssets)
                {
                    if (asset.name != fallback.name)
                    {
                        _manager.AddLanguage(CreateContainer(asset));
                    }
                }

                _isLoaded = true;
            }
        }

        /*
		Creates a language container from given TextAsset
		*/
        private static LanguageContainer CreateContainer(TextAsset textAsset)
        {
            var language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), textAsset.name);
            return new LanguageContainer(language, textAsset.text);
        }

        /// <summary>
        /// Gets or sets the current language
        /// </summary>
        public static SystemLanguage CurrentLanguage
        {
            get
            {
                Init();
                return _manager.CurrentLanguage;
            }
            set
            {
                Init();
                _manager.CurrentLanguage = value;
            }
        }

        /// <summary>
        /// Returns a list of all avaiable languages
        /// </summary>
        public static SystemLanguage[] AllLanguages
        {
            get
            {
                Init();
                return _manager.AllLanguages;
            }
        }

        /// <summary>
        /// Returns true if given identifier exists for this language
        /// </summary>
        /// <param name="id">Identifier</param>
        public static bool Contains(string id)
        {
            Init();
            return _manager.Contains(id);
        }

        /// <summary>
        /// Returns text from given identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Localized string</returns>
        public static string Get(string id)
        {
            Init();
            return _manager.Get(id);
        }

        /// <summary>
        /// Returns text from given identifier, and replace {0}-placeholders with given paramters
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="parameters">Parameters to replace placeholders</param>
        /// <returns>Localized string</returns>
        public static string Get(string id, params object[] parameters)
        {
            Init();
            return _manager.Get(id, parameters);
        }
    }
}