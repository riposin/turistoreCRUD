using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;
using TuriStore.Models;
using TuriStore.Entities;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Linq;

namespace TuriStore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//private List<ItemView> _allItemView;
		private ObservableCollection<ItemView> _allItemView;
		private readonly ItemModel _itemModel;

		public MainWindow()
		{
			InitializeComponent();

			_itemModel = new ItemModel();

			FillGrid();
		}
		
		private void FillGrid()
		{
			//_allItemView =_itemModel.GetAll().ConvertAll(new Converter<Item, ItemView>(Converters.ItemToItemView));
			_allItemView = new ObservableCollection<ItemView>(_itemModel.GetAll().ConvertAll(new Converter<Item, ItemView>(Converters.ItemToItemView)));

			// This works with both ObservableCollection and non-ObservableCollection.
			itemsGrid.ItemsSource = null;
			itemsGrid.ItemsSource = _allItemView;
		}

		#region Events
		private void Window_Mousedown(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();
			}
		}

		private void BtnMinimize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void ItemsGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if(itemsGrid.SelectedItem != null)
			{
				Item itemSelected = itemsGrid.SelectedItem as Item;
				txtSKUCode.Text = itemSelected.sku;
				txtDescription.Text = itemSelected.description;
				txtExistence.Text = itemSelected.existence.ToString();
				txtTax.Text = itemSelected.tax.ToString();
				txtUnitPrice.Text = itemSelected.unit_price.ToString();

				EnableTexts();
				txtSKUCode.IsEnabled = false;
				txtExistence.IsEnabled = false;

				btnSave.IsEnabled = true;
				btnDelete.IsEnabled = true;
				btnNew.IsEnabled = true;
				
			}
		}

		private void BtnNew_Click(object sender, RoutedEventArgs e)
		{
			itemsGrid.SelectedItem = null;

			EnableTexts();
			ClearTexts();

			btnNew.IsEnabled = false;
			btnSave.IsEnabled = true;
			btnDelete.IsEnabled = false;
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			Item itemToSave;
			IEnumerable<ItemView> queryFind;
			bool isNew;
			
			queryFind = _allItemView.Where(x => x.sku == txtSKUCode.Text);
			isNew = queryFind.Count() == 0;

			if (isNew)
			{
				itemToSave = new Item();
				itemToSave.sku = txtSKUCode.Text;
				itemToSave.description = txtDescription.Text;
				itemToSave.tax = string.IsNullOrWhiteSpace(txtTax.Text) ? 0 : Int64.Parse(txtTax.Text);
				itemToSave.unit_price = string.IsNullOrWhiteSpace(txtUnitPrice.Text) ? 0 : double.Parse(txtUnitPrice.Text);
				itemToSave.existence = string.IsNullOrWhiteSpace(txtExistence.Text) ? 0 : long.Parse(txtExistence.Text);
			}
			else
			{
				//itemToSave = itemsGrid.SelectedItem as Item;
				itemToSave = queryFind.First() as Item;

				// It is possible to update restricted fields(GUID, SKU...) but the Model(Item) ignores any change over those fields when updating/saving.
				itemToSave.description = txtDescription.Text;
				itemToSave.tax = string.IsNullOrWhiteSpace(txtTax.Text) ? 0 : Int64.Parse(txtTax.Text);
				itemToSave.unit_price = string.IsNullOrWhiteSpace(txtUnitPrice.Text) ? 0 : double.Parse(txtUnitPrice.Text);
			}

			try
			{
				_itemModel.Save(itemToSave);
				
				ClearTexts();
				DisableTexts();

				btnNew.IsEnabled = true;
				btnSave.IsEnabled = false;
				btnDelete.IsEnabled = false;

				if (isNew)
				{
					_allItemView.Add(Converters.ItemToItemView(itemToSave));
				}
				else
				{
					itemsGrid.Items.Refresh();      // This only works with ObservableCollection and is useful when updating the data.
					itemsGrid.SelectedItem = null;  // To allow re-selecction of same Item after updating.
				}
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult Result = MessageBox.Show("Confirme que desea eliminar el SKU: " + txtSKUCode.Text, "Confirmación requerida", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (Result == MessageBoxResult.Yes)
			{
				try
				{
					_itemModel.Delete(txtSKUCode.Text);
					_allItemView.Remove(_allItemView.Where(x => x.sku == txtSKUCode.Text).First());

					ClearTexts();
					DisableTexts();

					btnNew.IsEnabled = true;
					btnSave.IsEnabled = false;
					btnDelete.IsEnabled = false;
				}
				catch (ArgumentException ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void BtnRefresh_Click(object sender, RoutedEventArgs e)
		{
			FillGrid();
		}
		#endregion

		#region Validators
		public void OnlyPositiveIntegersValidator(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		public void OnlyPositiveNumbersValidator(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9.]+");
			e.Handled = regex.IsMatch(e.Text);
		}
		#endregion

		#region Helpers
		private void ClearTexts()
		{
			txtSKUCode.Text = "";
			txtDescription.Text = "";
			txtExistence.Text = "0";
			txtTax.Text = "0";
			txtUnitPrice.Text = "0.0";
		}

		private void EnableTexts()
		{
			txtSKUCode.IsEnabled = true;
			txtDescription.IsEnabled = true;
			txtExistence.IsEnabled = true;
			txtTax.IsEnabled = true;
			txtUnitPrice.IsEnabled = true;
		}

		private void DisableTexts()
		{
			txtSKUCode.IsEnabled = false;
			txtDescription.IsEnabled = false;
			txtExistence.IsEnabled = false;
			txtTax.IsEnabled = false;
			txtUnitPrice.IsEnabled = false;
		}
		#endregion
	}
}
