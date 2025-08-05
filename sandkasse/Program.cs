using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace sandkasse
{
    internal class Program
    {
        static bool isLoggedIn = false;
        static void Main(string[] args)
        {
            bool isRunning = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Velkomme til Sandkasse!");
                Console.WriteLine("Vælg venligst en mulighed: ");
                Console.WriteLine("0. Afslut program");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. VerdensTid");
                Console.WriteLine("3. Vis Bruger Info");
                if (isLoggedIn)
                {
                    Console.WriteLine("4. Logud");
                    
                }

                int input;
                try
                {
                    input = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Ugyldigt input");
                    Console.ReadKey();
                    continue; //Genstart loopen for gyldigt input
                }
                switch (input)
                {
                    case 0:
                        isRunning = false;
                        break;
                    case 1:
                        Login();
                        break;
                    case 2:
                        VerdensTid();
                        break;
                    
                    case 3:
                        if (isLoggedIn)
                        {
                            Console.WriteLine("Bruger Info: ");
                            Console.WriteLine("Brugernavn: Administrator");
                            Console.WriteLine("Status: Logget ind");
                        }
                        else
                        {
                            Console.WriteLine("Du skal logge ind for at se bruger info.");
                        }
                         break;
                    case 4:
                        if (isLoggedIn)
                        {
                            isLoggedIn = false;
                            Console.WriteLine("Logged ud...");
                        }
                        else
                        {
                            Console.WriteLine("Fejlintastning...");
                        }
                        break;
                    default:
                        Console.WriteLine("Fejlintastning... Prøve igen");
                        break;
                }
            
            Console.ReadKey();
            }
        while (isRunning);
        }
        private static void VerdensTid()
        {
            Console.Clear();
            Console.WriteLine("Vælg en tidszone:");
            Console.WriteLine("1. UTC ");
            Console.WriteLine("2. København");
            Console.WriteLine("3. Morocco");
            Console.WriteLine("4. New York");
            Console.WriteLine("5. Tokyo");
            Console.WriteLine("0. Tilbage til hovedmenuen");
            Console.Write("Indtast dit valg: ");
            string choice = Console.ReadLine();
            string timeZoneId = null;

            switch (choice) 
            {               
                case "1":
                    Console.WriteLine($"UTC: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
                    break;
                case "2":
                    timeZoneId = "Romance Standard Time";
                    break;
                case "3":
                    timeZoneId = "Morocco Standard Time";
                    break;
                case "4":
                    timeZoneId = "Eastern Standard Time";
                    break;
                case "5":
                    timeZoneId = "Tokyo Standard Time";
                    break;
                case "0":
                    return; //Metode til Tilbage til hovedmenuen
                default:
                    Console.WriteLine("Ugyldigt valg, prøv igen.");
                    return; // Method for Ugyligt input
            }
            if (timeZoneId != null)
            {
                try
                {
                    TimeZoneInfo selectedZone = TimeZoneInfo.FindSystemTimeZoneById("timeZoneId");
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, selectedZone);
                    Console.WriteLine($"{selectedZone.DisplayName}: {localTime:yyyy-MM-dd HH:mm:ss}");                   
                }
                catch (TimeZoneNotFoundException)
                {
                    Console.WriteLine("Tidszone ikke fundet.");
                }
            }
            Console.WriteLine("Tryk på en tast for at fortsætte...");
            Console.ReadKey();
        }


        private static void Login()
        {
            Console.WriteLine("Login");
            Console.WriteLine("Intast Brugernavn:");
            string username = Console.ReadLine();
            Console.WriteLine("Intast Kodeord:");
            string password = Console.ReadLine();
            
            if (username == "Administrator" && password == "Password")
            {
                isLoggedIn = true;
                Console.WriteLine("Login successful!");
            }
            else            
                Console.WriteLine("Kendes ikke, prøve igen.");
            
        }
    }
}

