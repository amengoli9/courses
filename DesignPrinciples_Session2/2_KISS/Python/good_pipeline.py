# KISS — CORRETTO: funzioni piccole e componibili, ognuna fa una cosa sola.
# Il chiamante assembla la pipeline esplicitamente — niente flag nascosti.

from collections.abc import Iterable, Callable
from typing import TypeVar
import logging

T = TypeVar("T")
logger = logging.getLogger(__name__)


def filter_items(items: Iterable[T], predicate: Callable[[T], bool]) -> list[T]:
    return [x for x in items if predicate(x)]


def transform_items(items: Iterable[T], transform: Callable[[T], T]) -> list[T]:
    return [transform(x) for x in items]


def dedupe_preserving_order(items: Iterable[T]) -> list[T]:
    seen: set[T] = set()
    result = []
    for x in items:
        if x not in seen:
            seen.add(x)
            result.append(x)
    return result


def truncate(items: list[T], max_size: int) -> list[T]:
    return items[:max_size] if len(items) > max_size else items


# Il caller compone le operazioni di cui ha bisogno — niente flag opachi
def process_users(users: list) -> list:
    active = filter_items(users, lambda u: u.is_active)
    unique = dedupe_preserving_order(active)
    limited = truncate(unique, max_size=100)
    logger.info("Processed %d users -> %d", len(users), len(limited))
    return limited
