# YAGNI — VIOLAZIONE: 16 parametri per gestire casi ipotetici.
# In realtà viene sempre chiamata con format="csv" e gli altri default.
# Tutta la complessità esiste per un futuro che forse non arriverà.

def export_data(
    data,
    format="csv",
    encoding="utf-8",
    delimiter=",",
    quote_char='"',
    line_terminator="\n",
    include_headers=True,
    headers=None,
    transform_cell=None,
    skip_empty=False,
    null_value="",
    max_rows=None,
    output=None,
    compression=None,
    metadata=None,
    progress_callback=None,
):
    """Export data in vari formati con tante opzioni."""
    # ...300 righe di logica condizionale per gestire ogni combinazione...
    pass
