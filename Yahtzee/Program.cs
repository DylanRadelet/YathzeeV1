﻿using System;
using System.Collections.Generic;

class Program
{
    // test GitHub
    public List<string> pseudos = new List<string>();
    private int joueurActuel = 0;
    private int[,] scores;
    private string[] categories = { "Total de 1", "Total de 2", "Total de 3", "Total de 4", "Total de 5", "Total de 6", "Brelan", "Carré", "Full", "Grande Suite", "Petite Suite", "Yatzee", "Chance" };
    private int[] valeursDes = new int[5];
    private bool[,] categoriesUtilisees;

    // private : Accesible uniquement à l'intérieur de la classe ou elle est définie.

    static void Main(string[] args)
    {
        Console.WindowWidth = 160;
        Console.WindowHeight = 30;

        bool AfficherDesAvecScore = true;

        Console.Clear();

        Program p = new Program();
        Console.Write("A combien de joueur voulez-vous jouer ? (De 1 à 4 joueurs) ");
        int Nb_Joueur = p.EnregisterJoueur();
        p.scores = new int[p.pseudos.Count, p.categories.Length];
        for (int i = 0; i < p.scores.GetLength(0); i++)
        {
            for (int j = 0; j < p.scores.GetLength(1); j++)
            {
                p.scores[i, j] = -1;
            }
        }
        p.MelangerJoueurs();
        p.AfficherJoueurs();

        int manchesJouees = 0;
        const int manchesTotal = 1;

        while (manchesJouees < manchesTotal)
        {
            for (int i = 0; i < p.pseudos.Count; i++)
            {
                Console.Clear();
                p.LancerDes();
                AfficherDesAvecScore = true;
                p.AfficherTableauScores(AfficherDesAvecScore);
                p.InscrireScore();
                Console.Clear();
                AfficherDesAvecScore = false;
                p.AfficherTableauScores(AfficherDesAvecScore);
                Console.WriteLine($"Appuyez sur ENTER pour continuer.");
                Console.ReadLine();
                p.joueurActuel = (p.joueurActuel + 1) % p.pseudos.Count;
            }
            manchesJouees++;
        }

        Console.WriteLine("Fin du jeu. Voici les scores finaux :");
        p.AfficherTableauScores(AfficherDesAvecScore);
        p.AnnoncerGagnant();
        Console.ReadLine();
    }

    public int EnregisterJoueur()
    {
        int Nb_Joueur = 0;
        while (Nb_Joueur < 1 || Nb_Joueur > 4)
        {
            try // J'essaie
            {
                Nb_Joueur = int.Parse(Console.ReadLine());
                if (Nb_Joueur < 1 || Nb_Joueur > 4)
                {
                    Console.WriteLine($"ERREUR : Entrez un nombre valide entre 1 et 4.");
                }
            }
            catch // Spécifie mon action si une erreur se produit
            {
                Console.WriteLine($"ERREUR : Entrez un nombre valide entre 1 et 4.");
            }
        }
        int nombreTotalJoueurs = Nb_Joueur;
        int JoueurNum = 0;
        while (Nb_Joueur > 0)
        {
            JoueurNum += 1;
            string pseudo;
            do
            {
                Console.Write($"Pseudo joueur n°{JoueurNum} (2-15 caractères) : ");
                pseudo = Console.ReadLine();
                if (pseudo.Length < 2 || pseudo.Length > 15)
                {
                    Console.WriteLine($"ERREUR : \"{pseudo}\" n'est pas valide. ");
                }
            }
            while (pseudo.Length < 2 || pseudo.Length > 15);
            pseudos.Add(pseudo);
            Nb_Joueur -= 1;
        }
        categoriesUtilisees = new bool[pseudos.Count, categories.Length]; // Initialise 'false' à la création de cette matrice bool
        return nombreTotalJoueurs;
    }

