# DRY — VIOLAZIONE: la convenzione "importi in migliaia di euro" è ripetuta tre volte.
# Se il management chiede milioni, vanno aggiornati tutti i metodi.

class ReportService:
    def quarterly_revenue(self, sales):
        total = sum(s.amount for s in sales)
        return total / 1000  # migliaia di euro

    def yearly_revenue(self, sales):
        total = sum(s.amount for s in sales)
        return total / 1000  # migliaia di euro

    def by_region_revenue(self, sales, region):
        total = sum(s.amount for s in sales if s.region == region)
        return total / 1000  # migliaia di euro
