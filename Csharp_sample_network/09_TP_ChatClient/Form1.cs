﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _09_TP_ChatClient
{
    public partial class FrmClient : Form
    {
        //Déclaration des objects nécessaires au client.
        private UdpClient _client;

        private bool _continuer;
        private Thread _thEcouteur;

        /// <summary>
        /// Constructeur de la base FrmClient
        /// </summary>
        public FrmClient()
        {
            InitializeComponent();

            //On crée automatiquement le client qui sera en charge d'envoyer les messages au serveur.
            _client = new UdpClient();
            _client.Connect("127.0.0.1", 1523);

            //Initialisation des objets nécessaires au client.
            // On lance également le thread qui sera en charge d'écouter.
            _continuer = true;
            _thEcouteur = new Thread(new ThreadStart(ThreadEcouteur));
            _thEcouteur.Start();
        }

        /// <summary>
        /// Fonction en charge d'écouter les communications réseau.
        /// </summary>
        private void ThreadEcouteur()
        {
            //Déclaration du Socket d'écoute.
            UdpClient ecouteur = null;

            //Création sécurisé du Socket.
            try
            {
                ecouteur = new UdpClient(5053);
            }
            catch 
            {
                MessageBox.Show("Impossible de se lier au port UDP 5053. " +
                    "Verifiez vos configurations réseau.");
                return;
            }

            //Définition du Timeout.
            ecouteur.Client.ReceiveTimeout = 1000;

            //Bouclage infini d'écoute de port
            while (_continuer)
            {
                try
                {
                    IPEndPoint ip = null;
                    byte[] data = ecouteur.Receive(ref ip);

                    //Invocation de la méthode AjouterLog afin que les données soient 
                    // inscrites dans la TextBox
                    this.Invoke(new Action<string>(AjouterLog), Encoding.Default.GetString(data));
                }
                catch 
                {
                }
            }

            ecouteur.Close();
        }

        /// <summary>
        /// Méthode qui sert à ajouter un message à la console du Client.
        /// </summary>
        /// <param name="data"></param>
        private void AjouterLog(string data)
        {
            txtResultat.AppendText("\r\n" + data);
        }

        /// <summary>
        /// Gestion de l'envoie d'un message. ¨Pas besoin d'un thread séparé pour cela,
        /// les données sont trop légères pour que ça ne vaille la peine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnvoyer_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes(txtMessage.Text);
            _client.Send(data, data.Length);

            txtMessage.Clear();
            txtMessage.Focus();
        }

        /// <summary>
        /// Gestion de la fermeture du formulaire. 
        /// On arrête d'écouter et on attend la fermeture complète du thread d'écoute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            _continuer = false;
            _client.Close();
            _thEcouteur.Join();
        }


    }
}
