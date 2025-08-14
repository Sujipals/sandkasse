using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangmann
{

    internal class Program
    {
        static string lettersGuessed = "";
        static string theWord;
        static int life = 5;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            SetupGame();

        }
        private static void SetupGame()
        {
            Console.WriteLine("Velkommen til Hangman!");
            Console.WriteLine("Venlig indtast et ord til gættet:");
            theWord = Console.ReadLine().ToLower();
            Console.Clear();//skjuler ordet for gætteren
            StartGame();
        }
        private static void StartGame()
        {
            while (life > 0 && !IsWordGuessed())
            {
                ShowWord();
                char ch = GuessedLetter();
                //Tilføj kun bogstav, hvis det ikke er gættet før
                if (!lettersGuessed.Contains(ch))
                {
                    lettersGuessed += ch;
                    CheckIfLetterIsUsed(ch);
                }
                else
                {
                    Console.WriteLine("Du har allerede gættede dette bogstav!");
                }
            }
            if (life > 0)
            {
                Console.WriteLine("Du gættet et ord :" + theWord);
                Console.WriteLine("Tak for at spille Hangman!");
            }
            else
            {
                Console.WriteLine("Du hangede en man :(¤¤)\n");
                Console.WriteLine("Rigtigt ordet er: " + theWord);
                Console.WriteLine("Tak for at spille Hangman!");
            }
        }
        //tjekker hele ordet er gættet
        private static bool IsWordGuessed()
        {
            foreach (char ch in theWord)
            {
                if (!lettersGuessed.Contains(ch))
                return false;
            }
            return true;
        }
        //tjekker om bogstavet findes i ordet
        static bool CheckIfLetterIsUsed(char ch)
        {
            if (theWord.Contains(ch))
            {
                Console.WriteLine("Korrekt! Bogstavet '" + ch + "' findes i ordet.");
                return true;
            }
            else
            {
                life--;
                Console.WriteLine("Forkert:1 Bogstavet '" + ch + "' findes ikke i ordet.");
                Console.WriteLine("Du har " + life + " liv tilbage");
                return false;
            }
        }
        static void ShowWord()
        {
            Console.WriteLine("Ordet skal findes..");
            foreach (char ch in theWord)
            {
                if (lettersGuessed.Contains(ch)) 
                    Console.Write(ch + " ");
                else
                    Console.Write("_ ");
            }
            Console.WriteLine();
        }

        //Henter et bogstav fra spilleren
        static char GuessedLetter()
        {
            Console.WriteLine("Gæt et bogstav:");
            string input = Console.ReadLine().ToLower();


            // Tjekker om input er et enkelt bogstav
            // while (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
            //{
            //  Console.Write("Ugyldigt input. Indtast kun ét bogstav: ");
            //input = Console.ReadLine().ToLower();
            //}
            //return input[0];///
            if (input.Length == 1 && char.IsLetter(input[0]))
            {
                return input[0];
            }
            else
            {
                Console.WriteLine("Ugyldigt input. Indtast venligst et enkelt bogstav.");
                return GuessedLetter(); // Rekursiv kald for at få et gyldigt bogstav
            }
        }
    }
}