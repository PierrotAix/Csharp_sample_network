using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _05_TP_partie1
{




    class Program
    {
        private static Mutex _security = new Mutex();

        public class ListeTriable<T>
        {
            private T[] _liste = new T[4];
            public int Count { get; private set; }

            public ListeTriable()
            {
                Count = 0;
            }

            public T this[int i]
            {
                get
                {
                    return _liste[i];
                }
            }

            public void Ajouter(T element)
            {
                //on multiplis la taille par deux si nécéssaire
                if (_liste.Length >= Count)
                {
                    Array.Resize<T>(ref _liste, _liste.Length * 2);
                }

                //Ajout de l'élément à la fin de la liste et incrémentation du décompte d'éléments
                // dans la liste
                _liste[Count++] = element;
            }

            public ListeTriable<T> Tri()
            {
                ListeTriable<T> listeTriableTrie = new ListeTriable<T>();
                for (int i = 0; i < Count; i++)
                {
                    listeTriableTrie.Ajouter(this[i]);
                }
                return listeTriableTrie;

                // tri a bulle en comparant les valeurs de this[i]
                /*
                int n = Count - 1;
                for (int i = n; i >= 1; i--)
                {
                    for (int j = 2; j <= i ; j++)
                    {
                        if (this[j-1] - this[j] < 0) // ca ne marche pas
                        {

                        }
                        T temp = this[j - 1];
                    }
                }
                */
            }

            //public int CompareElement(T element1, T element2)
            //{

            //    if (element1 is int && element2 is int)
            //    {
            //        Int16 = Int32.TryParse()
            //        return (int)element1 < element2 ? -1 : element1 > element2 ? 1 : 0;
            //    }
            //}

            public void Trier(Func<T, T, int> trieur)
            {
                //tri a bulle
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = i + 1; j < Count; j++)
                    {
                        int resultat = trieur.Invoke(_liste[i], _liste[j]);

                        if (resultat >= 1)
                        {
                            //on échange les éléments si le deuxiere élément est plus grans.
                            T element1 = _liste[i];
                            _liste[i] = _liste[j];
                            _liste[j] = element1;
                        }
                    }
                }
            }

            public void DebuterTri(Func<T, T, int> trieur, Action<ListeTriable<T>> callback)
            {
                //On demarre le nouveau thread.
                Thread sortThread = new Thread(delegate ()
                {
                    // sécurité par Mutex
                    _security.WaitOne();

                    for (int i = 0; i < Count - 1; i++)
                    {
                        for (int j = i + 1; j < Count; j++)
                        {
                            int resultat = trieur.Invoke(_liste[i], _liste[j]);

                            if (resultat <= 1)
                            {
                                T element1 = _liste[i];
                                _liste[i] = _liste[j];
                                _liste[j] = element1;
                            }
                        }
                    }

                    //et on relache la sécurité
                    _security.ReleaseMutex();

                    //On invoque la méthode rappel si elle est défini.
                    if (callback != null)
                        callback.Invoke(this);

                }); // fin du delegate

                //Démarrage du thread de tri
                sortThread.Start();
            }
        }



        public class Personne
        {
            public string Prenom { get; set; }
            public string Nom { get; set; }
            public int Age { get; set; }

            public Personne(string prenom, string nom, int age)
            {
                Prenom = prenom;
                Nom = nom;
                Age = age;
            }

            public override string ToString()
            {
                return String.Format("{0} {1} -> {2} ans", Prenom, Nom, Age);
            }
        }

        static void Main(string[] args)
        {

            // Test de ListeTriable
            // sur des entiers
            ListeTriable<int> listeTriable = new ListeTriable<int>();
            listeTriable.Ajouter(45);
            listeTriable.Ajouter(20);
            listeTriable.Ajouter(10);
            listeTriable.Ajouter(13);
            listeTriable.Ajouter(606);

            ListeTriable<int> listeTriableTrie = new ListeTriable<int>();
            listeTriableTrie = listeTriable.Tri(); //ne marche pas

            for (int i = 0; i < listeTriable.Count; i++)
            {
                Console.WriteLine("L'élement listeTriable[" + i + "] est " + listeTriable[i].ToString());
            }

            for (int i = 0; i < listeTriableTrie.Count; i++)
            {
                Console.WriteLine("L'élement listeTriableTrie[" + i + "] est " + listeTriableTrie[i].ToString());
            }

            // sur des personnes
            ListeTriable<Personne> listeTriablePersonne = new ListeTriable<Personne>();
            listeTriablePersonne.Ajouter(new Personne("Christine", "DESMAZES", 51));
            listeTriablePersonne.Ajouter(new Personne("Julia", "DESMAZES", 23));
            listeTriablePersonne.Ajouter(new Personne("Flora", "DESMAZES", 20));
            listeTriablePersonne.Ajouter(new Personne("Pierre", "DESMAZES", 52));

            Console.WriteLine("Avant le tri ----------------------------------------");
            for (int i = 0; i < listeTriablePersonne.Count; i++)
            {
                Console.WriteLine("L'élement " + i + " est " + listeTriablePersonne[i].ToString());
            }

            if (false) // code avant le multitache
            {
                // On tri selon la logique spécifiée... ici c'est le critere de l'age
                listeTriablePersonne.Trier(delegate (Personne p1, Personne p2)
                {
                    return p1.Age - p2.Age;
                });
                Console.WriteLine("Après le tri ----------------------------------------");
                for (int i = 0; i < listeTriablePersonne.Count; i++)
                {
                    Console.WriteLine("L'élement " + i + " est " + listeTriablePersonne[i].ToString());
                }
                /*
                 *Avant le tri ----------------------------------------
                L'élement 0 est Christine DESMAZES -> 51 ans
                L'élement 1 est Julia DESMAZES -> 23 ans
                L'élement 2 est Flora DESMAZES -> 20 ans
                L'élement 3 est Pierre DESMAZES -> 52 ans
                Après le tri ----------------------------------------
                L'élement 0 est Flora DESMAZES -> 20 ans
                L'élement 1 est Julia DESMAZES -> 23 ans
                L'élement 2 est Christine DESMAZES -> 51 ans
                L'élement 3 est Pierre DESMAZES -> 52 ans
                 * */
            }
            else
            {
                listeTriablePersonne.DebuterTri(delegate (Personne p1, Personne p2)
                {
                    return p1.Age - p2.Age;
                }, TriFin);

                Console.WriteLine("Après le tri ----------------------------------------");
                /*
                 Avant le tri ----------------------------------------
                L'élement 0 est Christine DESMAZES -> 51 ans
                L'élement 1 est Julia DESMAZES -> 23 ans
                L'élement 2 est Flora DESMAZES -> 20 ans
                L'élement 3 est Pierre DESMAZES -> 52 ans
                Après le tri ----------------------------------------
                Pierre DESMAZES -> 52 ans
                Christine DESMAZES -> 51 ans
                Julia DESMAZES -> 23 ans
                Flora DESMAZES -> 20 ans* 
                 * */
            }





            /*
            InitTable();
            System.Console.WriteLine("Tableau initial :");
            AfficherTable();
            TriBulle();
            System.Console.WriteLine("Tableau une fois trié :");
            AfficherTable();
            */
            /*
             * 
             *Tableau initial :
            25 , 7 , 14 , 26 , 25 , 53 , 74 , 99 , 24 , 98 , 89 , 35 , 59 , 38 , 56 , 58 , 36 , 91 , 52 ,
            Tableau une fois trié :
            7 , 14 , 24 , 25 , 25 , 26 , 35 , 36 , 38 , 52 , 53 , 56 , 58 , 59 , 74 , 89 , 91, 98 , 99 ,
             * */

            Console.ReadKey();
        }

        private static void TriFin(ListeTriable<Personne> liste)
        {
            for (int i = 0; i < liste.Count; i++)
            {
                Console.WriteLine(liste[i]);
            }
        }

        static int[] table; // le tableau à trier, par exemple 19 éléments
    static void AfficherTable()
    {
        // Affichage du tableau  
        int n = table.Length - 1;
        for (int i = 1; i <= n; i++)
            System.Console.Write(table[i] + " , ");
        System.Console.WriteLine();
    }
    static void InitTable()
    {
        int[] tableau = { 0 ,25, 7 , 14 , 26 , 25 , 53 , 74 , 99 , 24 , 98 ,
                                  89 , 35 , 59 , 38 , 56 , 58 , 36 , 91 , 52 };
        table = tableau;
    }

    static void TriBulle()
    {
        // sous-programme de Tri à bulle  : on trie les éléments du n°1 au  n°19 
        int n = table.Length - 1;
        for (int i = n; i >= 1; i--)
            for (int j = 2; j <= i; j++)
                if (table[j - 1] > table[j])
                {
                    int temp = table[j - 1];
                    table[j - 1] = table[j];
                    table[j] = temp;
                }
        /* Dans le cas où l'on démarre le tableau à l'indice zéro 
            on change les bornes des indices i et j: 
                for ( int i = n; i >= 0; i--) 
                for ( int j = 1; j <= i; j++)  
                     if ....... reste identique 
        */
    }
}
}
