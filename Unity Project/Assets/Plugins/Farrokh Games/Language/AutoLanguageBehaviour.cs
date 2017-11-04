using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.Language
{
    /// <summary>
    /// Behaviour for automatically localizing a UnityEngine.UI.Text, 
    /// using its initial value as an identifier, when the language changes.
    /// </summary>
    public class AutoLanguageBehaviour : MonoBehaviour
    {
        protected Text _text;
        protected string _identifier;

        void Awake()
        {
            _text = GetComponent<Text>();
            _identifier = _text.text;
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

        private void HandleLanguageChanged()
        {
            UpdateText(_text, _identifier);
        }

        protected virtual void UpdateText(Text text, string identifier)
        {
            text.text = Language.Get(identifier);
        }
    }
}