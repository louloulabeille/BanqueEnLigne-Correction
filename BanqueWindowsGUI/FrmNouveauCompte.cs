using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Banque;
using BanqueWindowsGUI.Properties;
using System.Diagnostics;

namespace BanqueWindowsGUI
   
{
    /// <summary>
    /// Création d'un nouveau compte  externe à la banque
    /// </summary>
    public partial class FrmNouveauCompte : Form
    {
        Comptes listeComptes;

        public FrmNouveauCompte()
        {
            InitializeComponent();
        }

        public FrmNouveauCompte(Comptes comptes) :this()
        {
            listeComptes = comptes;
        }

        private void FrmNouveauCompte_Load(object sender, EventArgs e)
        {
            
        }

        private void BtnValider_Click(object sender, EventArgs e)
        {

            // Pour ne pas sortir du dialog avec DialogResult.OK
            // Lorsque des erreurs subsistent 
            // Utiliser this.DialogResult = DialogResult.None
            if ( VerifFormulaire() )
            {
                try
                {
                    Compte newCompte = new Compte()
                    {
                        CodeClient = "23456754",
                        CodeBanque = codeBanqueTextBox.Text,
                        CodeGuichet = codeGuichetTextBox.Text,
                        Numero = numeroCompteTextBox.Text,
                        CleRIB = cleRIBTextBox.Text,
                        LibelleCompte = libellécompteTextBox.Text,
                    };
                    AjouterCompte(newCompte);
                }
                catch (Exception eX)
                {
                    Debug.WriteLine(eX.Message);
                }
            }
            else
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private void BtnAbandonner_Click(object sender, EventArgs e)
        {
            
        }
        private void AjouterCompte(Compte nouveauCompte)
        {
            
            listeComptes.Add(nouveauCompte);
        }

        #region méthode de vérification
        /// <summary>
        /// méthode ed vérifaication du formulaire
        /// </summary>
        /// <returns></returns>
        private bool VerifFormulaire ()
        {
            string codeBanque = codeBanqueTextBox.Text.Trim();
            string codeGuichet = codeGuichetTextBox.Text.Trim();
            string numeroCompte = numeroCompteTextBox.Text.Trim();
            if ( VerifCode(ref codeBanque) && VerifCode(ref codeGuichet) && VerifLibelle(libellécompteTextBox.Text.Trim())
                && VerifNumeroCompte( ref numeroCompte) && VerifCleRib(cleRIBTextBox.Text.Trim()) )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// vérification du code guichet ou banque
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool VerifCode(ref string text)
        {
            return !(string.IsNullOrEmpty(text) | !Compte.VerifCodeBanqueGuichet(ref text));
        }
        /// <summary>
        /// vérification du numéro de compte
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool VerifNumeroCompte( ref string text)
        {
            return !(string.IsNullOrEmpty(text) | !Compte.VerifCompteBanquaire(ref text));
        }
        /// <summary>
        /// verif du libellé
        /// </summary>
        /// <param name="libelle"></param>
        /// <returns></returns>
        private bool VerifLibelle (string libelle)
        {
            return !string.IsNullOrEmpty(libelle);
        }
        /// <summary>
        /// méthode de vérification du champ rib
        /// avant calcul du rib pour savoir s"il est bon
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool VerifCleRib(string text)
        {
            string codeBanque = codeBanqueTextBox.Text.Trim();
            string codeGuichet = codeGuichetTextBox.Text.Trim();
            string numeroCompte = numeroCompteTextBox.Text.Trim();
            ulong rib;
            if (VerifCode(ref codeBanque) && VerifCode(ref codeGuichet) && VerifNumeroCompte(ref numeroCompte))
            {
                if (string.IsNullOrEmpty(text) | !ulong.TryParse(text, out rib))
                {
                    return false;
                }
                else
                {
                    if ( !(Compte.CalculRib(codeBanqueTextBox.Text, codeGuichetTextBox.Text, numeroCompteTextBox.Text) == rib))
                    {
                        return false;
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// méthode de vefification des champ code banque et guichet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void VerifTextBox(object sender)
        {
            TextBox tB = sender as TextBox;
            string text = tB.Text.Trim();
            switch (tB.Name) 
            {
                case "codeBanqueTextBox":
                    errorProviderCodeBanque.Clear();
                    if ( VerifCode(ref text) )
                    {
                        tB.Text = text;
                    }
                    else
                    {
                        errorProviderCodeBanque.SetError(tB, "Le code banque doit comporter au moins 5 caractères et que des chiffres.");
                    }
                    break;
                case "codeGuichetTextBox":
                    errorProviderCodeGuichet.Clear();
                    if (VerifCode(ref text))
                    {
                        tB.Text = text;
                    }
                    else
                    {
                        errorProviderCodeGuichet.SetError(tB, "Le code guichet doit comporter au moins 5 caractères et que des chiffres.");
                    }
                    break;
                case "numeroCompteTextBox":
                    errorProviderNumeroCompte.Clear();
                    if ( VerifNumeroCompte(ref text))
                    {
                        tB.Text = text;
                    }
                    else
                    {
                        errorProviderNumeroCompte.SetError(tB, "Le numéro de compte banquaire doit comporter au moins 11 caractères de type chiffre et majuscule.");
                    }
                    break;
                case "cleRIBTextBox":
                    errorProviderCleRib.Clear();

                    if ( !VerifCleRib(tB.Text) )
                    {
                        errorProviderCleRib.SetError(tB, "La clé rib saisie est invalide.");
                    }
                    
                    break;
                case "libellécompteTextBox":
                    if ( !VerifLibelle(text))
                    {
                        errorProviderLibelle.SetError(tB, "Le libellé du compte est obligatoire.");
                    }
                    break;
            }
        }

        #endregion

        #region event du formulaire
        /// <summary>
        /// event validating sur les TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventTextBoxValidating(object sender, CancelEventArgs e)
        {
            VerifTextBox(sender);
        }

        /// <summary>
        /// event sur Validaded des textbox Code et numéro de compte
        /// sert pour revalider le rib
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventTextRib(object sender, EventArgs e)
        {
            if ( VerifCleRib( cleRIBTextBox.Text.Trim() ) )
            {
                errorProviderCleRib.Clear();
            }
        }
        #endregion
    }
}
