using System;
using System.Collections.Generic;
using System.Linq;

namespace SantasWorkshop
{
    /// <summary>
    /// QUESTA CLASSE FA TUTTO! 
    /// Gestisce elfi, giocattoli, consegne, inventario, lettere dei bambini,
    /// renne, slitta, logistica... Un vero incubo natalizio! 🎅💥
    /// </summary>
    public class WorkshopManager
    {
        private List<Child> children = new List<Child>();
        private List<Toy> toys = new List<Toy>();
        private int elfEnergy = 100;
        private int reindeerCount = 9;

        // Metodo gigantesco che fa TUTTO - viola Single Responsibility
        public void ProcessChristmasLetter(string childName, int age, string behavior,
            string toyType, string country, bool isChristmasEve)
        {
            Console.WriteLine($"\n📨 Lettera ricevuta da {childName}!");

            // Verifica comportamento del bambino (hardcoded)
            bool isNaughty = false;
            if (behavior == "Cattivo")
            {
                isNaughty = true;
                Console.WriteLine($"❌ {childName} è stato cattivo! Riceverà carbone!");
                return;
            }
            else if (behavior == "Birichino")
            {
                Console.WriteLine($"⚠️ {childName} è stato birichino... valutazione in corso");
                // Logica complessa inline
                if (age < 6)
                {
                    Console.WriteLine("È piccolo, gli diamo una possibilità!");
                }
                else
                {
                    Console.WriteLine("Troppo grande per fare il birichino! Carbone!");
                    return;
                }
            }

            // Calcola complessità del giocattolo - logica hardcoded (viola Open/Closed)
            int productionTime = 0;
            int materialsNeeded = 0;

            if (toyType == "Trenino")
            {
                productionTime = 5;
                materialsNeeded = 10;
                Console.WriteLine("🚂 Trenino: complessità media");
            }
            else if (toyType == "Bambola")
            {
                productionTime = 3;
                materialsNeeded = 7;
                Console.WriteLine("👧 Bambola: complessità bassa");
            }
            else if (toyType == "VideoGame")
            {
                productionTime = 8;
                materialsNeeded = 15;
                Console.WriteLine("🎮 VideoGioco: complessità alta");
            }
            else if (toyType == "Puzzle")
            {
                productionTime = 2;
                materialsNeeded = 5;
                Console.WriteLine("🧩 Puzzle: complessità molto bassa");
            }
            else if (toyType == "Bicicletta")
            {
                productionTime = 10;
                materialsNeeded = 20;
                Console.WriteLine("🚲 Bicicletta: complessità molto alta");
            }
            else
            {
                productionTime = 4;
                materialsNeeded = 8;
                Console.WriteLine("🎁 Giocattolo generico");
            }

            // Assegna elfi in base al paese - logica rigida (viola Open/Closed)
            string assignedElf = "";
            if (country == "Italia")
            {
                assignedElf = "Pasqualino";
                Console.WriteLine("🧝 Elfo italiano Pasqualino assegnato!");
            }
            else if (country == "USA")
            {
                assignedElf = "Jingles";
                Console.WriteLine("🧝 Elfo americano Jingles assegnato!");
            }
            else if (country == "Giappone")
            {
                assignedElf = "Yuki";
                Console.WriteLine("🧝 Elfo giapponese Yuki assegnato!");
            }
            else
            {
                assignedElf = "Buddy";
                Console.WriteLine("🧝 Elfo generico Buddy assegnato!");
            }

            // Controlla energia elfi
            elfEnergy -= productionTime;
            if (elfEnergy < 20)
            {
                Console.WriteLine("⚡ ATTENZIONE: Energia elfi bassa! Pausa biscotti necessaria!");
                elfEnergy = 100;
                Console.WriteLine("🍪 Elfi hanno mangiato biscotti! Energia ripristinata!");
            }

            // Priorità consegna - logica complessa inline
            int priority = 0;
            if (isChristmasEve)
            {
                priority = 1;
                Console.WriteLine("🔥 URGENTE: Vigilia di Natale! Priorità massima!");
            }
            else if (age < 5)
            {
                priority = 2;
                Console.WriteLine("👶 Bambino piccolo: priorità alta");
            }
            else if (country == "Italia" || country == "Polo Nord")
            {
                priority = 3;
                Console.WriteLine("🌍 Paese vicino: priorità media");
            }
            else
            {
                priority = 4;
                Console.WriteLine("✈️ Paese lontano: priorità normale");
            }

            // Crea e salva il giocattolo
            var toy = new Toy
            {
                Type = toyType,
                ChildName = childName,
                ProductionTime = productionTime,
                Priority = priority,
                AssignedElf = assignedElf,
                Country = country
            };
            toys.Add(toy);

            var child = new Child
            {
                Name = childName,
                Age = age,
                Behavior = behavior,
                Country = country,
                RequestedToy = toyType
            };
            children.Add(child);

            // Invia notifica diretta - accoppiamento stretto (viola Dependency Inversion)
            Console.WriteLine("\n📧 === NOTIFICA VIA EMAIL ===");
            Console.WriteLine($"A: santa@northpole.com");
            Console.WriteLine($"Oggetto: Nuovo ordine #{toys.Count}");
            Console.WriteLine($"Bambino: {childName} ({age} anni)");
            Console.WriteLine($"Giocattolo: {toyType}");
            Console.WriteLine($"Elfo: {assignedElf}");
            Console.WriteLine("============================\n");

            // Aggiorna database diretto - accoppiamento stretto
            Console.WriteLine($"[NorthPoleDB] INSERT INTO Productions (child, toy, elf) VALUES ('{childName}', '{toyType}', '{assignedElf}')");

            // Log su file (hardcoded)
            Console.WriteLine($"[LOG FILE] {DateTime.Now}: Produzione avviata per {childName}");
        }

