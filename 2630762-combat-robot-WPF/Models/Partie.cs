using System;
using System.Collections.Generic;
using System.Text;

namespace _2630762_combat_robot_wpf.Models
{
    public class Partie
    {
        public int Status { get; set; } = 2;

        public Robot? RobotServeur { get; set; }

        public Robot? RobotClient { get; set; }

        public string MessageAction { get; set; } = "";

        public Partie(int statut)
        {
            Status = statut;
        }

        public Partie(int statut, Robot robotServeur, Robot robotClient)
        {
            Status = statut;
            RobotServeur = robotServeur;
            RobotClient = robotClient;
        }

        public Partie() { }
    }
}
