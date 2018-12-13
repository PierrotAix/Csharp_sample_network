using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _04_Exemple_BackgroundWorker
{
    //public delegate void DoWorkEventHandler(object sender, DoWorkEventArgs e);

    public partial class Form1 : Form
    {
        private bool _etat = false;

        

        public Form1()
        {
            InitializeComponent();

            //On demande à ce que le BackgroundWorker supporte le rapprt de progrés et l'annulation.
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            //On abonne le BackgroundWorker aux évenements requis.
            backgroundWorker1.DoWork+= new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;

            //Tant et aussi longtemps que la barre n'a pas atteint le 100 % et 
            // qu'on ne demande pas à annuler...
            while (i < 100 && !backgroundWorker1.CancellationPending)
            {
                //On attend 150 ms.
                Thread.Sleep(150);

                //on retrouve la valeur la plus petite enter 100 et i + 3
                i = Math.Min(100, i + 3);

                //On rapporte le progrès fait.
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Le bouton joue le role de démarrage comme d'annulation selon la situation.
            if(!_etat)
            {
                backgroundWorker1.RunWorkerAsync();
                button1.Text = "Annuler";
            }
            else
            {
                backgroundWorker1.CancelAsync();
                button1.Text = "Démarrer";
            }

            _etat = !_etat;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //On fait avancer la Progress Bar
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // lorsque c'est terminé, on affiche un message indiquant la fin de l'activité
            MessageBox.Show("Le BackgroundWorker a terminé");
        }
    }
}
