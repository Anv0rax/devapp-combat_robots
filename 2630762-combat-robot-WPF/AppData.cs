using _2630762_combat_robot_wpf.Models;
using _2630762_combat_robot_wpf.ViewModels;
using _2630762_combat_robot_wpf.Views;

namespace _2630762_combat_robot_wpf
{
    public static class FauxMainViewModel
    {
        public static void ResetApp()
        {
            instanceClient?.Dispose();
            instanceServeur?.Dispose();
        }

        public static Client? instanceClient { get; set; }
        public static Serveur? instanceServeur { get; set; }

        // =======================================
        //                  CLIENT
        // =======================================

        public static AccueilClientViewModel AccueilCliVM {get; set; }

        public static AccueilClientPage AccueilCliPage { get; set; }

        // =======================================
        //                  SERVEUR
        // =======================================

        public static AccueilServeurViewModel AccueilServVM { get; set; }

        public static AccueilServeurPage AccueilServPage { get; set; }

        // =======================================

        public static ConfigRobotViewModel ConfigRobotVM { get; set; }

        public static ConfigRobotPage ConfigRobPage { get; set; }


        public static CombatViewModel JeuVM { get; set; }

        public static CombatPage JeuPage { get; set; }

        public static FinDeJeuViewModel FinDeJeuVM { get; set; }

        public static FinDeJeuPage FinDeJeuPage { get; set; }
    }
}
