# YAGNI — CORRETTO: implementa solo il caso reale di oggi.
# Quando servirà TSV o JSON, aggiungi una funzione dedicata.
# Alla terza funzione avrai dati sufficienti per astrarre (Rule of Three).

import csv


def export_to_csv(rows: list[dict], file_path: str) -> None:
    """Export delle righe in CSV con UTF-8 e header. È l'unico caso che ci serve oggi."""
    if not rows:
        return

    with open(file_path, "w", encoding="utf-8", newline="") as f:
        writer = csv.DictWriter(f, fieldnames=list(rows[0].keys()))
        writer.writeheader()
        writer.writerows(rows)


# Quando arriverà il secondo formato, si aggiunge qui — indipendente, senza toccare l'esistente:
# def export_to_tsv(rows: list[dict], file_path: str) -> None: ...
# def export_to_json(rows: list[dict], file_path: str) -> None: ...
