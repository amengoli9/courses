# SRP — VIOLAZIONE: una classe, tre responsabilità, tre attori.
# Calcolo → chi conosce il dominio
# PDF → chi gestisce il formato visuale
# Email → chi gestisce l'infrastruttura di comunicazione

class Report:
    def __init__(self, data: list[dict]):
        self.data = data

    # Attore: dominio business
    def calculate(self) -> dict:
        return {
            "count": len(self.data),
            "total": sum(d["amount"] for d in self.data),
            "average": sum(d["amount"] for d in self.data) / max(len(self.data), 1),
        }

    # Attore: chi gestisce il layout del PDF
    def to_pdf(self, path: str) -> None:
        from reportlab.pdfgen import canvas
        c = canvas.Canvas(path)
        # ... 50 righe di rendering PDF ...
        c.save()

    # Attore: chi gestisce il provider SMTP
    def email_to(self, recipient: str) -> None:
        import smtplib
        from email.message import EmailMessage
        # ... costruzione e invio email ...
