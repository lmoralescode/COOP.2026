namespace COOPERATIVA.V2.ViewsModel;

public class _3_RegistroViewsModel : ContentPage
{
	public _3_RegistroViewsModel()
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