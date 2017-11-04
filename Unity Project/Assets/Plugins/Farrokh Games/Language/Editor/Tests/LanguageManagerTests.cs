using System;
using NUnit.Framework;
using UnityEngine;

namespace FarrokhGames.Language
{
    [TestFixture]
    public class LanguageManagerTests
    {
        private LanguageContainer GetEnglish()
        {
            return new LanguageContainer(SystemLanguage.English,
            "# English \n" +
            "test1=Short text\n" +
            "test2=Text with one {0} parameter.\n" +
            "test3=English only text\n" +
            "test4=English only text with parameter {0}\n"
            );
        }

        private LanguageContainer GetSwedish()
        {
            return new LanguageContainer(SystemLanguage.Swedish,
            "# Swedish \n" +
            "test1=Kort text\n" +
            "test2=Text med en {0} parameter.\n"
            );
        }

        [Test]
        public void CTOR_Success()
        {
            var manager = new LanguageManager(GetEnglish());
            Assert.That(manager.AllLanguages.Length, Is.EqualTo(1));
        }

        [Test]
        public void CTOR_NullFallback_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new LanguageManager(null));
        }

        [Test]
        public void AddLanguage_Success()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            Assert.That(manager.AllLanguages.Length, Is.EqualTo(2));
        }

        [Test]
        public void AddLanguage_Twice_Exception()
        {
            var manager = new LanguageManager(GetEnglish());
            Assert.Throws<InvalidOperationException>(() => manager.AddLanguage(GetEnglish()));
        }

        [Test]
        public void CurrentLanguage_Set_Success()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.CurrentLanguage, Is.EqualTo(SystemLanguage.Swedish));
        }

        [Test]
        public void CurrentLanguage_SetToNonExisting_Exception()
        {
            var manager = new LanguageManager(GetEnglish());
            Assert.Throws<InvalidOperationException>(() => manager.CurrentLanguage = SystemLanguage.Afrikaans);
        }

        [Test]
        public void AllLanguages_ReturnsCorrectLanguageCount()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            Assert.That(manager.AllLanguages.Length, Is.EqualTo(2));
        }

        [Test]
        public void Contains_Correct_ReturnsTrue()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Contains("test1"), Is.True);
        }

        [Test]
        public void Contains_Incorrect_ReturnsFalse()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Contains("derp"), Is.False);
        }

        [Test]
        public void Contains_FallbackOnly_ReturnsTrue()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Contains("test3"), Is.True);
        }

        [Test]
        public void Get_Correct_ReturnsTrue()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Get("test1"), Is.EqualTo("Kort text"));
        }

        [Test]
        public void Get_Incorrect_ReturnsFalse()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Get("derp"), Is.EqualTo("**!derp!**"));
        }

        [Test]
        public void Get_FallbackOnly_ReturnsTrue()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Get("test3"), Is.EqualTo("English only text"));
        }

        [Test]
        public void Get_WithParameter_Correct_ReturnsTrue()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Get("test2", "test"), Is.EqualTo("Text med en test parameter."));
        }

        [Test]
        public void Get_WithParameter_Incorrect_ReturnsFalse()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Get("derp", "test"), Is.EqualTo("**!derp!**"));
        }

        [Test]
        public void Get_WithParameter_FallbackOnly_ReturnsTrue()
        {
            var manager = new LanguageManager(GetEnglish());
            manager.AddLanguage(GetSwedish());
            manager.CurrentLanguage = SystemLanguage.Swedish;
            Assert.That(manager.Get("test4", "test"), Is.EqualTo("English only text with parameter test"));
        }
    }
}