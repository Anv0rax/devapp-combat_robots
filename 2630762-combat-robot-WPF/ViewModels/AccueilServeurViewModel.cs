using _2630762_combat_robot_wpf.Models;
using _2630762_combat_robot_wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _2630762_combat_robot_wpf.ViewModels
{
    public class AccueilServeurViewModel : BaseViewModel
    {
        private string txtBase = "Lancer l'hébergement";

        private int intInput = 50000;

        private string nomInput = "Anonyme";

        private string texteBoutonHebergement;

        private bool hebergementEnCours = false;

        CancellationTokenSource annulerHebergement;

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

        public string TexteBoutonHebergement
        {
            get => texteBoutonHebergement;
            set
            {
                texteBoutonHebergement = value;
                OnPropertyChanged("TexteBoutonHebergement");
            }
        }

        public string Ip { get; private set; }

        public AccueilServeurViewModel(Page page) : base(page)
        {
            Ip = Serveur.AvoirIpLocale();
            texteBoutonHebergement = txtBase;
        }

        public void LancerHebergement()
        {
            if (!hebergementEnCours)
            {
                AppData.instanceServeur = new Serveur(NomInput, IntInput);
                AppData.instanceClient = null;
                hebergementEnCours = true;
                annulerHebergement = new CancellationTokenSource();
                var annuleToken = annulerHebergement.Token;

                TexteBoutonHebergement = "En Attente...";

                Thread thread = new Thread(() =>
                {
                    try
                    {
                        AppData.instanceServeur.LancerHebergement();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            pageAssociee.NavigationService.Navigate(new ConfigRobotPage());
                        });
                        
                    }
                    catch (Exception ex)
                    {
                        if (annuleToken.IsCancellationRequested) return;

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TexteBoutonHebergement = "[ERREUR]";
                            ErreurMessage = $"{ex.Message}";
                        });

                        AppData.instanceServeur.Dispose();
                        Thread errorThread = new Thread(() =>
                        {
                            Thread.Sleep(2000);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                TexteBoutonHebergement = txtBase;
                                ErreurMessage = "";
                            });
                            hebergementEnCours = false;
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
                    annulerHebergement.Cancel();
                    AppData.instanceServeur.Dispose();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TexteBoutonHebergement = "Hébergement annulé";
                    });
                    hebergementEnCours = false;
                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(2000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TexteBoutonHebergement = txtBase;
                        });
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TexteBoutonHebergement = "[ERREUR]";
                        ErreurMessage = $"{ex.Message}";
                    });
                    Thread errorThread = new Thread(() =>
                    {
                        Thread.Sleep(2000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TexteBoutonHebergement = txtBase;
                            ErreurMessage = "";
                        });
                    });
                    errorThread.IsBackground = true;
                    errorThread.Start();
                }
            }
        }
        public void RelancerHebergement()
        {
            AppData.instanceServeur?.Dispose();
            hebergementEnCours = false;
            ErreurMessage = "";
            TexteBoutonHebergement = txtBase;
            LancerHebergement();
        }
    }
}
