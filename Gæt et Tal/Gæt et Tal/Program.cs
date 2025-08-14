using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gæt_et_Tal
{
    internal class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {

            Console.WriteLine("Velkommen til Gæt et tal spil!");
            int secretNumber = GenerateRandomNumber(1, 25);
            int attempts = 0;
            int maxAttempts = 5;
            bool hasWon = false;

            while (attempts < maxAttempts && !hasWon)
            {
                int guess = GetUserGuess();
                attempts++;

                if (CheckGuess(guess, secretNumber))
                {
                    DisplayWinMessage(attempts);
                    hasWon = true;
                }
                else
                {
                    GiveHint(guess, secretNumber);
                    Console.WriteLine(($"Forsøg tilbage: {maxAttempts - attempts}"));
                }
            }
            if (!hasWon)
            {
                DisplayLoseMessage(secretNumber);
            }
        }
        //Genererer en tilfældig tal
        static int GenerateRandomNumber(int min, int max)
        { 
            return rnd.Next(min, max + 1);
        }
        //fået bruger's gætet tal
        static int GetUserGuess()
        {
            int guess;
            Console.Write("Indtast dit gæt(1-25): ");
            while(!int.TryParse(Console.ReadLine(), out guess) || guess < 1 || guess > 25)
            {
                Console.WriteLine("Ugyldigt input. Venlig indtast nummer mellem 1-25: ");
            }
            return guess;
        }
        //Tjekker nummer er korrekt
        static bool CheckGuess(int guess, int secretNumber)
        {
            return guess == secretNumber;
        }
        // giver hint
        static void GiveHint(int guess, int secretNumber) 
        {
            if (guess < secretNumber)
                Console.WriteLine("For Lavt! Prøve et højere tal!");
            else
                Console.WriteLine("For Højt! Prøve et lavere tal!");
        }
        //viser winner besked
        static void DisplayWinMessage(int attempts)
        {
            Console.WriteLine($"Tillykke! Du Gættet tallet på {attempts} forsøg!" );
        }
        //viser loser besked
        static void  DisplayLoseMessage(int secretNumber)
        {
            Console.WriteLine($"ohhh! Du løb tør for forsøg , Tallet var {secretNumber}.");
        }


    }
}
