// Humble Object — Implementazione concreta della View (WinForms)
// Questa classe è "umile": non contiene logica di business.
// Sa solo esporre valori, ricevere comandi visuali, emettere eventi.
// Non viene testata con unit test — eventualmente con test GUI (WinAppDriver, ecc.).

public partial class LoginForm : Form, ILoginView
{
    public LoginForm()
    {
        InitializeComponent();
        loginButton.Click += (s, e) => LoginRequested?.Invoke(this, EventArgs.Empty);
    }

    public string Username => usernameTextBox.Text;
    public string Password => passwordTextBox.Text;

    public string ErrorMessage
    {
        set
        {
            errorLabel.Text = value;
            errorLabel.Visible = !string.IsNullOrEmpty(value);
        }
    }

    public bool IsBusy
    {
        set
        {
            usernameTextBox.Enabled = !value;
            passwordTextBox.Enabled = !value;
            loginButton.Enabled = !value;
        }
    }

    public event EventHandler? LoginRequested;

    public void CloseWithSuccess()
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
