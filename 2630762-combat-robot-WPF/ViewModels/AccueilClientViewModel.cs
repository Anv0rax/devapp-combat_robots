using _2630762_combat_robot_wpf.Models;
using _2630762_combat_robot_wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _2630762_combat_robot_wpf.ViewModels
{
    public class AccueilClientViewModel : BaseViewModel
    {
        private string txtBase = "Rejoindre une partie";
        private int intInput = 50000;

        private string nomInput = "Anonyme";

        private string ipInput = "";

        private string texteBoutonRejoindre;

        private bool joindreEnCours = false;
        CancellationTokenSource annulerRejoindre;
        IPAddress ipTraduite;

        public int IntInput
        {
            get => intInput;
            set
            {
                intInput = value;
                OnPropertyChanged("IntInput");
            }
        }

        public string NomInput
        {
            get => nomInput;
            set
            {
                nomInput = value;
                OnPropertyChanged("NomInput");
            }
        }

        public string IpInput
        {
            get => ipInput;
            set
            {
                ipInput = value;
                OnPropertyChanged("IpInput");
            }
        }

        public string TexteBoutonRejoindre
        {
            get => texteBoutonRejoindre;
            set
            {
                texteBoutonRejoindre = value;
                OnPropertyChanged("TexteBoutonRejoindre");
            }
        }

        public string Ip { get; private set; }

        public AccueilClientViewModel(Page page) : base(page)
        {
            Ip = Serveur.AvoirIpLocale();
            texteBoutonRejoindre = txtBase;
        }

        public void Rejoindre()
        {
            if (!joindreEnCours)
            {
                ipTraduite = Systeme.SaisirIP();
                IpInput = ipTraduite.ToString();

                FauxMainViewModel.instanceClient = new Client(NomInput, IntInput);
                FauxMainViewModel.instanceServeur = null;
                joindreEnCours = true;
                annulerRejoindre = new CancellationTokenSource();
                var annuleToken = annulerRejoindre.Token;

                TexteBoutonRejoindre = "Connexion en cours...";

                Thread thread = new Thread(() =>
                {
                    try
                    {
                        FauxMainViewModel.instanceClient.SeConnecter(ipTraduite, IntInput);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TexteBoutonRejoindre = txtBase;
                            pageAssociee.NavigationService.Navigate(new ConfigRobotPage());
                        });
                        
                    }
                    catch (Exception ex)
                    {
                        if (annuleToken.IsCancellationRequested) return;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TexteBoutonRejoindre = "[ERREUR]";
                            ErreurMessage = $"{ex.Message}";
                        });
                        FauxMainViewModel.instanceClient.Dispose();
                        Thread errorThread = new Thread(() =>
                        {
                            Thread.Sleep(2000);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                TexteBoutonRejoindre = txtBase;
                                ErreurMessage = "";
                            });
                            joindreEnCours = false;
                        });
                        errorThread.IsBackground = true;
                        errorThread.Start();
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                try
                {
                    annulerRejoindre.Cancel();
                    FauxMainViewModel.instanceClient.Dispose();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TexteBoutonRejoindre = "Connexion annulé";
                    });
                    joindreEnCours = false;
                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(2000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TexteBoutonRejoindre = txtBase;
                        });
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TexteBoutonRejoindre = "[ERREUR]";
                        ErreurMessage = $"{ex.Message}";
                    });
                    Thread errorThread = new Thread(() =>
                    {
                        Thread.Sleep(2000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TexteBoutonRejoindre = txtBase;
                            ErreurMessage = "";
                        });
                    });
                    errorThread.IsBackground = true;
                    errorThread.Start();
                }
            }
        }
    }
}
