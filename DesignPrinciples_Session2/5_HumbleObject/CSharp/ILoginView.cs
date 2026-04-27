// Humble Object — Interfaccia della View (lato "umile")
// Espone solo proprietà e eventi. Zero logica di business.
// È di proprietà del Presenter (livello business), non della View (livello UI).

public interface ILoginView
{
    // Dati letti dalla UI
    string Username { get; }
    string Password { get; }

    // Comandi scritti dal Presenter sulla UI
    string ErrorMessage { set; }
    bool IsBusy { set; }

    // Evento: l'utente ha cliccato il pulsante Login
    event EventHandler LoginRequested;

    // Chiude la finestra dopo un login riuscito
    void CloseWithSuccess();
}
