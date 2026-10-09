using System.Windows;

namespace _2630762_combat_robot_wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += (s, args) =>
            {
                Fermeture();
                args.Handled = true;
            };

            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                Fermeture();
            };

            TaskScheduler.UnobservedTaskException += (s, args) =>
            {
                args.SetObserved();
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Fermeture();
            base.OnExit(e);
        }
        private void Fermeture()
        {

        }
    }

}
