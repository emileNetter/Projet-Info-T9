using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Info_S1_v4
{
    class Program
    {
        public static string traductionMultimap(string messageEncode, char[][] caracteres) // penser à gestion des erreurs si faute de frappe
        {
            char lettre = ' ';
            string messageTraduit = "";
            int lgMessage = messageEncode.Length;
            int compteur = 0;



            for (int i = 0; i < lgMessage; i++)
            {
                compteur = 0;

                int indice = (int)char.GetNumericValue(messageEncode[i]);
                while (i < lgMessage - 1 && messageEncode[i] == messageEncode[i + 1])
                {

                    compteur++;
                    i++;
                }
                try
                {
                    if (indice != 0)
                    {
                        lettre = caracteres[indice - 1][compteur];
                        messageTraduit = messageTraduit + lettre;
                    }

                }
                catch 
                {
                    Console.WriteLine("Le format de la chaîne d'entrée est incorrect" );
                }
            }

            return messageTraduit;


        }

        public static string[,] decodeMotDico(char[][] caracteres)// esssayer de charger dico une fois pour toutes dans une autree fonction
        {

            string[] motDico = System.IO.File.ReadAllLines(@"dicoFR.txt");
            int tailleDico = motDico.Length;

            string[,] tabDecodeMot = new string[tailleDico, 2];
            string code = "";
            char lettre = ' ';
            int m = 0;
            int n = 0;


            for (int i = 0; i < tailleDico; i++)
            {
                code = "";
                tabDecodeMot[i, 0] = motDico[i].ToLower();//ecrit en minuscule les mots
                int tailleMot = tabDecodeMot[i, 0].Length;

                for (int k = 0; k < tailleMot; k++)
                {

                    lettre = tabDecodeMot[i, 0][k];

                    for (m = 0; m < caracteres.Length; m++)//chercher a s'arreter des qu'on trouve la lettre
                    {
                        for (n = 0; n < caracteres[m].Length; n++)
                        {
                            if (lettre == caracteres[m][n])
                            {

                                code += (m + 1).ToString();// car indice décalé de 1 par rapport à ce qu'on tape au clavier

                            }

                        }
                    }
 
                }

                tabDecodeMot[i, 1] = code;
            }
            return tabDecodeMot;
        }

        public static string traductionT9(string messageEncode,string[,] tabDecodeMot)
        {
            string[] messagesPossibles = new string[10];
            string messageTraduit = "";
            int i = 0;
            string mot = "";
            
            int compteurMessagePossible = 0;

            while (i < tabDecodeMot.Length / 2)
            {
                if (messageEncode == tabDecodeMot[i, 1])
                {
                    messageTraduit = tabDecodeMot[i, 0];
                    messagesPossibles[compteurMessagePossible] = messageTraduit;
                    compteurMessagePossible++;
                }
                i++;
            }

            int nouvelleTaille = compteurMessagePossible;

            if (nouvelleTaille > 1)
            {
                for (int k = 0; k < nouvelleTaille; k++)
                {

                    Console.WriteLine((k + 1) + ": " + messagesPossibles[k]);
                }
                Console.WriteLine("Quel mot voulez vous ? <Taper l'indice à coté du mot souhaité>");
                int indice = int.Parse(Console.ReadLine());
                mot = messagesPossibles[indice - 1];
            }          
                
            else
            {
                mot = messageTraduit;
            }

            return mot;    

        }

        public static string executionMultitap(char[][] tab)
        {
            string message = "";
            Console.WriteLine("Tapez votre message encodé <Pour faire un espace taper une fois 1>");
            message = Console.ReadLine();
            return (traductionMultimap(message, tab));
        }

        public static string executionT9(char[][]tab)
        {
            string message = "";
            string[,] dicoPlusCode = decodeMotDico(tab);//on charge une fois pour toute le dico puis on l'utilise en paramètre pour traductionT9
            string nouveauMot = "";
            int autreMot = 1;
            string phrase = "";
            do
            {
                try
                {
                    Console.WriteLine("Tapez votre message encodé ");
                    message = Console.ReadLine();
                    int i = int.Parse(message);
                    nouveauMot = traductionT9(message, dicoPlusCode);
                    phrase += " " + nouveauMot;
                    Console.WriteLine(phrase);
                    Console.WriteLine("Pour ajouter un autre mot à votre phrase, taper 1");
                    autreMot = int.Parse(Console.ReadLine());

                }
                
                catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
            while (autreMot == 1);
            
            return phrase;

        }

        static void Main(string[] args)
        {
            // int lgint=log10(entiervoulu)+1; permet de calculer la longueur d'un entier
            //(int)char.GetNumericValue(char voulu) permet d'avoir la valeur de l'entier correspondant au caractère


            Console.WriteLine(" ___________________________________");
            Console.WriteLine("|     1     |     2     |     3     |");
            Console.WriteLine("| _ . , ! ? | a   b   c | d   e   f |");
            Console.WriteLine("|___________|___________|___________|");
            Console.WriteLine("|     4     |     5     |     6     |");
            Console.WriteLine("| g   h   i | j   k   l | m   n   o |");
            Console.WriteLine("|___________|___________|___________|");
            Console.WriteLine("|     7     |     8     |     9     |");
            Console.WriteLine("|  p q r s  | t   u   v |  w x y z  |");
            Console.WriteLine("|___________|___________|___________|");
            Console.WriteLine("|           |     0     |           |");
            Console.WriteLine("|           |chgt lettre|           |");
            Console.WriteLine("|___________|___________|___________|");


            char[][] clavier = new char[9][];
            clavier[0] = new char[] { ' ', '.', ',', '!', '?' };
            clavier[1] = new char[] { 'a', 'b', 'c' };
            clavier[2] = new char[] { 'd', 'e', 'f' };
            clavier[3] = new char[] { 'g', 'h', 'i' };
            clavier[4] = new char[] { 'j', 'k', 'l' };
            clavier[5] = new char[] { 'm', 'n', 'o' };
            clavier[6] = new char[] { 'p', 'q', 'r', 's' };
            clavier[7] = new char[] { 't', 'u', 'v' };
            clavier[8] = new char[] { 'w', 'x', 'y', 'z' };


            string message = "";
           

            //mettre tout ca dans une fonction(conseil de Clermont)
            do// mettre ton truc de try pour le choix du clavier et traduction des messagesEncode
            {
                Console.WriteLine("Avec quel type de clavier voulez vous écrire ?<Taper 1 pour le multitap; 2 pour le T9>");
                int choixClavier = int.Parse(Console.ReadLine());

                if (choixClavier == 1 | choixClavier == 2)
                {
                   
                    if (choixClavier == 1)
                    {
                        message=executionMultitap(clavier);
                        
                    }
                    else if (choixClavier == 2)
                    {
                        message = executionT9(clavier) + ".";//Proposer choix ponctuation de fin de phrase serait cool pour T9
                    }
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine("Il y a un problème dans votre choix de clavier, recommencer");
                }
            }
            while (message != "");
            Console.ReadLine();

        }
    }
}
