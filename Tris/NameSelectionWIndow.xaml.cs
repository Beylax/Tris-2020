using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tris
{
    /// <summary>
    /// Logica di interazione per NameSelectionWIndow.xaml
    /// </summary>
    public partial class NameSelectionWIndow : Window
    {
        private static bool v = false;
        public NameSelectionWIndow()
        {
            InitializeComponent();
        }

        //Evento del click dei due bottoni
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnContinua)
            {
                App.unoControUno = new TrisOneVsOneWindow();
                TrisOneVsOneWindow.nomi = true;
                TrisOneVsOneWindow.nomeRosso = txtbxGiocatoreRosso.Text;
                TrisOneVsOneWindow.nomeBlu = txtbxGiocatoreBlu.Text;
                App.unoControUno.Show();
                v = true;
                this.Close();
            }
            if(sender == btnNoNomi)
            {
                App.unoControUno = new TrisOneVsOneWindow();
                TrisOneVsOneWindow.nomi = false;
                App.unoControUno.Show();
                v = true;
                Close();
            }
        }

        //Abilitazione del bottone continua per stringhe non vuote.
        private void txtbxGiocatore_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(txtbxGiocatoreBlu.Text)) || string.IsNullOrWhiteSpace(txtbxGiocatoreRosso.Text))
            {
                btnContinua.IsEnabled = false;
            }
            else
            {
                btnContinua.IsEnabled = true;
            }
        }

        //Disabilitazione del bottone continua all'avvio.
        private void NameSelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            btnContinua.IsEnabled = false;
        }

        private void NameSelectionWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!v)
            {
                App.selezione = new SelectionWindow();
                App.selezione.Show();
            }
        }
    }
}
