using _2630762_combat_robot_wpf.Models;
using _2630762_combat_robot_wpf.Views;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace _2630762_combat_robot_wpf.ViewModels
{
    public class CombatViewModel : BaseViewModel
    {
        public CombatViewModel(Page page) : base(page) 
        {
            try
            {
                AppData.instanceClient?.RecevoirPartie();
                AppData.instanceServeur?.TransmettreMiseAJour();
            }
            catch(SocketException socex)
            {
                if (AppData.instanceServeur != null)
                {
                    AppData.instanceServeur.LancerHebergement();
                    pageAssociee.NavigationService.Navigate(AppData.AccueilServPage);
                    return;
                }
                pageAssociee.NavigationService.Navigate(new AccueilPage());
                return;
            }

            PartieEnCours = AppData.instanceClient?.Partie ?? AppData.instanceServeur?.Partie;

            if(PartieEnCours == null || PartieEnCours.Status == -10)
            {
                pageAssociee.NavigationService.Navigate(new ConfigRobotPage());
            }

        }

        private bool EstServeur => AppData.instanceServeur != null;
        private Joueur? JoueurLocal 
        { 
            get => (Joueur?)AppData.instanceClient ?? AppData.instanceServeur; 
        }


        public Robot? RobotJoueur
        {
            get => EstServeur ? PartieEnCours?.RobotServeur : PartieEnCours?.RobotClient;
        }

        public Robot? RobotAdverse 
        {
            get => EstServeur ? PartieEnCours?.RobotClient : PartieEnCours?.RobotServeur;
        }

        public int ActionChoisie { get; private set; }


        private Partie partieEnCours;

        public Partie PartieEnCours
        {
            get => partieEnCours;
            set
            {
                partieEnCours = value;
                OnPropertyChanged("PartieEnCours");
                OnPropertyChanged("RobotJoueur");
                OnPropertyChanged("RobotAdverse");
            }
        }

        private bool actionsActives = true;

        public bool ActionsActives
        {
            get => actionsActives;
            set 
            { 
                actionsActives = value; 
                OnPropertyChanged("ActionsActives"); 
            }
        }

        private string messageStatut = "";

        public string MessageStatut
        {
            get => messageStatut;
            set 
            { 
                messageStatut = value; 
                OnPropertyChanged("MessageStatut"); 
            }
        }

        public void JouerTour(int action)
        {
            try
            {
                ActionChoisie = action;
                ActionsActives = false;
                MessageStatut = "En attente de l'adversaire";

                Thread thread = new Thread(() =>
                {
                    try
                    {
                        if (AppData.instanceClient != null)
                        {
                            AppData.instanceClient.EnvoyerAction(Systeme.JouerAction());
                            AppData.instanceClient.RecevoirPartie();
                        }
                        else if (AppData.instanceServeur != null)
                        {
                            AppData.instanceServeur.RecevoirAction();
                            AppData.instanceServeur.JouerTourServeur();
                            AppData.instanceServeur.TransmettreMiseAJour();
                        }

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            PartieEnCours = JoueurLocal?.Partie;
                            PartieEnCours.RobotServeur.ArmureTemporaire = 0;
                            PartieEnCours.RobotClient.ArmureTemporaire = 0;
                            switch (PartieEnCours.Status)
                            {
                                case -11:
                                    ActionsActives = true;
                                    MessageStatut = "Action invalide, recommencer le tour";
                                    break;
                                case 1:
                                    MessageStatut = "";
                                    ActionsActives = true;
                                    break;
                                case -2:
                                    MessageStatut = "";
                                    AllerFinDeJeu();
                                    break;
                                case -1:
                                    MessageStatut = "";
                                    AllerFinDeJeu();
                                    break;
                            }
                        });
                    }
                    catch (SocketException)
                    {
                        AppData.AccueilServVM.RelancerHebergement();
                        pageAssociee.NavigationService.Navigate(AppData.AccueilServPage);
                    }
                    catch (Exception ex)
                    {
                        MessageStatut = $"{ex.Message}";
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch(SocketException)
            {
                if (EstServeur)
                {
                    AppData.AccueilServVM.RelancerHebergement();
                    pageAssociee.NavigationService.Navigate(AppData.AccueilServPage);
                }
            }
        }

        private void AllerFinDeJeu()
        {
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(2000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    pageAssociee.NavigationService.Navigate(new FinDeJeuPage());
                });
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