        // Metodo che cambia comportamento in base al tipo - viola Liskov Substitution
        public void DeliverPresent(string deliveryType, int toyIndex)
        {
            if (toyIndex >= toys.Count)
            {
                Console.WriteLine("❌ Giocattolo non trovato!");
                return;
            }

            var toy = toys[toyIndex];

            // Comportamento COMPLETAMENTE diverso in base al tipo
            if (deliveryType == "Slitta")
            {
                // Consegna tradizionale con renne
                Console.WriteLine("\n🎅 === CONSEGNA CON SLITTA ===");
                Console.WriteLine($"Babbo Natale sale sulla slitta con {reindeerCount} renne");
                Console.WriteLine("🦌 Rudolf guida con il naso rosso!");
                Console.WriteLine($"🌟 Volo magico verso {toy.Country}");
                Console.WriteLine($"🏠 Scende dal camino");
                Console.WriteLine($"🎁 Deposita il {toy.Type} sotto l'albero");
                Console.WriteLine($"🍪 Mangia biscotti e beve latte");
                Console.WriteLine("✨ Torna al Polo Nord volando");
                reindeerCount--; // Una renna si stanca!
                if (reindeerCount < 6)
                {
                    Console.WriteLine("⚠️ Troppe poche renne! Richiamiamo quelle di riserva!");
                    reindeerCount = 9;
                }
            }
            else if (deliveryType == "Drone")
            {
                // Consegna moderna (completamente diversa!)
                Console.WriteLine("\n🚁 === CONSEGNA CON DRONE ===");
                Console.WriteLine($"Drone-Elfo attivato");
                Console.WriteLine($"GPS impostato su {toy.Country}");
                Console.WriteLine($"📦 Pacco lasciato alla porta");
                Console.WriteLine("Nessun biscotto ☹️");
                // Drone NON usa le renne!
            }
            else if (deliveryType == "Teletrasporto")
            {
                // Consegna magica futuristica (ancora diversa!)
                Console.WriteLine("\n✨ === TELETRASPORTO MAGICO ===");
                Console.WriteLine("⭐ Magia natalizia attivata!");
                Console.WriteLine($"💫 *POOF* Regalo appare sotto l'albero di {toy.ChildName}!");
                Console.WriteLine("Consegna istantanea!");
                // Teletrasporto ignora completamente logistica e renne!
            }
            else
            {
                // Fallback generico (comportamento inconsistente)
                Console.WriteLine($"📬 Regalo spedito via posta a {toy.ChildName}");
            }
        }

        // Interfaccia grassa - viola Interface Segregation
        // Metodi che solo alcuni client useranno

        public void FeedReindeer()
        {
            // Solo per consegne con slitta, ma tutti devono implementarlo!
            Console.WriteLine("🥕 Renne nutrite con carote magiche");
        }

        public void ChargeWorkshopBattery()
        {
            // Solo per consegne con drone, ma è pubblico
            Console.WriteLine("🔋 Batterie laboratorio ricaricate");
        }

        public void CastChristmasMagicSpell()
        {
            // Solo per teletrasporto, ma disponibile sempre
            Console.WriteLine("🪄 Incantesimo di Natale lanciato!");
        }

        public void OrganizeElfParty()
        {
            // Metodo random che non tutti useranno
            Console.WriteLine("🎉 Festa degli elfi organizzata!");
        }

        public void PolishSleigh()
        {
            // Solo per slitta
            Console.WriteLine("✨ Slitta lucidata a specchio!");
        }

        public void UpdateGPSSatellites()
        {
            // Solo per drone
            Console.WriteLine("🛰️ Satelliti GPS aggiornati");
        }

