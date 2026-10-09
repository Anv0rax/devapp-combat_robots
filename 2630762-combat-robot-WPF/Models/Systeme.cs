using _2630762_combat_robot_wpf.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using _2630762_combat_robot_wpf.ViewModels;

namespace _2630762_combat_robot_wpf.Models
{
    public static class Systeme
    {
        public static int SaisieEntier { get; set; }

        public static int SaisirInt()
        {
            return SaisieEntier =- SaisieEntier;
        }

        public static void AfficherPartie(Partie partie)
        {
            FauxMainViewModel.JeuVM.PartieEnCours = partie;
        }

        public static (int, int, int, int) SaisirConfigRobot()
        {
            return FauxMainViewModel.ConfigRobotVM.InfoInputRobot();
        }

        public static IPAddress SaisirIP()
        {
            IPAddress? ip;

            IPAddress.TryParse(FauxMainViewModel.AccueilCliVM.IpInput, out ip);
            if (ip == null)
                ip = new IPAddress([127, 0, 0, 1]);
            return ip;
        }

        public static int JouerAction()
            => FauxMainViewModel.JeuVM.ActionChoisie;

        public static void AfficherEnAttente()
        {
            ;
        }
    }
}
