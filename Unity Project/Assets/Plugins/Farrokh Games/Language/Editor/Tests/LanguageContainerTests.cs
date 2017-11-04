using NUnit.Framework;
using UnityEngine;

namespace FarrokhGames.Language
{
    [TestFixture]
    public class LanguageContainerTests
    {
        private string GetLanguageFile()
        {
            return
            "# TestFile \n" +
            "test1=Hello\n" +
            "test2=World\n" +
            "test3=Longer text with spaces!\n" +
            "test4=Text with one {0} parameter.\n" +
            "test5=Text with two {0}, {1} parameters.\n" +
            "test6=Text with three {0}, {1}, {2} parameters.\n"+
            "test7=Text with mixed {1}, {2}, {0} parameters.\n"+
            "test8=Text with high {2} parameter.";
        }

        [Test]
        public void CTOR_Success()
        {
            Assert.DoesNotThrow(() => new LanguageContainer(SystemLanguage.English, string.Empty));
        }

        [Test]
        public void CTOR_UnidenticalIDs_Exception()
        {
            var wrongfile = "test1=Hello\ntest1=World\ntest2=Hello World";
            Assert.Throws<System.FormatException>(() => new LanguageContainer(SystemLanguage.English, wrongfile));
        }

        [TestCase(SystemLanguage.English)]
        [TestCase(SystemLanguage.Swedish)]
        [TestCase(SystemLanguage.French)]
        [TestCase(SystemLanguage.German)]
        public void Language_Correct(SystemLanguage language)
        {
            var container = new LanguageContainer(language, GetLanguageFile());
            Assert.That(container.Language, Is.EqualTo(language));
        }

        [Test]
        public void UsedCharacters_Correct()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            Assert.That(container.UsedCharacters.Length, Is.EqualTo(30));
        }

        [Test]
        public void Count_Correct()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            Assert.That(container.Count, Is.EqualTo(8));
        }

        [TestCase("test1", ExpectedResult = true)]
        [TestCase("test3", ExpectedResult = true)]
        [TestCase("test6", ExpectedResult = true)]
        [TestCase("herp", ExpectedResult = false)]
        [TestCase("derp", ExpectedResult = false)]
        [TestCase("lerp", ExpectedResult = false)]
        public bool Contains_ReturnsCorrect(string id)
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            return container.Contains(id);
        }

        [TestCase("test1", ExpectedResult = "Hello")]
        [TestCase("test2", ExpectedResult = "World")]
        [TestCase("test3", ExpectedResult = "Longer text with spaces!")]
        public string Get_CorrectID_ReturnsString(string id)
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            return container.Get(id);
        }

        [TestCase("herp", ExpectedResult = "**!herp!**")]
        [TestCase("totallywrong", ExpectedResult = "**!totallywrong!**")]
        [TestCase("NoNoThatsWrong", ExpectedResult = "**!NoNoThatsWrong!**")]
        public string Get_IncorrectID_ReturnsErroredString(string id)
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            return container.Get(id);
        }

        [Test]
        public void Get_WithOneParameter_ReturnsParameterizedString()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            Assert.That(container.Get("test4", "awesome"), Is.EqualTo("Text with one awesome parameter."));
        }
        
        [Test]
        public void Get_WithTwoParameters_ReturnsParameterizedString()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            Assert.That(container.Get("test5", "cool", "and great"), Is.EqualTo("Text with two cool, and great parameters."));
        }

                
        [Test]
        public void Get_WithThreeParameters_ReturnsParameterizedString()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            Assert.That(container.Get("test6", "icy", "sweet", "and pretty"), Is.EqualTo("Text with three icy, sweet, and pretty parameters."));
        }

        [Test]
        public void Get_WithMixedParameters_ReturnsParameterizedString()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            Assert.That(container.Get("test7", "icy", "sweet", "and pretty"), Is.EqualTo("Text with mixed sweet, and pretty, icy parameters."));
        }

                [Test]
        public void Get_WithHighParameter_ReturnsParameterizedString()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            Assert.That(container.Get("test8", "icy", "sweet", "and pretty"), Is.EqualTo("Text with high and pretty parameter."));
        }

        [Test]
        public void Get_WithWrongNumberOfParamters_FormatException()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            container.Get("test1", "wrongparam");
        }

        [Test]
        public void Get_WidthParameters_IncorrectID_ReturnsErroredString()
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            var text = container.Get("wrongtext", "param");
            Assert.That(text, Is.EqualTo("**!wrongtext!**"));
        }

        [TestCase("test1", ExpectedResult = false)]
        [TestCase("test2", ExpectedResult = false)]
        [TestCase("test3", ExpectedResult = false)]
        [TestCase("test4", ExpectedResult = true)]
        [TestCase("test5", ExpectedResult = true)]
        [TestCase("test6", ExpectedResult = true)]
        [TestCase("test7", ExpectedResult = true)]
        [TestCase("test8", ExpectedResult = true)]
        [TestCase("derp", ExpectedResult = false)]
        public bool HasParameter_ReturnsCorrect(string id)
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            return container.HasParameter(id);
        }

        [TestCase("test1", ExpectedResult = -1)]
        [TestCase("test2", ExpectedResult = -1)]
        [TestCase("test3", ExpectedResult = -1)]
        [TestCase("test4", ExpectedResult = 0)]
        [TestCase("test5", ExpectedResult = 1)]
        [TestCase("test6", ExpectedResult = 2)]
        [TestCase("test7", ExpectedResult = 2)]
        [TestCase("test8", ExpectedResult = 2)]
        [TestCase("derp", ExpectedResult = -1)]
        public int HighestParameter_ReturnsCorrectNumber(string id)
        {
            var container = new LanguageContainer(SystemLanguage.English, GetLanguageFile());
            return container.HighestParameter(id);
        }
    }
}