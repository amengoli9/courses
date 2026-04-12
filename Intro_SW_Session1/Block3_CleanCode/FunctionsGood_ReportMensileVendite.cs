// ===================================================================
// BLOCCO 3 — Clean Code: I Principi di Robert C. Martin
// Sezione 3.2 — Funzioni piccole, una sola responsabilità
//
// ✅ Ogni metodo fa una cosa sola.
// Il metodo Genera è di 5 righe: ogni riga è un passo chiaro.
// Ogni pezzo è testabile indipendentemente.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block3_CleanCode;

// --- Interfacce: ogni responsabilità è isolata ---

public interface IVenditeRepository
{
    List<Vendita> GetVenditePerMese(int mese, int anno);
}

public interface ICalcolatoreStatistiche
{
    StatisticheVendite Calcola(List<Vendita> vendite);
}

public interface IReportFormatter
{
    string FormattaHtml(StatisticheVendite statistiche, int mese, int anno);
}

public interface IFileSaver
{
    string Salva(string contenuto, int mese, int anno);
}

public interface IEmailSender
{
    void InviaReportAllaDirezione(string percorsoFile, int mese, int anno);
}

// --- Classe principale: orchestra il flusso ---

public class ReportMensileVendite
{
    private readonly IVenditeRepository _venditeRepository;
    private readonly ICalcolatoreStatistiche _calcolatore;
    private readonly IReportFormatter _formatter;
    private readonly IFileSaver _fileSaver;
    private readonly IEmailSender _emailSender;

    public ReportMensileVendite(
        IVenditeRepository venditeRepository,
        ICalcolatoreStatistiche calcolatore,
        IReportFormatter formatter,
        IFileSaver fileSaver,
        IEmailSender emailSender)
    {
        _venditeRepository = venditeRepository;
        _calcolatore = calcolatore;
        _formatter = formatter;
        _fileSaver = fileSaver;
        _emailSender = emailSender;
    }

    public void Genera(int mese, int anno)
    {
        var vendite = _venditeRepository.GetVenditePerMese(mese, anno);
        var statistiche = _calcolatore.Calcola(vendite);
        var reportHtml = _formatter.FormattaHtml(statistiche, mese, anno);
        var percorsoFile = _fileSaver.Salva(reportHtml, mese, anno);
        _emailSender.InviaReportAllaDirezione(percorsoFile, mese, anno);
    }
}
