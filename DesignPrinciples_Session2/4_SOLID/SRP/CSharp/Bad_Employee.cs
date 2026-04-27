// SRP — VIOLAZIONE: una classe risponde a TRE attori diversi.
// HR → CalculatePay, Finanza → GenerateReport, IT → Save
// Un cambio di un attore rischia di rompere il codice degli altri.

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal HourlyRate { get; set; }
    public List<TimeEntry> TimeEntries { get; set; }

    // Regola di calcolo paga — di competenza di HR / Risorse Umane
    public decimal CalculatePay()
    {
        var regular = TimeEntries.Where(t => !t.IsOvertime).Sum(t => t.Hours);
        var overtime = TimeEntries.Where(t => t.IsOvertime).Sum(t => t.Hours);
        return regular * HourlyRate + overtime * HourlyRate * 1.5m;
    }

    // Formato del report — di competenza dell'ufficio Finanza
    public string GenerateReport()
    {
        return $"{Name}, {Id}, {CalculatePay():C}";
    }

    // Persistenza — di competenza del team IT / DBA
    public void Save()
    {
        using var connection = new SqlConnection("Server=...;Database=...;");
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Employees ...";
        cmd.ExecuteNonQuery();
    }
}
