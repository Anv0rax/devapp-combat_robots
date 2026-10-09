using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace _2630762_combat_robot_wpf.Models
{
    public class Client : Joueur
    {
        public Client(string nom, int port)
        {
            Port = port;
            Nom = nom;
        }

        public void SeConnecter(IPAddress ip, int port)
        {
            EndPoint = new IPEndPoint(ip, port);
            Socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(EndPoint);
        }

        public void EnvoyerAction(int typeAction)
        {
            byte[] msg = { (byte)typeAction };
            Socket!.Send(msg, SocketFlags.None);
        }

        public void RecevoirMiseAJour()
        {
            byte[] buffer = new byte[1024];
            int received = Socket.Receive(buffer, SocketFlags.None);
            string json = Encoding.UTF8.GetString(buffer, 0, received);
            Partie = JsonSerializer.Deserialize<Partie>(json);
        }

        public override void ConfigRobot()
        {
            (int pv, int armure, int force, int dex) data = Systeme.SaisirConfigRobot();

            var options = new JsonSerializerOptions { IncludeFields = true };

            string json = JsonSerializer.Serialize(data, options);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            Socket!.Send(bytes);
        }

        public void RejouerPartie(int decision)
        {
            byte[] msg = { (byte)decision };
            Socket!.Send(msg, SocketFlags.None);
        }
    }
}