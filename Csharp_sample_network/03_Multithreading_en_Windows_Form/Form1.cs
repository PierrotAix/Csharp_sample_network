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

namespace _03_Multithreading_en_Windows_Form
{
    public partial class Form1 : Form
    {
        private Random rand = new Random((int)DateTime.Now.Ticks);

        private int[] tableau = new int[1000000];

        //On crée notre delegate
        public delegate void MontrerProgres(int valeur);

        bool termine = true;

        public Form1()
        {
            InitializeComponent();

            // on genere ici un tableau d'enier aléatoires
            for (int i = 0; i < tableau.Length; i++)
            {
                tableau[i] = rand.Next(50000);
            }
        }

        public void Selection()
        {
            // On va simplement compter les nombre du tableau inférieur à 500
            int total = 0;

            for (int i = 0; i < tableau.Length; i++)
            {
                if (tableau[i] < 500)
                {
                    total++;
                }

                //Puis, on incrémente le ProgressBar.
                int valeur = (int)(i / (double)tableau.Length * 100);

                //On achete la paix, on entoure notre Invoke d'un try...catch !
                try
                {
                    //On invoque le delegate pour qu'il effectue la tâche sur le temps de l'autre thread.
                    Invoke((MontrerProgres)Progres, valeur);
                }
                catch (Exception)
                {
                    return;
                }
            }

            termine = true;
        }

        private void Progres(int valeur)
        {
            // On met la valeur dans le contrôle Windows Forms.
            progressBar1.Value = valeur;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // todo
            MessageBox.Show("Je suis dans Form1_FormClosing()");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //petite sécurité pour éviter plusieurs threads en même temps
            if (termine)
            {
                //ON cree le thread
                Thread t1 = new Thread(new ThreadStart(Selection));

                termine = false;

                //puis on le lance
                t1.Start();
            }


        }
    }
}
