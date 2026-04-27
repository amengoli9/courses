# DRY — CORRETTO: la convenzione di display è centralizzata in to_display_units().
# Il nome cattura il PERCHÉ (convenzione management), non solo il cosa (/ 1000).

REVENUE_DISPLAY_DIVISOR = 1000  # i report mostrano cifre in migliaia di euro


def to_display_units(amount: float) -> float:
    """Converte un importo in euro nelle unità di visualizzazione standard del management."""
    return amount / REVENUE_DISPLAY_DIVISOR


class ReportService:
    def quarterly_revenue(self, sales):
        return to_display_units(sum(s.amount for s in sales))

    def yearly_revenue(self, sales):
        return to_display_units(sum(s.amount for s in sales))

    def by_region_revenue(self, sales, region):
        return to_display_units(sum(s.amount for s in sales if s.region == region))
