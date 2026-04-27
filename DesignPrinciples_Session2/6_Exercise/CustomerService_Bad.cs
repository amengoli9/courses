// ═══════════════════════════════════════════════════════════════════════════════
// ESERCIZIO "CACCIA ALLE VIOLAZIONI"
// ═══════════════════════════════════════════════════════════════════════════════
//
// Individua tutte le violazioni dei principi in questo snippet.
// Annota le tue osservazioni prima di confrontarti con gli altri.
//
// Hint: in ~30 righe sono presenti almeno 5 violazioni di principi diversi
//       più un bug di sicurezza grave.
// ═══════════════════════════════════════════════════════════════════════════════

public class CustomerService
{
    public string Process(int customerId, string action, string data,
                          bool sendEmail, bool log)
    {
        var connection = new SqlConnection("Server=...;Database=...;");
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT * FROM Customers WHERE Id = {customerId}";
        var reader = cmd.ExecuteReader();
        if (!reader.Read()) return "Customer not found";

        var customer = new Customer
        {
            Id = (int)reader["Id"],
            Name = (string)reader["Name"],
            Email = (string)reader["Email"],
            Discount = 0.10m   // Sconto standard del 10%
        };
        reader.Close();

        string result = "";
        if (action == "GetTotal")
        {
            var cmd2 = connection.CreateCommand();
            cmd2.CommandText = $"SELECT SUM(Amount) FROM Orders WHERE CustomerId = {customerId}";
            var total = (decimal)cmd2.ExecuteScalar();
            result = $"Total: {total * 0.90m:C}";  // applico 10% di sconto
        }
        else if (action == "UpdateName")
        {
            var cmd3 = connection.CreateCommand();
            cmd3.CommandText = $"UPDATE Customers SET Name = '{data}' WHERE Id = {customerId}";
            cmd3.ExecuteNonQuery();
            result = "Updated";
        }
        else if (action == "Delete")
        {
            var cmd4 = connection.CreateCommand();
            cmd4.CommandText = $"DELETE FROM Customers WHERE Id = {customerId}";
            cmd4.ExecuteNonQuery();
            result = "Deleted";
        }

        if (sendEmail)
        {
            var smtp = new SmtpClient("smtp.azienda.it");
            var msg = new MailMessage("noreply@azienda.it", customer.Email);
            msg.Subject = "Operazione completata";
            msg.Body = $"Caro {customer.Name}, l'operazione {action} è stata completata.";
            smtp.Send(msg);
        }

        if (log)
        {
            File.AppendAllText(@"C:\logs\customers.log",
                $"[{DateTime.Now}] {action} on customer {customerId}\n");
        }

        connection.Close();
        return result;
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// TRACCIA SOLUZIONE (per il docente — da non mostrare durante l'esercizio)
// ═══════════════════════════════════════════════════════════════════════════════
//
// 1. SRP — 4 attori in una classe:
//    Persistenza (DBA), logica business (Finanza), notifica (Marketing), logging (IT)
//
// 2. OCP — switch implicito su `action`:
//    aggiungere "Suspend" richiede di modificare Process()
//
// 3. KISS — metodo enorme:
//    5 parametri (2 booleani muti), stringhe magiche, 4+ livelli di responsabilità
//
// 4. DRY — lo sconto 10% vive in due posti:
//    Discount = 0.10m  (riga 25)  e  total * 0.90m  (riga 35)
//    Se lo sconto cambia, è facile aggiornarne solo uno
//
// 5. DIP — dipendenze tecnologiche hardcoded:
//    new SqlConnection, new SmtpClient, File.AppendAllText, DateTime.Now
//    → non testabile senza DB, SMTP, file system
//
// 6. SICUREZZA — SQL injection:
//    $"UPDATE ... SET Name = '{data}'"  → data è una stringa esterna non sanificata
//    Usare query parametrizzate: cmd.Parameters.AddWithValue("@Name", data)
// ═══════════════════════════════════════════════════════════════════════════════
