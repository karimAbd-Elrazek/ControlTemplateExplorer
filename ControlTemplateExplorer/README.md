# 🎨 WPF Control Template Explorer

A powerful, modern WPF application for exploring and examining the XAML templates behind all WPF controls. Perfect for developers who want to understand how WPF controls are constructed internally or need to create custom control templates.

![WPF Control Template Explorer](https://img.shields.io/badge/WPF-.NET%208-blue)
![License](https://img.shields.io/badge/License-MIT-green)
![Version](https://img.shields.io/badge/Version-1.0.0-orange)

## ✨ Features

- **🔍 Comprehensive Control Discovery**: Automatically scans and lists all WPF controls from the PresentationFramework assembly
- **🎯 Smart Search**: Quickly find controls with real-time search functionality
- **📋 Template Visualization**: View the complete XAML template for any WPF control
- **💾 Export Options**: Copy templates to clipboard or save to files
- **🌙 Modern Dark Theme**: Professional, developer-friendly interface
- **📊 Template Analytics**: Shows line count and element statistics
- **⚡ Performance Optimized**: Efficient template loading and display

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Windows OS (WPF requirement)
- Visual Studio 2022 or VS Code (recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/WPF-Control-Template-Explorer.git
   cd WPF-Control-Template-Explorer
   ```

2. **Build the application**
   ```bash
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

### Quick Start

1. Launch the application
2. Browse the list of WPF controls on the left panel
3. Use the search box to quickly find specific controls
4. Click on any control to view its template
5. Copy templates to clipboard or save to files for your projects

## 🖼️ Screenshots

### Main Interface
The application features a clean, modern interface with a control list on the left and template viewer on the right.

### Template View
View beautifully formatted XAML templates with syntax highlighting and detailed information.

## 🛠️ How It Works

The application uses .NET reflection to:

1. **Scan Assemblies**: Examines the PresentationFramework.dll assembly
2. **Discover Controls**: Finds all public, concrete classes that inherit from Control
3. **Instantiate Controls**: Creates instances of controls to access their templates
4. **Extract Templates**: Uses XamlWriter to serialize ControlTemplate objects to XAML
5. **Format Output**: Provides clean, readable XAML with proper indentation

## 📁 Project Structure

```
WPF-Control-Template-Explorer/
├── MainWindow.xaml              # Main UI layout
├── MainWindow.xaml.cs           # Core application logic
├── App.xaml                     # Application definition
├── App.xaml.cs                  # Application startup logic
├── ControlTemplateExplorer.csproj # Project configuration
├── README.md                    # This file
├── LICENSE                      # MIT license
└── .gitignore                   # Git ignore rules
```

## 🔧 Technical Details

- **Framework**: .NET 8.0 Windows
- **UI Technology**: WPF (Windows Presentation Foundation)
- **Architecture**: MVVM-inspired pattern with code-behind
- **Key Technologies**:
  - System.Reflection for type discovery
  - XamlWriter for template serialization
  - ObservableCollection for data binding
  - Modern WPF styling and theming

## 🎯 Use Cases

- **Learning WPF**: Understand how built-in controls are structured
- **Custom Control Development**: Use existing templates as starting points
- **Template Customization**: Extract and modify templates for your applications
- **WPF Education**: Great tool for teaching WPF control architecture
- **Debugging**: Examine control structures when troubleshooting styling issues

## 🤝 Contributing

Contributions are welcome! Here are some ways you can help:

1. **Report Bugs**: Open an issue if you find any problems
2. **Suggest Features**: Ideas for new functionality are appreciated
3. **Submit Pull Requests**: Fix bugs or add new features
4. **Documentation**: Help improve documentation and examples

### Development Setup

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Test thoroughly
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## 📋 Roadmap

- [ ] Syntax highlighting for XAML templates
- [ ] Template comparison functionality
- [ ] Export to different formats (PDF, HTML)
- [ ] Custom control template validation
- [ ] Integration with popular WPF UI libraries
- [ ] Template history and favorites
- [ ] Dark/Light theme toggle

## 🐛 Known Issues

- Some complex controls may not instantiate properly (handled gracefully)
- Very large templates might take a moment to load
- Custom controls from external assemblies are not included

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Inspired by WPF documentation examples
- Built with modern WPF best practices
- Thanks to the WPF community for continuous inspiration

## 📞 Support

If you encounter any issues or have questions:

- **Issues**: [GitHub Issues](https://github.com/karimAbd-Elrazek/WPF-Control-Template-Explorer/issues)
- **Discussions**: [GitHub Discussions](https://github.com/karimAbd-Elrazek/WPF-Control-Template-Explorer/discussions)

## ⭐ Show Your Support

If you find this project helpful, please consider:
- Giving it a ⭐ on GitHub
- Sharing it with other WPF developers
- Contributing to its development

---

**Made with ❤️ for the WPF community**