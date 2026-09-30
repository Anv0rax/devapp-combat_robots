using System;
using System.Collections.Generic;
using System.Text;

namespace _2630762_combat_robot_wpf.Models
{
    public class Partie
    {
        public int Statut { get; set; }

        public Robot? RobotServeur { get; set; }

        public Robot? RobotClient { get; set; }

        public Partie(int statut)
        {
            Statut = statut;
        }

        public Partie(int statut, Robot robotServeur, Robot robotClient)
        {
            Statut = statut;
            RobotServeur = robotServeur;
            RobotClient = robotClient;
        }

        public Partie() { }
    }
}
