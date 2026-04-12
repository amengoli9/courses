// ===================================================================
// BLOCCO 4 — Code Smells: Dare un Nome ai Problemi
// Smell 1: Long Method
//
// ❌ LONG METHOD — Questo metodo fa almeno 4 cose:
//    Fase 1: Validazione file
//    Fase 2: Parsing
//    Fase 3: Validazione dati
//    Fase 4: Generazione risultato
//
// Ognuna potrebbe essere un metodo separato: ValidaFile, ParseRighe,
// ValidaRecord, CreaRisultato. Il metodo principale diventerebbe di 4 righe.
// ===================================================================

using Intro_SW_Session1.Models;

namespace Intro_SW_Session1.Block4_CodeSmells;

public class ImportatoreDati
{
    public ImportResult ImportaDaFile(string percorsoFile)
    {
        // ---------- Fase 1: Validazione file ----------
        if (string.IsNullOrWhiteSpace(percorsoFile))
            throw new ArgumentException("Il percorso del file è obbligatorio.");

        if (!File.Exists(percorsoFile))
            throw new FileNotFoundException(
                $"Il file '{percorsoFile}' non esiste.");

        var estensione = Path.GetExtension(percorsoFile).ToLower();
        if (estensione != ".csv" && estensione != ".tsv")
            throw new InvalidOperationException(
                $"Formato '{estensione}' non supportato. Usare .csv o .tsv.");

        var dimensione = new FileInfo(percorsoFile).Length;
        if (dimensione == 0)
            throw new InvalidOperationException("Il file è vuoto.");

        if (dimensione > 100 * 1024 * 1024) // 100 MB
            throw new InvalidOperationException(
                "Il file supera il limite di 100 MB.");

        // ---------- Fase 2: Parsing ----------
        var separatore = estensione == ".csv" ? ',' : '\t';
        var righe = File.ReadAllLines(percorsoFile);
        var intestazione = righe[0].Split(separatore);
        var records = new List<Dictionary<string, string>>();
        var errori = new List<string>();

        for (int i = 1; i < righe.Length; i++)
        {
            var campi = righe[i].Split(separatore);

            if (campi.Length != intestazione.Length)
            {
                errori.Add(
                    $"Riga {i + 1}: attesi {intestazione.Length} campi, " +
                    $"trovati {campi.Length}.");
                continue;
            }

            var record = new Dictionary<string, string>();
            for (int j = 0; j < intestazione.Length; j++)
            {
                record[intestazione[j].Trim()] = campi[j].Trim();
            }
            records.Add(record);
        }

        // ---------- Fase 3: Validazione dati ----------
        var recordsValidi = new List<Dictionary<string, string>>();
        foreach (var record in records)
        {
            bool valido = true;

            if (!record.ContainsKey("Nome") ||
                string.IsNullOrWhiteSpace(record["Nome"]))
            {
                errori.Add($"Record senza Nome valido: {string.Join(", ", record.Values)}");
                valido = false;
            }

            if (record.ContainsKey("Email") &&
                !string.IsNullOrWhiteSpace(record["Email"]))
            {
                if (!record["Email"].Contains("@") ||
                    !record["Email"].Contains("."))
                {
                    errori.Add($"Email non valida: {record["Email"]}");
                    valido = false;
                }
            }

            if (record.ContainsKey("Eta"))
            {
                if (!int.TryParse(record["Eta"], out int eta) ||
                    eta < 0 || eta > 150)
                {
                    errori.Add($"Età non valida: {record["Eta"]}");
                    valido = false;
                }
            }

            if (valido)
                recordsValidi.Add(record);
        }

        // ---------- Fase 4: Generazione risultato ----------
        return new ImportResult
        {
            TotaleRighe = righe.Length - 1,
            RigheImportate = recordsValidi.Count,
            RigheScartate = records.Count - recordsValidi.Count,
            RigheConErroriFormato = righe.Length - 1 - records.Count,
            Errori = errori,
            Dati = recordsValidi
        };
    }
}
