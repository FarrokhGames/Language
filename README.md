## An easy-to-use localization solution for Unity3D

<img src="Documentation/language.gif?raw=true" alt="Zenject" width="512px" height="512px"/>

Please note that google translate was involved! :)

---

## Features

- Super simple, driven by ```txt-files``` located in the Resource-folder.
- Supports ```multiple languages with fallback``` for languages missing some of the localized texts.
- Add ```placeholders``` inside your localized text, and fill them from code.
- Defaults to the users ```system language```.
- Contains behaviour for ```automatic localizing``` of your Unity UI Text-fields.

---

## Installation
Simply copy the folder "Unity Project/Assets/Plugins" into your project and you're good to go. Optionally, you can add the folder "Unity Project/Assets/Example" to get started right away.

---

## Language Files
Language-files are simple txt-files containing comments, identifiers, texts and placeholders. The naming convention needs to follow the ```SystemLanguage``` enum found in ```UnityEngine```.

English.txt
```
# This is a comment
my.identifier=A simple text
gold.earned=You have earned {0} gold coins.
```

---

## Usage
The language system is accssible through the static ```Language.cs```.

```cs
var myText = Language.Get("my.identifier"); // Returns "A simple text"
var myParamText = Language.Get("gold.earned", 1000); // Returns "You have earned 1000 gold coins."
```

---

## Changing Language
Its possible to change language at runtime, provided the lanugage you're changing to actually exists and is represented by a txt-file.

```cs
Language.CurrentLangauge = SystemLanguage.Swedish; // Changes current language to swedish
```

---

## Callbacks
Whenever the currrent language changes, the Language.OnLanguageChanged is invoked. This is useful as it lets you update your texts the instant the language changes.

```cs
Language.OnLanguageChanged += () => {
  _goldText.text = Language.Get("gold.earned", _goldEarned);
};
```

---

# Unit Tests
The Language system is tested with more than 60 Unit Tests that can be found in ```LanguageContainerTests.cs```, and ```LanguageManagerTests.cs```.

<img src="Documentation/unittests.png?raw=true" alt="Zenject" width="393px" height="793px"/>

---

## License
    MIT License

    Copyright (c) 2017 Farrokh Games

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
