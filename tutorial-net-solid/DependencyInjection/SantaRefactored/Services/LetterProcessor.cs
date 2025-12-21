using SantasWorkshop.Interfaces;
using SantasWorkshop.Models;

namespace SantasWorkshop.Services;

/// <summary>
/// Implementazione del processore di lettere
/// [S] Una sola responsabilità: orchestrare il processing delle lettere
/// [D] Dipende da astrazioni, non da implementazioni concrete
/// </summary>
public class LetterProcessor : ILetterProcessor
{
    private readonly IBehaviorValidator _behaviorValidator;
    private readonly IToyConfigurationProvider _toyConfigProvider;
    private readonly IElfAssignmentService _elfAssignmentService;
    private readonly IElfEnergyManager _elfEnergyManager;
    private readonly IPriorityCalculator _priorityCalculator;
    private readonly IChildRepository _childRepository;
    private readonly IToyRepository _toyRepository;
    private readonly INotificationService _notificationService;
    private readonly IDatabaseLogger _databaseLogger;

    public LetterProcessor(
        IBehaviorValidator behaviorValidator,
        IToyConfigurationProvider toyConfigProvider,
        IElfAssignmentService elfAssignmentService,
        IElfEnergyManager elfEnergyManager,
        IPriorityCalculator priorityCalculator,
        IChildRepository childRepository,
        IToyRepository toyRepository,
        INotificationService notificationService,
        IDatabaseLogger databaseLogger)
    {
        _behaviorValidator = behaviorValidator;
        _toyConfigProvider = toyConfigProvider;
        _elfAssignmentService = elfAssignmentService;
        _elfEnergyManager = elfEnergyManager;
        _priorityCalculator = priorityCalculator;
        _childRepository = childRepository;
        _toyRepository = toyRepository;
        _notificationService = notificationService;
        _databaseLogger = databaseLogger;
    }

    public void ProcessLetter(LetterRequest request)
    {
        Console.WriteLine($"\n📨 Lettera ricevuta da {request.ChildName}!");

        // Valida comportamento
        var validationResult = _behaviorValidator.Validate(request.Behavior, request.Age);
        if (!string.IsNullOrEmpty(validationResult.Message))
        {
            if (validationResult.Message.Contains("\n"))
            {
                var parts = validationResult.Message.Split('\n');
                Console.WriteLine($"⚠️ {request.ChildName} {parts[0]}");
                Console.WriteLine(parts[1]);
            }
            else
            {
                Console.WriteLine($"❌ {request.ChildName} {validationResult.Message}");
            }
        }

        if (!validationResult.IsValid)
        {
            return;
        }

        // Ottieni configurazione giocattolo
        var toyConfig = _toyConfigProvider.GetConfiguration(request.ToyType);
        Console.WriteLine(toyConfig.Description);

        // Assegna elfo
        string assignedElf = _elfAssignmentService.AssignElf(request.Country);

        // Gestisci energia elfi
        _elfEnergyManager.ConsumeEnergy(toyConfig.ProductionTime);
        if (_elfEnergyManager.NeedsRecharge())
        {
            _elfEnergyManager.Recharge();
        }

        // Calcola priorità
        int priority = _priorityCalculator.CalculatePriority(request.IsChristmasEve, request.Age, request.Country);

        // Crea e salva giocattolo
        var toy = new Toy
        {
            Type = request.ToyType,
            ChildName = request.ChildName,
            ProductionTime = toyConfig.ProductionTime,
            Priority = priority,
            AssignedElf = assignedElf,
            Country = request.Country
        };
        _toyRepository.Add(toy);

        // Crea e salva bambino
        var child = new Child
        {
            Name = request.ChildName,
            Age = request.Age,
            Behavior = request.Behavior,
            Country = request.Country,
            RequestedToy = request.ToyType
        };
        _childRepository.Add(child);

        // Invia notifica
        _notificationService.SendProductionNotification(child, toy, assignedElf);

        // Log nel database
        _databaseLogger.LogProduction(request.ChildName, request.ToyType, assignedElf);
    }
}