    public void AfficherJoueurs()
    {
        Console.Clear();
        Console.WriteLine("Ordre de départ : ");
        int JoueurNum = 0;
        foreach (var pseudo in pseudos)
        {
            JoueurNum += 1;
            Console.WriteLine($"n°{JoueurNum} {pseudo}");
        }
        Console.ReadLine();
    }

    public void MelangerJoueurs()
    {
        Random random = new Random();
        int n = pseudos.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (pseudos[i], pseudos[j]) = (pseudos[j], pseudos[i]); // Mélange aléatoirement les joueurs
        }
    }

    public void LancerDes()
    {
        Random random = new Random();
        bool[] garder = new bool[5];
        bool premierLancer = true;
        Console.WriteLine($"Tour de {pseudos[joueurActuel]}:");
        int nombreDeLancers = 0;

        while (nombreDeLancers < 2)
        {
            if (premierLancer)
            {
                for (int i = 0; i < 5; i++)
                {
                    valeursDes[i] = random.Next(1, 7);
                }
                premierLancer = false;
            }

            AfficherDes(valeursDes);
            Console.WriteLine("| 1 - 2 - 3 - 4 - 5 |");
            Console.Write("");
            string input = Console.ReadLine();
            bool erreurDeSaisie = false;

            if (!string.IsNullOrEmpty(input)) //Ni null ni vide
            {
                foreach (var index in input.Split())
                {
                    if (int.TryParse(index.Trim(), out int idx) && idx >= 1 && idx <= 5)
                    {
                        garder[idx - 1] = true;
                    }
                    else
                    {
                        Console.WriteLine($"Attention : '{index}' n'est pas un numéro de dé valide. Réessayez.");
                        erreurDeSaisie = true;
                        break;
                    }
                }
            }

            if (erreurDeSaisie)
            {
                continue;
            }

            
            for (int i = 0; i < 5; i++)
            {
                if (!garder[i])
                {
                    valeursDes[i] = random.Next(1, 7);
                }
            }

            nombreDeLancers++;
            Array.Clear(garder, 0, garder.Length);
        }
    }

    private void AfficherDes(int[] valeursDes)
    {
        string topLine = "╔";
        string middleLine = "║";
        string bottomLine = "╚";

        for (int i = 0; i < valeursDes.Length; i++)
        {
            topLine += "═══╦";
            middleLine += $" {valeursDes[i]} ║";
            bottomLine += "═══╩";
        }

        topLine = topLine.Substring(0, topLine.Length - 1) + "╗";
        middleLine += "";
        bottomLine = bottomLine.Substring(0, bottomLine.Length - 1) + "╝";

        Console.WriteLine(topLine);
        Console.WriteLine(middleLine);
        Console.WriteLine(bottomLine);
    }

    public void AfficherTableauScores(bool AfficherDesAvecScore)
    {
        Console.Clear();
        if (AfficherDesAvecScore == true)
        {
            Console.WriteLine("Vos dés :");
            AfficherDes(valeursDes);
        }

        Console.WriteLine("Tableau de Scores:");
        Console.Write("╔═══════════════");
        foreach (var _ in pseudos)
        {
            Console.Write("╦══════════════");
        }
        Console.WriteLine("╗");

        Console.Write("║               ");
        foreach (var pseudo in pseudos)
        {
            Console.Write($"║ {pseudo,-13}");
        }
        Console.WriteLine("║");

        Console.Write("╠═══════════════");
        foreach (var _ in pseudos)
        {
            Console.Write("╬══════════════");
        }
        Console.WriteLine("╣");

        foreach (string categorie in categories)
        {
            Console.Write($"║ {categorie,-14}");
            for (int j = 0; j < pseudos.Count; j++)
            {
                // (condition) ? si oui : si non [ternaire]
                Console.Write($"║ {(scores[j, Array.IndexOf(categories, categorie)] == -1 ? "X" : scores[j, Array.IndexOf(categories, categorie)]),13}");
            }
            Console.WriteLine("║");
        }

        Console.Write("╚═══════════════");
        foreach (var _ in pseudos)
        {
            Console.Write("╩══════════════");
        }
        Console.WriteLine("╝");
    }

    public void InscrireScore()
    {
        Console.WriteLine("Entrez 'i' pour inscrire votre score dans une catégorie:");

        string input = Console.ReadLine();
        while (input.ToLower() != "i")
        {
            Console.WriteLine("ERREUR : Entrez 'i' pour inscrire votre score dans une catégorie:");
            input = Console.ReadLine();
        }

        Console.WriteLine("Choisissez une catégorie:");
        for (int i = 0; i < categories.Length; i++)
        {
            if (!categoriesUtilisees[joueurActuel, i])
                Console.WriteLine($"{i + 1}. {categories[i]}");
        }

        int choix;
        if (int.TryParse(Console.ReadLine(), out choix) && choix >= 1 && choix <= categories.Length && !categoriesUtilisees[joueurActuel, choix - 1])
        {
            int scorePotentiel = CalculerScore(choix, valeursDes);
            scores[joueurActuel, choix - 1] = scorePotentiel;
            categoriesUtilisees[joueurActuel, choix - 1] = true;
        }
        else
        {
            Console.WriteLine("Choix invalide ou catégorie déjà utilisée.");
        }
    }

    public void AnnoncerGagnant()
    {
        int[] totalScores = new int[pseudos.Count];
        for (int j = 0; j < pseudos.Count; j++)
        {
            for (int i = 0; i < categories.Length; i++)
            {
                totalScores[j] += scores[j, i];
            }
        }

        List<(string Pseudo, int Score)> resultats = new List<(string Pseudo, int Score)>();
        for (int i = 0; i < pseudos.Count; i++)
        {
            resultats.Add((pseudos[i], totalScores[i]));
        }

        resultats.Sort((x, y) => y.Score.CompareTo(x.Score));

        Console.WriteLine("Classement Final :");
        foreach (var resultat in resultats)
        {
            Console.WriteLine($"{resultat.Pseudo} : {resultat.Score}");
        }

        var gagnant = resultats[0];
    }

    private int CalculerScore(int choix, int[] valeursDes)
    {
        int score = 0;
        switch (choix)
        {
            case 1:  // Total de 1
            case 2:  // Total de 2
            case 3:  // Total de 3
            case 4:  // Total de 4
            case 5:  // Total de 5
            case 6:  // Total de 6
                foreach (int val in valeursDes)
                {
                    if (val == choix) score += val;
                }
                break;
            case 7:  // Brelan
                if (valeursDes.GroupBy(v => v).Any(g => g.Count() >= 3))
                    score = valeursDes.Sum();
                break;
            case 8:  // Carré
                if (valeursDes.GroupBy(v => v).Any(g => g.Count() >= 4))
                    score = valeursDes.Sum();
                break;
            case 9:  // Full
                var counts = valeursDes.GroupBy(v => v).Select(g => g.Count()).OrderBy(c => c);
                if (counts.SequenceEqual(new[] { 2, 3 }))
                    score = 25;
                break;
            case 10:  // Grande Suite
                var grande = new[] { 1, 2, 3, 4, 5, 6 };
                if (grande.Where(x => valeursDes.Contains(x)).Count() >= 5)
                    score = 40;
                break;
            case 11:  // Petite Suite
                var petite = new HashSet<int>(valeursDes);
                bool hasPetiteSuite = petite.Where(x => petite.Contains(x + 1)).Count() >= 3;
                if (hasPetiteSuite)
                    score = 30;
                break;
            case 12:  // Yatzee
                if (valeursDes.All(v => v == valeursDes[0]))
                    score = 50;
                break;
            case 13:  // Chance
                score = valeursDes.Sum();
                break;
            default:
                score = 0;
                break;
        }
        return score;
    }
    //Console.SetCursorPosition(X, Y);
}