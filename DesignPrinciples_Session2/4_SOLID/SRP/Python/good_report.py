# SRP — CORRETTO: tre classi, tre responsabilità, tre attori separati.

class ReportData:
    def __init__(self, rows: list[dict]):
        self.rows = rows


class ReportCalculator:
    def calculate(self, report: ReportData) -> dict:
        rows = report.rows
        return {
            "count": len(rows),
            "total": sum(r["amount"] for r in rows),
            "average": sum(r["amount"] for r in rows) / max(len(rows), 1),
        }


class ReportPdfRenderer:
    def render(self, report: ReportData, path: str) -> None:
        from reportlab.pdfgen import canvas
        c = canvas.Canvas(path)
        # ... rendering ...
        c.save()


class ReportEmailer:
    def __init__(self, smtp_client):
        self._smtp = smtp_client

    def send(self, attachment_path: str, recipient: str) -> None:
        # ... costruzione e invio email ...
        ...
