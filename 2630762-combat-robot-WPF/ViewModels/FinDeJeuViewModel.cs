using _2630762_combat_robot_wpf.Models;
using _2630762_combat_robot_wpf.Views;
using System;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace _2630762_combat_robot_wpf.ViewModels
{
    public class FinDeJeuViewModel : BaseViewModel
    {
        private string titre = "";

        private string sousTitre = "";

        private string couleurTitre = "Gray";

        private bool choixActif = true;

        private bool attenteEnCours = false;

        public string Titre
        {
            get => titre;
            set
            {
                titre = value;
                OnPropertyChanged("Titre");
            }
        }

        public string SousTitre
        {
            get => sousTitre;
            set
            {
                sousTitre = value;
                OnPropertyChanged("SousTitre");
            }
        }

        public string CouleurTitre
        {
            get => couleurTitre;
            set
            {
                couleurTitre = value;
                OnPropertyChanged("CouleurTitre");
            }
        }

        public bool ChoixActif
        {
            get => choixActif;
            set
            {
                choixActif = value;
                OnPropertyChanged("ChoixActif");
            }
        }

        public bool EstServeur => FauxMainViewModel.instanceServeur != null;
        public bool EstClient => FauxMainViewModel.instanceClient != null;

        public FinDeJeuViewModel(Page page) : base(page)
        {
            Partie? partie = FauxMainViewModel.instanceClient?.Partie ?? FauxMainViewModel.instanceServeur?.Partie;

            if ((partie.Status == -2 && EstServeur) || (partie.Status == -1 && EstClient))
            {
                Titre = "Victoire !";
                SousTitre = "Votre robot a gagné !";
                CouleurTitre = "ForestGreen";
            }
            else
            {
                Titre = "Défaite...";
                SousTitre = "Votre robot a été détruit.";
                CouleurTitre = "Firebrick";
            }
        }

        public void AttendreDecision()
        {
            if (!EstServeur || attenteEnCours) 
                return;
            attenteEnCours = true;

            Thread thread = new Thread(() =>
            {
                int decision;
                try
                {
                    decision = FauxMainViewModel.instanceServeur.RejouerPartie();
                }
                catch (Exception)
                {
                    decision = 2;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (decision == 1)
                    {
                        FauxMainViewModel.instanceServeur.Partie = null;
                        pageAssociee.NavigationService.Navigate(new ConfigRobotPage());
                    }
                    else
                    {
                        pageAssociee.NavigationService.Navigate(FauxMainViewModel.AccueilServPage);
                        FauxMainViewModel.AccueilServVM.RelancerHebergement();
                    }
                });
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void Rejouer()
        {
            if (ChoixActif)
            {
                ChoixActif = false;

                try
                {
                    FauxMainViewModel.instanceClient.RejouerPartie(1);
                    FauxMainViewModel.instanceClient.Partie = null;
                    pageAssociee.NavigationService.Navigate(new ConfigRobotPage());
                }
                catch (SocketException socex)
                {
                    pageAssociee.NavigationService.Navigate(new AccueilPage());
                }
                catch (Exception ex)
                {
                    ErreurMessage = $"{ex.Message}";
                    ChoixActif = true;
                }
            }
        }

        public void Quitter()
        {
            if(ChoixActif)
            {
                ChoixActif = false;

                try
                {
                    FauxMainViewModel.instanceClient.RejouerPartie(2);
                }
                catch (Exception) { }

                FauxMainViewModel.ResetApp();
                pageAssociee.NavigationService.Navigate(new AccueilPage());
            }
        }
    }
}