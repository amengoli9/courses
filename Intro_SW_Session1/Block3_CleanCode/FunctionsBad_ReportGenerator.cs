// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.2 — Funzioni piccole, una sola responsabilità
//
// ❌ Questa funzione fa almeno 5 cose diverse:
//    1. Recupera i dati dal database
//    2. Calcola statistiche
//    3. Genera HTML
//    4. Salva su file
//    5. Manda email
// ===================================================================

using System.Data.SqlClient;
using System.Net.Mail;
using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block3_CleanCode;

public class ReportGenerator
{
    public void GeneraReportMensile(int mese, int anno)
    {
        // 1. Recupera i dati
        var connessione = new SqlConnection("Server=...");
        connessione.Open();
        var comando = new SqlCommand(
            $"SELECT * FROM Vendite WHERE Mese = {mese} AND Anno = {anno}",
            connessione);
        var reader = comando.ExecuteReader();
        var vendite = new List<Vendita>();
        while (reader.Read())
        {
            vendite.Add(new Vendita
            {
                Id = reader.GetInt32(0),
                Prodotto = reader.GetString(1),
                Importo = reader.GetDecimal(2),
                Data = reader.GetDateTime(3)
            });
        }
        connessione.Close();

        // 2. Calcola statistiche
        var totale = vendite.Sum(v => v.Importo);
        var media = vendite.Average(v => v.Importo);
        var massimo = vendite.Max(v => v.Importo);
        var minimo = vendite.Min(v => v.Importo);
        var perProdotto = vendite
            .GroupBy(v => v.Prodotto)
            .ToDictionary(g => g.Key, g => g.Sum(v => v.Importo));

        // 3. Genera HTML
        var html = "<html><body>";
        html += $"<h1>Report Vendite {mese}/{anno}</h1>";
        html += $"<p>Totale: {totale:C}</p>";
        html += $"<p>Media: {media:C}</p>";
        html += $"<p>Max: {massimo:C} - Min: {minimo:C}</p>";
        html += "<table><tr><th>Prodotto</th><th>Vendite</th></tr>";
        foreach (var p in perProdotto)
        {
            html += $"<tr><td>{p.Key}</td><td>{p.Value:C}</td></tr>";
        }
        html += "</table></body></html>";

        // 4. Salva su file
        File.WriteAllText($"Report_{mese}_{anno}.html", html);

        // 5. Manda email
        var client = new SmtpClient("smtp.azienda.com");
        var messaggio = new MailMessage(
            "report@azienda.com",
            "direzione@azienda.com",
            $"Report Vendite {mese}/{anno}",
            "In allegato il report mensile.");
        messaggio.Attachments.Add(
            new Attachment($"Report_{mese}_{anno}.html"));
        client.Send(messaggio);
    }
}
