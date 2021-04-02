using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banque;
namespace BanqueConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreerComptes();
            //TesterHollerith();
            //Modulo();
            //TestVerifCodeBanqueGuichet();
            //TestVerifCompteBanquaire();
            //TestTranformeCompte();
            //TestCalculRib();
            TestSaisieCompte();
        }

        private static void TesterHollerith()
        {
            Console.WriteLine("Test fonction de transcodage Hollerith");
            int equivalent;
            Hollerith.Transcoder('Z', out equivalent);
            Console.WriteLine($"caractere : Z valeur : {equivalent}");

            Console.ReadLine();

        }

        private static void CreerComptes()
        {
            Comptes comptes = new Comptes();
            Compte compte = new Compte
            {
                CodeClient = "23456754",
                CodeBanque = "20041",
                CodeGuichet = "01006",
                Numero = "0068875R027",
                CleRIB = "70",
                LibelleCompte = "Mickaël Barrer Banque Postale"
            };
            comptes.Add(compte);
            compte = new Compte
            {
                CodeClient = "23456754",
                CodeBanque = "10907",
                CodeGuichet = "00237",
                Numero = "44219104266",
                CleRIB = "03",
                LibelleCompte = "Bost Banque Populaire courant"
            };
            comptes.Add(compte);
            compte = new Compte
            {
                CodeClient = "23456754",
                CodeBanque = "10907",
                CodeGuichet = "00237",
                Numero = "64286104261",
                CleRIB = "20",
                LibelleCompte = "Bost CASDEN"
            };
            comptes.Add(compte);
            comptes.Save(Properties.Settings.Default.BanqueAppData);

            comptes = new Comptes();
            comptes.Load(Properties.Settings.Default.BanqueAppData);
            Console.WriteLine($"{comptes.Count} comptes sont présents dans la collection");
            
            foreach (Compte item in comptes)
            {
                Console.WriteLine(item.ToString());
            }

        }
        static void Modulo()
        {
            Console.WriteLine($"Modulo 100 % 97 : {100 % 97}");
            Console.Read();
        }

        /// <summary>
        /// jeu de tests de la méthode de vérification du code guichet ou banque
        /// </summary>
        private static void TestVerifCodeBanqueGuichet()
        {
            Debug.WriteLine("Vérification de code guichet ou banque");
            string code1 = "12345";
            Debug.WriteLine($"code 12345 true = {Compte.VerifCodeBanqueGuichet(ref code1)}");
            Debug.WriteLine($"code 12345 doit retourner 12345 = {code1}");
            string code2 = "123";
            Debug.WriteLine($"code 123 true = {Compte.VerifCodeBanqueGuichet(ref code2)}");
            Debug.WriteLine($"code 123 doit retourner 00123 = {code2}");
            string code3 = "012541";
            Debug.WriteLine($"code 012541 false = {Compte.VerifCodeBanqueGuichet(ref code3)}");
            Debug.WriteLine($"code 012541 doit retourner 012541 = {code3}");
            string code4 = "A";
            Debug.WriteLine($"code A false = {Compte.VerifCodeBanqueGuichet(ref code4)}");
            Debug.WriteLine($"code A doit retourner 0000A = {code4}");
            string code5 = "1A3";
            Debug.WriteLine($"code 1A3 false = {Compte.VerifCodeBanqueGuichet(ref code5)}");
            Debug.WriteLine($"code 1A3 doit retourner 001A3 = {code5}");

        }

        /// <summary>
        /// jeu de tests de la méthode de vérification du compte banquaire
        /// </summary>
        private static void TestVerifCompteBanquaire()
        {
            Debug.WriteLine("Vérification du compte banquaire");
            string code1 = "50662254";
            Debug.WriteLine($"compte 50662254 true = {Compte.VerifCompteBanquaire(ref code1)}");
            Debug.WriteLine($"compte 50662254 doit retourner 00050662254 = {code1} true {"00050662254" == code1}");
            string code2 = "148F125G123";
            Debug.WriteLine($"compte 148F125G123 true = {Compte.VerifCompteBanquaire(ref code2)}");
            Debug.WriteLine($"compte 148F125G123 doit retourner 148F125G123 = {code2}");
            string code3 = "012541a4511";
            Debug.WriteLine($"compte 012541a4511 false = {Compte.VerifCompteBanquaire(ref code3)}");
            Debug.WriteLine($"compte 012541a4511 doit retourner 012541a4511 = {code3}");
            string code4 = "451A1125";
            Debug.WriteLine($"compte 451A1125 True = {Compte.VerifCompteBanquaire(ref code4)}");
            Debug.WriteLine($"compte 451A1125 doit retourner 000451A1125 = {code4} true {"000451A1125"==code4}");
            string code5 = "N1A3451255P";
            Debug.WriteLine($"compte N1A3451255P True = {Compte.VerifCompteBanquaire(ref code5)}");
            Debug.WriteLine($"compte N1A3451255P doit retourner N1A3451255P = {code5} true {"N1A3451255P"==code5}");
            string code6 = "141R5226633663";
            Debug.WriteLine($"compte 141R5226633663 False = {Compte.VerifCompteBanquaire(ref code6)}");
            Debug.WriteLine($"compte 141R5226633663 doit retourner 141R5226633663 = {code6} true {"141R5226633663"== code6}");
        }

        /// <summary>
        /// méthode de test qui test la méthode TranformeCompte()
        /// </summary>
        private static void TestTranformeCompte()
        {
            Compte c = new Compte();
            Debug.WriteLine("Transforme compte banquaire");
            Debug.WriteLine($"compte 00000A12345 doit retourner 00000112345 = {Compte.TranformeCompte("00000A12345")}");
            Debug.WriteLine($"compte 00000A12345 doit True = {"00000112345" == Compte.TranformeCompte("00000A12345")}");
            Debug.WriteLine($"compte 00000212345 doit retourner 00000212345 = {Compte.TranformeCompte("00000212345")}");
            Debug.WriteLine($"compte 00000212345 doit True = {"00000212345" == Compte.TranformeCompte("00000212345")}");
            Debug.WriteLine($"compte AJDRG212345 doit retourner 11497212345 = {Compte.TranformeCompte("AJDRG212345")}");
            Debug.WriteLine($"compte AJDRG212345 doit True = {"11497212345" == Compte.TranformeCompte("AJDRG212345")}");
        }

        /// <summary>
        /// test du calcul du rib
        /// </summary>
        private static void TestCalculRib()
        {
            Debug.WriteLine("Test du calcul du numéro de rib");
            Debug.WriteLine($"rib 66 = {Compte.CalculRib("30003","00530","00050662254")}");
            Debug.WriteLine($"rib true = {66 == Compte.CalculRib("30003", "00530", "00050662254")}"); 
        }

        private static void TestSaisieCompte()
        {
            try
            {
                ulong uL = Compte.CalculRib("03003", "00530", "00050662254");
                Compte c1 = new Compte()
                {
                    CodeClient = "23456754",
                    CodeBanque = "3003",
                    CodeGuichet = "530",
                    Numero = "50662254",
                    CleRIB = "85",
                    LibelleCompte="mon Compte"
                };


                Debug.WriteLine($"Code client true = {c1.CodeClient.Equals("23456754")} et code client : 23456754 = {c1.CodeClient}");
                Debug.WriteLine($"true  = {c1.CodeBanque.Equals("03003")} et code banque : 03003 = {c1.CodeBanque} ");
                Debug.WriteLine($"true  = {c1.CodeGuichet.Equals("00530")} et code banque : 00530 = {c1.CodeGuichet} ");
                Debug.WriteLine($"true  = {c1.Numero.Equals("00050662254")} et code banque : 00050662254 = {c1.Numero} ");
                Debug.WriteLine($"true  = {c1.CleRIB.Equals("85")} et code banque : 85 = {c1.CleRIB} ");
                
                Compte c2 = new Compte()
                {
                    CodeClient = "23456754",
                    CodeBanque = "20041",
                    CodeGuichet = "1006",
                    Numero = "68875R027",
                    CleRIB = "70",
                    LibelleCompte = "Propriété Barrer Banque Postale"
                };
                Debug.WriteLine(c2.ToString());

            }
            catch ( ApplicationException aE )
            {
                Debug.WriteLine(aE);
                Debug.WriteLine(aE.Source);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(e.Source);
            }
        }
    }
}