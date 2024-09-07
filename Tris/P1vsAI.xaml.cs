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
using System.IO;
using System.Threading;

namespace Tris
{
    /// <summary>
    /// Logica di interazione per P1vsCPU.xaml
    /// </summary>
    public partial class P1vsAI : Window
    {
        public P1vsAI()
        {
            InitializeComponent();
        }

        Button[] bottoni;
        string nomeG1 = null;
        string nomeG2 = null;
        string[,] grigliatris;
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }
        public void resetGriglia()
        {
            //Reset bottoni
            for (int i = 0; i < bottoni.Length; i++)
            {
                bottoni[i].Content = " ";
                bottoni[i].IsHitTestVisible = true;
                bottoni[i].Opacity = 0;
            }
            //Reset matrice
            for (int i = 0; i < grigliatris.GetLength(0); i++)
            {
                for (int j = 0; j < grigliatris.GetLength(1); j++)
                {
                    grigliatris[i, j] = "n";
                }
            }
            //Reset Giocatore
            TxtBkGiocatore.Text = "Giocatore1";
            TxtBkRosso.FontWeight = FontWeights.Heavy;
            TxtBkBlu.FontWeight = FontWeights.Normal;
            //Reset progressbar
            PbTempo.Value = 0;
            PbTempo.BeginAnimation(ProgressBar.ValueProperty, null);
            PbTempo.Background = Brushes.Red;
            //TextBlock vittoria
            TxtBkVittoria.Visibility = Visibility.Hidden;
            TxtBkVittoria.Text = " vince!";
            pareggio = 0;
            //Reset stato
            stato = 0;
        }
        int punteggioMax;
        public void reset()
        {
            //Aggiorno il punteggio nel file
            if (int.Parse(TxtBkPuntegioRosso.Text) > int.Parse(TxtBkPuntegioBlu.Text))
            {
                punteggioMax = int.Parse(TxtBkPuntegioRosso.Text);
            }
            else
            {
                punteggioMax = int.Parse(TxtBkPuntegioBlu.Text);
            }
            bool ok = false;
            int tmp = 0;
            int l = 0;
            bool oke;
            int tmp1;
            leggifile = new StreamReader("PMax.txt");
            while (!leggifile.EndOfStream)
            {
                oke = int.TryParse(leggifile.ReadLine(), out tmp1);
                if (punteggioMax > tmp1)
                {
                    ok = true;
                    tmp = l;
                }
                l++;
            }
            leggifile.Close();
            if (ok)
            {
                scrivifile = new StreamWriter("Podio.txt");
                scrivifile.WriteLine(nomeVincitore);
                scrivifile.Close();
                scrivifile = new StreamWriter("PMax.txt");
                scrivifile.WriteLine(punteggioMax);
                scrivifile.Close();
                TxtBkPrimo.Text = nomeVincitore;
                TxtBkPrimoPunteggio.Text = punteggioMax.ToString();
            }
            resetGriglia();
            //Disabilito i bottoni
            OnOffBtn(false);
            //Reset nomi
            LblG1.Content = "";
            TxtBkNome.Text = "";
            nomeG = 0;
            BtnInserisciNome.IsEnabled = true;
            TxtNomeGiocatore.Text = "";
            TxtBkControlloNomi.Visibility = Visibility.Visible;
            //Reset Selezione
            CbxSelezione.IsEnabled = false;
            CbxSelezione.SelectedIndex = -1;
            //Reset label nomi
            TxtBkSelezionaSimbolo.Visibility = Visibility.Visible;
            TxtBkNome.Visibility = Visibility.Visible;
            //Reset punteggi
            TxtBkPuntegioRosso.Text = "0";
            TxtBkPuntegioBlu.Text = "0";
            //Reset variabili
            oks = false;
            BtnIniziaAI.IsEnabled = false;
        }
        StreamWriter scrivifile;
        StreamReader leggifile;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //array di bottoni
            bottoni = new Button[9]
            {
                Btn1,
                Btn2,
                Btn3,
                Btn4,
                Btn5,
                Btn6,
                Btn7,
                Btn8,
                Btn9
            };
            //Matrice per il controllo
            grigliatris = new string[3, 3]
            {
                { "n", "n", "n"},
                { "n", "n", "n"},
                { "n", "n", "n"}
            };
            //Setto il punteggio più alto
            leggifile = new StreamReader("Podio.txt");
            TxtBkPrimo.Text = leggifile.ReadLine();
            leggifile.Close();
            leggifile = new StreamReader("PMax.txt");
            TxtBkPrimoPunteggio.Text = leggifile.ReadLine();
            leggifile.Close();
            reset();
            //Creo lo stream per la memoria dell'AI
            sw = File.AppendText("MemoriaAI.txt");
            sw.Close();
        }
        int nomeG = 0;
        private void BtnInserisciNome_Click(object sender, RoutedEventArgs e)
        {
            //Prendo in input il nome del giocatore
            nomeG1 = TxtNomeGiocatore.Text;
            TxtBkNome.Text = $"per {nomeG1}";
            CbxSelezione.IsEnabled = true;
            LblG1.Content = nomeG1;
            TxtNomeGiocatore.Text = "";
            if (CbxSelezione.SelectedIndex != -1)
            {
                //Abilito i bottoni
                OnOffBtn(true);
                BtnIniziaAI.IsEnabled = true;
            }
            nomeG++;
            TxtBkControlloNomi.Visibility = Visibility.Hidden;
            BtnInserisciNome.IsEnabled = false;
        }
        int stato = 0;
        public static int pareggio = 0;
        bool oks = false;
        StreamWriter sw;
        StreamReader sr;
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //Nella memoria della CPU il simbolo dellìutente viene sempre considerato una "x"
            sw = File.AppendText("MemoriaAI.txt");
            //Mi salvo nella memoria dell'AI la situazione della griglia
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    string tmp = grigliatris[i, k];
                    //Se il simbolo del giocatore è "o" lo trasformo in "x" perchè così l'AI nella sua memoria ha che il giocatore è una "x"
                    if (simboloG1 == "O")
                    {
                        if (tmp == "O")
                        {
                            tmp = "X";
                        }
                        else if(tmp == "X")
                        {
                            tmp = "O";
                        }
                    }
                    sw.Write(tmp + " ");
                }
            }
            oks = false;
            Button btnRicevuto = (Button)sender;
            pareggio++;
            PbTempo.Value = 0;
            PbTempo.BeginAnimation(ProgressBar.ValueProperty, null);
            //Capisco da che bottone mi è arrivato il click e aggiorno il bottone
            aggiornaBottone(stato, btnRicevuto);
            grigliatris[Grid.GetRow(btnRicevuto), Grid.GetColumn(btnRicevuto)] = simbolo;
            //Mi salvo nella memoria dell'AI quello che ha fatto l'utente
            sw.WriteLine(Grid.GetRow(btnRicevuto) + " " + Grid.GetColumn(btnRicevuto));
            sw.Close();
            //Controllo se il giocatore ha vinto (La variabile bottone viene aggiornta ogni volta che si richiama il metodo aggiornabottone, e assume il valore del bottone cliccato)
            if (controllaTris(grigliatris, btnRicevuto))
            {
                oks = true;
                if (simbolo == simboloG1)
                {
                    TxtBkVittoria.Text = nomeG1 + " " + TxtBkVittoria.Text;
                    nomeVincitore = nomeG1;
                    TxtBkPuntegioRosso.Text = (int.Parse(TxtBkPuntegioRosso.Text) + 1).ToString();
                }
                else
                {
                    TxtBkVittoria.Text = "AI" + " " + TxtBkVittoria.Text;
                    nomeVincitore = nomeG2;
                    TxtBkPuntegioBlu.Text = (int.Parse(TxtBkPuntegioBlu.Text) + 1).ToString();
                }
                TxtBkVittoria.Visibility = Visibility.Visible;
                PbTempo.Value = 0;
                PbTempo.BeginAnimation(ProgressBar.ValueProperty, null);
                OnOffBtn(false);
            }
            else if (pareggio != 9)
            {
                //Gioca l'AI
                azioniAI();
                if (controllaTris(grigliatris, btnTmp))
                {
                    oks = true;
                    if (simbolo == simboloG1)
                    {
                        TxtBkVittoria.Text = nomeG1 + " " + TxtBkVittoria.Text;
                        nomeVincitore = nomeG1;
                        TxtBkPuntegioRosso.Text = (int.Parse(TxtBkPuntegioRosso.Text) + 1).ToString();
                    }
                    else
                    {
                        TxtBkVittoria.Text = "AI" + " " + TxtBkVittoria.Text;
                        nomeVincitore = nomeG2;
                        TxtBkPuntegioBlu.Text = (int.Parse(TxtBkPuntegioBlu.Text) + 1).ToString();
                    }
                    TxtBkVittoria.Visibility = Visibility.Visible;
                    PbTempo.Value = 0;
                    PbTempo.BeginAnimation(ProgressBar.ValueProperty, null);
                    OnOffBtn(false);
                }
            }
            if (pareggio == 9 && !oks)
            {
                TxtBkVittoria.Visibility = Visibility.Visible;
                TxtBkVittoria.Text = "Pareggio";
                //Fermo la progressbar
                PbTempo.Value = 0;
                PbTempo.BeginAnimation(ProgressBar.ValueProperty, null);
            }
        }
        Button btnTmp;
        Button[] bottoniRimanenti;
        string nomeVincitore;
        static bool controllaTris(string[,] mat, Button btn)
        {
            bool ok = false;
            if (controllaRiga(mat, btn) || controllaColonna(mat, btn) || controllaDiagonali(mat, btn))
            {
                ok = true;
            }
            return ok;
        }
        static bool controllaRiga(string[,] mat, Button btn)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = false;
            for (int i = 0; i < 3; i++)
            {
                sb.Append(mat[Grid.GetRow(btn), i]);
            }
            if (sb.ToString() == "XXX" || sb.ToString() == "OOO")
            {
                ok = true;
            }
            return ok;
        }
        static bool controllaColonna(string[,] mat, Button btn)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = false;
            for (int i = 0; i < 3; i++)
            {
                sb.Append(mat[i, Grid.GetColumn(btn)]);
            }
            if (sb.ToString() == "XXX" || sb.ToString() == "OOO")
            {
                ok = true;
            }
            return ok;
        }
        static bool controllaDiagonali(string[,] mat, Button btn)
        {
            StringBuilder sb = new StringBuilder();
            bool ok = false;
            //Controllo prima diagonale
            for (int i = 0; i < 3; i++)
            {
                sb.Append(mat[i, i]);
            }
            //controllo seconda diagonale
            int j = 0;
            StringBuilder s = new StringBuilder();
            for (int i = 2; i >= 0; i--)
            {
                s.Append(mat[i, j]);
                j++;
            }
            if (sb.ToString() == "XXX" || sb.ToString() == "OOO")
            {
                ok = true;
            }
            if (s.ToString() == "XXX" || s.ToString() == "OOO")
            {
                ok = true;
            }
            return ok;
        }
        string simbolo;
        public void aggiornaBottone(int statoS, Button btn)
        {
            if (statoS == 0)
            {
                btn.Foreground = Brushes.Red;
                btn.Content = simboloG1;
                TxtBkRosso.FontWeight = FontWeights.Normal;
                TxtBkBlu.FontWeight = FontWeights.Heavy;
                stato = 1;
                PbTempo.Background = Brushes.Blue;
                simbolo = simboloG1;
            }
            else if (stato == 1)
            {
                btn.Foreground = Brushes.Blue;
                btn.Content = simboloG2;
                TxtBkBlu.FontWeight = FontWeights.Normal;
                TxtBkRosso.FontWeight = FontWeights.Heavy;
                stato = 0;
                PbTempo.Background = Brushes.Red;
                simbolo = simboloG2;
            }
            btn.IsHitTestVisible = false;
            btn.Opacity = 100;
            //Per progressbar (diminuzione a tempo)
            Duration duration = new Duration(TimeSpan.FromSeconds(20));
            System.Windows.Media.Animation.DoubleAnimation doubleanimation = new System.Windows.Media.Animation.DoubleAnimation(200.0, duration);
            PbTempo.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
            //bottone = btn;
        }
        string simboloG1;
        string simboloG2;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selezionato = (ComboBoxItem)CbxSelezione.SelectedItem;
            //Per quando premo il pusante di reset
            if (CbxSelezione.SelectedIndex != -1)
            {
                simboloG1 = selezionato.Content.ToString();
                if (simboloG1 == "X")
                {
                    simboloG2 = "O";
                }
                else
                {
                    simboloG2 = "X";
                }
                CbxSelezione.Tag = simboloG1;
                CbxSelezione.IsEnabled = false;
                if (nomeG == 1)
                {
                    //Abilito i bottoni del tris
                    OnOffBtn(true);
                    BtnIniziaAI.IsEnabled = true;
                }
                TxtBkSelezionaSimbolo.Visibility = Visibility.Hidden;
                TxtBkNome.Visibility = Visibility.Hidden;
            }
        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window1 = new MainWindow();
            window1.Close();
            this.Close();
            Environment.Exit(0);
        }
        private void BtnSelezionaMod_Click(object sender, RoutedEventArgs e)
        {
            this.IsHitTestVisible = false;
            MainWindow Selezione = new MainWindow();
            Selezione.Show();
        }
        private void PbTempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PbTempo.Value == 60)
            {
                if (simbolo == simboloG1)
                {
                    TxtBkVittoria.Text = nomeG2 + " tempo scaduto";
                    TxtBkPuntegioRosso.Text = (int.Parse(TxtBkPuntegioRosso.Text) + 1).ToString();
                    nomeVincitore = nomeG1;
                }
                else
                {
                    TxtBkVittoria.Text = nomeG1 + " tempo scaduto";
                    TxtBkPuntegioBlu.Text = (int.Parse(TxtBkPuntegioBlu.Text) + 1).ToString();
                    nomeVincitore = nomeG2;
                }
                TxtBkVittoria.Visibility = Visibility.Visible;
                //Disabilito tutti i bottoni del tris
                OnOffBtn(false);
            }
        }
        public void OnOffBtn(bool onoff)
        {
            for (int i = 0; i < bottoni.Length; i++)
            {
                bottoni[i].IsHitTestVisible = onoff;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow window1 = new MainWindow();
            window1.Close();
            this.Close();
            sw.Close();
            Environment.Exit(0);
        }

        private void BtnResetAI_Click(object sender, RoutedEventArgs e)
        {
            sw = new StreamWriter("MemoriaAI.txt");
            sw.Close();
        }

        private void BtnResetGiocatori_Click(object sender, RoutedEventArgs e)
        {
            resetGriglia();
        }

        private void BtnIniziaAI_Click(object sender, RoutedEventArgs e)
        {
            stato = 1;
            azioniAI();
        }
        public void azioniAI()
        {          
            string stmp;
            //Se il giocatore non ha vinto l'AI può giocare
            sr = new StreamReader("MemoriaAI.txt");
            int bottoneR = -1, bottoneC = -1;
            bool ok = false;
            while (!sr.EndOfStream && !ok)
            {
                bool nellamemoria = true;
                string s = sr.ReadLine().Replace(" ", "");
                for (int i = 0; i < 3; i++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        //Se il simbolo del giocatore 1 è "x" consdiero il simbolo dell'AI come "x" in modo tale da porter prendere informazioni dalla memoria
                        stmp = grigliatris[i, k];
                        if (simboloG1 == "X")
                        {
                            if (stmp == "O")
                            {
                                stmp = "X";
                            }
                            else if (stmp == "X")
                            {
                                stmp = "O";
                            }
                        }
                        if (stmp != s[i + k + (2 * i)].ToString())
                        {
                            nellamemoria = false;
                        }
                    }
                }
                if (nellamemoria)
                {
                    bottoneR = int.Parse(s[9].ToString());
                    bottoneC = int.Parse(s[10].ToString());
                    ok = true;
                }
            }
            sr.Close();
            bottoniRimanenti = new Button[9 - pareggio];
            int j = 0;
            for (int i = 0; i < grigliatris.GetLength(0); i++)
            {
                for (int k = 0; k < grigliatris.GetLength(1); k++)
                {
                    if (grigliatris[i, k] == "n")
                    {
                        bottoniRimanenti[j] = bottoni[i + k + (2 * i)];
                        j++;
                    }
                }
            }
            //Se non ho trovato niente nella mia memoria
            if (bottoneR == -1)
            {
                Random rn = new Random();
                btnTmp = bottoniRimanenti[rn.Next(9 - pareggio)];
            }
            else
            {
                btnTmp = bottoni[bottoneR + bottoneC + (2 * bottoneR)];
            }
            //Aggiorno la griglia
            grigliatris[Grid.GetRow(btnTmp), Grid.GetColumn(btnTmp)] = simboloG2;
            aggiornaBottone(stato, btnTmp);
            pareggio++;
        }
    }
    
}
