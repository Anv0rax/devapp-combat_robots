using _2630762_combat_robot_wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _2630762_combat_robot_wpf.Views
{
    /// <summary>
    /// Interaction logic for FinDeJeu.xaml
    /// </summary>
    public partial class FinDeJeuPage : Page
    {
        private readonly FinDeJeuViewModel viewModel;

        public FinDeJeuPage()
        {
            InitializeComponent();
            FauxMainViewModel.FinDeJeuPage = this;
            FauxMainViewModel.FinDeJeuVM = new FinDeJeuViewModel(this);
            viewModel = FauxMainViewModel.FinDeJeuVM;
            DataContext = viewModel;

            // On attend que la page soit affichée avant de toucher au réseau / à la navigation
            Loaded += (s, e) => viewModel.AttendreDecision();
        }

        private void Rejouer_Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Rejouer();
        }

        private void Quitter_Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Quitter();
        }
    }
}