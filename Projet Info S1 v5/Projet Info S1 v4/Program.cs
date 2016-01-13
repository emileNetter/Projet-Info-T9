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

                if (indice != 0)
                {
                    lettre = caracteres[indice - 1][compteur];
                    messageTraduit = messageTraduit + lettre;
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
                tabDecodeMot[i, 0] = motDico[i].ToLower();
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


<<<<<<< HEAD
                                   code += (m + 1).ToString();// car indice décalé de 1 par rapport à ce qu'on tape au clavier
                                   
                               }
                               
                           }
                       }
                    
=======

>>>>>>> origin/master
                }

                tabDecodeMot[i, 1] = code;
            }
            return tabDecodeMot;
        }

        public static void traductionT9(string messageEncode, char[][] tab)
        {
            string[] messagesPossibles = new string[10];
            string messageTraduit = "";
            int i = 0;
            string mot = "";
            string phrase = "";
            int autreMot = 0;


            string[,] tabDecodeMotBis = decodeMotDico(tab);
            int compteurMessagePossible = 0;

            while (i < tabDecodeMotBis.Length / 2)
            {
                if (messageEncode == tabDecodeMotBis[i, 1])
                {
                    messageTraduit = tabDecodeMotBis[i, 0];
                    messagesPossibles[compteurMessagePossible] = messageTraduit;
                    compteurMessagePossible++;

                }
                i++;
            }

            int nouvelleTaille = compteurMessagePossible;
            for (int k = 0; k < nouvelleTaille; k++)
            {

                Console.WriteLine((k + 1) + ": " + messagesPossibles[k]);




            }
            if (nouvelleTaille > 1)
            {
                Console.WriteLine("Quel mot voulez vous ? <Taper l'indice à coté du mot souhaité>");
                int indice = int.Parse(Console.ReadLine());
                Console.WriteLine("\n" + messagesPossibles[indice - 1]);
                mot = messagesPossibles[indice - 1];
            }
            else
            {
                mot = messageTraduit;
            }


            do// fait pas attention a ca pour le moment, je reflechis
            {
                Console.WriteLine("Pour ajouter un autre mot à votre phrase, taper 1");
                autreMot = int.Parse(Console.ReadLine());

                phrase += " " + mot;
            }
            while (autreMot == 1);


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




            char[][] tab = new char[9][];
            tab[0] = new char[] { ' ', '.', ',', '!', '?' };
            tab[1] = new char[] { 'a', 'b', 'c' };
            tab[2] = new char[] { 'd', 'e', 'f' };
            tab[3] = new char[] { 'g', 'h', 'i' };
            tab[4] = new char[] { 'j', 'k', 'l' };
            tab[5] = new char[] { 'm', 'n', 'o' };
            tab[6] = new char[] { 'p', 'q', 'r', 's' };
            tab[7] = new char[] { 't', 'u', 'v' };
            tab[8] = new char[] { 'w', 'x', 'y', 'z' };


            string message = "";
            string[,] dicoPlusCode = decodeMotDico(tab);




            do// mettre ton truc de try pour le choix du clavier et traduction des messagesEncode
            {
                Console.WriteLine("Avec quel type de clavier voulez vous écrire ?<Taper 1 pour le multitap; 2 pour le T9>");
                int choixClavier = int.Parse(Console.ReadLine());

                if (choixClavier == 1 | choixClavier == 2)
                {

                    if (choixClavier == 1)
                    {
                        Console.WriteLine("Tapez votre message encodé <Pour faire un espace taper une fois 1>");
                        message = Console.ReadLine();
                        Console.WriteLine(traductionMultimap(message, tab));
                    }
                    else if (choixClavier == 2)
                    {

                        traductionT9(message, tab);
                    }
                }
                else
                {
                    Console.WriteLine("Il y a un problème dans votre choix de clavier, recommencer");
                }
            }
            while (message != "");
            Console.ReadLine();
<<<<<<< HEAD
            
=======



>>>>>>> origin/master
        }
    }
}
