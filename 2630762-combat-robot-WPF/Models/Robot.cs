using System;
using System.Collections.Generic;
using System.Text;

namespace _2630762_combat_robot_wpf.Models
{
    public class Robot
    {
        public int Pv { get; set; } = 100;
        public int Armure { get; set; } = 0;
        public int Force { get; set; } = 10;
        public int Energie { get; set; } = 0;
        public int ArmureTemporaire { get; set; } = 0;

        public void Defendre()
        {
            ArmureTemporaire = 10;
        }

        public void Recharger() 
        {
            Energie += 50;
        }

        public void SubirDegat(int force, bool puissante)
        {
            int degatsBruts = puissante ? force * 2 : force;
            int degats = Math.Max(0, degatsBruts - (Armure + ArmureTemporaire));
            Pv = Math.Max(0, Pv - degats);
        }

        public bool VerifierConfiguration()
        {
            int verifPv = (Pv - 100)/10;
            int verifArmure = Armure/2;
            int verifForce = Force/2;

            return (verifPv+verifArmure+verifForce) == 10;
        }

        public static bool VerifierPtConfig(int a, int b, int c)
        {
            return (a + b + c) == 10;
        }

        public static bool VerifierPtConfig((int a, int b, int c) tuple)
        {
            return (tuple.a + tuple.b + tuple.c) == 10;
        }

        public Robot() { }

        public Robot(int _pv, int _armure, int _force, int _energie)
        {
            Pv = _pv;
            Armure = _armure;
            Force = _force;
            Energie = _energie;
        }

        public Robot(int ptPv, int ptArmure, int ptForce)
        {
            if (!VerifierPtConfig(ptPv, ptArmure, ptForce))
                throw new ArgumentException("Configuration invalide");
            for (int i = 0; i < ptPv; i++)
                Pv += 10;

            for (int i = 0; i < ptArmure; i++)
                Armure += 2;

            for (int i = 0; i < ptForce; i++)
                Force += 2;
        }
    }
}
