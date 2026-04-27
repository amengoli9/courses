# Humble Object — Il Clock (parte "umile")
# SystemClock ha una sola riga di codice reale: datetime.now().
# Non c'è logica da testare qui — non ne vale la pena in unit test.
# FakeClock invece è controllabile nei test: puoi impostare e avanzare il tempo.

from datetime import datetime, timedelta


class SystemClock:
    """Clock di produzione: restituisce l'orario reale. Non deterministico → non testabile."""
    def now(self) -> datetime:
        return datetime.now()


class FakeClock:
    """Clock controllabile per i test: il tempo è deterministico e avanzabile a piacere."""
    def __init__(self, initial: datetime):
        self._now = initial

    def now(self) -> datetime:
        return self._now

    def advance(self, delta: timedelta) -> None:
        self._now += delta
