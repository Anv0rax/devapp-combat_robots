using _2630762_combat_robot_wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2630762_combat_robot_wpf.Views
{
    /// <summary>
    /// Interaction logic for AccueilClientPage.xaml
    /// </summary>
    public partial class AccueilClientPage : Page
    {
        private readonly AccueilClientViewModel viewModel;

        public AccueilClientPage()
        {
            InitializeComponent();
            AppData.AccueilCliPage = this;
            AppData.AccueilCliVM = new AccueilClientViewModel(this);
            viewModel = AppData.AccueilCliVM;
            DataContext = viewModel;
        }

        private void Rejoindre_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Rejoindre();
        }

        private void Accueil_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccueilPage());
        }
    }
}
