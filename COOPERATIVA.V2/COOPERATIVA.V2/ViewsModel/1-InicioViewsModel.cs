namespace COOPERATIVA.V2.ViewsModel;

public class _1_InicioViewsModel : ContentPage
{
	public _1_InicioViewsModel()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}