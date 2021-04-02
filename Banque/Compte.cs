using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Diagnostics;

namespace Banque
{
    /// <summary>
    /// Compte en banque
    /// </summary>
    public class Compte
    {
        #region Champs
        private string _codeClient ;
        private string _codeBanque;
        private string _codeGuichet;
        private string _numero;
        private string _cleRIB;
        private string _libelleCompte;


        #endregion

        #region Propriétés
        public string CodeClient
        {
            get { return _codeClient; }
            set 
            {
                //if ( string.IsNullOrEmpty(value) ) { throw new ApplicationException("Le code client doit être obligatoire."); }
                _codeClient = value; 
            }
        }
        public string CodeBanque
        {
            get { return _codeBanque; }
            set 
            {
                if (string.IsNullOrEmpty(value) |  !Compte.VerifCodeBanqueGuichet(ref value)) 
                {
                    throw (new ApplicationException("Le code banque doit comporter au moins 5 caractères et que des chiffres."));
                }
                _codeBanque = value; 
            }
        }
        public string CodeGuichet
        {
            get { return _codeGuichet; }
            set 
            {
                if ( string.IsNullOrEmpty(value) | !Compte.VerifCodeBanqueGuichet(ref value) )
                {
                    throw (new ApplicationException("Le code guichet doit comporter moins 5 caractères et que des chiffres."));
                }
                _codeGuichet = value; 
            }
        }

        public string Numero
        {
            get { return _numero; }
            set 
            { 
                if ( string.IsNullOrEmpty(value) | !Compte.VerifCompteBanquaire(ref value) )
                {
                    throw (new ApplicationException("Le numéro de compte banquaire doit comporter au moins 11 caractères de type chiffre et majuscule."));
                }
                _numero = value; 
            }
        }
        public string CleRIB
        {
            get { return _cleRIB; }
            set 
            { 
                if (string.IsNullOrEmpty(value )) { throw(new ApplicationException("La clé rib est obligatoire.")); }
                if ( !VerifCalculRib( value) )
                {
                    throw (new ApplicationException("La clé rib est incorrecte."));
                }
                _cleRIB = value; 
            }
        }
        public string LibelleCompte
        {
            get { return _libelleCompte; }
            set 
            {
                if (string.IsNullOrEmpty(value)) { throw (new ApplicationException("Le libellé de compte est obbligatoire.")); }
                _libelleCompte = value; 
            }
        }
        #endregion

        #region méthode hérité object
        /// <summary>
        /// Chaine représentant l'objet instancié.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, @"{0};{1};{2};{3};{4};{5}", this.CodeClient, this.CodeBanque, this.CodeGuichet, this.Numero, this.CleRIB, this.LibelleCompte);
        }
        /// <summary>
        /// Deux comptes sont égaux si codes client,Banque,Guichet et Numéros de compte
        /// sont identiques 
        /// </summary>
        /// <returns>Vrai si les deux objets sont égaux</returns>
        public override bool Equals(Object compte)
        {
            Compte compteRef = compte as Compte;
            if (compteRef == null) return false;
            return compteRef.CodeClient == CodeClient
                      && compteRef.CodeBanque == CodeBanque
                      && compteRef.CodeGuichet == CodeGuichet
                     && compteRef.Numero == Numero;
        }

        public override int GetHashCode()
        {
            int hashCode;
            hashCode = string.IsNullOrEmpty(_codeClient) ? 0 : _codeClient.GetHashCode();
            hashCode = string.IsNullOrEmpty(_codeBanque) ? hashCode : hashCode ^ _codeBanque.GetHashCode();
            hashCode = string.IsNullOrEmpty(_codeGuichet) ? hashCode : hashCode ^ _codeGuichet.GetHashCode();
            hashCode = string.IsNullOrEmpty(_numero) ? hashCode : hashCode ^ _numero.GetHashCode();
            return hashCode;
        }
        /// <summary>
        /// opérateur relationnel ==
        /// </summary>
        /// <param name="compteA">Instance Compte</param>
        /// <param name="compteB">Instance Compte</param>
        /// <returns>Vrai si égaux</returns>
        public static bool operator ==(Compte compteA, Compte compteB)
        {
            if (compteA is null) return compteB is null;
            return compteA.Equals(compteB);
        }
        /// <summary>
        ///  opérateur relationnel !=
        /// </summary>
        /// <param name="compteA">Instance Compte</param>
        /// <param name="compteB">Instance Compte</param>
        /// <returns>Vrai si différents</returns>
        public static bool operator !=(Compte compteA, Compte compteB)
        {
            if (compteA is null) return (object)compteB != null;
            return !compteA.Equals(compteB);
        }
        #endregion

