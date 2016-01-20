using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_info_v8
{
    class Program
    {

        public static void afficheClavier()
    {
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
    }


        public static string traductionMultimap( string messageEncode, char[][] caracteres) 
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

        public static string[,] decodeMotDico(char[][] caracteres)
        {

            string[] motDico = System.IO.File.ReadAllLines("dicoFR.txt");
            int tailleDico = motDico.Length;

            string[,] tabDecodeMot = new string[tailleDico, 2];
            string code = "";
            char lettre = ' ';
            int m = 0;
            int n = 0;


            for (int i = 0; i < tailleDico; i++)
            {
                code = "";
                tabDecodeMot[i, 0] = motDico[i].ToLower();   //ecrit en minuscule les mots
                int tailleMot = tabDecodeMot[i, 0].Length;

                for (int k = 0; k < tailleMot; k++)
                {

                    lettre = tabDecodeMot[i, 0][k];

                    for (m = 0; m < caracteres.Length; m++)  //chercher a s'arreter dès qu'on trouve la lettre
                    {
                        for (n = 0; n < caracteres[m].Length; n++)
                        {
                            if (lettre == caracteres[m][n])
                            {

                                code += (m + 1).ToString();   // car indice décalé de 1 par rapport à ce qu'on tape au clavier

                            }

                        }
                    }
 
                }

                tabDecodeMot[i, 1] = code;
            }
            return tabDecodeMot;
        }

        public static void ecrireDansDico(string mot_a_rajouter)//fonction ecrivant dans dico, mot réutilisable qu'une fois le shell fermé suite à sa création car on ne recharge pas le dico au cours de l'éxécution (cf Internet)
        {
            using (System.IO.StreamWriter nouveauDico =
           new System.IO.StreamWriter("dicoFR.txt", true)) 
            {
                nouveauDico.WriteLine(mot_a_rajouter.ToUpper());
            }
                      
        }

        public static void enregistrerPhrase(string phrase)
        {
            using (System.IO.StreamWriter nouveauHistorique =
           new System.IO.StreamWriter("HistoriquePhrase.txt", true)) 
            {
                nouveauHistorique.WriteLine(phrase.ToLower());
            }
        }
           

        public static string traductionT9(string messageEncode,string[,] tabDecodeMot,char [][]tab)
        {
            string[] messagesPossibles = new string[10];
            string messageTraduit = "";
            int i = 0;
            string mot = "";
            string a = "";
            
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

            if (nouvelleTaille > 1)//on entre la dedans si on a plusieurs mots possible pour une même combinaison
            {
               
                bool erreurIndice = false;
                do
                {
                    erreurIndice = false;
                    try
                    {
                        for (int k = 0; k < nouvelleTaille; k++)
                        {

                            Console.WriteLine((k + 1) + ": " + messagesPossibles[k]);
                        }

                        Console.WriteLine("Quel mot voulez vous ? <Tapez l'indice à coté du mot souhaité; Tapez 0 si le mot voulu n'est pas présent>");//try catch encore à faire pour chaque ReadLine
                        int indice = int.Parse(Console.ReadLine());
                        Console.Clear();
                        
                        while (indice > compteurMessagePossible)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Aucun mot ne correspond à l'indice entré");
                            Console.ForegroundColor = ConsoleColor.White;
                            for (int k = 0; k < nouvelleTaille; k++)
                            {

                                Console.WriteLine((k + 1) + ": " + messagesPossibles[k]);
                            }


                            Console.WriteLine("Quel mot voulez vous ? <Tapez l'indice à coté du mot souhaité; Tapez 0 si le mot voulu n'est pas présent>");
                            indice = int.Parse(Console.ReadLine());
                            Console.Clear();

                        }
                        if (indice == 0)//on entre la dedans si le mot n'est pas présent
                        {
                            bool retourChoixIndice = false;//on repart d'ici si erreur d'indice
                            do
                            {
                                retourChoixIndice = false;
                                try
                                {

                                    Console.WriteLine("Tapez 1 si vous pensez vous être trompé dans le code; Tapez 2 si vous voulez ajouter votre mot dans le dictionnaire");
                                    int indiceBis = int.Parse(Console.ReadLine());
                                    Console.Clear();

                                    if (indiceBis == 1 | indiceBis == 2)
                                    {
                                        if (indiceBis == 1)
                                        {

                                            mot = executionT9(tab);//on repart au début pour retaper le code et on le stocke dans mot pour pas perdre la phrase déjà commencée(si commencée)
                                        }
                                        else
                                        {

                                            Console.WriteLine("Entrez le mot que vous souhaitez ajouter en multitap");
                                            mot = Console.ReadLine();
                                            a = traductionMultimap(mot, tab);
                                            Console.Clear();
                                            Console.WriteLine(a);
                                            Console.WriteLine("Mot sauvegardé dans le dictionnaire, vous ne pourrez le retrouver en tapant son code qu'une fois cette éxécution terminée");

                                            ecrireDansDico(a);
                                            mot = a;
                                            

                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Erreur dans l'indice");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        retourChoixIndice = true;
                                    }


                                }
                                catch (Exception)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Erreur dans l'indice");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    retourChoixIndice = true;

                                }
                            }
                            while (retourChoixIndice == true);
                        }

                        else
                        {
                            mot = messagesPossibles[indice - 1];
                        }
                    }
                    catch (Exception ind)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ind.Message);
                        Console.ForegroundColor = ConsoleColor.White;

                        erreurIndice = true;
                    }
                }
                while (erreurIndice == true);

            }

            else//Une seule possibilité de mot proposé, ou aucun mot proposé
            {
                mot = messageTraduit;
                if (mot == "")
                {
                    bool erreurIndiceAjoutMot = false;
                    do
                    {
                        erreurIndiceAjoutMot = false;
                        try
                        {

                            Console.WriteLine("Aucun mot trouvé, taper 0 pour rajouter le mot voulu, sinon taper tout autre chiffre");
                            int indiceBisBis = int.Parse(Console.ReadLine());
                            Console.Clear();

                            if (indiceBisBis == 0)
                            {
                                afficheClavier();
                                Console.WriteLine("Entrer le mot que vous souhaitez ajouter en multitap");
                                mot = Console.ReadLine();
                                a = traductionMultimap(mot, tab);
                                Console.Clear();
                                Console.WriteLine(a);

                                ecrireDansDico(a);
                                mot = a;

                                Console.WriteLine("Mot sauvegardé dans le dictionnaire,vous pourrez le retrouver en tapant son code qu'une fois cette éxécution terminée");                                

                            }
                        }
                        catch (Exception indB)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(indB.Message);
                            Console.ForegroundColor = ConsoleColor.White;

                            erreurIndiceAjoutMot = true;
                        }
                    }
                    while (erreurIndiceAjoutMot == true);
                }
                else
                {
                    bool erreurAutInd = false;
                    do
                    {
                        erreurAutInd = false;

                        try
                        {
                            Console.WriteLine(mot);
                            Console.WriteLine("Si ce n'est pas le mot que vous souhaitez, taper 0 pour rajouter le mot voulu; Taper tout autre chiffre sinon");
                            int autreIndice = int.Parse(Console.ReadLine());
                            Console.Clear();
                            if (autreIndice == 0)
                            {
                                Console.WriteLine("Entrer le mot que vous souhaitez ajouter en multitap");
                                mot = Console.ReadLine();
                                a = traductionMultimap(mot, tab);
                                Console.Clear();
                                Console.WriteLine(a);

                                ecrireDansDico(a);
                                mot = a;

                                Console.WriteLine("Mot sauvegardé dans le dictionnaire,vous pourrez le retrouver en tapant son code qu'une fois cette éxécution terminée");
                                
                            }

                        }
                        catch (Exception autInd)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(autInd.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                            erreurAutInd = true;
                        }
                    }
                    while (erreurAutInd == true);
                }
            }
            return mot;    

        }

        public static string executionMultitap(char[][] tab)//pbm si erreur une fois
        {
            afficheClavier();
            string message = "";
            string a = "";
            try
            {
                
                Console.WriteLine("Tapez votre message encodé <Pour faire un espace taper une fois 1>");
                message = Console.ReadLine();
                
                int i = int.Parse(message);
            }

            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                
                
                executionMultitap(tab);
            }
            a=traductionMultimap(message, tab);
            return a;
        }

        public static string executionT9(char[][]tab)
        {
            afficheClavier();
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
                    nouveauMot = traductionT9(message, dicoPlusCode,tab);
                    phrase += " " + nouveauMot;
                    Console.WriteLine(phrase);

                    bool errAutMot = false;
                    do
                    {
                        errAutMot = false;
                        try
                        {
                            Console.WriteLine("Pour ajouter un autre mot à votre phrase en T9 tapez 1; de la ponctuation taper 2; tout autre chiffre pour arrêter la saisie en T9");
                            autreMot = int.Parse(Console.ReadLine());
                            Console.Clear();
                            afficheClavier();
                        }
                        catch (Exception autMot)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(autMot.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                            errAutMot = true;
                        }
                    }
                    while (errAutMot == true);

                    if (autreMot == 2)//ajout ponctuation 
                    {
                        bool erreurPonct = false;
                        do
                        {
                            erreurPonct = false;
                            char[] ponctuation = new char[tab[0].Length];
                            
                            do
                            {
                                
                                for (int z = 0; z < tab[0].Length; z++)
                                {
                                    ponctuation[z] = tab[0][z];
                                    Console.WriteLine("{0} : {1}", z + 1, ponctuation[z]);
                                }

                                try
                                {
                                    
                                    Console.WriteLine("Tapez l'indice à coté de la ponctuation voulue");
                                    int indPonctuation = int.Parse(Console.ReadLine());
                                    Console.Clear();
                                    
                                    if ((indPonctuation - 1) > ponctuation.Length)
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Erreur d'indice");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        
                                        erreurPonct = true;
                                    }
                                    else
                                    {
                                        phrase += (tab[0][indPonctuation - 1]);
                                        Console.WriteLine(phrase);
                                        
                                        bool erreurPonctBis=false;
                                        do
                                        {
                                            erreurPonctBis = false;
                                            try
                                            {
                                                Console.WriteLine("Pour ajouter un autre mot à votre phrase en T9 tapez 1; de la ponctuation tapez 2; tout autre chiffre pour arrêter la saisie en T9");
                                                autreMot = int.Parse(Console.ReadLine());
                                                Console.Clear();
                                            }
                                            catch (Exception indPonct)
                                            {
                                                Console.Clear();
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.WriteLine(indPonct.Message);
                                                Console.ForegroundColor = ConsoleColor.White;
                                                erreurPonctBis = true;
                                            }
                                        }
                                        while (erreurPonctBis == true);

                                        
                                    }
                                }


                                catch (Exception ponc)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ponc.Message);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    
                                    erreurPonct = true;
                                }
                            }
                            while (erreurPonct == true);
                        }
                        while (autreMot == 2);
                    }
                        
                }
                
                catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                
            }

        }
            while (autreMot == 1);
            
            return phrase;

        }

        public static void LancementProgramme(char[][] tab)
        { 
            string message = "";
            int choixClavier = 0;
            do
            {
                try
                {
                    
                    Console.WriteLine("Avec quel type de clavier voulez-vous écrire ?<Tapez 0 pour arrêter; 1 pour le multitap; 2 pour le T9; 3 pour consulter l'historique>");
                    choixClavier = int.Parse(Console.ReadLine());
                    Console.Clear();
                    

                    if (choixClavier == 1 | choixClavier == 2 |choixClavier==0 |choixClavier==3)//On entre dedans si on tape indice correspondant à un clavier
                    {

                        if (choixClavier == 1)//Multitap
                        {
                            message = executionMultitap(tab);

                        }
                        else if (choixClavier == 2)//T9
                        {
                            message = executionT9(tab);
                        }

                        else if(choixClavier==3)//Consultation Historique
                        {
                            Console.Clear();
                            string[] historique = System.IO.File.ReadAllLines("HistoriquePhrase.txt");
                            int tailleHistorique = historique.Length;

                           
                            bool erreurIndPh=false;
                            do
                            {
                                erreurIndPh = false;
                                for (int i = 0; i < tailleHistorique; i++)
                                {
                                    Console.WriteLine("{0} :{1}", (i + 1), historique[i]);
                                }
                                try
                                {
                                    Console.WriteLine("Voulez-vous selectionnner une phrase parmi celles proposées ? Si oui tapez l'indice à côté de cette phrase ; sinon tapez 0");
                                    int indicePhrase = int.Parse(Console.ReadLine());
                                    Console.Clear();
                                    if (indicePhrase == 0)
                                    {
                                        LancementProgramme(tab);
                                    }
                                    else if ((indicePhrase - 1) > tailleHistorique)
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Erreur d'indice");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        erreurIndPh = true;
                                    }
                                    else
                                    {
                                        message = historique[indicePhrase - 1];
                                    }
                                }
                                catch (Exception indPh)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(indPh.Message);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    erreurIndPh = true;
                                }
                            }
                            while (erreurIndPh == true);
                                       
                        }
                        
                        else//Fin du message 
                        {
                            Console.Clear();
                            Console.WriteLine("Message terminé :");
                            
                        }
                        Console.WriteLine(message);
                        
                    }
                    else// erreur d'indice de clavier
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Il y a un problème dans votre choix de clavier, recommencez");
                        Console.ForegroundColor = ConsoleColor.White;
                        
                        LancementProgramme(tab);
                    }
                }
                catch (Exception errClavier)// erreur d'indice de clavier
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Il y a un problème dans votre choix de clavier, recommencez");
                    Console.ForegroundColor = ConsoleColor.White;
                    
                    LancementProgramme(tab);

                }

            }
            while (message != "" && choixClavier!=0);
            enregistrerPhrase(message);//stockage du message dans l'historique
            Console.ReadLine();
        }

        static void Main(string[] args)
        
        {
          
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


            LancementProgramme(clavier);
        
       }
   }
}

