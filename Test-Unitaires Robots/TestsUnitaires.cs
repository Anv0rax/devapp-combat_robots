using _2630762_combat_robot_wpf.Models;
using System.Net;

namespace Test_Unitaires_Robots
{
    public class TestsConnexionClient
    {
        [Fact]
        public void Correcte()
        {
            Serveur serv = new Serveur("Serveur", 50000);

            Thread thread = new Thread(() => serv.LancerHebergement());
            thread.IsBackground = true;
            thread.Start();
            Thread.Sleep(200);

            bool connexionReussie;

            try
            {
                Client cli = new Client("Client", 50000);
                IPAddress ip = new IPAddress([127, 0, 0, 1]);
                cli.SeConnecter(ip, 50000);
                connexionReussie = true;
                cli.Dispose();
            }
            catch (Exception) { connexionReussie = false; }

            Assert.True(connexionReussie);
            serv.Dispose();
        }

        [Fact]
        public void PortIncorrecte()
        {
            Serveur serv = new Serveur("Serveur", 50001);

            Thread thread = new Thread(() => serv.LancerHebergement());
            thread.IsBackground = true;
            thread.Start();
            Thread.Sleep(200);

            bool connexionReussie;

            try
            {
                Client cli = new Client("Client", 40000);
                IPAddress ip = new IPAddress([127, 0, 0, 1]);
                cli.SeConnecter(ip, 40000);
                connexionReussie = true;
                cli.Dispose();
            }
            catch (Exception) { connexionReussie = false; }

            Assert.False(connexionReussie);
            serv.Dispose();
        }

        [Fact]
        public void IpIncorrecte()
        {
            Serveur serv = new Serveur("Serveur", 50002);

            Thread thread = new Thread(() => serv.LancerHebergement());
            thread.IsBackground = true;
            thread.Start();
            Thread.Sleep(200);

            bool connexionReussie;

            try
            {
                Client cli = new Client("Client", 50002);
                IPAddress ip = new IPAddress([1, 1, 1, 1]);
                cli.SeConnecter(ip, 50002);
                connexionReussie = true;
                cli.Dispose();
            }
            catch (Exception) { connexionReussie = false; }

            Assert.False(connexionReussie);
            serv.Dispose();
        }
    }








    public class TestsEnvoieAction
    {
        [Fact]
        public void Correcte()
        {
            int actionATester = 1;

            Serveur serv = new Serveur("Serveur", 50003);

            Thread thread = new Thread(() => serv.LancerHebergement());
            thread.IsBackground = true;
            thread.Start();
            Thread.Sleep(200);

            bool resultat;

            try
            {
                Client cli = new Client("Client", 50003);
                IPAddress ip = new IPAddress([127, 0, 0, 1]);
                cli.SeConnecter(ip, 50003);

                serv.Partie = new Partie();

                cli.EnvoyerAction(actionATester);
                serv.RecevoirAction();
                resultat = true;
                cli.Dispose();
            }
            catch (Exception) { resultat = false; }
            finally
            {
                resultat = serv.Partie.Status == 1;
            }

            Assert.True(resultat);
            serv.Dispose();
        }

        [Fact]
        public void Incorrecte()
        {
            int actionATester = 9;

            Serveur serv = new Serveur("Serveur", 50004);

            Thread thread = new Thread(() => serv.LancerHebergement());
            thread.IsBackground = true;
            thread.Start();
            Thread.Sleep(200);

            bool resultat;

            try
            {
                Client cli = new Client("Client", 50004);
                IPAddress ip = new IPAddress([127, 0, 0, 1]);
                cli.SeConnecter(ip, 50004);

                serv.Partie = new Partie();

                cli.EnvoyerAction(actionATester);
                serv.RecevoirAction();
                resultat = true;
                cli.Dispose();
            }
            catch (Exception) { resultat = false; }
            finally
            {
                resultat = serv.Partie.Status == -11;
            }

            Assert.True(resultat);
            serv.Dispose();
        }
    }
}