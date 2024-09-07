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
    /// Logica di interazione per PrintWIndow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        private static int tipostampa;
        public PrintWindow()
        {
            InitializeComponent();
        }

        //Alla chiusura della window di stampa ritornare a quella del tris.
        private void PrintWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.unoControUno.IsEnabled = true;
            App.unoControUno.Show();
        }

        //Click del bottone ok
        private void btnConferma_Click(object sender, RoutedEventArgs e)
        {
            //Gestione degli errori nella nominazione del file.
            try
            {
                //Inizializzazione dello stream di scrittura.
                StreamWriter scrivi = new StreamWriter($@"Risultati\{txtbxNomeFile.Text}.txt", true);
                
                //Decisione del tipo di stampa.
                switch (tipostampa)
                {
                    //Se la stampa è della matrice.
                    case 1:
                        {
                            int n;
                            char[,] matriceTrisCaratteri = new char[3, 3];
                            int[,] mat = App.Matricizza(TrisOneVsOneWindow.vettoreTris);
                            for (int r = 0; r < mat.GetLength(0); r++)
                            {
                                for (int c = 0; c < mat.GetLength(1); c++)
                                {
                                    n = mat[r, c];
                                    switch (n)
                                    {
                                        case 1:
                                            {
                                                matriceTrisCaratteri[r, c] = 'X';
                                                break;
                                            }
                                        case 0:
                                            {
                                                matriceTrisCaratteri[r, c] = ' ';
                                                break;
                                            }
                                        case -1:
                                            {
                                                matriceTrisCaratteri[r, c] = 'O';
                                                break;
                                            }
                                    }
                                }
                            }
                            scrivi.WriteLine();
                            scrivi.WriteLine(TrisOneVsOneWindow.msgVittoria);
                            for (int r = 0; r < matriceTrisCaratteri.GetLength(0); r++)
                            {
                                for (int c = 0; c < matriceTrisCaratteri.GetLength(1); c++)
                                {
                                    scrivi.Write(matriceTrisCaratteri[r, c].ToString());
                                }
                                scrivi.WriteLine();
                            }
                            scrivi.WriteLine();
                            scrivi.Close();
                            break;
                        }
                        //Se la stampa è del vettore.
                    case 2:
                        {
                            scrivi.WriteLine();
                            for (int i = 0; i < TrisOneVsOneWindow.vettoreTris.Length; i++)
                            {
                                scrivi.Write(TrisOneVsOneWindow.vettoreTris[i] + "/");
                            }
                            scrivi.WriteLine();
                            scrivi.Close();
                            break;
                        }
                        //Se la stampa è dei risultati.
                    case 3:
                        {
                            scrivi.WriteLine();
                            scrivi.WriteLine(TrisOneVsOneWindow.stringaPunteggiCompleti);
                            scrivi.WriteLine();
                            scrivi.Close();
                            break;
                        }
                }
            }
            catch (FormatException err)
            {
                MessageBox.Show(err.Message, "Errore di formato", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (IOException err)
            {
                MessageBox.Show(err.Message, "Errore di IO", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (ArgumentException err)
            {
                MessageBox.Show(err.Message + "\nErrore nel nome del file, controllare che non siano stati utilizzati simboli o caratteri non consentiti dal sistema", "Errore nella denominazione del file", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                txtbxNomeFile.Text = string.Empty;
            }
        }

        //Scelte del tipo di stampa.
        private void rdbtnStampaMatrice_Checked(object sender, RoutedEventArgs e)
        {
            tipostampa = 1;
            btnConferma.IsEnabled = true;
        }

        private void rdbtnStampaVettore_Checked(object sender, RoutedEventArgs e)
        {
            tipostampa = 2;
            btnConferma.IsEnabled = true;
        }

        private void rdbtnStampaRisultatoNomi_Checked(object sender, RoutedEventArgs e)
        {
            tipostampa = 3;
            btnConferma.IsEnabled = true;
        }

        //Al caricamento:
        private void PrintWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Disabilitazione del bottone conferma e azzeramento della casella di testo e dei bottoni di scelta.
            btnConferma.IsEnabled = false;
            txtbxNomeFile.Text = string.Empty;
            rdbtnStampaMatrice.IsChecked = false;
            rdbtnStampaRisultatoNomi.IsChecked = false;
            rdbtnStampaVettore.IsChecked = false;
        }

        private void txtbxNomeFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtbxNomeFile.Text))
            {
                rdbtnStampaMatrice.IsEnabled = true;
                rdbtnStampaVettore.IsEnabled = true;
                rdbtnStampaRisultatoNomi.IsEnabled = true;
            }
            else
            {
                rdbtnStampaMatrice.IsEnabled = false;
                rdbtnStampaVettore.IsEnabled = false;
                rdbtnStampaRisultatoNomi.IsEnabled = false;
            }
        }
    }
}
