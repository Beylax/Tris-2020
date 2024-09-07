using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logica di interazione per TrisAiWindow.xaml
    /// </summary>
    public partial class TrisAiWindow : Window
    {
        //Variabile del percorso del file della memoria.
        private static string percorsoMem;
        private static int[,] matrice;
        private static Image imag0;
        private static Image imag1;
        private static Image imag2;
        private static Image imag3;
        private static Image imag4;
        private static Image imag5;
        private static Image imag6;
        private static Image imag7;
        private static Image imag8;
        private static Button bot0;
        private static Button bot1;
        private static Button bot2;
        private static Button bot3;
        private static Button bot4;
        private static Button bot5;
        private static Button bot6;
        private static Button bot7;
        private static Button bot8;
        private static TextBlock TTV;
        private static bool inizioGiocatore;
        private static int contamosse;
        private static int[] vtt;
        private static int vincitore;
        public TrisAiWindow()
        {
            InitializeComponent();
        }
        private void TrisAiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SelectionWindow finestraSelezione = new SelectionWindow();
            finestraSelezione.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            contamosse = 0;
            percorsoMem = @"Mem\tmem.txt";
            txtbxPercorso.Text = percorsoMem;
            imag0 = img0;
            imag1 = img1;
            imag2 = img2;
            imag3 = img3;
            imag4 = img4;
            imag5 = img5;
            imag6 = img6;
            imag7 = img7;
            imag8 = img8;
            bot0 = btnImg0;
            bot1 = btnImg1;
            bot2 = btnImg2;
            bot3 = btnImg3;
            bot4 = btnImg4;
            bot5 = btnImg5;
            bot6 = btnImg6;
            bot7 = btnImg7;
            bot8 = btnImg8;
            TTV = TxtVittoria;
            matrice = new int[3, 3];
            vtt = new int[9];
            vincitore = 0;
        }

        private static bool Pareggio()
        {
            if (inizioGiocatore)
            {
                for (int i = 0; i < matrice.GetLength(0); i++)
                {
                    for (int j = 0; j < matrice.GetLength(1); j++)
                    {
                        if (matrice[i, j] == 0)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                int conta = 0;
                for (int i = 0; i < matrice.GetLength(0); i++)
                {
                    for (int j = 0; j < matrice.GetLength(1); j++)
                    {
                        if (matrice[i, j] == 1)
                        {
                            conta++;
                        }
                    }
                }
                if (conta == 4)
                {
                    EseguiAI();
                    ScriviMem();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        private static void DisabilitaTutto()
        {
            bot0.IsEnabled = false;
            bot1.IsEnabled = false;
            bot2.IsEnabled = false;
            bot3.IsEnabled = false;
            bot4.IsEnabled = false;
            bot5.IsEnabled = false;
            bot6.IsEnabled = false;
            bot7.IsEnabled = false;
            bot8.IsEnabled = false;
        }

        private static void ControlloVittoria()
        {
            if (App.ControlloVittoriaGenerale(matrice, out vincitore))
            {
                if (vincitore == 1)
                {
                    TTV.Text = "Il giocatore vince";
                }
                else
                {
                    TTV.Text = "La AI vince";
                }
                DisabilitaTutto();
            }
        }

        //Qui viene cambiato il percorso del file della memoria della AI.
        private void btnPercorso_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog sceltaPercorso = new OpenFileDialog();
            sceltaPercorso.Filter = "File di testo|*.txt";
            sceltaPercorso.DefaultExt = "*.txt";
            if ((bool)sceltaPercorso.ShowDialog())
            {
                percorsoMem = sceltaPercorso.FileName;
            }
            txtbxPercorso.Text = percorsoMem;
        }

        private static void ScriviMem()
        {
            string k;
            StreamWriter scrivi = new StreamWriter(percorsoMem, true);
            scrivi.WriteLine(vincitore);
            int[,] mat = App.Matricizza(vtt);
            for (int r = 0; r < mat.GetLength(0); r++)
            {
                for (int c = 0; c < mat.GetLength(1); c++)
                {
                    if (c == 2)
                    {
                        k = "";
                    }
                    else
                    {
                        k = "|";
                    }
                    scrivi.Write(mat[r, c] + k);
                }
                scrivi.WriteLine();
            }
            scrivi.Close();
        }

        private static bool ControlloPossibilità()
        {
            StreamReader leggi = new StreamReader(percorsoMem);
            while(!leggi.EndOfStream)
            {
                if (leggi.ReadLine() == "1")
                {
                    leggi.Close();
                    return true;
                }
            }
            leggi.Close();
            return false;
        }

        private static int[] Studia()
        {
            bool v = false;
            int[] pos = new int[2] { -1, -1 };
            int[,] mt = new int[3, 3];
            int casuale = 0;
            Random rd = new Random();
            StreamReader leggi = new StreamReader(percorsoMem);
            string[] posizioni;
            if (ControlloPossibilità())
            {
                do
                {
                    casuale = rd.Next(0, 6);
                    while (!leggi.EndOfStream)
                    {
                        if (leggi.ReadLine() == "1" && (casuale == 0))
                        {
                            v = true;
                            posizioni = leggi.ReadLine().Split('|');
                            mt[0, 0] = int.Parse(posizioni[0]);
                            mt[0, 1] = int.Parse(posizioni[1]);
                            mt[0, 2] = int.Parse(posizioni[2]);
                            posizioni = leggi.ReadLine().Split('|');
                            mt[1, 0] = int.Parse(posizioni[0]);
                            mt[1, 1] = int.Parse(posizioni[1]);
                            mt[1, 2] = int.Parse(posizioni[2]);
                            posizioni = leggi.ReadLine().Split('|');
                            mt[2, 0] = int.Parse(posizioni[0]);
                            mt[2, 1] = int.Parse(posizioni[1]);
                            mt[2, 2] = int.Parse(posizioni[2]);
                        }
                        else
                        {
                            leggi.ReadLine();
                            leggi.ReadLine();
                            leggi.ReadLine();
                            casuale = rd.Next(0, 6);
                        }
                    }
                    if (!v)
                    {
                        leggi = new StreamReader(percorsoMem);
                    }
                    else
                    {
                        leggi.Close();
                    }
                } while (!v);
                int i = 1;
                do
                {

                    for (int r = 0; r < mt.GetLength(0); r++)
                    {
                        for (int c = 0; c < mt.GetLength(1); c++)
                        {
                            if (mt[r, c] == i)
                            {
                                pos[0] = r;
                                pos[1] = c;
                            }
                        }
                    }
                    if (inizioGiocatore)
                    {
                        if ((pos[0] == -1 || pos[1] == -1) || (i == 5))
                        {
                            pos = new int[2] { -1, -1 };
                            break;
                        }
                    }
                    else
                    {
                        if ((pos[0] == -1 || pos[1] == -1) || (i == 4))
                        {
                            pos = new int[2] { -1, -1 };
                            break;
                        }
                    }
                    i++;
                } while ((matrice[pos[0], pos[1]]) != 0);
            }
            leggi.Close();
            return pos;

        }

        private void btnImg_Click(object sender, RoutedEventArgs e)
        {
            contamosse++;
            if (sender.Equals(btnImg0))
            {
                vtt[0] = contamosse;
                matrice[0, 0] = 1;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img0, btnImg0);
                    return;
                }
                RiempiTabellaGiocatore(img0, btnImg0);
            }
            if (sender.Equals(btnImg1))
            {
                vtt[1] = contamosse;
                matrice[0, 1] = 1;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img1, btnImg1);
                    return;
                }
                RiempiTabellaGiocatore(img1, btnImg1);
            }
            if (sender.Equals(btnImg2))
            {
                vtt[2] = contamosse;
                matrice[0, 2] = 1;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img2, btnImg2);
                    return;
                }
                RiempiTabellaGiocatore(imag2, btnImg2);
            }
            if (sender.Equals(btnImg3))
            {
                vtt[3] = contamosse;
                matrice[1, 0] = 1;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img3, btnImg3);
                    return;
                }
                RiempiTabellaGiocatore(img3, btnImg3);
            }
            if (sender.Equals(btnImg4))
            {
                matrice[1, 1] = 1;
                vtt[4] = contamosse;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img4, btnImg4);
                    return;
                }
                RiempiTabellaGiocatore(img4, btnImg4);
            }
            if (sender.Equals(btnImg5))
            {
                matrice[1, 2] = 1;
                vtt[5] = contamosse;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img5, btnImg5);
                    return;
                }
                RiempiTabellaGiocatore(img5, btnImg5);
            }
            if (sender.Equals(btnImg6))
            {
                vtt[6] = contamosse;
                matrice[2, 0] = 1;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img6, btnImg6);
                    return;
                }
                RiempiTabellaGiocatore(img6, btnImg6);
            }
            if (sender.Equals(btnImg7))
            {
                matrice[2, 1] = 1;
                vtt[7] = contamosse;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img7, btnImg7);
                    return;
                }
                RiempiTabellaGiocatore(img7, btnImg7);
            }
            if (sender.Equals(btnImg8))
            {
                vtt[8] = contamosse;
                matrice[2, 2] = 1;
                ControlloVittoria();
                if (Pareggio())
                {
                    TxtVittoria.Text = "Pareggio";
                    TxtVittoria.Background = Brushes.LightCoral;
                    RiempiTabellaGiocatore(img8, btnImg8);
                    return;
                }
                RiempiTabellaGiocatore(img8, btnImg8);
            }
            if (vincitore == 0)
                EseguiAI();
            ControlloVittoria();
            if (vincitore != 0)
                ScriviMem();
        }
        private static int[] ControlloPossibiliVittorieGiocatore()
        {
            int[] posizioniVittoria;
            //Controllo righe.
            posizioniVittoria = ControlloRighe(2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Controllo colonne.
            posizioniVittoria = ControlloColonne(2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Controllo diagonale Sinistra-Destra.
            posizioniVittoria = ControlloDiagonaleSD(2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Controllo diagonale Destra-Sinistra.
            posizioniVittoria = ControlloDiagonaleDS(2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Nessuna vittoria possibile
            posizioniVittoria = new int[2] { -1, -1 };
            return posizioniVittoria;
        }
        private static void EseguiAI()
        {
            int[] pos;
            pos = ControlloPossibiliVittorieAI();
            if (pos[0] != -1 && pos[1] != -1)
            {
                RiempiTabellaAI(pos);
            }
            else
            {
                pos = ControlloPossibiliVittorieGiocatore();
                if (pos[0] != -1 && pos[1] != -1)
                {
                    RiempiTabellaAI(pos);
                }
                else
                {
                    pos = Studia();
                    if (pos[0] != -1 && pos[1] != -1)
                    {
                        RiempiTabellaAI(pos);
                    }
                    else
                    {
                        Random nm = new Random();
                        do
                        {
                            pos[0] = nm.Next(0, 3);
                            pos[1] = nm.Next(0, 3);
                        } while (matrice[pos[0], pos[1]] != 0);
                        RiempiTabellaAI(pos);
                    }
                }

            }
        }

        private static int[] ControlloPossibiliVittorieAI()
        {
            int[] posizioniVittoria;
            //Controllo righe.
            posizioniVittoria = ControlloRighe(-2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Controllo colonne.
            posizioniVittoria = ControlloColonne(-2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Controllo diagonale Sinistra-Destra.
            posizioniVittoria = ControlloDiagonaleSD(-2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Controllo diagonale Destra-Sinistra.
            posizioniVittoria = ControlloDiagonaleDS(-2);
            if (posizioniVittoria[0] != -1 && posizioniVittoria[1] != -1)
            {
                return posizioniVittoria;
            }
            //Nessuna vittoria possibile
            posizioniVittoria = new int[2] { -1, -1 };
            return posizioniVittoria;
        }

        private static void RiempiTabellaAI(int[] posizioni)
        {
            Uri ur = new Uri(@"Images\redx.png", UriKind.RelativeOrAbsolute);
            matrice[posizioni[0], posizioni[1]] = -1;
            switch (posizioni[0])
            {
                case 0:
                    {
                        switch (posizioni[1])
                        {
                            case 0:
                                {
                                    imag0.Source = new BitmapImage(ur);
                                    bot0.IsEnabled = false;
                                    break;
                                }
                            case 1:
                                {
                                    imag1.Source = new BitmapImage(ur);
                                    bot1.IsEnabled = false;
                                    break;
                                }
                            case 2:
                                {
                                    imag2.Source = new BitmapImage(ur);
                                    bot2.IsEnabled = false;
                                    break;
                                }
                        }
                        break;
                    }
                case 1:
                    {
                        switch (posizioni[1])
                        {
                            case 0:
                                {
                                    imag3.Source = new BitmapImage(ur);
                                    bot3.IsEnabled = false;
                                    break;
                                }
                            case 1:
                                {
                                    imag4.Source = new BitmapImage(ur);
                                    bot4.IsEnabled = false;
                                    break;
                                }
                            case 2:
                                {
                                    imag5.Source = new BitmapImage(ur);
                                    bot5.IsEnabled = false;
                                    break;
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        switch (posizioni[1])
                        {
                            case 0:
                                {
                                    imag6.Source = new BitmapImage(ur);
                                    bot6.IsEnabled = false;
                                    break;
                                }
                            case 1:
                                {
                                    imag7.Source = new BitmapImage(ur);
                                    bot7.IsEnabled = false;
                                    break;
                                }
                            case 2:
                                {
                                    imag8.Source = new BitmapImage(ur);
                                    bot8.IsEnabled = false;
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
        private static void RiempiTabellaGiocatore(Image immagine, Button bot)
        {
            immagine.Source = new BitmapImage(new Uri(@"Images\blueo.jpg", UriKind.RelativeOrAbsolute));
            bot.IsEnabled = false;
        }

        private static int[] ControlloDiagonaleDS(int sommaP)
        {
            int somma = 0;
            int posizioneR = -1;
            int posizioneC = -1;
            int[] posizioniVittoria = new int[2] { -1, -1 };
            int r = 0;
            for (int c = matrice.GetLength(0) - 1; c >= 0; c--)
            {
                somma += matrice[r, c];
                if (matrice[r, c] == 0)
                {
                    posizioneR = r;
                    posizioneC = c;
                }
                r++;
            }
            if (somma == sommaP)
            {
                posizioniVittoria[0] = posizioneR;
                posizioniVittoria[1] = posizioneC;
                return posizioniVittoria;
            }
            return posizioniVittoria;
        }

        private static int[] ControlloDiagonaleSD(int sommaP)
        {
            int somma = 0;
            int posizioneR = -1;
            int[] posizioniVittoria = new int[2] { -1, -1 };
            for (int r = 0; r < matrice.GetLength(0); r++)
            {
                somma += matrice[r, r];
                if (matrice[r, r] == 0)
                {
                    posizioneR = r;
                }
            }
            if (somma == sommaP)
            {
                posizioniVittoria[0] = posizioneR;
                posizioniVittoria[1] = posizioneR;
                return posizioniVittoria;
            }
            return posizioniVittoria;
        }

        private static int[] ControlloRighe(int sommaP)
        {
            int somma;
            int posizioneR = -1;
            int posizioneC = -1;
            int[] posizioniVittoria = new int[2] { -1, -1 };
            //Controllo delle righe
            somma = 0;
            for (int r = 0; r < matrice.GetLength(0); r++)
            {
                somma = 0;
                for (int c = 0; c < matrice.GetLength(1); c++)
                {
                    somma += matrice[r, c];
                    if (matrice[r, c] == 0)
                    {
                        posizioneC = c;
                        posizioneR = r;
                    }
                }
                if (somma == sommaP)
                {
                    posizioniVittoria[0] = posizioneR;
                    posizioniVittoria[1] = posizioneC;
                    return posizioniVittoria;
                }
            }
            return posizioniVittoria;
        }

        private static int[] ControlloColonne(int sommaP)
        {
            int somma;
            int posizioneR = -1;
            int posizioneC = -1;
            int[] posizioniVittoria = new int[2] { -1, -1 };
            //Controllo delle colonne
            somma = 0;
            for (int c = 0; c < matrice.GetLength(1); c++)
            {
                somma = 0;
                for (int r = 0; r < matrice.GetLength(0); r++)
                {
                    somma += matrice[r, c];
                    if (matrice[r, c] == 0)
                    {
                        posizioneR = r;
                        posizioneC = c;
                    }
                }
                if (somma == sommaP)
                {
                    posizioniVittoria[0] = posizioneR;
                    posizioniVittoria[1] = posizioneC;
                    return posizioniVittoria;
                }
            }
            return posizioniVittoria;
        }

        private void btnInizio_Click(object sender, RoutedEventArgs e)
        {
            btnInizio.IsEnabled = false;
            btnImg0.IsEnabled = true;
            btnImg1.IsEnabled = true;
            btnImg2.IsEnabled = true;
            btnImg3.IsEnabled = true;
            btnImg4.IsEnabled = true;
            btnImg5.IsEnabled = true;
            btnImg6.IsEnabled = true;
            btnImg7.IsEnabled = true;
            btnImg8.IsEnabled = true;
            Random rd = new Random();
            if (rd.Next(0, 2) == 1)
            {
                EseguiAI();
                inizioGiocatore = false;
            }
            else
            {
                inizioGiocatore = true;
            }
        }
    }
}
