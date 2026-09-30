using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;


namespace _2630762_combat_robot_wpf.Models
{
    public class Serveur : Joueur
    {
        public Robot CreerRobot(int ptPv, int ptArmure, int ptDegats)
        {
            if (ptPv + ptArmure + ptDegats == 10)
                return new Robot(ptPv, ptArmure, ptDegats);
            else
                throw new ArgumentException("Configuration Invalide !");
        }

        public void LancerHebergement()
        {
            Socket ecoute = Socket!;
            try
            {
                ecoute.Bind(EndPoint!);
                ecoute.Listen(1);
                Socket = ecoute.Accept();
            }
            finally
            {
                ecoute.Dispose();
            }
        }

        public bool VerifAction(int action)
        {
            bool resultat = true;
            resultat &= (action >= 1) && (action <= 4);
            if (action == 2)
            {
                resultat &= Partie.RobotClient.Energie >= 50;
            }
            return resultat;
        }

        public void RecevoirAction()
        {
            byte[] buffer = new byte[4];
            int received = Socket!.Receive(buffer, SocketFlags.None);

            int action = BitConverter.ToInt32(buffer, 0);
            if (VerifAction(action))
            {
                AppliquerAction(false, action);
                Partie!.Statut = 1;
            }
            else
            {
                Partie!.Statut = -11;
            }
        }

        public void AppliquerAction(bool serveur, int action)
        {
            if (serveur)
            {
                switch (action)
                {
                    case 1:
                        Partie!.RobotClient!.SubirDegat(Partie!.RobotServeur!.Force, false);
                        break;
                    case 2:
                        Partie!.RobotClient!.SubirDegat(Partie!.RobotServeur!.Force, true);
                        break;
                    case 3:
                        Partie!.RobotServeur!.Defendre();
                        break;
                    case 4:
                        Partie!.RobotServeur!.Recharger();
                        break;
                }
            }
            else
            {
                switch (action)
                {
                    case 1:
                        Partie!.RobotServeur!.SubirDegat(Partie!.RobotClient!.Force, false);
                        break;
                    case 2:
                        Partie!.RobotServeur!.SubirDegat(Partie!.RobotClient!.Force, true);
                        break;
                    case 3:
                        Partie!.RobotClient!.Defendre();
                        break;
                    case 4:
                        Partie!.RobotClient!.Recharger();
                        break;
                }
            }
        }

        public void JouerTourServeur()
        {
            int action = Systeme.JouerAction();
            bool resultat = true;

            resultat &= (action >= 1) && (action <= 4);
            if (action == 2)
            {
                resultat &= Partie.RobotServeur.Energie >= 50;
            }
            if (Partie!.Statut != -11 && resultat)
            {
                AppliquerAction(true, action);
            }
            else
                Partie!.Statut = -11;
        }

        public void TransmettreMiseAJour()
        {
            if (Partie.RobotServeur.Pv < 1)
                Partie.Statut = -1;
            if (Partie.RobotClient.Pv < 1)
                Partie.Statut = -2;
            string json = JsonSerializer.Serialize(Partie);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            Socket!.Send(bytes, SocketFlags.None);
        }

        public Serveur(string nom, int port) 
        {
            Nom = nom;
            Port = port; 
            EndPoint = new IPEndPoint(IPAddress.Any, Port);
            Socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public static string AvoirIpLocale() // https://stackoverflow.com/questions/6803073/get-local-ip-address
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        public override (int, int, int) ConfigRobot()
        {
            (int pv, int armure, int force) = Systeme.SaisirConfigRobot();
            if (Robot.VerifierPtConfig(pv, armure, force))
            {
                Partie ??= new Partie();
                Partie.RobotServeur = new Robot(pv, armure, force);
            }
            else throw new ArgumentException("Configuration Invalide");
            return (pv,  armure, force);
        }

        public void RecevoirConfigClient()
        {
            Partie ??= new Partie();

            byte[] buffer = new byte[1024];
            int received = Socket!.Receive(buffer);
            string reader = Encoding.UTF8.GetString(buffer, 0, received);

            var options = new JsonSerializerOptions { IncludeFields = true };

            (int pv, int armure, int force) = JsonSerializer.Deserialize<(int, int, int)>(reader, options);

            if (Robot.VerifierPtConfig(pv, armure, force))
            {
                Partie!.RobotClient = new Robot(pv, armure, force);
            }
            else
            {
                Partie!.Statut = -10;
            }
        }

        public int RecevoirDecision()
        {
            byte[] buffer = new byte[4];
            int received = Socket!.Receive(buffer, SocketFlags.None);

            if (received == 0)
                return 2;

            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
