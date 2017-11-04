using System;
using UnityEngine.UI;

namespace FarrokhGames.Language
{
    public class LanguageTimeParameter : AutoLanguageBehaviour
    {
        protected override void UpdateText(Text text, string identifier)
        {
            UpdateText();
        }

        void Update()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            _text.text = Language.Get(_identifier, time);
        }
    }
}