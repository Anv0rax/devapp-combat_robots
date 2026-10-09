using _2630762_combat_robot_wpf.Models;
using _2630762_combat_robot_wpf.Views;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _2630762_combat_robot_wpf.ViewModels
{
    public class ConfigRobotViewModel : BaseViewModel
    {
        public ConfigRobotViewModel(Page page) : base(page) { }

        public int PointsConfig { get => Robot.pointsConfig; }

        private int vieInput = 0;

        public int VieInput
        {
            get => vieInput;
            set
            {
                vieInput = value;
                OnPropertyChanged("VieInput");
            }
        }

        private int forceInput = 0;

        public int DegatsInput
        {
            get => forceInput;
            set
            {
                forceInput = value;
                OnPropertyChanged("DegatsInput");
            }
        }

        private int armureInput = 0;

        public int ArmureInput
        {
            get => armureInput;
            set
            {
                armureInput = value;
                OnPropertyChanged("ArmureInput");
            }
        }

        private int dexInput = 0;

        public int DexInput
        {
            get => dexInput;
            set
            {
                dexInput = value;
                OnPropertyChanged("DexInput");
            }
        }

        public (int, int, int, int) InfoInputRobot()
            => (VieInput, ArmureInput, DegatsInput, DexInput);

        public void EnvoyerRobot()
        {
            ErreurMessage = "Configuration en cours";

            Thread thread = new Thread(() =>
            {
                try
                {
                    (int p, int a, int f, int d) config = (0, 0, 0, 0);


                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        config = Systeme.SaisirConfigRobot();

                        if (!Robot.VerifierPtConfig(config.p, config.a, config.f, config.d))
                        {
                            ErreurMessage = "Configuration Invalide";
                        }
                        else
                        {
                            ErreurMessage = "";
                            if (FauxMainViewModel.instanceClient != null)
                            {
                                FauxMainViewModel.instanceClient.ConfigRobot();
                            }
                            else if (FauxMainViewModel.instanceServeur != null)
                            {
                                FauxMainViewModel.instanceServeur.RecevoirConfigClient();
                                FauxMainViewModel.instanceServeur.ConfigRobot();
                            }
                            pageAssociee.NavigationService.Navigate(new CombatPage());
                        }
                    });
                }
                catch (SocketException)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        GererBug();
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ErreurMessage = $"{ex.Message}";
                    });

                    Thread errorThread = new Thread(() =>
                    {
                        Thread.Sleep(2000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            GererBug();
                        });
                    });
                    errorThread.IsBackground = true;
                    errorThread.Start();
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void GererBug()
        {
            if (FauxMainViewModel.instanceServeur != null)
            {
                pageAssociee.NavigationService.Navigate(FauxMainViewModel.AccueilServPage);
                FauxMainViewModel.AccueilServVM.RelancerHebergement();
            }
            else
            {
                pageAssociee.NavigationService.Navigate(new AccueilPage());
            }
        }
    }

 
}
