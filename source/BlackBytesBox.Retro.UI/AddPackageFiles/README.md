# BlackBytesBox.Retro.UI

A lightweight WinForms control library that brings retro‑inspired UI enhancements and an embedded terminal experience to legacy projects — shipped as a **.NET 2.0‑compatible NuGet package**.

---

## ✨ Feature Overview

| Control | Purpose | Highlights |
|---------|---------|------------|
| **GifSpinner** | Animated loading indicator | Transparency‑aware · Scales to fit · Easy start/stop |
| **CmdHostControl** | Embedded interactive `cmd.exe` | ANSI fallback · Safe designer detection · Send commands programmatically |
| **PlaceholderTextBox** | `TextBox` with placeholder text | Custom hide‑on‑focus · Flicker‑free paint |
| **RichTextBoxExtendend** | Log/output viewer | Buffered append queue · Auto‑scroll delay · Optional caret hiding |
| **VerticalLabel** | Rotated text label | 90° by default · Adjustable angle · ClearType antialiasing |

---

## 🚀 Quick Start

```csharp
// Spinner
var spinner = new GifSpinner {
    GifImage = Image.FromFile("spinner.gif"),
    Dock     = DockStyle.Fill
};
spinner.Start();

// Embedded shell
var shell = new CmdHostControl { Dock = DockStyle.Fill };
shell.SendCommand("dir");
```

---

## 📦 Installation

Using the Package Manager Console:

```
Install-Package BlackBytesBox.Retro.UI
```

Or via .NET CLI:

```
dotnet add package BlackBytesBox.Retro.UI
```

---

## 🛠 Requirements

- **Framework**: .NET Framework 2.0 or newer  
- **Platform**: Windows (WinForms)

*Every control gracefully degrades on very old runtimes while still feeling modern in current Visual Studio.*

---

## 🤔 Why Choose This Library?

- **Legacy Friendly** – Works on .NET 2.0 without losing modern coding ergonomics  
- **Zero External Dependencies** – Pure WinForms; drop the DLL into any solution  
- **Open Source** – MIT licensed, contributions welcome

---

## ⚖️ License

Released under the MIT License — free to use, modify, and distribute. Attribution appreciated.

---

## 👤 Maintainer

**BlackBytesBox** — Crafting future‑proof tools for past‑proof platforms.
