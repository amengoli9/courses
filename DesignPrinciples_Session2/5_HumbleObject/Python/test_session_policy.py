# Humble Object — Test della SessionPolicy
# Eseguono in millisecondi, deterministicamente, senza time.sleep().
# FakeClock permette di controllare il tempo nei test.

import pytest
from datetime import datetime, timedelta
from clock import FakeClock
from session_policy import SessionPolicy


def test_session_not_expired_just_after_login():
    clock = FakeClock(datetime(2024, 6, 1, 10, 0, 0))
    policy = SessionPolicy(clock)
    last_activity = clock.now()

    clock.advance(timedelta(minutes=10))

    assert not policy.is_session_expired(last_activity)


def test_session_expired_after_timeout():
    clock = FakeClock(datetime(2024, 6, 1, 10, 0, 0))
    policy = SessionPolicy(clock)
    last_activity = clock.now()

    clock.advance(timedelta(minutes=31))

    assert policy.is_session_expired(last_activity)


def test_session_not_expired_at_exact_timeout():
    clock = FakeClock(datetime(2024, 6, 1, 10, 0, 0))
    policy = SessionPolicy(clock)
    last_activity = clock.now()

    clock.advance(timedelta(minutes=30))

    assert not policy.is_session_expired(last_activity)


def test_time_until_expiry_decreases_over_time():
    clock = FakeClock(datetime(2024, 6, 1, 10, 0, 0))
    policy = SessionPolicy(clock)
    last_activity = clock.now()

    clock.advance(timedelta(minutes=10))

    assert policy.time_until_expiry(last_activity) == timedelta(minutes=20)


def test_time_until_expiry_is_zero_after_expiration():
    clock = FakeClock(datetime(2024, 6, 1, 10, 0, 0))
    policy = SessionPolicy(clock)
    last_activity = clock.now()

    clock.advance(timedelta(minutes=60))

    assert policy.time_until_expiry(last_activity) == timedelta(0)

# Senza FakeClock saremmo costretti a time.sleep(31 * 60) — 31 minuti di attesa.
# Con FakeClock: 5 test in < 1ms.