        #region méthode de vérification static
        /// <summary>
        /// méthode static pour vérifier le code banque et guichet
        /// doit comporter 5 caractère alphanumérique
        /// </summary>
        /// <param name="code">code banque ou guichet</param>
        /// <returns></returns>
        public static bool VerifCodeBanqueGuichet( ref string code )
        {
            int longeur = 5;
            string pattern = @"^[0-9]{5}$";
            Regex rgx = new Regex(pattern);
            if ( rgx.IsMatch(code) )
            {
                return true;
            }
            else
            {
                code = code.Length >= longeur ? code : Compte.AjoutZero(code, longeur);
                return rgx.IsMatch(code);
            }
        }

        /// <summary>
        /// méthode static de vérification de compte banquaire
        /// doit comporter au moins 11 caractères de types
        /// chiffre et lettre majuscule
        /// </summary>
        /// <param name="code">Compte en banque à vérifier</param>
        /// <returns></returns>
        public static bool VerifCompteBanquaire( ref string code )
        {
            int longueur = 11;
            string pattern = @"^[0-9A-Z]{11}$";
            Regex rgx = new Regex(pattern);
            if (rgx.IsMatch(code))
            {
                return true;
            }
            else
            {
                code = code.Length >= longueur ? code : Compte.AjoutZero(code, longueur);
                return rgx.IsMatch(code);
            }
        }

        /// <summary>
        /// méthode de vérification si le rib correspond bien
        /// au numéro de rib saisie
        /// </summary>
        /// <param name="codeBanque">code banque du compte</param>
        /// <param name="codeGuichet">code guichet du compte</param>
        /// <param name="compte">numéro de compte</param>
        /// <param name="ribSaisie">rib saisie</param>
        /// <returns></returns>
        public static bool VerifCleRib (string codeBanque, string codeGuichet, string compte, string ribSaisie)
        {
            StringBuilder sB = new StringBuilder();
            ulong rib = Compte.CalculRib(codeBanque, codeGuichet, compte);
            sB.Append(Compte.AjoutZero(rib.ToString(),2));
            return sB.ToString().Equals(ribSaisie);
        }

        #endregion

        #region methode de vérification non static
        /// <summary>
        /// vérification si la clé rib est correcte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool VerifCalculRib(string value)
        {
            if (string.IsNullOrEmpty(_codeBanque) | string.IsNullOrEmpty(_codeGuichet) | string.IsNullOrEmpty(Numero))
            {
                throw (new ApplicationException("La saisie du code guichet, banque et le numéro de compte est obligatoire."));
            }
            return Compte.CalculRib(this.CodeBanque, this.CodeGuichet, Compte.TranformeCompte(this.Numero)).ToString().Equals(value);

        }
        #endregion

        #region methode de classe
        /// <summary>
        /// calcul le numéro de rib d'un compte banquaire
        /// 97-long.part(concat(codebanque+codeguichet+compte banquaire))*100 modulo 97
        /// </summary>
        /// <param name="codeBanque"></param>
        /// <param name="codeGuichet"></param>
        /// <param name="compte"></param>
        /// <returns></returns>
        public static ulong CalculRib(string codeBanque, string codeGuichet, string compte)
        {
            StringBuilder sB = new StringBuilder();
            ulong reste;
            reste = ulong.Parse(codeBanque)%97;
            sB.Append(reste);
            sB.Append(codeGuichet);
            reste = ulong.Parse(sB.ToString()) % 97;

            sB.Clear();
            sB.Append(reste);
            sB.Append(Compte.TranformeCompte(compte));
            reste = ulong.Parse(sB.ToString()) % 97;
            
            sB.Clear();
            sB.Append(reste);
            sB.Append("00");
            reste = ulong.Parse(sB.ToString()) % 97;

            return 97-reste;
        }

        /// <summary>
        /// retourne le compte après transformation
        /// par la classe Hollerith
        /// </summary>
        /// <param name="compte"></param>
        /// <returns></returns>
        public static string TranformeCompte( string compte )
        {
            StringBuilder sB = new StringBuilder();
            foreach (char c in compte)
            {
                int equivalent;
                if (Hollerith.Transcoder(c, out equivalent))
                {
                    sB.Append(equivalent);
                }else
                {
                    sB.Append(c);
                }
            }
            return sB.ToString();
        }


        /// <summary>
        /// méthode qui rajoute des zéro à gauche
        /// si le nombre de caractère est insufisant
        /// </summary>
        /// <param name="chaine">chaine à vérifier</param>
        /// <param name="taille">taille de la chaine final</param>
        /// <returns></returns>
        public static string AjoutZero( string chaine, int taille)
        {
            int diff = taille - chaine.Length;
            StringBuilder sG = new StringBuilder();
            if ( diff > 0)
            {
                for ( int i = 0; i < diff; i++ )
                {
                    sG.Append("0");
                }
                sG.Append(chaine);
                return sG.ToString();
            }
            return chaine;
        }

        #endregion
    }
}
