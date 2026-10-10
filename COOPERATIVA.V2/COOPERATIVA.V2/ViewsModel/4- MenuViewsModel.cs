namespace COOPERATIVA.V2.ViewsModel;

public class _4__MenuViewsModel : ContentPage
{
	public _4__MenuViewsModel()
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