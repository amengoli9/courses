// Humble Object — Test del Presenter
// Eseguono in millisecondi, deterministicamente, senza GUI, senza rete.
// FakeLoginView simula la View: è una classe normale senza nulla di grafico.

public class LoginPresenterTests
{
    private readonly FakeLoginView _view = new();
    private readonly Mock<IAuthenticationService> _authService = new();
    private readonly Mock<ILogger> _logger = new();
    private readonly LoginPresenter _presenter;

    public LoginPresenterTests()
    {
        _presenter = new LoginPresenter(_view, _authService.Object, _logger.Object);
    }

    [Fact]
    public void EmptyUsername_ShowsValidationError()
    {
        _view.Username = "";
        _view.Password = "abc123";

        _view.RaiseLoginRequested();

        Assert.Equal("Username obbligatorio", _view.ErrorMessage);
        _authService.Verify(
            s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public void ValidCredentials_ClosesWindow()
    {
        _view.Username = "alice";
        _view.Password = "secret";
        _authService.Setup(s => s.Authenticate("alice", "secret"))
                    .Returns(new AuthResult { Success = true });

        _view.RaiseLoginRequested();

        Assert.True(_view.WasClosedWithSuccess);
    }

    [Fact]
    public void InvalidCredentials_ShowsServerError()
    {
        _view.Username = "alice";
        _view.Password = "wrong";
        _authService.Setup(s => s.Authenticate("alice", "wrong"))
                    .Returns(new AuthResult { Success = false, Error = "Credenziali errate" });

        _view.RaiseLoginRequested();

        Assert.Equal("Credenziali errate", _view.ErrorMessage);
        Assert.False(_view.WasClosedWithSuccess);
    }

    [Fact]
    public void AuthServiceThrows_ShowsTechnicalError()
    {
        _view.Username = "alice";
        _view.Password = "secret";
        _authService.Setup(s => s.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                    .Throws<HttpRequestException>();

        _view.RaiseLoginRequested();

        Assert.Equal("Errore tecnico. Riprova più tardi.", _view.ErrorMessage);
    }
}

// Fake della View: classe normale in memoria, niente Windows Forms
public class FakeLoginView : ILoginView
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string ErrorMessage { get; set; } = "";
    public bool IsBusy { get; set; }
    public bool WasClosedWithSuccess { get; private set; }

    public event EventHandler? LoginRequested;
    public void CloseWithSuccess() => WasClosedWithSuccess = true;
    public void RaiseLoginRequested() => LoginRequested?.Invoke(this, EventArgs.Empty);
}
