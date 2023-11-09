using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;


namespace NAOUFEL_PENDU
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //creation d'un timer
        private DispatcherTimer timer;
        private TimeSpan timeLeft = TimeSpan.FromMinutes(5) + TimeSpan.FromSeconds(30);

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            Launch();
        }

        private void InitializeTimer()
        { //configuration du timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); //intervalle de temps de 1 s
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        //demarrer le timer
        private void StartTimer()
        {
            timer.Start();
        }
        //mettre en pause le timer
        private void StopTimer()
        {
            timer.Stop();
        }
        //reset du timer
        private void ResetTimer()
        {
            timer.Stop();  // Arrête le timer
            timeLeft = TimeSpan.FromMinutes(5) + TimeSpan.FromSeconds(30);  // Réinitialise le temps restant
            TB_timer.Text = timeLeft.ToString(@"mm\:ss");  // Met à jour l'affichage du temps
            StartTimer();  // Redémarre le timer
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
                TB_timer.Text = timeLeft.ToString(@"mm\:ss"); // Met à jour le Label avec le temps restant
            }
            else
            {
                timer.Stop();
                TB_Display.Text = " PERDU Vous êtes trop lent.";

                // Réinitialise le jeu
            }
        }

        //variable binaire de conditions
        bool Lettre_dedans = false;


        //chaines de caractere
        string mot_devine = "";
        string mot_affiche = "";
        //Liste des mots
        string[] ListMot =  {
        "PATATE", "OLIVE", "MUR", "VILLE", "ECRAN", "BOULE", "VOITURE", "CAMION", "PASTORALE", "LOURD", 
        "SALUT", "MONDE", "INFORMATIQUE", "PROGRAMMATION", "DEVELOPPEUR", "C#", "LANGAGE", "CONCEPTION",
        "APPLICATION", "ORDINATEUR", "SYSTEME", "LOGICIEL", "FICHIER", "COMPOSANT", "INTERFACE", "CLASSE",
        "OBJET", "BIBLIOTHEQUE", "METHODE", "VARIABLE", "CONSTANTE", "CONDITION", "BOUCLE", "TABLEAU",
        "STRUCTURE", "ALGORITHME", "EDITEUR", "COMPILATION", "DEBUGAGE", "OPTIMISATION", "RESEAU", "INTERNET",
        "SECURITE", "CRYPTAGE", "DEBOGAGE", "ERREUR", "EXCEPTION", "TRAITANT", "GESTIONNAIRE", "ASSEMBLAGE",
        "DEPLOIEMENT", "VERSION", "GESTION", "UTILISATEUR", "ADMINISTRATEUR", "AUTHENTIFICATION",
        "AUTORISATION", "SESSION", "IDENTIFIANT", "INSTRUIT", "POLLEN", "GYMNASTE", "CAGES", "RAPIDE", "MOTEUR",
        };

        string mot_affiche_sans_espace = "";

        //definitions de random + valeur de depart de nb de vie
        Random random = new Random();
        int NB_Vie = 0;

        int TimeBarTimer = 331;
        //def des variables pour le Launch
        int indexAleatoire;
        string MotAleatoire;
        string MotEtoile;

        //fonction pour la progress bar du timer
        public async void BarTimer()
        {
            for (int i = 0; i < TimeBarTimer; i++)//boucle pour enleer 1 a la valeur (300 seconde = 5 min 30 s)
            {
                TimeBarTimer--;
                ProgressBarTimer.Value = TimeBarTimer;
                await Task.Delay(1000); // Attendez 1 seconde sans bloquer le thread principal
            }
        }
        //fonction pour le demarage du jeu
        public void Launch()
        {

            //reactivation de boutton
            foreach (Button tout_bouton in Lettres.Children.OfType<Button>())
            {
                tout_bouton.IsEnabled = true;
            }
            //reset du nb de vie + selection du mot a deviner + le symbole * est afficher au nombre de lettres du mot caché + reset de ProgressBar
            mot_affiche = "";
            NB_Vie = 7;
            Random var = new Random();
            mot_devine = ListMot[var.Next(ListMot.Length)];
            for (int i = 0; i < mot_devine.Length; i++)
            {
                mot_affiche += "*";
            }
            TB_Display.Text = mot_affiche;
            TB_Life.Text = NB_Vie.ToString();
            progressBar.Value = NB_Vie;
            ProgressBarTimer.Value = TimeBarTimer;
            BarTimer();
        }

        //fonction pour verifier si Lettre proposer est la bonne
            public void Btn_CLICK(object sender, RoutedEventArgs e)
            {//on recupere la valeur du boutton 
            Button btn = sender as Button;
            string btnContent = btn.Content.ToString();//conversion en str
            StringBuilder newMotAffiche = new StringBuilder(mot_affiche);

            for (int i = 0; i < mot_devine.Length; i++)//verif de la letre proposer, en repetant i fois (i = nb de lettre du mot)
                {
                    if (btnContent == mot_devine[i].ToString())//si letrre proposer = lettre dans le mot --> desactive le BTN + met a jour le tableau newMotAffiche
                        {
                            Lettre_dedans = true;
                            if (Char.IsLetter(mot_devine[i]))
                                {
                                    newMotAffiche[i] = btnContent[0];
                                }
                        }
                }

            mot_affiche = newMotAffiche.ToString();//afficher la lettre trouver
            TB_Display.Text = mot_affiche;

            if (Lettre_dedans == false)//si lettre proposer est fausse  : enlever 1 vie + actualiser la barre de vie
            {
                NB_Vie -= 1;
                progressBar.Value = NB_Vie;
                TB_Life.Text = NB_Vie.ToString();

                if (NB_Vie == 0)//si nb vie tombe a 0 = perdu + texte + stop du timer
                {
                    TB_Display.Text = "PERDU , LE MOT ETAIT :" + "\n" + mot_devine;
                    timer.Stop();
                }

            }

            else if (mot_devine == mot_affiche)//si toute les lettres sont trouver
            {
                TB_Display.Text = "BIEN JOUER VOUS AVEZ GAGNER";
            }

            Uri resourceUri = new Uri("/PICTURE/"+ NB_Vie + "_vie.png", UriKind.Relative);//affiche une image en fonction du nb de vie
            Image_pendu.Source = new BitmapImage(resourceUri);


            //desactivation  du boutton la lettre utiliser + desactiasion de la variable Lettre_dedans (représente la condition sur l'etat la lettre proposer)
            Lettre_dedans = false;
            btn.IsEnabled = false;

            }



        private void TB_RESTART_Click(object sender, RoutedEventArgs e)//boutton du restart, remet a 0 la vie/timer/images
        {
            Launch();
            ResetTimer();
            Uri resourceUri = new Uri("/PICTURE/" + "7_vie.png", UriKind.Relative);
            Image_pendu.Source = new BitmapImage(resourceUri);
            TimeBarTimer = 331;
            ProgressBarTimer.Value = TimeBarTimer;
        }

        private void TB_STOP_Click(object sender, RoutedEventArgs e)//boutton de fermeture du jeu
        {
            Application.Current.Shutdown();

        }

    }

    }
