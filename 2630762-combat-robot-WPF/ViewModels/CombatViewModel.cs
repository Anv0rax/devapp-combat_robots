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
                FauxMainViewModel.instanceClient?.RecevoirMiseAJour();
                FauxMainViewModel.instanceServeur?.TransmettreMiseAJour();
            }
            catch(SocketException socex)
            {
                if (FauxMainViewModel.instanceServeur != null)
                {
                    FauxMainViewModel.instanceServeur.LancerHebergement();
                    pageAssociee.NavigationService.Navigate(FauxMainViewModel.AccueilServPage);
                    return;
                }
                pageAssociee.NavigationService.Navigate(new AccueilPage());
                return;
            }

            PartieEnCours = FauxMainViewModel.instanceClient?.Partie ?? FauxMainViewModel.instanceServeur?.Partie;

            if(PartieEnCours == null || PartieEnCours.Status == -10)
            {
                pageAssociee.NavigationService.Navigate(new ConfigRobotPage());
            }

        }

        private bool EstServeur => FauxMainViewModel.instanceServeur != null;
        private Joueur? JoueurLocal 
        { 
            get => (Joueur?)FauxMainViewModel.instanceClient ?? FauxMainViewModel.instanceServeur; 
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

        private string messageAction = "";
        public string MessageAction
        {
            get => messageAction;
            set
            {
                messageAction = value;
                OnPropertyChanged("MessageAction");
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
                        if (FauxMainViewModel.instanceClient != null)
                        {
                            FauxMainViewModel.instanceClient.EnvoyerAction(Systeme.JouerAction());
                            FauxMainViewModel.instanceClient.RecevoirMiseAJour();
                        }
                        else if (FauxMainViewModel.instanceServeur != null)
                        {
                            FauxMainViewModel.instanceServeur.RecevoirAction();
                            FauxMainViewModel.instanceServeur.TransmettreMiseAJour();
                        }

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            PartieEnCours = JoueurLocal?.Partie;
                            switch (PartieEnCours.Status)
                            {
                                case -11:
                                    ActionsActives = true;
                                    MessageStatut = "Action invalide, recommencer le tour";
                                    break;
                                case 1:
                                    MessageStatut = "";
                                    MessageAction = partieEnCours.MessageAction;
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
                    catch (Exception)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (EstServeur)
                            {

                                FauxMainViewModel.AccueilServVM.RelancerHebergement();
                                pageAssociee.NavigationService.Navigate(FauxMainViewModel.AccueilServPage);
                            }
                            else
                            {
                                pageAssociee.NavigationService.Navigate(new AccueilPage());
                            }
                        });
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch(Exception)
            {
                if (EstServeur)
                {
                    FauxMainViewModel.AccueilServVM.RelancerHebergement();
                    pageAssociee.NavigationService.Navigate(FauxMainViewModel.AccueilServPage);
                }
                else
                {
                    pageAssociee.NavigationService.Navigate(new AccueilPage());
                }
            }
        }

        private void AllerFinDeJeu()
        {
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(500);
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
