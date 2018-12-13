using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_delegates
{
    class Program
    {
        //Mon delegate aura exactement la même signature que ma méthode !
        delegate bool PremierDelegate(string i);

        static void Main(string[] args)
        {
            //Test01(); // simple

            //Test02(); //utilisation de PremierDelegate

            //Test03();   // signature d'une méthoded: nom et ordre des paramètres
            // définition d'un délégate : type de retour et ordre des paramètres selon leur type

            //Test04(); // utilisation d'un delegate en tant que paramètre

            //Test05(); // Les dekegate génériques Finc<T, TResults> et Action<T>

            //Test06(); // Les méthodes anonymes

            //Test07(); //portée de vatiable un peu bizarre dans les mthodes anonymes

            Test08(); // Cas concret d'utiisation des méthodes anonymes

            Console.ReadKey();
        }

        private static void Test08()
        {
            //création d'une liste de personnes
            List<Personne> liste = new List<Personne>()
            {
                new Personne() {Nom = "Smith" , Prenom = "John" , Age=39},
                new Personne() {Nom = "West" , Prenom = "Adam" , Age=26},
                new Personne() {Nom = "Lebeau" , Prenom = "Ginette" , Age=58},
                new Personne() {Nom = "Lejeunesse" , Prenom = "Emilie" , Age=16}
            };

            // On rie la liste en lui spécifiant comment le faire (delegate Comparison)
            liste.Sort(delegate(Personne p1, Personne p2)
            {
                return p1.Age - p2.Age;
            });

            // Pour chaque personne dans la liste on l'affiche dans la console.
            liste.ForEach(delegate (Personne p) { Console.WriteLine(p); });

            /*
             Emilie Lejeunesse - 16 ans
            Adam West - 26 ans
            John Smith - 39 ans
            Ginette Lebeau - 58 ans
             * */

        }

        private static void Test07()
        {
            Console.WriteLine("Test de méthodes anonymes");

            int i = 13;
            int j = 42;

            Func<int> additionner = delegate () { return i + j; };

            Console.WriteLine(additionner.Invoke()); // Invoke est optionel

            Console.WriteLine(additionner()); // Invoke est optionel
            /*
             Test de méthodes anonymes
                55
                55
             * */

        }

        private static void Test06()
        {
            //Création d'un delegate avec une méthode anonyme. Dans ce cas, on pourra réappeler la méthode
            //tant et aussi longtemps que le delegate additionner sera dans la portée de la méthode.
            Func<int, int, int> additionner = new Func<int, int, int>(delegate (int i, int j) { return i + j; });

            //Forme allégée, mais tout aussi efficace. Attention à bien respecter la signature du delegate.
            Func<int, int, int> soustraire = (delegate (int i, int j) { return i - j; });

            //On passe à la méthode Afficher le delegate à éxécuter et les arguments.
            Afficher05(additionner, 25, 19);
            Afficher05(soustraire, 52, 17);
            /*
            25 < Test06 > b__2_0 19 = 44
            52 < Test06 > b__2_1 17 = 35
            */


            //On peut définir la méthode anonyme à utiliser "in-line". Attention, après l'éxécution de cette
            //ligne, on perd la référence mémoire à cette méthode.
            Afficher05(new Func<int, int, int>(delegate (int i, int j) { return i * j; }), 10, 12);

            //On peut aussi également utiliser la syntaxe simplifiée si la signature respecte
            //les requis de Func<int, int, int>.
            Afficher05((delegate (int i, int j) { return i / j; }), 325, 5);
            /*
             10 <Test06>b__2_2 12 = 120
             325 <Test06>b__2_3 5 = 65
             * */


        }



        // Action
        //  Le plus simple
        public delegate void Action();

        //  Le plus compliqué
        public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16>(
                T1 arg1,
                T2 arg2,
                T3 arg3,
                T4 arg4,
                T5 arg5,
                T6 arg6,
                T7 arg7,
                T8 arg8,
                T9 arg9,
                T10 arg10,
                T11 arg11,
                T12 arg12,
                T13 arg13,
                T14 arg14,
                T15 arg15,
                T16 arg16
            );

        // Function
        // Le plus simple:
        public delegate TResult Func<out TResult>();

        //   Le plus compliqué
        public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, out TResult>(
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15,
            T16 arg16
        );

        private static void Test05()
        {
            Afficher05(Add, 25, 19);
            Afficher05(Sub, 52, 17);
            Afficher05(Mul, 10, 52);
            Afficher05(Div, 325, 5);
            /*
            25 Add 19 = 44
            52 Sub 17 = 35
            10 Mul 52 = 520
            325 Div 5 = 65
             * */
        }

        private static void Afficher05(Func<int, int, int> calcul, int i, int j)
        {
            Console.WriteLine("{0} {1} {2} = {3}", i, calcul.Method.Name, j , calcul(i,j));
        }

        delegate int Calcul(int i1, int i2);

        private static void Test04()
        {
            //On passe à la méthode Afficher la méthode à lancer et les arguments.
            Afficher(Add, 25, 19);
            Afficher(Sub, 52, 17);
            Afficher(Mul, 10, 52);
            Afficher(Div, 325, 5);

            /*
            25 Add 19 = 44
            52 Sub 17 = 35
            10 Mul 52 = 520
            325 Div 5 = 65
             * */
        }

        //Méthodes très simples qui ont toutes un type de retour et des paramètres identiques.
        private static int Add(int i, int j) { return i + j; }
        private static int Sub(int i, int j) { return i - j; }
        private static int Mul(int i, int j) { return i * j; }
        private static int Div(int i, int j) { return i / j; }


        private static void Afficher(Calcul methodeCalcul, int i, int j)
        {
            Console.WriteLine("{0} {1} {2} = {3}", i, methodeCalcul.Method.Name, j , methodeCalcul(i,j));
        }

        private static void Test03()
        {
            Methode b = new Methode(ObtientNomComplet);

            string resultat = b("Pierre", "Desmazes");
            Console.WriteLine("Résultat : " + resultat);
            /*
             * Résultat : Pierre Desmazes
             * */
        }

        delegate string Methode(string a, string b);

        private static string ObtientNomComplet(string prenom, string nom)
        {
            return String.Format("{0} {1}", prenom, nom);
        }

        private static void Test02()
        {
            //Je crée une variable a qui contiendra la méthode Test.
            PremierDelegate a = new PremierDelegate(Test);

            //Au lieu d'appeler Test, je vais appeler a, ce qui me donnera le
            //même résultat !
            bool resultat = a("Ceci est un test négatif");
            Console.WriteLine("Resultat du premier test : " + resultat.ToString());

            bool res2 = a("Positif");
            Console.WriteLine("Resultat du second test : " + res2.ToString());
            /*
             Resultat du premier test : False
            Resultat du second test : True 
             * */
        }

        private static void Test01()
        {
            bool resultat = Test("Ceci est un test négatif");
            Console.WriteLine("Resultat du premier test : " + resultat.ToString());

            bool res2 = Test("Positif");
            Console.WriteLine("Resultat du second test : " + res2.ToString());
        }

        private static bool Test(string test)
        {
            return test.Length < 15;
        }
    }

    public class Personne
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} - {2} ans", Prenom, Nom, Age);
        }

    }
}
