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
        Console.WriteLine(" ___________________________________");  // affiche un clavier pour aider à la saisie
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
            int lgMessage = messageEncode.Length;  // récupère la longueur du message tapé
            int compteur = 0;

            for (int i = 0; i < lgMessage; i++)  // on boucle sur chaque chiffre du message encodé
            {
                compteur = 0;

                int indice = (int)char.GetNumericValue(messageEncode[i]);
                while (i < lgMessage - 1 && messageEncode[i] == messageEncode[i + 1])
                {

                    compteur++;
                    i++;
                }
                
                    if (indice != 0) // le zéro permet de séparer lorsque l'on doit écrire plusieurs fois le même chiffre pour des lettres différentes.
                    {
                        lettre = caracteres[indice - 1][compteur];
                        messageTraduit = messageTraduit + lettre;
                    }

            }

            return messageTraduit;

        }

        public static string[,] decodeMotDico(char[][] caracteres)  // associe à chaque mot du dico son code en t9
        {

            string[] motDico = System.IO.File.ReadAllLines("dicoFR.txt"); // on écrit les mots du dico dans un tableau
            int tailleDico = motDico.Length;

            string[,] tabDecodeMot = new string[tailleDico, 2]; 
            string code = "";
            char lettre = ' ';
            int m = 0;
            int n = 0;

            for (int i = 0; i < tailleDico; i++) // boucle sur tous les mots du dico
            {
                code = "";
                tabDecodeMot[i, 0] = motDico[i].ToLower(); // écris en minuscule les mots
                int tailleMot = tabDecodeMot[i, 0].Length;

                for (int k = 0; k < tailleMot; k++) // boucle sur chaque lettre du mot
                {

                    lettre = tabDecodeMot[i, 0][k];

                    for (m = 0; m < caracteres.Length; m++)  
                    {
                        for (n = 0; n < caracteres[m].Length; n++)
                        {
                            if (lettre == caracteres[m][n])
                            {

                                code += (m + 1).ToString();  // car indice décalé de 1 par rapport à ce qu'on tape au clavier

                            }

                        }
                    }
 
                }

                tabDecodeMot[i, 1] = code; 
            }
            return tabDecodeMot;
        }

        public static void ecrireDansDico(string mot_a_rajouter) //fonction écrivant dans le dico, mot réutilisable qu'une fois le shell fermé suite à sa création car on ne recharge pas le dico au cours de l'éxécution 
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
                nouveauHistorique.WriteLine(phrase.ToUpper());
            }
        }
           
        public static string traductionT9(string messageEncode,string[,] tabDecodeMot,char [][]tab) 
        {
            string[] messagesPossibles = new string[10];
            string messageTraduit = "";
            int i = 0;
            string mot = "";
            string a = "";
            
            int compteurMessagePossible = 0; // à un code correspond 1 ou plusieurs messages

            while (i < tabDecodeMot.Length / 2) // on cherche le code correspondant dans le tableau
            {
                if (messageEncode == tabDecodeMot[i, 1])
                {
                    messageTraduit = tabDecodeMot[i, 0];
                    messagesPossibles[compteurMessagePossible] = messageTraduit; // on écrit les différents messages possibles dans un tableau
                    compteurMessagePossible++; 
                }
                i++;
            }

            int nouvelleTaille = compteurMessagePossible;

            if (nouvelleTaille > 1)   //on entre la dedans si on a plusieurs mots possible pour une même combinaison
            {
               
                bool erreurIndice = false;
                do
                {
                    erreurIndice = false;
                    try
                    {
                        for (int k = 0; k < nouvelleTaille; k++) // on affiche les différents messages possibles
                        {

                            Console.WriteLine((k + 1) + ": " + messagesPossibles[k]);
                        }

                        Console.WriteLine("Quel mot voulez-vous ? <Tapez l'indice à coté du mot souhaité ; Tapez 0 si le mot voulu n'est pas présent>");//try catch encore à faire pour chaque ReadLine
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


                            Console.WriteLine("Quel mot voulez-vous ? <Tapez l'indice à coté du mot souhaité ; Tapez 0 si le mot voulu n'est pas présent>");
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

                                    Console.WriteLine("Tapez 0 si vous voulez ajouter votre mot dans le dictionnaire ; tout autre chiffre si vous pensez vous être trompé dans le code");
                                    int indiceBis = int.Parse(Console.ReadLine());
                                    Console.Clear();

                                    if (indiceBis == 0)
                                    {


                                        Console.WriteLine("Entrez le mot que vous souhaitez ajouter en multitap");
                                        mot = Console.ReadLine();
                                        a = traductionMultimap(mot, tab);
                                        Console.Clear();
                                        Console.WriteLine(a);
                                        Console.WriteLine("Mot sauvegardé dans le dictionnaire, vous ne pourrez le retrouver en tapant son code qu'une fois cetteeéxécution terminée");

                                        ecrireDansDico(a);
                                        mot = a;

                                    }
                                }
                                              
                                catch (Exception) // permet de gérer les exeptions
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

            else    //Une seule possibilité de mot proposée, ou aucun mot proposé
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

                            Console.WriteLine("Aucun mot trouvé, tapez 0 pour rajouter le mot voulu, sinon tapez tout autre chiffre pour reommencer");
                            int indiceBisBis = int.Parse(Console.ReadLine());
                            Console.Clear();

                            if (indiceBisBis == 0)
                            {
                                afficheClavier();
                                Console.WriteLine("Entrez le mot que vous souhaitez ajouter en multitap");
                                mot = Console.ReadLine();
                                a = traductionMultimap(mot, tab);
                                Console.Clear();
                                Console.WriteLine(a);

                                ecrireDansDico(a);
                                mot = a;
                                Console.WriteLine("Mot sauvegardé dans le dictionnaire, vous ne pourrez le retrouver en tapant son code qu'une fois cette éxécution terminée");
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
                            Console.WriteLine("Si ce n'est pas le mot que vous souhaitez, tapez 0 pour rajouter le mot voulu; Tapez tout autre chiffre sinon");
                            int autreIndice = int.Parse(Console.ReadLine());
                            Console.Clear();
                            if (autreIndice == 0)
                            {
                                Console.WriteLine("Entrez le mot que vous souhaitez ajouter en multitap");
                                mot = Console.ReadLine();
                                a = traductionMultimap(mot, tab);
                                Console.Clear();
                                Console.WriteLine(a);
                                ecrireDansDico(a);
                                mot = a;
                                Console.WriteLine("Mot sauvegardé dans le dictionnaire, vous ne pourrez le retrouver en tapant son code qu'une fois cette exécution terminée");
                                
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

        public static string executionMultitap(char[][] tab)
        {
            string message = "";
            string a = "";
            bool errMttap = false;
            do
            {
                afficheClavier();
                
                errMttap = false;
                try
                {

                    Console.WriteLine("Tapez votre message encodé <Pour faire un espace taper une fois 1>");
                    message = Console.ReadLine();

                    long i = long.Parse(message);
                }

                catch (Exception ex) // gère les erreurs de saisie
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    errMttap = true;

                }
            }
            while (errMttap == true);
            a=traductionMultimap(message, tab); // le message est traduit grâce à la fonction traductionMultitap
            Console.Clear();
            return a; // renvoie le message décodé
            
        }

        public static string executionT9(char[][]tab)
        {
            afficheClavier();
            string message = "";
            string[,] dicoPlusCode = decodeMotDico(tab); //on charge une fois pour toute le dico puis on l'utilise en paramètre pour traductionT9
            string nouveauMot = "";
            int autreMot = 1;
            string phrase = "";
            do
            {
                try
                {
                    
                    Console.WriteLine("Tapez votre message encodé ");
                    message = Console.ReadLine();
                    
                    long i = long.Parse(message);
                    nouveauMot = traductionT9(message, dicoPlusCode,tab); // message traduit grâce à la fonction traductionT9
                    phrase += " " + nouveauMot;
                    Console.WriteLine(phrase);

                    bool errAutMot = false;
                    do
                    {
                        errAutMot = false;
                        try
                        {
                            Console.WriteLine("Pour ajouter un autre mot à votre phrase en T9 tapez 1; de la ponctuation tapez 2; tout autre chiffre pour arrêter la saisie en T9");
                            autreMot = int.Parse(Console.ReadLine());
                            Console.Clear();
                            afficheClavier();
                        }
                        catch (Exception autMot)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(autMot.Message);  // renvoie le message d'erreur correspondant
                            Console.ForegroundColor = ConsoleColor.White;
                            errAutMot = true;
                        }
                    }
                    while (errAutMot == true);

                    if (autreMot == 2)  //ajout ponctuation 
                    {
                        bool erreurPonct = false;
                        do
                        {
                            erreurPonct = false;
                            char[] ponctuation = new char[tab[0].Length]; // crée un tableau de la taille du nombre de caractères de ponctuation présent dans le tableau clavier
                            
                            do
                            {
                                
                                for (int z = 0; z < tab[0].Length; z++) // affiche toute la ponctuation
                                {
                                    ponctuation[z] = tab[0][z];
                                    Console.WriteLine("{0} : {1}", z + 1, ponctuation[z]);
                                }

                                try
                                {
                                    
                                    Console.WriteLine("Tapez l'indice à côté de la ponctuation voulue");
                                    int indPonctuation = int.Parse(Console.ReadLine());
                                    Console.Clear();
                                    
                                    if ((indPonctuation - 1) > ponctuation.Length) // cas où l'indice est trop grand 
                                    {
                                        Console.Clear();
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Erreur d'indice");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        
                                        erreurPonct = true;
                                    }
                                    else
                                    {
                                        phrase += (tab[0][indPonctuation - 1]); // rajoute la ponctuation à la phrase
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

        public static void LancementProgramme(char[][] tab) // gère les affichages permettant de choisir son mode de saisie et les erreurs de saisie
        { 
            string message = "";
            int choixClavier = 0;
            do
            {
                try
                {                    
                    Console.WriteLine("Avec quel type de clavier voulez vous écrire ?\n<Taper 0 pour arrêter; 1 pour le multitap; 2 pour le T9; 3 pour consulter l'historique>");
                    choixClavier = int.Parse(Console.ReadLine());
                    Console.Clear();
                    

                    if (choixClavier == 1 | choixClavier == 2 |choixClavier==0 |choixClavier==3)   //On entre dedans si on tape indice correspondant à un clavier
                    {

                        if (choixClavier == 1)   //Multitap
                        {
                            message = executionMultitap(tab);
                            
                        }
                        else if (choixClavier == 2)   //T9
                        {
                            message = executionT9(tab);
                            
                        }

                        else if(choixClavier==3)   //Consultation Historique
                        {
                            Console.Clear();
                            string[] historique = System.IO.File.ReadAllLines("HistoriquePhrase.txt"); // écrit les mots du fichier HistoriquePhrase dans un tableau
                            int tailleHistorique = historique.Length;

                           
                            bool erreurIndPh=false;
                            do
                            {
                                erreurIndPh = false;
                                for (int i = 0; i < tailleHistorique; i++)
                                {
                                    Console.WriteLine("{0} :{1}", (i + 1), historique[i].ToLower());
                                }
                                try
                                {
                                    Console.WriteLine("Voulez-vous selectionnner une phrase parmi celles proposées ? Si oui tapez l'indice à côté de cette phrase; sinon tapez 0");
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
                                        message = historique[indicePhrase - 1].ToLower();
                                        
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
                catch 
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Il y a un problème dans votre choix de clavier, recommencez");
                    Console.ForegroundColor = ConsoleColor.White;
                    
                    LancementProgramme(tab);

                }
                
            }
            
            while (message != "" && choixClavier!=0);
            
            enregistrerPhrase(message);   //stockage du message dans l'historique
            Console.ReadLine();
        }

        static void Main(string[] args)
        
        {
            
            char[][] clavier = new char[9][];  // initialisation du tableau de tableaux contenant le "clavier"
            clavier[0] = new char[] { ' ', '.', ',', '!', '?' };
            clavier[1] = new char[] { 'a', 'b', 'c' };
            clavier[2] = new char[] { 'd', 'e', 'f' };
            clavier[3] = new char[] { 'g', 'h', 'i' };
            clavier[4] = new char[] { 'j', 'k', 'l' };
            clavier[5] = new char[] { 'm', 'n', 'o' };
            clavier[6] = new char[] { 'p', 'q', 'r', 's' };
            clavier[7] = new char[] { 't', 'u', 'v' };
            clavier[8] = new char[] { 'w', 'x', 'y', 'z' };

            LancementProgramme(clavier); // clavier est passé en paramètre il servira pour les autres fonctions
       }
   }
}

