# ISP — Python con typing.Protocol (duck typing strutturale, Python 3.8+)
# Le classi NON devono dichiarare esplicitamente di implementare un protocollo:
# basta avere i metodi giusti. Il type checker (mypy/pyright) verifica strutturalmente.

from typing import Protocol


class Readable(Protocol):
    def open(self, path: str) -> None: ...


class Writable(Protocol):
    def save(self, path: str) -> None: ...


class Printable(Protocol):
    def print_doc(self) -> None: ...


class Signable(Protocol):
    def sign(self, certificate: object) -> None: ...


# Le funzioni dichiarano solo il protocollo che usano
def add_to_print_queue(item: Printable) -> None:
    item.print_doc()


def sign_document(doc: Signable, cert: object) -> None:
    doc.sign(cert)


# Implementazione concreta: compatibile per duck typing con Readable, Writable, Printable
class PlainTextEditor:
    def open(self, path: str) -> None:
        print(f"Opening {path}")

    def save(self, path: str) -> None:
        print(f"Saving to {path}")

    def print_doc(self) -> None:
        print("Printing...")


# mypy accetterà add_to_print_queue(PlainTextEditor()) senza dichiarazioni esplicite.
# Non accetterà sign_document(PlainTextEditor(), cert) perché manca il metodo sign().