        // Dipendenze concrete hardcoded - viola Dependency Inversion
        public void SaveToNorthPoleDatabase()
        {
            // Accoppiamento diretto a un database specifico
            Console.WriteLine("\n[NORTH POLE DB v2.5] Connessione a northpole-sql.santa.local:3306");
            Console.WriteLine("[NORTH POLE DB] BEGIN TRANSACTION");

            foreach (var toy in toys)
            {
                Console.WriteLine($"[NORTH POLE DB] INSERT INTO Toys (type, child, elf) " +
                    $"VALUES ('{toy.Type}', '{toy.ChildName}', '{toy.AssignedElf}')");
            }

            foreach (var child in children)
            {
                Console.WriteLine($"[NORTH POLE DB] INSERT INTO Children (name, behavior) " +
                    $"VALUES ('{child.Name}', '{child.Behavior}')");
            }

            Console.WriteLine("[NORTH POLE DB] COMMIT");
        }

        public void GenerateChristmasReport()
        {
            // Logica di reporting mescolata con business logic
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("🎄 REPORT DI NATALE - WORKSHOP DI BABBO NATALE 🎅");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"📊 Totale bambini: {children.Count}");
            Console.WriteLine($"🎁 Totale giocattoli prodotti: {toys.Count}");
            Console.WriteLine($"⚡ Energia elfi rimanente: {elfEnergy}%");
            Console.WriteLine($"🦌 Renne disponibili: {reindeerCount}");

            Console.WriteLine("\n📈 STATISTICHE PER PAESE:");
            var byCountry = toys.GroupBy(t => t.Country);
            foreach (var group in byCountry)
            {
                Console.WriteLine($"  {group.Key}: {group.Count()} regali");
            }

            Console.WriteLine("\n🎮 GIOCATTOLI PIÙ RICHIESTI:");
            var byType = toys.GroupBy(t => t.Type);
            foreach (var group in byType.OrderByDescending(g => g.Count()).Take(3))
            {
                Console.WriteLine($"  {group.Key}: {group.Count()} richieste");
            }

            Console.WriteLine("\n👼 COMPORTAMENTO BAMBINI:");
            var goodKids = children.Count(c => c.Behavior == "Buono");
            var naughtyKids = children.Count(c => c.Behavior == "Cattivo");
            Console.WriteLine($"  😇 Buoni: {goodKids}");
            Console.WriteLine($"  😈 Cattivi: {naughtyKids}");

            Console.WriteLine(new string('=', 60) + "\n");
        }

        public void SendToSantasPrivateFax()
        {
            // Accoppiamento a tecnologia specifica obsoleta!
            Console.WriteLine("\n📠 INVIO FAX A BABBO NATALE...");
            Console.WriteLine("☎️  Composizione numero: +999-NORTHPOLE");
            Console.WriteLine("📄 *beep* *boop* *screech*");
            Console.WriteLine("✅ Fax inviato con successo!");
        }
    }

    // Classi dati semplici
    public class Child
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Behavior { get; set; }
        public string Country { get; set; }
        public string RequestedToy { get; set; }
    }

    public class Toy
    {
        public string Type { get; set; }
        public string ChildName { get; set; }
        public int ProductionTime { get; set; }
        public int Priority { get; set; }
        public string AssignedElf { get; set; }
        public string Country { get; set; }
    }

    // Programma di test
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🎅🎄 WORKSHOP DI BABBO NATALE - Sistema di Gestione 🎄🎅");
            Console.WriteLine("      (Versione Problematica - Pre-SOLID)\n");
            Console.WriteLine(new string('*', 60));

            var workshop = new WorkshopManager();

            // Scenario 1: Bambina buona italiana
            workshop.ProcessChristmasLetter("Sofia", 7, "Buono", "Bambola", "Italia", false);
            workshop.DeliverPresent("Slitta", 0);

            Console.WriteLine("\n" + new string('-', 60) + "\n");

            // Scenario 2: Bambino birichino americano
            workshop.ProcessChristmasLetter("Tommy", 5, "Birichino", "VideoGame", "USA", false);
            workshop.DeliverPresent("Drone", 1);

            Console.WriteLine("\n" + new string('-', 60) + "\n");

            // Scenario 3: Urgenza vigilia di Natale!
            workshop.ProcessChristmasLetter("Yuki", 4, "Buono", "Trenino", "Giappone", true);
            workshop.DeliverPresent("Teletrasporto", 2);

            Console.WriteLine("\n" + new string('-', 60) + "\n");

            // Scenario 4: Bambino cattivo (riceve carbone)
            workshop.ProcessChristmasLetter("Marco", 10, "Cattivo", "Bicicletta", "Italia", false);

            Console.WriteLine("\n" + new string('-', 60) + "\n");

            // Scenario 5: Altri ordini
            workshop.ProcessChristmasLetter("Emma", 6, "Buono", "Puzzle", "Francia", false);
            workshop.DeliverPresent("Slitta", 3);

            // Report finale
            workshop.GenerateChristmasReport();
            workshop.SaveToNorthPoleDatabase();
            workshop.SendToSantasPrivateFax();

            Console.WriteLine("\n🎄 Buon Natale a tutti! 🎅");
        }
    }
}