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

        public bool EstServeur => AppData.instanceServeur != null;
        public bool EstClient => AppData.instanceClient != null;

        public FinDeJeuViewModel(Page page) : base(page)
        {
            Partie? partie = AppData.instanceClient?.Partie ?? AppData.instanceServeur?.Partie;

            if ((partie.Status == -2 && EstServeur) || (partie.Status == -1 && EstClient))
            {
                Titre = "Victoire !";
                SousTitre = "Votre robot est sorti vainqueur du combat.";
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
                    decision = AppData.instanceServeur.RecevoirDecision();
                }
                catch (Exception)
                {
                    decision = 2;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (decision == 1)
                    {
                        AppData.instanceServeur.Partie = null;
                        pageAssociee.NavigationService.Navigate(new ConfigRobotPage());
                    }
                    else
                    {
                        pageAssociee.NavigationService.Navigate(AppData.AccueilServPage);
                        AppData.AccueilServVM.RelancerHebergement();
                    }
                });
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void Rejouer()
        {
            if (!ChoixActif) return;
            ChoixActif = false;

            try
            {
                AppData.instanceClient.EnvoyerDecision(1);
                AppData.instanceClient.Partie = null;
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

        public void Quitter()
        {
            if (!ChoixActif) return;
            ChoixActif = false;

            try
            {
                AppData.instanceClient.EnvoyerDecision(2);
            }
            catch (Exception) { }

            AppData.ResetApp();
            pageAssociee.NavigationService.Navigate(new AccueilPage());
        }
    }
}