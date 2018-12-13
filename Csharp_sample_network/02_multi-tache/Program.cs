using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02_multi_tache
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test01();

            //Test02(); // Exemple de passage de paramètre au thread

            //Test03(); //Les mécanistes de synchronisation

            //Test04(); // exemple du mécanisme de lock

            //Test05(); // exemple du mécanisme de Mutex

            //Test06(); // exemple du mécanisme de SemaphoreSlim

            //Test07(); // exemple du mécanisme de Join

            Test08(); //comment bien arrêter un thread avec Join

            Console.ReadKey();
        }

        private static void Test08()
        {
            Thread thread = new Thread(Test);

            thread.Start();
            Thread.Sleep(100);

            // on demande au thread de s'arrêter au prochain passege d'un moment qui semble naturel.
            _continuer = false;

            //On attend que le thread se termine proprement.
            thread.Join();

            /*
            154
            155
            156
            157
            158
            159
            160
            161
            162
            163
             * */
        }

        private static void Test()
        {
            // on fait 10000 itérations, tant et aussi longtemps que l'on peut continuer (variable de contrôle).
            for (int i = 0; i < 10000 && _continuer; i++)
            {
                Console.WriteLine(i);
            }
        }

        private static void Test07()
        {
            Thread th = new Thread(new ParameterizedThreadStart(Afficher02));

            Thread th2 = new Thread(new ParameterizedThreadStart(Afficher02));

            th.Start("A");

            //On attend la fin du thread A avant de commencer le thread B.
            th.Join();

            th2.Start("B");
        }

        private static void Test06()
        {
            Console.Title = "Exemple de SemaphoreSlim";

            //Creation des threads.
            for (int i = 0; i < 10; i++)
            {
                new Thread(Entrer).Start(i);
            }

            /*
             La personne #0 veut entrer
            La personne #2 veut entrer
            #2 vient d'entrer dans le bar
            #0 vient d'entrer dans le bar
            #0 a quitté le building !
            La personne #1 veut entrer
            #1 vient d'entrer dans le bar
            La personne #3 veut entrer
            #3 vient d'entrer dans le bar
            La personne #4 veut entrer
            La personne #5 veut entrer
            La personne #6 veut entrer
            La personne #7 veut entrer
            La personne #8 veut entrer
            La personne #9 veut entrer
            #1 a quitté le building !
            #4 vient d'entrer dans le bar
            #2 a quitté le building !
            #5 vient d'entrer dans le bar
            #3 a quitté le building !
            #6 vient d'entrer dans le bar
            #4 a quitté le building !
            #7 vient d'entrer dans le bar
            #5 a quitté le building !
            #8 vient d'entrer dans le bar
            #6 a quitté le building !
            #9 vient d'entrer dans le bar
            #7 a quitté le building !
            #8 a quitté le building !
            #9 a quitté le building !
             * */
        }

        private static void Entrer(object n)
        {
            Console.WriteLine("La personne #{0} veut entrer", n);

            //Le doorman attendra qu'il y ait de la place
            doorman.Wait();
            Console.WriteLine("#{0} vient d'entrer dans le bar", n);
            Thread.Sleep((int)n * 1000);
            Console.WriteLine("#{0} a quitté le building !", n);

            //Le doorman peut maintenant faire entrer quelqu'un d'autre
            doorman.Release();
        }

        private static void Test05()
        {
            Console.Title = "Exemple de Mutex";

            //On cree et on démarre les threads
            Thread init = new Thread(InitialiserM);
            init.Start();

            Thread mul = new Thread(Multiplier);
            mul.Start();

            Thread div = new Thread(DiviserM);
            div.Start();

            //On laisse les threads fonctionner un peu...
            Thread.Sleep(3000);

            //On demande à ce que les opérations se terminent
            _quitter = true;



        }

        private static void DiviserM()
        {
            while (!_quitter)
            {
                //On demande le Mutex de division
                _muxDiviser.WaitOne();

                //On divise
                Console.WriteLine("{0} / {1} = {2}", _valDiv[0], _valMul[1], _valDiv[0] / _valDiv[1]);

                //On relache le Mutex de Division
                _muxDiviser.ReleaseMutex();

                //On tombe endormis pour 200ms
                Thread.Sleep(200);

                /*
                 * ...
                 Nouvelles valeurs !
                Nouvelles valeurs !
                4 x 8 = 32
                11 / 8 = 1
                Nouvelles valeurs !
                Nouvelles valeurs !
                3 x 7 = 21
                13 / 7 = 1
                Nouvelles valeurs !
                Nouvelles valeurs !
                 * */
            }
        }

        private static void Multiplier()
        {
            while (!_quitter)
            {
                //On demande le Mutex de multiplication
                _muxMultiplier.WaitOne();

                //On multiplie.
                Console.WriteLine("{0} x {1} = {2}", _valMul[0], _valMul[1], _valMul[0] * _valMul[1] );

                //On relâche le Mutex.
                _muxMultiplier.ReleaseMutex();

                //On tombe endormis pour 200ms.
                Thread.Sleep(200);
            }
        }

        private static void InitialiserM()
        {
            while (!_quitter)
            {
                //On demande au thread d'attendre jusqu'à ce qu'il ait le contrôle sur les Mutex.
                _muxMultiplier.WaitOne();
                _muxDiviser.WaitOne();

                for (int i = 0; i < TAILLE_TABLEAU; i++)
                {
                    //on assigne au tableau de nouvelles valeurs
                    _valMul[i] = _rand.Next(2, 20);
                    _valDiv[i] = _rand.Next(2, 20);
                }

                Console.WriteLine("Nouvelles valeurs !");

                //On relache les Mutex
                _muxDiviser.ReleaseMutex();
                _muxMultiplier.ReleaseMutex();

                //On tomne endormi pour 100ms.
                Thread.Sleep(100);

            }
        }

        private static void Test04()
        {
            Console.Title = "Démonstration des lock";

            //On crée les threads.
            Thread init = new Thread(Initialiser);
            init.Start();

            Thread reinit = new Thread(Reinitialiser);
            reinit.Start();

            Thread div = new Thread(Diviser);
            div.Start();

            // On les laisse travailler pendant 3 secondes
            Thread.Sleep(3000);
            // puis on leur demande de quitter.
            _quitter = true;

            /*
             * 
            Division par 0
            Division par 0
            14 / 17 = 0,823529411764706
            7 / 10 = 0,7
            17 / 20 = 0,85
            1 / 2 = 0,5
            15 / 26 = 0,576923076923077
            11 / 3 = 3,66666666666667
            18 / 29 = 0,620689655172414
            6 / 15 = 0,4
            0 / 23 = 0
             * */
        }

        private static void Diviser()
        {
            //Boucle infinie contrôlée.
            while (!_quitter)
            {
                //On verouille pendant les opérations
                lock (_lock)
                {
                    //Erreur si le dénominateur est nul
                    if (_denominateur == 0)
                        Console.WriteLine("Division par 0");
                    else
                    {
                        Console.WriteLine("{0} / {1} = {2}", _numerateur, _denominateur, _numerateur/(double)_denominateur);
                    }

                    //On recommence dans 275ms.
                    Thread.Sleep(275);
                }

            }
        }

        private static void Reinitialiser()
        {
            //Boucle infinie contrôlée.
            while (!_quitter)
            {
                // on verouille l'accès aux variables tant que l'on a pas terminé.
                lock (_lock)
                {
                    //Réinitialisation des valeurs
                    _numerateur = 0;
                    _denominateur = 0;
                }

                //on recommence dans 300ms.
                Thread.Sleep(300);

            }
        }

        private static void Initialiser()
        {
            //Boucle infinie contrôlée.
            while (!_quitter)
            {
                //On verrouille l'accès aux variables tant que l'on a pas terminé.
                lock (_lock)
                {
                    //initialisation des valeurs
                    _numerateur = _rand.Next(20);
                    _denominateur = _rand.Next(2, 30);
                }

                //On rececommence dans 250 ms.
                Thread.Sleep(250);
            }
        }

        private static void Test03()
        {
            Console.Title = "Variable de contrôle";

            // On crée un tableau de threads.
            Thread[] threads = new Thread[5];

            // On itère à travers le tableau afin de creer et lancer les threads.
            for (int i = 0; i < threads.Length; i++)
            {
                //Création et lancement des threads.
                threads[i] = new Thread(OperThread);
                threads[i].Start();

                //On laisse passer 500 ms entre les création de thread
                Thread.Sleep(500);
            }

            //On demande à ce que tous les threads quittent.
            _quitter = true;

            /*
             * Début du thread 1
            Thread 1 a la contrôle
            Début du thread 2
            Thread 2 a la contrôle
            Thread 1 a la contrôle
            Début du thread 3
            Thread 3 a la contrôle
            Thread 2 a la contrôle
            Début du thread 4
            Thread 4 a la contrôle
            Thread 1 a la contrôle
            Thread 3 a la contrôle
            Début du thread 5
            Thread 5 a la contrôle
            Thread 2 a la contrôle
            Thread 4 terminé
            Thread 1 terminé
            Thread 3 terminé
            Thread 5 terminé
            Thread 2 terminé 
             * */
        }

        static void OperThread()
        {
            //On donne au thread un identificateur unique
            int id = ++_identificateur;

            Console.WriteLine("Début du thread {0}",id);

            while (!_quitter)
            {
                //On fait des choses ici tant qu'on ne désire pas quitter...
                Console.WriteLine("Thread {0} a la contrôle", id);

                // on met le thread en sommeil pour 1 seconde
                Thread.Sleep(1000);
            }

            Console.WriteLine("Thread {0} terminé", id);

        }

        private static void Test02()
        {
            Thread th = new Thread(new ParameterizedThreadStart(Afficher02));

            Thread th2 = new Thread(new ParameterizedThreadStart(Afficher02));

            th.Start("A");
            th2.Start("B");

        }
        /// <summary>
        /// La méthode prend en paramètre un et un seul paramètre de type Object.
        /// </summary>
        /// <param name="texte"></param>
        private static void Afficher02(object texte)
        {
            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine((string)texte);
            }
            Console.WriteLine("<----------- thread {0} terminé------------->", (string)texte);
        }

        private static void Test01()
        {
            //new Thread(fonction).Start(); //thread indépendant

            //On initialise l'object Thread en lui passant la méthode
            //à exécuter dans le nouveau thread. Ça vous rappelle pas
            //certains delegates ça ?
            Thread th = new Thread(Afficher);

            //Un thread, ça ne part pas tout seul. Il faut lui indiquer de
            //commencer l'exécution.
            th.Start();
        }

        private static void Afficher()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("A");
            }            
        }

        //queslques variables à portée globale pour le Test03 --------------------------------
        private static bool _quitter = false; // aussi utile pour le Test04 et Test05
        private static int _identificateur = 0;

        // utile pour le Test04 ---------------------------------------------------------------
        //variable témoin du lock
        private static Object _lock = new object();

        //Sert à initialiser des valeurs pseudos-aléatoires.
        private static Random _rand = new Random((int)DateTime.Now.Ticks); // aussi utile pour Test05

        //Varaiables globales étant affectées par les threads.
        private static int _numerateur;
        private static int _denominateur;

        // Utile pour Test05 sur les Mutex ----------------------------------------------------
        private const int TAILLE_TABLEAU = 2;

        //on cree les Mutex
        private static Mutex _muxMultiplier = new Mutex();
        private static Mutex _muxDiviser = new Mutex();

        //On crée les tableaux de valeurs.
        private static int[] _valDiv = new int[TAILLE_TABLEAU];
        private static int[] _valMul = new int[TAILLE_TABLEAU];

        // Utile pour Test06 sur les SemaphoreSlim----------------------------------------------
        static SemaphoreSlim doorman = new SemaphoreSlim(3); // declaration avec en paramètre le nombre de places disponibles.

        // Utile pour Test08 sur les Join----------------------------------------------
        private static bool _continuer = true;

    }
}
