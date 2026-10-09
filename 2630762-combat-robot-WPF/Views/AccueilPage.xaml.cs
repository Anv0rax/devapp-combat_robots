using _2630762_combat_robot_wpf.Views;
using System.Windows;
using System.Windows.Controls;

namespace _2630762_combat_robot_wpf.Views
{
    public partial class AccueilPage : Page
    {
        public AccueilPage()
        {
            InitializeComponent();
            FauxMainViewModel.ResetApp();
        }

        private void JouerServeur_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccueilServeurPage());
        }

        private void JouerClient_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccueilClientPage());
        }
    }
}
