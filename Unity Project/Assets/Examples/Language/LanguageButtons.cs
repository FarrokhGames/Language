using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.Language
{
    public class LanguageButtons : MonoBehaviour
    {
        [SerializeField] private Button _prefabButton;

        private Dictionary<SystemLanguage, Button> _buttons;

        void Awake()
        {
            _buttons = new Dictionary<SystemLanguage, Button>();
            foreach (var language in Language.AllLanguages)
            {
                var btn = GameObject.Instantiate(_prefabButton.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<Button>();
                btn.GetComponentInChildren<Text>().text = language.ToString();
                btn.onClick.AddListener(() => HandleClicked(language));
                _buttons.Add(language, btn);
            }
            GameObject.Destroy(_prefabButton.gameObject);
        }

        void OnEnable()
        {
            Language.OnLanguageChanged += HandleLanguageChanged;
            HandleLanguageChanged();
        }

        void OnDisable()
        {
            Language.OnLanguageChanged -= HandleLanguageChanged;
        }

        void HandleClicked(SystemLanguage language)
        {
            Language.CurrentLanguage = language;
        }

        void HandleLanguageChanged()
        {
            foreach (var kvp in _buttons)
            {
                kvp.Value.interactable = Language.CurrentLanguage != kvp.Key;
            }
        }
    }
}