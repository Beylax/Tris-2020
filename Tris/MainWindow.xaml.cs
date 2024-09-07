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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tris
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void RdBtnSelezione_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == RdBtnG1vsG2)
            {
                App.selezione = 1;
            }
            else if (sender == RdBtnG1vCPU)
            {
                App.selezione = 2;
            }
            else if (sender == RdBtnG1vsAI)
            {
                App.selezione = 3;
            }
            BtnOK.IsEnabled = true;
        }
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            //Se è la prima volta che apro il programma, devo creare le finestre come oggetto
            if (App.tmp == 0)
            {
                App.p1VSp2 = new P1vsP2();
                App.p1VSCPU = new P1vsCPU();
                App.p1VSAI = new P1vsAI();
                App.tmp++;
            }
            //Vedo quale scelta è stata fatta e in base ad essa mostro e nascondo le varie finestre
            switch (App.selezione)
            {
                case 1:
                    {
                        App.p1VSp2.IsHitTestVisible = true;
                        if (App.selezione != App.selezione1)
                        {
                            App.p1VSp2.Show();
                            App.p1VSCPU.Hide();
                            App.p1VSAI.Hide();
                        }
                        break;
                    }
                case 2:
                    {
                        App.p1VSCPU.IsHitTestVisible = true;
                        if (App.selezione != App.selezione1)
                        {
                            App.p1VSCPU.Show();
                            App.p1VSp2.Hide();
                            App.p1VSAI.Hide();
                        }
                        break;
                    }
                case 3:
                    {
                        App.p1VSAI.IsHitTestVisible = true;
                        if (App.selezione != App.selezione1)
                        {
                            App.p1VSAI.Show();
                            App.p1VSp2.Hide();
                            App.p1VSCPU.Hide();
                        }
                        break;
                    }
            }
            App.selezione1 = App.selezione;
            Close();
        }
    }
}
