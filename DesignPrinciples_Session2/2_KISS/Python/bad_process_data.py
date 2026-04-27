# KISS — VIOLAZIONE: 10 parametri con flag booleani muti, mode magico con valori
# multipli, il tipo di ritorno cambia (list o dict!) a seconda degli argomenti.

def process_data(data, mode="default", validate=True, log=False,
                 max_size=None, transform=None, filter_fn=None,
                 sort=False, dedupe=False, group_by=None):
    if data is None:
        if mode == "strict":
            raise ValueError("data is None")
        return []

    result = data if isinstance(data, list) else list(data)

    if filter_fn:
        result = [x for x in result if filter_fn(x)]

    if transform:
        result = [transform(x) for x in result]

    if dedupe:
        if mode == "preserve-order":
            seen = set()
            result = [x for x in result if not (x in seen or seen.add(x))]
        else:
            result = list(set(result))

    if sort:
        if isinstance(sort, str):
            result = sorted(result, key=lambda x: getattr(x, sort, None))
        else:
            result = sorted(result)

    if group_by:
        from collections import defaultdict
        groups = defaultdict(list)
        for x in result:
            key = group_by(x) if callable(group_by) else getattr(x, group_by, None)
            groups[key].append(x)
        result = dict(groups)

    if max_size is not None:
        if isinstance(result, list) and len(result) > max_size:
            if mode == "truncate":
                result = result[:max_size]
            elif mode == "strict":
                raise ValueError("too large")

    if log:
        print(f"Processed: {len(data)} items -> {len(result)}")

    return result
