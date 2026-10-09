using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace _2630762_combat_robot_wpf.Models
{
    public class Robot
    {
        public static readonly int pointsConfig = 30;
        public static readonly int pvDefaut = 100;
        public static readonly int ratioPv = 10;
        public static readonly int armureDefaut = 0;
        public static readonly int ratioArmure = 2;
        public static readonly int degatsDefaut = 10;
        public static readonly int ratioDegats = 2;
        public static readonly int energieDefaut = 0;
        public static readonly int dexteriteDefaut = 10;
        public static readonly int ratioDexterite = 2;
        public static readonly int enerigePourPuissante = 50;
        public static readonly int bonusPuissante = 2;
        public static readonly int bonusArmureTempo = 10;
        public static readonly int probaAttaque = 75;
        public static readonly int probaPuissante = 50;
        public static readonly int probaDefense = 75;

        static Robot()
        {
            string chemin = Path.Combine(AppContext.BaseDirectory, "statsdefaut.json");
            try
            {
                if (File.Exists(chemin))
                {
                    string json = File.ReadAllText(chemin);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var stats = JsonSerializer.Deserialize<StatsRecord>(json, options);

                    if (stats != null)
                    {
                        pointsConfig = stats.PointsConfig ?? pointsConfig;
                        pvDefaut = stats.PvDefaut ?? pvDefaut;
                        ratioPv = stats.RatioPv ?? ratioPv;
                        armureDefaut = stats.ArmureDefaut ?? armureDefaut;
                        ratioArmure = stats.RatioArmure ?? ratioArmure;
                        degatsDefaut = stats.DegatsDefaut ?? degatsDefaut;
                        ratioDegats = stats.RatioDegats ?? ratioDegats;
                        energieDefaut = stats.EnergieDefaut ?? energieDefaut;
                        dexteriteDefaut = stats.DexteriteDefaut ?? dexteriteDefaut;
                        ratioDexterite = stats.RatioDexterite ?? ratioDexterite;
                        enerigePourPuissante = stats.EnerigePourPuissante ?? enerigePourPuissante;
                        bonusPuissante = stats.BonusPuissante ?? bonusPuissante;
                        bonusArmureTempo = stats.BonusArmureTempo ?? bonusArmureTempo;
                        probaAttaque = stats.ProbaAttaque ?? probaAttaque;
                        probaPuissante = stats.ProbaPuissante ?? probaPuissante;
                        probaDefense = stats.ProbaDefense ?? probaDefense;
                    }
                }
            }
            catch (Exception) { }
        }

        public int Pv { get; set; } = pvDefaut;
        public int Armure { get; set; } = armureDefaut;
        public int Degats { get; set; } = degatsDefaut;
        public int Energie { get; set; } = energieDefaut;
        public int ArmureTemporaire { get; set; } = 0;
        public int Dexterite { get; set; } = dexteriteDefaut;

        public void Defendre()
        {
            ArmureTemporaire = bonusArmureTempo + Armure;
        }

        public void Recharger() 
        {
            Energie += enerigePourPuissante;
        }

        public void SubirDegat(int force, bool puissante)
        {
            int degatsBruts = puissante ? force * bonusPuissante : force;
            int degats = Math.Max(0, degatsBruts - (Armure + ArmureTemporaire));
            Pv = Math.Max(0, Pv - degats);
        }

        public static bool VerifierPtConfig(int a, int b, int c, int d)
        {
            return (a + b + c + d) == pointsConfig;
        }

        public static bool VerifierPtConfig((int a, int b, int c, int d) tuple)
        {
            return (tuple.a + tuple.b + tuple.c + tuple.d) == pointsConfig;
        }

        public Robot() { }

        public Robot(int _pv, int _armure, int _force, int _energie, int _dex)
        {
            Pv = _pv;
            Armure = _armure;
            Degats = _force;
            Energie = _energie;
            Dexterite = _dex;
        }

        public Robot(int ptPv, int ptArmure, int ptDegats, int ptDex)
        {
            if (!VerifierPtConfig(ptPv, ptArmure, ptDegats, ptDex))
                throw new ArgumentException("Configuration invalide");

            Pv += ptPv * ratioPv;

            Armure += ptArmure * ratioArmure;

            Degats += ptDegats * ratioDegats;

            Dexterite += ptDex * ratioDexterite;
        }

        private record StatsRecord
        {
            public int? PointsConfig { get; set; }
            public int? PvDefaut { get; set; }
            public int? RatioPv { get; set; }
            public int? ArmureDefaut { get; set; }
            public int? RatioArmure { get; set; }
            public int? DegatsDefaut { get; set; }
            public int? RatioDegats { get; set; }
            public int? EnergieDefaut { get; set; }
            public int? DexteriteDefaut { get; set; }
            public int? RatioDexterite { get; set; }
            public int? EnerigePourPuissante { get; set; }
            public int? BonusPuissante { get; set; }
            public int? BonusArmureTempo { get; set; }
            public int? ProbaAttaque { get; set; }
            public int? ProbaPuissante { get; set; }
            public int? ProbaDefense { get; set; }
        }
    }
}
