using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace _07_Serialisation_binaire
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Personnage> equipe = Charger<List<Personnage>>("data.bin");

            if (equipe == null)
            {
                equipe = new List<Personnage>();

                equipe.Add(new Magicien() { Nom = "Galgutta", Niveau = 12, PointVie = 58, PointMagie = 200, NomBaguette = "ombra" });
                equipe.Add(new Guerrier() { Nom = "Ulgur", Niveau = 14, PointVie = 200, Force = 62, NomEpee = "Destructor" });
            }

            Enregistrer(equipe, "data.bin");

            Console.ReadKey();
        }

        private static void Enregistrer(object toSave, string path)
        {
            //On utilise la classe BinaryFormatter dans le namespace System.Runtime.Serialization.Formatters.Binary
            BinaryFormatter formatter = new BinaryFormatter();

            //La classe BinaryFormatter ne fonctionne qu'avec un flux et non pas un TextWriter
            //Nous allons donc utiliser un FileStream. Remarquez que n'importe quel flux est compatible.
            FileStream flux = null;
            try
            {
                //On ouvre le flux en mode création / écrasement de fichier et on donne
                // au flux le droit en écriture seulement
                flux = new FileStream(path, FileMode.Create, FileAccess.Write);
                // Et hop ! On sérialise !
                formatter.Serialize(flux, toSave);
                // On s'assure que le tout soit écrit dans le fichier
                flux.Flush();
            }
            catch { }
            finally
            {
                //Et on ferme le flux
                if (flux != null)
                    flux.Close();

            }
        }

        private static T Charger<T>(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream flux = null;
            try
            {
                //On ouvre le fichier en mode lecture seule.
                //De plus, puisqu'on a sélectionné le mode Open,
                //si le fichier n'existe pas, une exception sera levée.
                flux = new FileStream(path, FileMode.Open, FileAccess.Read);

                return (T)formatter.Deserialize(flux);
            }
            catch 
            {
                //On retrourne la valeur par défaut du type T.
                return default(T);
            }
            finally
            {
                if (flux != null)
                    flux.Close();
            }
        }
    }

    [Serializable()]
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
}
