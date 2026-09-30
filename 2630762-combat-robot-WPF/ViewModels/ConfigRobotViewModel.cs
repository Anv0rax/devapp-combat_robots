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

        public (int, int, int) InfoInputRobot()
            => (VieInput, ArmureInput, DegatsInput);

        public void EnvoyerRobot()
        {
            ErreurMessage = "Configuration en cours";

            Thread thread = new Thread(() =>
            {
                try
                {
                    (int p, int a, int f) config = (0, 0, 0);

                    if (AppData.instanceClient != null)
                    {
                        config = AppData.instanceClient.ConfigRobot();
                    }
                    else if (AppData.instanceServeur != null)
                    {
                        AppData.instanceServeur.RecevoirConfigClient();
                        config = AppData.instanceServeur.ConfigRobot();
                    }

                    if (!Robot.VerifierPtConfig(config.p, config.a, config.f))
                    {
                        ErreurMessage = "Configuration Invalide";
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            pageAssociee.NavigationService.Navigate(new CombatPage());
                            ErreurMessage = "";
                        });
                    }
                }
                catch (SocketException socex)
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

        private void GererBug()
        {
            if (AppData.instanceServeur != null)
            {
                pageAssociee.NavigationService.Navigate(AppData.AccueilServPage);
                AppData.AccueilServVM.RelancerHebergement();
            }
            else
            {
                pageAssociee.NavigationService.Navigate(new AccueilPage());
            }
        }
    }
}
