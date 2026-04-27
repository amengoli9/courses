# Humble Object — La logica di business (parte "ricca")
# SessionPolicy contiene le regole di scadenza della sessione.
# Dipende dal clock tramite duck typing (Protocol implicito): non conosce datetime.now().
# È testabile in modo deterministica con FakeClock.

from datetime import datetime, timedelta


class SessionPolicy:
    SESSION_TIMEOUT = timedelta(minutes=30)

    def __init__(self, clock):
        self._clock = clock  # iniettato: SystemClock in produzione, FakeClock in test

    def is_session_expired(self, last_activity: datetime) -> bool:
        now = self._clock.now()
        return (now - last_activity) > self.SESSION_TIMEOUT

    def time_until_expiry(self, last_activity: datetime) -> timedelta:
        now = self._clock.now()
        elapsed = now - last_activity
        return max(self.SESSION_TIMEOUT - elapsed, timedelta(0))
