// Humble Object — Il Presenter (parte "ricca di logica")
// Contiene tutta la logica di autenticazione: validazione, chiamata al servizio,
// gestione degli errori, logging. Non sa nulla di WinForms.
// Dipende da ILoginView (astrazione) — rispetta DIP.
// Ha una sola responsabilità — rispetta SRP.
// È testabile con unit test in millisecondi, senza aprire una finestra.

public class LoginPresenter
{
    private readonly ILoginView _view;
    private readonly IAuthenticationService _authService;
    private readonly ILogger _logger;

    public LoginPresenter(
        ILoginView view,
        IAuthenticationService authService,
        ILogger logger)
    {
        _view = view;
        _authService = authService;
        _logger = logger;

        _view.LoginRequested += OnLoginRequested;
    }

    public void OnLoginRequested(object? sender, EventArgs e)
    {
        var username = _view.Username;
        var password = _view.Password;

        if (string.IsNullOrWhiteSpace(username))
        {
            _view.ErrorMessage = "Username obbligatorio";
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            _view.ErrorMessage = "Password obbligatoria";
            return;
        }

        _view.IsBusy = true;
        _view.ErrorMessage = "";

        try
        {
            var result = _authService.Authenticate(username, password);
            if (result.Success)
            {
                _logger.LogInformation("Login successful for {User}", username);
                _view.CloseWithSuccess();
            }
            else
            {
                _logger.LogWarning("Login failed for {User}: {Reason}", username, result.Error);
                _view.ErrorMessage = result.Error;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Authentication error");
            _view.ErrorMessage = "Errore tecnico. Riprova più tardi.";
        }
        finally
        {
            _view.IsBusy = false;
        }
    }
}
