using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _06_Serialisation_des_objets
{

    class Program
    {
        private const string CHEMIN = "Test.xml";

        static void Main(string[] args)
        {
            //Test01(); // Exemple de sérialisation dans Test.xml

            //Test02(); //Exemple de sérialisation depuis Test.xml

            //Test03(); //Exemple de sérialisation d'une collection

            Test04(); //Exemple de l'héritage



            Console.ReadKey();
        }

        private static void Test04()
        {
            List<Personnage> equipe = new List<Personnage>();

            equipe.Add(new Magicien() { Nom = "Galgutta", Niveau = 12, PointVie = 58, PointMagie = 200, NomBaguette = "ombra" });
            equipe.Add(new Guerrier() { Nom = "Ulgur", Niveau = 14, PointVie = 200, Force = 62, NomEpee = "Destructor" });

            XmlSerializer serializer = new XmlSerializer(typeof(List<Personnage>));
            StreamWriter ecrivain = new StreamWriter("sauvegarde.sav");
            serializer.Serialize(ecrivain, equipe);
            ecrivain.Flush();
            ecrivain.Close();

            /*
             <?xml version="1.0" encoding="utf-8"?>
            <ArrayOfPersonnage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
              <Personnage xsi:type="Magicien">
                <Nom>Galgutta</Nom>
                <Niveau>12</Niveau>
                <PointVie>58</PointVie>
                <PointMagie>200</PointMagie>
                <NomBaguette>ombra</NomBaguette>
              </Personnage>
              <Personnage xsi:type="Guerrier">
                <Nom>Ulgur</Nom>
                <Niveau>14</Niveau>
                <PointVie>200</PointVie>
                <Force>62</Force>
                <NomEpee>Destructor</NomEpee>
              </Personnage>
            </ArrayOfPersonnage>
             * */
        }

        private static void Test03()
        {
            PersonneSimples personneSimples = null;

            if(File.Exists(CHEMIN))
            {
                personneSimples = PersonneSimples.Charger(CHEMIN);
            }
            else
            {
                personneSimples = new PersonneSimples();
            }

            //Etapes ici.
            personneSimples.Add(new PersonneSimple() { Nom = "Larivière", Prenom = "Germain" });
            personneSimples.Add(new PersonneSimple() { Nom = "Dancereau", Prenom = "Nancy" });

            //Sauvegarde
            personneSimples.Enregistrer(CHEMIN);

            /*
             *<?xml version="1.0" encoding="utf-8"?>
            <ArrayOfPersonneSimple xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
              <PersonneSimple>
                <Nom>Larivière</Nom>
                <Prenom>Germain</Prenom>
              </PersonneSimple>
              <PersonneSimple>
                <Nom>Dancereau</Nom>
                <Prenom>Nancy</Prenom>
              </PersonneSimple>
            </ArrayOfPersonneSimple>
             * */

        }

        private static void Test02()
        {
            //On crée une instance de XmlSerializer dans lequel on lui spécifie le type
            // de l'objet à sérialiser. On va utiliser l'opérateur typeof pour cela.
            XmlSerializer serializer = new XmlSerializer(typeof(Personne));

            //Création d'un StreamReader qui permet de lire un fichier dont on donne le chemin.
            StreamReader lecteur = new StreamReader("Test.xml");

            //On obtient une instance de l'objet désérialisé. 
            //Attention, la méthode Deserialize renvoie un objet de type Object.
            // Il faut donc le transtyper(caster).
            Personne p = (Personne)serializer.Deserialize(lecteur);

            //IMPORTANT : on ferme le flux en tous temps !!!
            lecteur.Close();

            // Affichage de l'objet
            Console.WriteLine($" nom {p.Nom} prenom: {p.Prenom} age {p.Age}");
            /*
             *  nom DEspotins prenom: Yvan age 55
             * */


        }

        private static void Test01()
        {
            Personne p = new Personne()
            {
                Nom = "DEspotins",
                Prenom = "Yvan",
                Age = 55,
                Adresse = new Adresse()
                {
                    Numero = 12,
                    Rue = " des saules",
                    Ville = "Montréal",
                    Pays = "CANADA"
                }
            };

            //On crée une instance de XmlSerializer dans lequel on lui spécifie le type
            //de l'objet à sérialiser. On utilise l'opérateur typeof pour cela.
            XmlSerializer serializer = new XmlSerializer(typeof(Personne));

            //Création d'un Stream Writer qui permet d'écrire dans un fichier.
            // on lui spécifie le chemin et par false c'est pour l'écraser si le flux
            // devrait mettre le contenu à la suite
            StreamWriter ecrivain = new StreamWriter("Test.xml", false);

            //On sérialise en spécifiant le flux d'écriture et l'objet à sérialiser.
            serializer.Serialize(ecrivain, p);

            //IMPORTANT : on ferme le flux en tous temps !!!
            ecrivain.Close();

            Console.WriteLine("Fin de l'écriture d'un fichier Test.xml");
            /*
             * contenu de D:\BITBUCKET\demo\Csharp_sample_network\Csharp_sample_network\06_Serialisation_des_objets\bin\Debug\Test.xml
             * avec [XmlAttribute()]
             *<?xml version="1.0" encoding="utf-8"?>
            <Personne xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Nom="DEspotins" Prenom="Yvan" Age="55">
              <Adresse Numero="12" Rue=" des saules" Ville="Montréal" Pays="CANADA" />
            </Personne>

            * sans [XmlAttribute()] devant Adresse.Numero , rue et Ville
            <?xml version="1.0" encoding="utf-8"?>
            <Personne xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Nom="DEspotins" Prenom="Yvan" Age="55">
              <Adresse Pays="CANADA">
                <Numero>12</Numero>
                <Rue> des saules</Rue>
                <Ville>Montréal</Ville>
              </Adresse>
            </Personne>

             * */
        }
    }

    #region Test01 et Test02 -----------------------------------------------------------
    [Serializable]
    public class Personne
    {
        [XmlAttribute()]
        public string Nom { get; set; }

        [XmlAttribute()]
        public string Prenom { get; set; }

        [XmlAttribute()]
        public int Age { get; set; }

        public Adresse Adresse { get; set; }

    }

    [Serializable]
    public class Adresse
    {
        [XmlAttribute()]
        public int Numero { get; set; }

        [XmlAttribute()]
        public string Rue { get; set; }

        [XmlAttribute()]
        public string Ville { get; set; }

        [XmlAttribute()]
        public string Pays { get; set; }

    }
    #endregion

    #region Test03 -----------------------------------------------------------
    [Serializable]
    public class PersonneSimple
    {
        [XmlAttribute()]
        public string Nom { get; set; }

        [XmlAttribute()]
        public string Prenom { get; set; }
    }

    [Serializable()]
    public class PersonneSimples : List<PersonneSimple> //On hérite d'une liste de personnes. 
    {
        /// <summary>
        /// Enregistre l'état courant de la classe dans un fichier au format XML.
        /// </summary>
        /// <param name="chemein"></param>
        public void Enregistrer(string chemin)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PersonneSimples));
            StreamWriter ecrivain = new StreamWriter(chemin);
            serializer.Serialize(ecrivain, this);
            ecrivain.Close();
        }

        public static PersonneSimples Charger(string chemin)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(PersonneSimples));
            StreamReader lecteur = new StreamReader(chemin);
            PersonneSimples p = (PersonneSimples)deserializer.Deserialize(lecteur);
            lecteur.Close();

            return p;
        }
    }

    #endregion

    #region Test04 -----------------------------------------------------------
    [Serializable(), XmlInclude(typeof(Guerrier)), XmlInclude(typeof(Magicien))]
    public abstract class Personnage
    {
        public string Nom { get; set; }

        public int Niveau { get; set; }

        public int PointVie { get; set; }
    }

    [Serializable]
    public class Magicien : Personnage
    {
        public int PointMagie { get; set; }

        public string NomBaguette { get; set; }
    }

    [Serializable]
    public class Guerrier : Personnage
    {
        public int Force { get; set; }

        public string NomEpee { get; set; }
    }


    #endregion
}
