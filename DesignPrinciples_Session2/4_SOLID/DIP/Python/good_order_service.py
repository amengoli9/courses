# DIP — CORRETTO: OrderService dipende da Protocol (astrazioni strutturali).
# Le implementazioni concrete vengono iniettate dall'esterno.
# In test si usano fake in memoria — niente DB, SMTP, Stripe reali.

from typing import Protocol


class OrderRepository(Protocol):
    def save(self, order: object) -> None: ...
    def find_by_id(self, order_id: int) -> object | None: ...


class EmailSender(Protocol):
    def send(self, to: str, subject: str, body: str = "") -> None: ...


class PaymentGateway(Protocol):
    def charge(self, amount: float, card_token: str) -> dict: ...


class OrderService:
    def __init__(
        self,
        repository: OrderRepository,
        email_sender: EmailSender,
        payments: PaymentGateway,
    ):
        self._repository = repository
        self._email_sender = email_sender
        self._payments = payments

    def place(self, order) -> None:
        if not order.items:
            raise ValueError("Ordine vuoto")

        result = self._payments.charge(order.total, order.customer.card_token)
        if not result.get("success"):
            raise RuntimeError(result.get("error", "Pagamento fallito"))

        self._repository.save(order)
        self._email_sender.send(order.customer.email, "Conferma ordine")


# ── Implementazioni di produzione (in un modulo "infrastructure") ──────────────

class SqliteOrderRepository:
    def save(self, order) -> None:
        import sqlite3
        conn = sqlite3.connect("orders.db")
        conn.execute("INSERT INTO orders ...", (order.id,))
        conn.commit()

    def find_by_id(self, order_id: int):
        return None


class SmtpEmailSender:
    def send(self, to: str, subject: str, body: str = "") -> None:
        import smtplib
        with smtplib.SMTP("smtp.azienda.it") as smtp:
            smtp.sendmail("noreply@azienda.it", to, f"Subject: {subject}\n\n{body}")


class StripePaymentGateway:
    def charge(self, amount: float, card_token: str) -> dict:
        import stripe
        charge = stripe.Charge.create(
            amount=int(amount * 100),
            currency="eur",
            source=card_token,
        )
        return {"success": charge.status == "succeeded", "error": None}


# ── Fake per i test (veloci, deterministici, zero infrastruttura) ───────────────

class FakeOrderRepository:
    def __init__(self):
        self.saved = []

    def save(self, order) -> None:
        self.saved.append(order)

    def find_by_id(self, order_id: int):
        return next((o for o in self.saved if o.id == order_id), None)


class FakeEmailSender:
    def __init__(self):
        self.sent = []

    def send(self, to: str, subject: str, body: str = "") -> None:
        self.sent.append({"to": to, "subject": subject})


class FakePaymentGateway:
    def __init__(self, success: bool = True):
        self._success = success

    def charge(self, amount: float, card_token: str) -> dict:
        return {"success": self._success, "error": None if self._success else "Card declined"}
