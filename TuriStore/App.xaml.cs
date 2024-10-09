using System.Windows;
using System.Windows.Controls;

namespace TuriStore
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(TextBox_GotKeyboardFocus));
			EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(TextBox_LostMouseCapture));
			EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(TextBox_LostKeyboardFocus));

			base.OnStartup(e);
		}

		private void TextBox_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			// Fixes issue when clicking cut/copy/paste in context menu
			if ((sender as TextBox).SelectionLength == 0)
				(sender as TextBox).SelectAll();
		}

		private void TextBox_LostMouseCapture(object sender, RoutedEventArgs e)
		{
			// If user highlights some text, don't override it
			if ((sender as TextBox).SelectionLength == 0)
				(sender as TextBox).SelectAll();

			// Further clicks will not select all
			(sender as TextBox).LostMouseCapture -= TextBox_LostMouseCapture;
		}

		private void TextBox_LostKeyboardFocus(object sender, RoutedEventArgs e)
		{
			// Once we've left the TextBox, return the select all behavior
			(sender as TextBox).LostMouseCapture += TextBox_LostMouseCapture;
		}
	}
}
