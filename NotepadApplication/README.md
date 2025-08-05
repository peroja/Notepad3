
# Notepad 3

**Notepad 3** is a feature-rich text editor for Windows, built with C# and Windows Forms. It is an open-source project licensed under the MIT License and aims to provide a modern alternative to traditional notepad applications with additional features like syntax highlighting, PDF export, and optional registration.

---

## Features

- ✅ Syntax highlighting for multiple languages: C#, VB, HTML, PHP, JS, SQL, Lua, XML
- 💾 Open, edit, and save any file
- 🎨 Customize text color, background color, and font
- 🖨️ Export content directly to a PDF file
- 🔎 Search, Replace, Go To Line, Undo/Redo support
- 🔐 Registration system: data saved to Windows Registry and sent to a web server
- 🌍 Online version checking and update prompt
- 🌐 HTML preview with dual browser control

---

## Project Structure

- `Form1.cs`: Main window with editor and menu
- `frmInfo.cs`: Info form displaying version and links
- `frmRegister.cs`: Registration form with Registry save and HTTP POST to server
- `HTMLPreview.cs`: HTML display using WebBrowser control
- `Program.cs`: Application entry point
- `*.php`: Backend scripts for registration

---

## Registration

Upon first launch:
- A unique **SysId** is generated and saved in the Windows Registry.
- The ID is sent to a remote server.
- Users can register their name and email via the registration form, which:
  - Stores data locally in the Registry
  - Sends it to the server at `register.php`

---

## License

This project is licensed under the MIT License. See source file headers for details.

---

## Dependencies

- .NET Framework / .NET Core Desktop Runtime
- [PdfSharpCore](https://github.com/ststeiger/PdfSharpCore)
- [FastColoredTextBox](https://github.com/PavelTorgashov/FastColoredTextBox)

---

## Building the Project

1. Open the solution in Visual Studio
2. Restore NuGet packages if required
3. Build and run the project

---

## Notes

Some parts of the application interface and code comments are in German.
