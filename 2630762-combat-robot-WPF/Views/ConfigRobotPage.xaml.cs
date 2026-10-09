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
    /// Interaction logic for ConfigRobotPage.xaml
    /// </summary>
    public partial class ConfigRobotPage : Page
    {
        private readonly ConfigRobotViewModel viewModel;

        public ConfigRobotPage()
        {
            InitializeComponent();
            FauxMainViewModel.ConfigRobPage = this;
            FauxMainViewModel.ConfigRobotVM = new ConfigRobotViewModel(this);
            viewModel = FauxMainViewModel.ConfigRobotVM;
            DataContext = viewModel;
        }

        private void ConfirmRobot_Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.EnvoyerRobot();
        }
    }
}
