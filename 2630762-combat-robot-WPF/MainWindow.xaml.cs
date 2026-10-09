using System.Windows;
using _2630762_combat_robot_wpf.Views;

namespace _2630762_combat_robot_wpf
{
    public partial class MainWindow : Window
    {
        //private Joueur? _client;
        //private Joueur? _serveur;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AccueilPage());
            //Closed += (s, e) => _client?.Dispose();
            //Closed += (s, e) => _serveur?.Dispose();
        }
    }
}
