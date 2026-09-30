using _2630762_combat_robot_wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace _2630762_combat_robot_wpf.Views
{
    /// <summary>
    /// Interaction logic for AccueilServeurPage.xaml
    /// </summary>
    public partial class AccueilServeurPage : Page
    {
        private readonly AccueilServeurViewModel viewModel;

        public AccueilServeurPage()
        {
            InitializeComponent();
            AppData.AccueilServPage = this;
            AppData.AccueilServVM = new AccueilServeurViewModel(this);
            viewModel = AppData.AccueilServVM;
            DataContext = viewModel;
        }

        private void Heberger_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.LancerHebergement();
        }

        private void Accueil_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccueilPage());
        }
    }
}
