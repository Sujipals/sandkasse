using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biograf
{
    internal class Program
    {
        
        // Konstantter for biografens antal sale, sæder og rækker
        const int AntalSal = 4;
        const int AntalSæder = 20;
        const int AntalRækker = 12;
        const string  FilNavn = "bookinger.txt";    

        //Data for film og biografens sæder
        static string[] Film = {"Demon Slayer",
                                "Lilo & Stitch", 
                                "Harry Potter", 
                                 "Weapons"};

        //Multidimensionelt array til at gemme booking-status
        //biograf[sal, række, sæde] = true for reserveret, false for ledigt
        static bool[,,] biograf = new bool[AntalSal, AntalRækker, AntalSæder];//[sal, række, sæde]


        //Simplet Login
        static string brugernavn = "admin";
        static string adgangskode = "1234";

        static void Main(string[] args)
        {
            //Indlæs gemte bookinger
            LoadBestillinger(FilNavn);

                //login
                if(!Login()) return;
            //Start hovedmenu
            bool køreProgram = true;

            //Hovedloop, kører indil brugeren afslutter
            while (køreProgram)
            {
                Console.Clear();
                Console.WriteLine("=== Biograf Booking System ===");
                Console.WriteLine("1. Se film liste:");
                Console.WriteLine("2. Se film bookinger:");
                Console.WriteLine("3. Book plads:");
                Console.WriteLine("4. Nulstil alle bookinger:");
                Console.WriteLine("5. Afbestil en specifik plads:");
                Console.WriteLine("6. Se Gemte Bestillinger: ");
                Console.WriteLine("7. Gem bestillinger og afslut program:");
                Console.WriteLine("Vælg en function:");
                string valg = Console.ReadLine();

                //Menu valg med switch-case
                switch (valg)
                {
                    case "1"://Se film liste
                        SeFilmListe();
                        break;

                    case "2"://Se film bestilinger
                        SeFilmBookinger();
                        break;
                    case "3"://Book plads
                        BestilPlads();
                        break;
                    case "4"://Nulstil alle bestillinger
                        NulstilBestillinger();
                        break;
                    case "5"://Afbestil en specifik plads
                        AfbestilPlads();
                        break;
                    case "6"://Se gemte bestillinger
                        SeGemteBestillinger();
                        break;
                    case "7"://GemBookinger og afslut program
                        GemBestillinger(FilNavn);
                        køreProgram = false;
                        break;
                    default://Ugyldigt valg
                        Console.WriteLine("Ugyldigt valg! Tryk på en tast for at forsætte...");
                        Console.ReadKey();
                        break;
                }
            }      
        }
        //Login metode med mulighed for retry
        static bool Login()
        {
            while (true)//Loop indtil login er succesfuldt
            {
            Console.Clear();
            Console.WriteLine("===========Velkommen til Biograf Booking System!===========");
            Console.WriteLine("=== Login ===");
            Console.Write("Brugernavn: ");
            string inputBrugernavn = Console.ReadLine();
            Console.Write("Adgangskode: ");
            string inputAdgangskode = Console.ReadLine();

                //Tjekker om brugernavn og adgangskode er korrekte
                if (inputBrugernavn == brugernavn && inputAdgangskode == adgangskode)
                {
                    Console.WriteLine("Login succesfuldt!  Tryk på en tast..");
                    Console.ReadKey();
                    return true;
                }
                else
                {
                    Console.WriteLine("Ugyldigt brugernavn eller adgangskode!");
                    Console.WriteLine("Vil du prøve igen? (j/n): ");
                    string retry = Console.ReadLine().ToLower();
                    if (retry != "j" && retry != "ja")
                    {
                        return false;//Bruger valgte ikke at prøve igen, afslut login
                    }
                }
            }
        }



        //1.Se filmliste
        static void SeFilmListe()//Viser en liste over tilgængelige film
        {
            Console.Clear();//Rydder konsollen
            Console.WriteLine("=== Tilgængelige film ===");

            //foreach bruges til at  vise film
            int index = 1;
            foreach (string film in Film)
            {
                Console.WriteLine($"{index}. {film}");
                index++;
            }

            Console.WriteLine("\n Tryk på en tast for at gå tilbage....");
            Console.ReadKey ();
        }

        //2.Se film bookinger
        static void SeFilmBookinger()//Viser bookinger for en valgt film
        {
            int sal = ValiderValg();//Validerer valg af film
            if (sal == -1) return;// Hvis ugyldigt valg, afslut metoden

            VisSal(sal);

            //Vis statistik for ledige/optagede pladser
            int ledige = 0, optagede = 0;           //Optagede og ledige pladser tælles
            for (int r = 0; r<AntalRækker; r++)     //Gennemløber rækker
            {                                       //for-loop bruges til at tælle optagede og ledige pladser
                for (int s = 0; s<AntalSæder; s++)  //Gennemløber sæder
                {
                    if (biograf[sal, r, s]) optagede++;// Hvis sædet er optaget, tælles det som optaget
                    else ledige++;// Hvis sædet er ledigt, tælles det som ledigt
                }
            }

            Console.WriteLine($"\nOptagede pladser: {optagede}");
            Console.WriteLine($"\nLedige pladser: {ledige}");
            Console.WriteLine($"\nTotal pladser i sal {sal + 1}: {AntalRækker * AntalSæder}");
            Console.WriteLine($"\nTryk på en tast for at gå tilbage....");
            Console.ReadKey();
            
        }

        //3.Bestil plads
        static void BestilPlads()//Metode til at booke en plads i biografen
        {
            Console.WriteLine("=== Book Plads ===");
            int sal = ValiderValg();//Validerer valg af film
            if (sal == -1) return;

            bool fortsætBooking = true;//Variabel til at styre om brugeren vil fortsætte med at booke pladser

            while (fortsætBooking)//Loop indtil brugeren ikke vil booke flere pladser
            {
                
                VisSal(sal);//Viser den valgte sal med bookinger

                Console.Write($"\nIndtast række nummer (1-{AntalRækker}): ");
                if (!int.TryParse(Console.ReadLine(), out int række) || række < 1 || række > AntalRækker)//Tjekker om række nummer er gyldigt
                {
                    Console.WriteLine("Ugyldigt række nummer!");
                    Console.ReadKey();// Hvis ugyldigt, vises fejlmeddelelse og fortsætter loopet
                    continue;// Hvis ugyldigt, fortsætter loopet
                }

                Console.Write($"\nIndtast sæde nummer (1-{AntalSæder}): ");
                if (!int.TryParse(Console.ReadLine(), out int sæde) || sæde < 1 || sæde > AntalSæder)
                {
                    Console.WriteLine("Ugyldigt sæde nummer!");
                    Console.ReadKey();
                    continue;
                }
                //if/else bruges til at tjekke om pladsen er ledig
                if (biograf[sal, række - 1, sæde - 1])// Hvis sædet allerede er booket
                {
                    Console.WriteLine("Sædet er allerede optaget!");
                }
                else
                {
                    biograf[sal, række - 1, sæde - 1] = true;// Hvis sædet er ledigt, bookes det
                    Console.WriteLine("\nSædet er nu bestilet!");
                    Console.WriteLine($"\n\nDu har bestilet sæde {sæde} i række {række} i sal {sal + 1}.");
                    Console.WriteLine("\nVil du Fortsæte til at bestile en anden plads? (j/n): ");
                    string fortsæt = Console.ReadLine().ToLower();//Bruger input til at afgøre om brugeren vil fortsætte med at booke pladser
                    fortsætBooking = fortsæt == "j" || fortsæt == "ja";// Hvis brugeren indtaster 'j' eller 'ja', fortsætter booking
                }
                Console.WriteLine("\n Tryk på en tast for at gå tilbage....");
                Console.ReadKey();
            }
        }


        //4.valider valg af film
        static int ValiderValg()//Metode til at validere valg af film
        {
            Console.Clear();
            Console.WriteLine("Vælg en film til at se bestillinger: ");//Viser en liste over tilgængelige film
            for (int i=0; i < Film.Length; i++)// For-loop bruges til at vise film
            {
                Console.WriteLine($"{i + 1}. {Film[i]}");// For-loop bruges til at vise film
            }
            Console.Write("Indtast nummer:");

            //if bruges til validering af input
            if(int.TryParse(Console.ReadLine(), out int filmIndex) && filmIndex >= 1 && filmIndex <= Film.Length)
            {
                return filmIndex - 1;// Returnerer det valgte film indeks (0-baseret)
            }
            else
            {
                Console.WriteLine("Ugyldigt valg!");// Hvis input ikke er gyldigt, vises fejlmeddelelse
                Console.ReadKey();
                return -1;// Returnerer -1 for at indikere ugyldigt valg
            }

        }


        // 5.Vis sal
        private static void VisSal(int salValg)//Metode til at vise en specifik sal med bookinger
        {
            Console.Clear();
            Console.WriteLine($"=== Du har valgt sal {salValg + 1}: filmen er {Film[salValg]} ===");// Viser den valgte sal og film
            Console.WriteLine("Rækker og Sæder: ");

            // farvet legend
            Console.ForegroundColor = ConsoleColor.Red;// Sætter farve til rød for optagede pladser
            Console.WriteLine("■ = Optaget");
            Console.ResetColor();// Reset farve til standard
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("■ = Ledigt");
            Console.ResetColor();


            //for-loop bruges til at vise hver række og sæde i den valgte sal
            for (int r = 0; r < AntalRækker; r++)// Gennemløber rækker
            {
                for (int s = 0;  s < AntalSæder; s++)
                {
                    //Console.WriteLine($"Række {r + 1}, Sæde {s + 1}: {(biograf[salValg, r, s] ? "Optaget" : "Ledigt")}");
                    //Ternary operator bruges til at vise om sædet er optaget eller ledigt
                    Console.ForegroundColor = biograf[salValg, r, s] ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write(" ■ ");// Viser en rød firkant for optagede pladser og grøn firkant for ledige pladser
                }
                Console.ResetColor();
                Console.WriteLine($"  Række: { r + 1}");// Viser rækkenummer efter hver række
            }
        }
        //6.Nulstil alle bestillinger
        //Metode til at nulstille alle bookinger i biografen
        static void NulstilBestillinger()
        {
            for (int sal = 0; sal < AntalSal; sal++)// Gennemløber alle sale
                for (int r = 0; r < AntalRækker; r++)// Gennemløber alle rækker i den valgte sal
                    for (int s = 0; s < AntalSæder; s++)// Gennemløber alle sæder i den valgte række
                        biograf[sal, r, s] = false;// Nulstiller alle bookinger ved at sætte dem til false

            Console.WriteLine("Alle bestillinger er nulstillet! Tryk på en tast..."); ;
            Console.ReadKey();
        }

        //7.Afbestil en specifik plads
        static void AfbestilPlads()//Metode til at afbestille en specifik plads i biografen
        {
            Console.Clear();
            Console.WriteLine("=== Afbestil Plads ===");
            int sal = ValiderValg();// Validerer valg af film
            if (sal == -1) return;// Hvis ugyldigt valg, afslut metoden
            VisSal(sal);// Viser den valgte sal med bookinger
            Console.Write($"\nIndtast række nummer (1-{AntalRækker}): ");// Brugeren bedes indtaste række nummer for at afbestille en plads
            //Tjekker om række nummer er gyldigt
            if (!int.TryParse(Console.ReadLine(), out int række) || række < 1 || række > AntalRækker)
            {
                Console.WriteLine("Ugyldigt række nummer!");
                Console.ReadKey();
                return;// Hvis ugyldigt, vises fejlmeddelelse og afslut metoden
            }
            Console.Write($"\nIndtast sæde nummer (1-{AntalSæder}): ");// Tjekker om sæde nummer er gyldigt
            if (!int.TryParse(Console.ReadLine(), out int sæde) || sæde < 1 || sæde > AntalSæder)// Tjekker om sæde nummer er gyldigt
            {
                Console.WriteLine("Ugyldigt sæde nummer!");
                Console.ReadKey();// Hvis ugyldigt, vises fejlmeddelelse og afslut metoden
                return;
            }
            //if/else bruges til at tjekke om pladsen er booket
            if (!biograf[sal, række - 1, sæde - 1])// Hvis sædet ikke er booket
            {
                Console.WriteLine("Sædet er ikke bestilet!");
            }
            else
            {
                biograf[sal, række - 1, sæde - 1] = false;// Hvis sædet er booket, afbestilles det ved at sætte det til false
                GemBestillinger(FilNavn);// Gemmer ændringerne til fil
                Console.WriteLine("\nSædet er nu afbestilt!");
            }
            
            Console.WriteLine("\n Tryk på en tast for at gå tilbage....");
            Console.ReadKey();// Venter på brugerinput før at gå tilbage til hovedmenuen
        }
        //8.Gem bookinger  til fil og læsvenligt format
        static void GemBestillinger(string filNavn)//Metode til at gemme bookinger til en fil
        {
            using (StreamWriter sw = new StreamWriter(filNavn))// Opretter en StreamWriter til at skrive til filen
            {
                for(int sal = 0; sal < AntalSal; sal++)// Gennemløber alle sale
                {
                    for(int r = 0; r < AntalRækker; r++)// Gennemløber alle rækker i den valgte sal
                    {
                        for(int s = 0; s < AntalSæder; s++)// Gennemløber alle sæder i den valgte række
                        {
                            if (biograf[sal, r, s])// Hvis sædet er booket
                            {
                                sw.WriteLine($"Sal {sal +1 }, Række {r + 1}, Sæde {s + 1}");// Skriver sal, række og sæde til filen
                            }
                        }
                    }
                }
            }
        }
        //9.Se gemte bestillinger
        static void SeGemteBestillinger()
        {
            Console.Clear();
            Console.WriteLine("=== Gemte Bestillinger ===");
            if (!File.Exists(FilNavn))// Tjekker om filen eksisterer
            {
                Console.WriteLine("Ingen gemte bestillinger fundet.");
                Console.WriteLine("Tryk på en tast for at gå tilbage...");
                Console.ReadKey();
                return;
            }
            else
            {
                // Hvis filen findes, læses den og vises indholdet
                foreach (string linje in File.ReadAllLines(FilNavn))
                {
                    Console.WriteLine(linje);
                }
            }

            Console.WriteLine("\nTryk på en tast for at gå tilbage....");
            Console.ReadKey();
        }
        //10.Indlæs bookinger fra fil
        static void LoadBestillinger(string filNavn)//Metode til at indlæse bookinger fra en fil
        {
            // Tjekker om filen eksisterer, hvis ikke, afslut metoden
            if (!File.Exists(FilNavn)) return;
            // Læser alle linjer fra filen og opdaterer biografens booking-status
            foreach (string linje in File.ReadAllLines(FilNavn))
            {
                // Tjekker om linjen er tom eller kun indeholder hvide tegn, hvis ja, fortsætter til næste linje
                if (string.IsNullOrWhiteSpace(linje)) continue;

                // Tjekker om linjen starter med "Sal" for at identificere formatet
                if (linje.TrimStart().StartsWith("Sal", StringComparison.OrdinalIgnoreCase))
                {
                    // Hvis linjen starter med "Sal", deles den op i dele for at få sal, række og sæde
                    string[] dele = linje.Replace(",", "").Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    int sal = int.Parse(dele[1]) -1;// Sal er 0-baseret, så træk 1 fra
                    int række = int.Parse(dele[3]) - 1;// Række er 0-baseret, så træk 1 fra
                    int sæde = int.Parse(dele[5]) - 1;// Sæde er 0-baseret, så træk 1 fra

                    biograf[sal, række, sæde] = true;// Sætter booking-status for det angivne sæde til true (booket)
                }
                else
                {
                    // Hvis linjen ikke starter med "Sal", antages det at den er i formatet "sal,række,sæde"
                    string[] dele = linje.Split(',');// Deler linjen op i dele baseret på komma
                    int sal = int.Parse(dele[0]);// Sal er 1-baseret, så ingen justering nødvendig
                    int række = int.Parse(dele[1]);// Række er 1-baseret, så ingen justering nødvendig
                    int sæde = int.Parse(dele[2]);// Sæde er 1-baseret, så ingen justering nødvendig

                    biograf[sal, række, sæde] = true;// Sætter booking-status for det angivne sæde til true (booket)
                }
            }
        }
    }
}
