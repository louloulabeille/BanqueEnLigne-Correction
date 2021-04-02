using Banque;
using BanqueWindowsGUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BanqueWindowsGUI
{
    public partial class MDIBanque : Form
    {
        private int childFormNumber = 0;

        public MDIBanque()
        {
            InitializeComponent();
        }

        private void EventFrmNouveauCompteClick(object sender, EventArgs e)
        {
            Comptes listeCompte = new Comptes();
            listeCompte.Load(Settings.Default.BanqueAppData);
            FrmNouveauCompte fNC = new FrmNouveauCompte(listeCompte);
            DialogResult dial =  fNC.ShowDialog();
        }
    }
}
