# DIP — VIOLAZIONE: OrderService crea direttamente le dipendenze concrete.
# Non testabile senza DB, SMTP e Stripe reali.

import sqlite3
import smtplib
import stripe


class OrderService:
    def place(self, order):
        if not order.items:
            raise ValueError("Ordine vuoto")

        # Dipendenza diretta da Stripe
        charge = stripe.Charge.create(
            amount=int(order.total * 100),
            currency="eur",
            source=order.customer.card_token,
        )
        if charge.status != "succeeded":
            raise RuntimeError("Pagamento fallito")

        # Dipendenza diretta da SQLite
        conn = sqlite3.connect("orders.db")
        conn.execute("INSERT INTO orders ...", (order.id,))
        conn.commit()

        # Dipendenza diretta da SMTP
        with smtplib.SMTP("smtp.azienda.it") as smtp:
            smtp.sendmail("noreply@azienda.it", order.customer.email, "Conferma ordine")
