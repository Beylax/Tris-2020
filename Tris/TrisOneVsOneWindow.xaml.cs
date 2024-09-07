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
    /// Logica di interazione per TrisOneVsOneWindow.xaml
    /// </summary>
    public partial class TrisOneVsOneWindow : Window
    {
        //Punteggi rosso e blu.
        private static int punteggioRosso = 0;
        private static int punteggioBlu = 0;
        private static int vincitore;
        public static string stringaPunteggiCompleti = "Nessuno ha ancora vinto una partita.";

        //Nomi assegnati nella window precendente.
        public static string nomeBlu;
        public static string nomeRosso;

        //Se i nomi sono stati abilitati.
        public static bool nomi;

        //Variabile per il conteggio delle mosse per verificare il pareggio.
        private static int contaPareggio = 0;

        //Variabile per capire di chi è il turno.
        private static int contaMosse;

        //Valore che si utilizza per controllare se una cella è piena o vuota.
        private static int valoreControllo;

        //Vettore che contiene le celle del Tris.
        public static int[] vettoreTris;

        //Messaggio che sarà usato dal print
        public static string msgVittoria = "Nessuno ha ancora vinto";
        public TrisOneVsOneWindow()
        {
            InitializeComponent();
        }

        //Metodo che apre la finestra della selezione alla chiusura della corrente.
        private void TrisOneVsOneWindow_Closing(object sender, EventArgs e)
        {
            App.selezione = new SelectionWindow();
            App.selezione.Show();
        }

        //Metodo che azzera tutto all'avvio della finestra, per preparare le variabili.
        private void TrisOneVsOneWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Azzeramento dei punteggi.
            punteggioBlu = 0;
            punteggioRosso = 0;

            //Disabilitazione del bottone salva.
            btnSalvaRisultato.IsEnabled = false;

            //Azzeramento della conta per la verifica del pareggio.
            contaPareggio = 0;

            //Abilitazione del pulsante di reset.
            btnReset.IsEnabled = false;

            //Creazione di una nuova tabella per il tris.
            vettoreTris = new int[9];

            //Randomizzazione dei valori per la scelta del giocatore e azzeramento variabile della casella da controllare.
            Random cas = new Random();
            contaMosse = cas.Next(0, 2);
            valoreControllo = 0;

            //Reset del rettangolo del colore al valore del giocatore iniziale.
            if (contaMosse == 1)
            {
                rtgColoreGiocatore.Fill = Brushes.LightBlue;
            }
            else
            {
                rtgColoreGiocatore.Fill = Brushes.LightCoral;
            }


            //Sono stati inseriti nomi nella finestra precedente?
            //Se no continua con i nomi standard:
            if (!nomi)
            {
                nomeBlu = "Blu";
                nomeRosso = "Rosso";
            }
            txtbkPunteggi.Text = $"{nomeRosso}: {punteggioRosso} - {nomeBlu}: {punteggioBlu}";
        }

        //Metodo che si avvia quando si clicca su una delle caselle.
        private void btnImg_Click(object sender, RoutedEventArgs e)
        {
            btnReset.IsEnabled = true;
            if (contaPareggio == 7)
            {
                txtbkVittoria.Text = "Pareggio";
                msgVittoria = txtbkVittoria.Text;
                //Disabilitazione
                AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
            }

            //Tutte le implementazioni dei controlli e della modifica della tabella.
            //Prima immagine (0)
            if (sender.Equals(btnImg0))
            {
                btnImg0.IsEnabled = false;
                valoreControllo = 0;
                ModificaVettore();
                img0.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
            //Seconda immagine (1)
            if (sender.Equals(btnImg1))
            {
                btnImg1.IsEnabled = false;
                valoreControllo = 1;
                ModificaVettore();
                img1.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
            //Terza immagine (2)
            if (sender.Equals(btnImg2))
            {
                btnImg2.IsEnabled = false;
                valoreControllo = 2;
                ModificaVettore();
                img2.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
            //Quarta immagine (3)
            if (sender.Equals(btnImg3))
            {
                btnImg3.IsEnabled = false;
                valoreControllo = 3;
                ModificaVettore();
                img3.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
            //Quinta immagine (4)
            if (sender.Equals(btnImg4))
            {
                btnImg4.IsEnabled = false;
                valoreControllo = 4;
                ModificaVettore();
                img4.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
            //Sesta immagine (5)
            if (sender.Equals(btnImg5))
            {
                btnImg5.IsEnabled = false;
                valoreControllo = 5;
                ModificaVettore();
                img5.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }

            }
            //Settima immagine (6)
            if (sender.Equals(btnImg6))
            {
                btnImg6.IsEnabled = false;
                valoreControllo = 6;
                ModificaVettore();
                img6.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
            //Ottava immagine (7)
            if (sender.Equals(btnImg7))
            {
                btnImg7.IsEnabled = false;
                valoreControllo = 7;
                ModificaVettore();
                img7.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
            //Nona immagine (8)
            if (sender.Equals(btnImg8))
            {
                btnImg8.IsEnabled = false;
                valoreControllo = 8;
                ModificaVettore();
                img8.Source = new BitmapImage(InsImmagine(contaMosse));
                SistemaMosse();
                if (App.ControlloVittoriaGenerale(App.Matricizza(vettoreTris), out vincitore))
                {
                    IncrementoPunteggiEModificaInterfaccia(vincitore, txtbkVittoria);
                    msgVittoria = txtbkVittoria.Text;
                    //Aggiornamento dei punti della partita.
                    AggiornamentoPuntiEDisabilitazione(txtbkPunteggi, btnImg0, btnImg1, btnImg2, btnImg3, btnImg4, btnImg5, btnImg6, btnImg7, btnImg8, btnSalvaRisultato, rtgColoreGiocatore);
                }
                contaPareggio++;

            }
        }

        private static void IncrementoPunteggiEModificaInterfaccia(int vincitore, TextBlock txtbkVittoria)
        {
            if (vincitore == 1)
            {
                punteggioRosso++;
                txtbkVittoria.Text = $"{nomeRosso} vince!";
                txtbkVittoria.Foreground = Brushes.Red;
            }
            if (vincitore == -1)
            {
                punteggioBlu++;
                txtbkVittoria.Text = $"{nomeBlu} vince";
                txtbkVittoria.Foreground = Brushes.Blue;
            }
        }

        private static void AggiornamentoPuntiEDisabilitazione(TextBlock punteggi, Button b1, Button b2, Button b3, Button b4, Button b5, Button b6, Button b7, Button b8, Button b9, Button bottRisultato, Rectangle r1)
        {
            //Aggiornamento dei punteggi.
            punteggi.Text = $"{nomeRosso}: {punteggioRosso} - {nomeBlu}: {punteggioBlu}";
            stringaPunteggiCompleti = $"{nomeRosso}: {punteggioRosso} - {nomeBlu}: {punteggioBlu}";

            //Disabilitazione bottoni.
            b1.IsEnabled = false;
            b2.IsEnabled = false;
            b3.IsEnabled = false;
            b4.IsEnabled = false;
            b5.IsEnabled = false;
            b6.IsEnabled = false;
            b7.IsEnabled = false;
            b8.IsEnabled = false;
            b9.IsEnabled = false;

            //Reset del colore di chi deve giocare.
            r1.Fill = Brushes.LightYellow;

            //Abilitazione del pulsante di salva.
            bottRisultato.IsEnabled = true;
        }

        //Metodo che modifica la tabella (Valori) del tris per i controlli successivi.
        private static void modificaTabellaTris(int val)
        {
            if (contaMosse == 0)
            {
                vettoreTris[val] = 1;
            }
            else
            {
                vettoreTris[val] = -1;
            }

        }

        //Metodo che verifica la possibilità di cliccare su una casella e la riempie.
        private static void ModificaVettore()
        {
            modificaTabellaTris(valoreControllo);
        }

        //Metodo che restituisce il percorso dell'immagine a seconda del giocatore a cui tocca.
        private static Uri InsImmagine(int giocatore)
        {
            string croce = "Images/redx.png";
            string tondo = "Images/blueo.jpg";
            string percorso = string.Empty;
            if (giocatore == 0)
            {
                percorso = croce;
            }
            else if (giocatore == 1)
            {
                percorso = tondo;
            }
            Uri ur = new Uri(percorso, UriKind.Relative);
            return ur;
        }

        //Metodo per il controllo delle mosse per l'identificazione del giocatore.
        private void SistemaMosse()
        {
            if (contaMosse == 1)
            {
                contaMosse = 0;
                rtgColoreGiocatore.Fill = Brushes.LightCoral;
            }
            else if (contaMosse == 0)
            {
                contaMosse = 1;
                rtgColoreGiocatore.Fill = Brushes.LightBlue;
            }
        }

        //Bottone di reset.
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

            //Disabilitazione del pulsante salva.
            btnSalvaRisultato.IsEnabled = false;

            //Disabilitazione del pulsante di reset.
            btnReset.IsEnabled = false;

            //Riabilitazione contenuti griglia.
            btnImg1.IsEnabled = true;
            btnImg0.IsEnabled = true;
            btnImg2.IsEnabled = true;
            btnImg3.IsEnabled = true;
            btnImg4.IsEnabled = true;
            btnImg5.IsEnabled = true;
            btnImg6.IsEnabled = true;
            btnImg7.IsEnabled = true;
            btnImg8.IsEnabled = true;

            //Reset delle immagini.
            Uri ur = new Uri("", UriKind.Relative);
            img0.Source = new BitmapImage(ur);
            img1.Source = new BitmapImage(ur);
            img2.Source = new BitmapImage(ur);
            img3.Source = new BitmapImage(ur);
            img4.Source = new BitmapImage(ur);
            img5.Source = new BitmapImage(ur);
            img6.Source = new BitmapImage(ur);
            img7.Source = new BitmapImage(ur);
            img8.Source = new BitmapImage(ur);

            //Reset del blocco di testo della vittoria.
            txtbkVittoria.Text = string.Empty;
            txtbkVittoria.Foreground = Brushes.Black;

            //Reset dei valori
            valoreControllo = 0;
            vettoreTris = new int[9];
            Random cas = new Random();
            contaMosse = cas.Next(0, 2);
            if (contaMosse == 1)
            {
                rtgColoreGiocatore.Fill = Brushes.LightBlue;
            }
            else
            {
                rtgColoreGiocatore.Fill = Brushes.LightCoral;
            }
            contaPareggio = 0;
        }

        private void btnChiudi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSalvaRisultato_Click(object sender, RoutedEventArgs e)
        {
            PrintWindow finestraSalva = new PrintWindow();
            finestraSalva.Show();
            this.IsEnabled = false;
            this.Hide();
        }
    }
}
