using _2630762_combat_robot_wpf.Models;
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
    /// Interaction logic for CombatPage.xaml
    /// </summary>
    public partial class CombatPage : Page
    {
        private readonly CombatViewModel viewModel;

        public CombatPage()
        {
            InitializeComponent();
            AppData.JeuPage = this;
            AppData.JeuVM = new CombatViewModel(this);
            viewModel = AppData.JeuVM;
            DataContext = viewModel;
        }

        private void Attaquer_Click(object sender, RoutedEventArgs e)
            => viewModel.JouerTour(1);

        private void AttaquePuissante_Click(object sender, RoutedEventArgs e)
            => viewModel.JouerTour(2);

        private void Defendre_Click(object sender, RoutedEventArgs e)
            => viewModel.JouerTour(3);

        private void Recharger_Click(object sender, RoutedEventArgs e)
            => viewModel.JouerTour(4);
    }
}
