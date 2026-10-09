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
        private readonly Random random = new Random();

        public Robot CreerRobot(int ptPv, int ptArmure, int ptDegats, int ptDex)
        {
            if (ptPv + ptArmure + ptDegats == Robot.pointsConfig)
                return new Robot(ptPv, ptArmure, ptDegats, ptDex);
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
                resultat &= Partie.RobotClient.Energie >= Robot.enerigePourPuissante;
            }
            return resultat;
        }

        public void RecevoirAction()
        {
            byte[] buffer = new byte[1];
            int received = Socket!.Receive(buffer, SocketFlags.None);

            int action = buffer[0];
            if (VerifAction(action))
            {
                Partie!.Status = 1;
                AppliquerAction(action, JouerTourServeur());
            }
            else
            {
                Partie!.Status = -11;
            }
        }

        public void AppliquerAction(int actionClient, int actionServeur)
        {
            if(Partie!.Status > 0)
            {
                Partie.MessageAction = "";
                if(actionClient == 3)
                {
                    if (CalculerReussite(Partie.RobotClient.Dexterite, Partie.RobotServeur.Dexterite, Robot.probaDefense))
                    {
                        Partie!.RobotClient!.Defendre();
                        Partie.MessageAction += "Le client défend\n";
                    }
                    else
                        Partie.MessageAction += "Le client a raté sa défense !\n";
                }
                if (actionServeur == 3)
                {
                    if (CalculerReussite(Partie.RobotServeur.Dexterite, Partie.RobotClient.Dexterite, Robot.probaDefense))
                    {    
                        Partie!.RobotServeur!.Defendre();
                        Partie.MessageAction += "Le serveur défend\n";
                    }
                    else
                        Partie.MessageAction += "Le serveur a raté sa défense !\n";
                }
                switch (actionServeur)
                {
                    case 1:
                        if (CalculerReussite(Partie.RobotServeur.Dexterite, Partie.RobotClient.Dexterite, Robot.probaAttaque) )
                        {    
                            Partie!.RobotClient!.SubirDegat(Partie!.RobotServeur!.Degats, false);
                            Partie.MessageAction += "Le serveur attaque\n";
                        }
                        else
                            Partie.MessageAction += "Le serveur a raté son attaque\n";
                        break;
                    case 2:
                        if (CalculerReussite(Partie.RobotServeur.Dexterite, Partie.RobotClient.Dexterite, Robot.probaPuissante) )
                        {    
                            Partie!.RobotClient!.SubirDegat(Partie!.RobotServeur!.Degats, true);
                            Partie.MessageAction += "Le serveur fait une attaque puissante\n";
                        }
                        else
                            Partie.MessageAction += "Le serveur a raté son attaque puissante\n";
                        break;
                    case 4:
                        Partie!.RobotServeur!.Recharger();
                        Partie.MessageAction += "Le serveur recharge\n";
                        break;
                }
                switch (actionClient)
                {
                    case 1:
                        if (CalculerReussite(Partie.RobotClient.Dexterite, Partie.RobotServeur.Dexterite, Robot.probaAttaque))
                        {    
                            Partie!.RobotServeur!.SubirDegat(Partie!.RobotClient!.Degats, false);
                            Partie.MessageAction += "Le client attaque\n";
                        }
                        else
                            Partie.MessageAction += "Le client a raté son attaque\n";
                        break;
                    case 2:
                        if (CalculerReussite(Partie.RobotClient.Dexterite, Partie.RobotServeur.Dexterite, Robot.probaPuissante))
                        {
                            Partie!.RobotServeur!.SubirDegat(Partie!.RobotClient!.Degats, true);
                            Partie.MessageAction += "Le client fait une attaque puissante\n";
                        }
                        else
                            Partie.MessageAction += "Le client a raté son attaque puissante\n";
                        break;
                    case 4:
                        Partie!.RobotClient!.Recharger();
                        Partie.MessageAction += "Le client recharge\n";
                        break;
                }
                Partie!.RobotServeur!.ArmureTemporaire = 0;
                Partie!.RobotClient!.ArmureTemporaire = 0;
                Partie!.Status = 1;
            }
        }

        public int JouerTourServeur()
        {
            int action = Systeme.JouerAction();
            bool resultat = true;

            resultat &= (action >= 1) && (action <= 4);
            if (action == 2)
            {
                resultat &= Partie.RobotServeur.Energie >= Robot.enerigePourPuissante;
            }
            if (Partie!.Status != -11 && resultat)
            {
                return action;
            }
            else
            {
                Partie!.Status = -11;
                return 0;
            }
        }

        public void TransmettreMiseAJour()
        {
            //if (Partie == null || Partie.RobotClient == null || Partie.RobotServeur == null)
            //    throw new ApplicationException("Partie vide");

            if (Partie.RobotServeur.Pv < 1 || Partie.RobotClient.Pv < 1)
            {
                if (Partie.RobotServeur.Pv < 1)
                    Partie.Status = -1;
                if (Partie.RobotClient.Pv < 1)
                    Partie.Status = -2;

                if (Partie.RobotServeur.Pv < 1 && Partie.RobotClient.Pv < 1)
                {
                    int diffDex = Partie.RobotServeur.Dexterite - Partie.RobotClient.Dexterite;
                    if(diffDex > 0)
                        Partie.Status = -2;
                    else
                        Partie.Status = -1;
                }
            }
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

        public override void ConfigRobot()
        {
            (int pv, int armure, int force, int dex) = Systeme.SaisirConfigRobot();
            if (Robot.VerifierPtConfig(pv, armure, force, dex))
            {
                Partie ??= new Partie(2);
                Partie.RobotServeur = new Robot(pv, armure, force, dex);
            }
            else throw new ArgumentException("Configuration Invalide");
        }

        public void RecevoirConfigClient()
        {
            Partie ??= new Partie(2);

            byte[] buffer = new byte[1024];
            int received = Socket!.Receive(buffer);
            string reader = Encoding.UTF8.GetString(buffer, 0, received);

            var options = new JsonSerializerOptions { IncludeFields = true };

            (int pv, int armure, int force, int dex) = JsonSerializer.Deserialize<(int, int, int, int)>(reader, options);

            if (Robot.VerifierPtConfig(pv, armure, force, dex))
            {
                Partie!.RobotClient = new Robot(pv, armure, force, dex);
                Partie.Status = 1;
            }
            else
            {
                Partie!.Status = -10;
            }
        }

        public int RejouerPartie()
        {
            byte[] buffer = new byte[1];
            int received = Socket!.Receive(buffer, SocketFlags.None);

            if (received == 0)
                return 2;

            return buffer[0];
        }

        private bool CalculerReussite(int DexAttaquant, int DexDefenseur, int probaDefaut)
        {
            int diffDex = DexAttaquant - DexDefenseur;

            int chanceReussite = probaDefaut + (diffDex * 5);

            int result = Math.Clamp(chanceReussite, 30, 95);

            int hasard = random.Next(1, 101);

            return hasard <= result;
        }

    }
}
