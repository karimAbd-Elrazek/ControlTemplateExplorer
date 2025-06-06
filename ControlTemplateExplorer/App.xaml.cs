using System;
using System.Windows;

namespace ControlTemplateExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up global exception handling
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An unexpected error occurred:\n\n{e.Exception.Message}",
                          "WPF Control Template Explorer - Error",
                          MessageBoxButton.OK,
                          MessageBoxImage.Error);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"A critical error occurred:\n\n{e.ExceptionObject}",
                          "WPF Control Template Explorer - Critical Error",
                          MessageBoxButton.OK,
                          MessageBoxImage.Error);
        }
    }
}