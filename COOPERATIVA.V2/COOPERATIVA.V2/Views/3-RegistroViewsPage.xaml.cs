namespace COOPERATIVA.V2.Views;

public partial class RegistroViewsPage : ContentPage
{
	public RegistroViewsPage()
	{
        InitializeComponent();
    }

    private async void OnIniciarSesionClicked(
       object sender, EventArgs e)
    {
        string usuario = CorreoEntry.Text?.Trim() ?? "";
        string clave = ContrasenaEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(usuario) ||
            string.IsNullOrWhiteSpace(clave))
        {
            await DisplayAlertAsync(
                "Aviso",
                "Ingrese el usuario y la contraseña.",
                "Aceptar");
            return;
        }

        // Credenciales exclusivamente para pruebas.
        if (usuario == "lmorales@gmail.com" && clave == "1234")
        {
            await Shell.Current.GoToAsync(
                nameof(MenuPage));
        }
        else
        {
            await DisplayAlertAsync(
                "Error",
                "Usuario o contraseña incorrectos.",
                "Aceptar");
        }
    }

    private void OnLimpiarSocioClicked(object sender, EventArgs e)
    {
        CorreoEntry.Text = "";
        ContrasenaEntry.Text = "";
    }

    private async void OnRegresarLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelarSocioClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}