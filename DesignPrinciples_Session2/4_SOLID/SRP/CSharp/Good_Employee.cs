// SRP — CORRETTO: ogni classe ha un solo "padrone" (attore).
// Employee → struttura dati di dominio
// PayCalculator → regola HR
// EmployeeReportGenerator → formato Finanza
// EmployeeRepository → persistenza IT

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal HourlyRate { get; set; }
    public IReadOnlyList<TimeEntry> TimeEntries { get; set; }
}

// Attore: HR — cambia se cambiano le regole di calcolo paga
public class PayCalculator
{
    public decimal Calculate(Employee employee)
    {
        var regular = employee.TimeEntries.Where(t => !t.IsOvertime).Sum(t => t.Hours);
        var overtime = employee.TimeEntries.Where(t => t.IsOvertime).Sum(t => t.Hours);
        return regular * employee.HourlyRate + overtime * employee.HourlyRate * 1.5m;
    }
}

// Attore: Finanza — cambia se cambia il formato del report
public class EmployeeReportGenerator
{
    private readonly PayCalculator _payCalculator;

    public EmployeeReportGenerator(PayCalculator payCalculator)
    {
        _payCalculator = payCalculator;
    }

    public string Generate(Employee employee) =>
        $"{employee.Name}, {employee.Id}, {_payCalculator.Calculate(employee):C}";
}

// Attore: IT — cambia se cambia lo schema DB o la tecnologia di persistenza
public class EmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Save(Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Employees ...";
        cmd.ExecuteNonQuery();
    }
}
