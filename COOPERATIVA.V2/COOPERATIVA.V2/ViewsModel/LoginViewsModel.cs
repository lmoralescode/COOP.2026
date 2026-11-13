using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using COOPERATIVA.V2.Views;

namespace COOPERATIVA.V2.ViewsModel;

public partial class LoginViewsModel : ObservableObject
{
    [ObservableProperty]
    private string usuario = string.Empty;

    [ObservableProperty]
    private string contrasena = string.Empty;

    [ObservableProperty]
    private string usuarioError = string.Empty;

    [ObservableProperty]
    private string contrasenaError = string.Empty;

    [RelayCommand]
    private async Task Ingresar()
    {
        UsuarioError = string.IsNullOrWhiteSpace(Usuario)
            ? "El correo electrónico es obligatorio."
            : !System.Net.Mail.MailAddress.TryCreate(Usuario, out _)
                ? "Introduce un correo electrónico válido."
                : string.Empty;

        ContrasenaError = string.IsNullOrWhiteSpace(Contrasena)
            ? "La contraseña es obligatoria."
            : string.Empty;

        if (!string.IsNullOrEmpty(UsuarioError) ||
            !string.IsNullOrEmpty(ContrasenaError))
            return;

        if (Usuario != "lmorales@gmail.com" || Contrasena != "1234")
        {
            UsuarioError = "El correo o la contraseña son incorrectos.";
            return;
        }

        await Shell.Current.GoToAsync("//MenuPage");

        Usuario = string.Empty;
        Contrasena = string.Empty;
    }

    [RelayCommand]
    private async Task IrARegistro()
    {
        await Shell.Current.GoToAsync(nameof(RegistroViewsPage));
    }

    [RelayCommand]
    private async Task Regresar()
    {
        await Shell.Current.GoToAsync("//InicioViewsPage");
    }
}