namespace COOPERATIVA.V2.Views
{
    public partial class LoginViewsPage : ContentPage
    {
        public LoginViewsPage()
        {
            InitializeComponent();
        }

        private async void LoginNav_Clicked(object sender, EventArgs e)
        {
            string usuario = TxtEmail.Text;
            string clave = TxtClave.Text;

            if (usuario != usuario || clave != clave)
            {

                await DisplayAlert("Error", "Por favor, ingrese usuario y contraseña.", "OK");

            }
             if (usuario != "lmorales@gmail.com") 
            {
                await DisplayAlert("Error", "Usuario incorrecto.", "OK");
            }

            else if (clave != "1234")
            {
                await DisplayAlert("Error", "Contraseña incorrecta.", "OK");
            }
            else
            {
                await Navigation.PushAsync(new MenuViewsPage());

            }
        }
    }
}
