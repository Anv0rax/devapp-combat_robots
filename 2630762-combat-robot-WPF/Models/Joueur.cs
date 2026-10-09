using System.Net;
using System.Net.Sockets;

namespace _2630762_combat_robot_wpf.Models
{
    public abstract class Joueur : IDisposable
    {
        private bool _disposed = false;
        public string Nom { get; set; } = "";
        public int Port { get; set; }
        public IPEndPoint? EndPoint { get; set; }
        public Socket? Socket { get; set; }
        public Partie? Partie { get; set; }

        public abstract void ConfigRobot();

        public void Dispose()
        {
            if(!_disposed)
            {
                if(Socket != null)
                {
                    try
                    {
                        Socket.Shutdown(SocketShutdown.Both);
                    }
                    catch (SocketException) { }
                    Socket.Dispose();
                    Socket = null;
                }
                GC.SuppressFinalize(this);
            }
        }
    }
}
