using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SantasWorkshop.Delivery;
using SantasWorkshop.Infrastructure;
using SantasWorkshop.Interfaces;
using SantasWorkshop.Interfaces.Delivery;
using SantasWorkshop.Services;

var host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
        options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
    })
    .ConfigureServices((context, services) =>
    {
        // === REGISTRAZIONE SERVIZI CON DEPENDENCY INJECTION ===


        // Servizi Singleton (stato condiviso)
        services.AddSingleton<IElfEnergyManager, ElfEnergyManager>();
        services.AddSingleton<IReindeerManager, ReindeerManager>();
        services.AddSingleton<IChildRepository, InMemoryChildRepository>();
        services.AddSingleton<IToyRepository, InMemoryToyRepository>();

        // Servizi Transient (senza stato)
        services.AddTransient<IBehaviorValidator, BehaviorValidator>();
        services.AddTransient<IToyConfigurationProvider, ToyConfigurationProvider>();
        services.AddTransient<IElfAssignmentService, ElfAssignmentService>();
        services.AddTransient<IPriorityCalculator, PriorityCalculator>();
        services.AddTransient<INotificationService, ConsoleNotificationService>();
        services.AddTransient<IDatabaseLogger, ConsoleDatabaseLogger>();
        services.AddTransient<IReportGenerator, ConsoleReportGenerator>();
        services.AddTransient<IDatabaseService, ConsoleDatabaseService>();
        services.AddTransient<IFaxService, ConsoleFaxService>();

        // Strategie di consegna
        services.AddTransient<ISleighDelivery, SleighDeliveryStrategy>();
        services.AddTransient<IDroneDelivery, DroneDeliveryStrategy>();
        services.AddTransient<ITeleportDelivery, TeleportDeliveryStrategy>();
        services.AddTransient<PostalDeliveryStrategy>();
        services.AddTransient<IDeliveryStrategyFactory, DeliveryStrategyFactory>();

        // Servizi principali
        services.AddTransient<ILetterProcessor, LetterProcessor>();
        services.AddTransient<IDeliveryService, DeliveryService>();
        services.AddTransient<IWorkshopFacade, WorkshopFacade>();
    })
    .Build();

// Esegui la demo
RunDemo(host.Services.GetRequiredService<IWorkshopFacade>());

void RunDemo(IWorkshopFacade workshop)
{
    Console.WriteLine("🎅🎄 WORKSHOP DI BABBO NATALE - Sistema di Gestione 🎄🎅");
    Console.WriteLine("      (Versione SOLID - Con Dependency Injection)\n");
    Console.WriteLine(new string('*', 60));

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
