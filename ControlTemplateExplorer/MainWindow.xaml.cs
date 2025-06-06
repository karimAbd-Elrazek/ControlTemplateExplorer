using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using Microsoft.Win32;

namespace ControlTemplateExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ControlTypeInfo> _allControlTypes;
        private ObservableCollection<ControlTypeInfo> _filteredControlTypes;
        private Grid _hiddenGrid; // For instantiating controls

        public MainWindow()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            _filteredControlTypes = new ObservableCollection<ControlTypeInfo>();
            lstTypes.ItemsSource = _filteredControlTypes;

            // Create a hidden grid for instantiating controls
            _hiddenGrid = new Grid();
            _hiddenGrid.Visibility = Visibility.Collapsed;
            ((Grid)this.Content).Children.Add(_hiddenGrid);

            LoadControlTypes();
        }

        private void LoadControlTypes()
        {
            try
            {
                lblStatus.Text = "Loading WPF controls...";

                Type controlType = typeof(Control);
                List<ControlTypeInfo> controlTypes = new List<ControlTypeInfo>();

                // Search all the types in the assembly where the Control class is defined
                Assembly assembly = Assembly.GetAssembly(typeof(Control));
                foreach (Type type in assembly.GetTypes())
                {
                    // Only add a type if it's a Control, a concrete class, and public
                    if (type.IsSubclassOf(controlType) && !type.IsAbstract && type.IsPublic)
                    {
                        controlTypes.Add(new ControlTypeInfo
                        {
                            Type = type,
                            Name = type.Name,
                            Namespace = type.Namespace,
                            FullName = type.FullName
                        });
                    }
                }

                // Sort the types alphabetically by name
                controlTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));

                _allControlTypes = controlTypes;
                UpdateFilteredList("");

                lblStatus.Text = $"Loaded {controlTypes.Count} WPF controls - Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading control types: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "Error loading controls";
            }
        }

        private void UpdateFilteredList(string searchText)
        {
            var filtered = string.IsNullOrEmpty(searchText)
                ? _allControlTypes
                : _allControlTypes.Where(c => c.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                             c.Namespace.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            _filteredControlTypes.Clear();
            foreach (var item in filtered)
            {
                _filteredControlTypes.Add(item);
            }

            lblControlCount.Text = $"{_filteredControlTypes.Count} controls";

            if (_filteredControlTypes.Count == 0 && !string.IsNullOrEmpty(searchText))
            {
                lblControlCount.Text += " (no matches)";
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilteredList(txtSearch.Text);
        }

        private void LstTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstTypes.SelectedItem is ControlTypeInfo selectedType)
            {
                LoadControlTemplate(selectedType);
            }
        }

        private void LoadControlTemplate(ControlTypeInfo controlTypeInfo)
        {
            try
            {
                lblStatus.Text = $"Loading template for {controlTypeInfo.Name}...";
                lblSelectedControl.Text = $"({controlTypeInfo.Name})";

                // Get the selected type
                Type type = controlTypeInfo.Type;

                // Try to instantiate the type
                ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
                if (constructorInfo == null)
                {
                    txtTemplate.Text = $"<< Cannot instantiate {type.Name}: No parameterless constructor found >>";
                    lblTemplateInfo.Text = "Unable to create instance";
                    lblStatus.Text = "Error: No parameterless constructor";
                    return;
                }

                Control control = (Control)constructorInfo.Invoke(null);

                // Add it to the hidden grid
                control.Visibility = Visibility.Collapsed;
                _hiddenGrid.Children.Add(control);

                // Force the control to load its template
                control.ApplyTemplate();

                // Get the template
                ControlTemplate template = control.Template;

                if (template != null)
                {
                    // Get the XAML for the template
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "  ",
                        NewLineChars = "\r\n",
                        NewLineHandling = NewLineHandling.Replace,
                        OmitXmlDeclaration = true
                    };

                    StringBuilder sb = new StringBuilder();
                    using (XmlWriter writer = XmlWriter.Create(sb, settings))
                    {
                        XamlWriter.Save(template, writer);
                    }

                    // Display the template
                    string xamlContent = sb.ToString();
                    txtTemplate.Text = FormatXaml(xamlContent);

                    // Count elements and provide info
                    int elementCount = CountXamlElements(xamlContent);
                    int lineCount = xamlContent.Split('\n').Length;
                    lblTemplateInfo.Text = $"{lineCount} lines, ~{elementCount} elements";

                    lblStatus.Text = $"Template loaded for {controlTypeInfo.Name}";
                }
                else
                {
                    txtTemplate.Text = $"<< {type.Name} has no default template >>";
                    lblTemplateInfo.Text = "No template available";
                    lblStatus.Text = "No template found";
                }

                // Remove the control from the hidden grid
                _hiddenGrid.Children.Remove(control);
            }
            catch (Exception ex)
            {
                txtTemplate.Text = $"<< Error generating template for {controlTypeInfo.Name}: {ex.Message} >>";
                lblTemplateInfo.Text = "Error occurred";
                lblStatus.Text = $"Error loading template: {ex.Message}";

                // Make sure we clean up any control that might have been added
                try
                {
                    if (_hiddenGrid.Children.Count > 0)
                    {
                        _hiddenGrid.Children.Clear();
                    }
                }
                catch { }
            }
        }

        private string FormatXaml(string xaml)
        {
            // Add some basic formatting improvements
            return xaml
                .Replace("><", ">\r\n<")
                .Replace("xmlns:", "\r\n    xmlns:");
        }

        private int CountXamlElements(string xaml)
        {
            // Simple count of opening tags
            int count = 0;
            for (int i = 0; i < xaml.Length - 1; i++)
            {
                if (xaml[i] == '<' && xaml[i + 1] != '/' && xaml[i + 1] != '!')
                {
                    count++;
                }
            }
            return count;
        }

        private void BtnCopyTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTemplate.Text))
                {
                    Clipboard.SetText(txtTemplate.Text);
                    lblStatus.Text = "Template copied to clipboard";

                    // Briefly change button text to show success
                    var originalContent = btnCopyTemplate.Content;
                    btnCopyTemplate.Content = "✓ Copied!";

                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(2);
                    timer.Tick += (s, args) =>
                    {
                        btnCopyTemplate.Content = originalContent;
                        timer.Stop();
                    };
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("No template to copy.", "Information",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying to clipboard: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSaveTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTemplate.Text))
                {
                    MessageBox.Show("No template to save.", "Information",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "XAML Files (*.xaml)|*.xaml|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = "xaml",
                    FileName = lstTypes.SelectedItem is ControlTypeInfo selected ?
                              $"{selected.Name}Template.xaml" : "ControlTemplate.xaml"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveDialog.FileName, txtTemplate.Text, Encoding.UTF8);
                    lblStatus.Text = $"Template saved to {Path.GetFileName(saveDialog.FileName)}";

                    // Briefly change button text to show success
                    var originalContent = btnSaveTemplate.Content;
                    btnSaveTemplate.Content = "✓ Saved!";

                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(2);
                    timer.Tick += (s, args) =>
                    {
                        btnSaveTemplate.Content = originalContent;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            txtTemplate.Text = "";
            lblSelectedControl.Text = "";
            lblTemplateInfo.Text = "";
            lstTypes.SelectedItem = null;
            LoadControlTypes();
        }
    }

    /// <summary>
    /// Information about a WPF control type
    /// </summary>
    public class ControlTypeInfo : INotifyPropertyChanged
    {
        private Type _type;
        private string _name;
        private string _namespace;
        private string _fullName;

        public Type Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Namespace
        {
            get => _namespace;
            set
            {
                _namespace = value;
                OnPropertyChanged(nameof(Namespace));
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}