using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TuriStore.Entities
{
	internal class ItemView : Item, INotifyPropertyChanged
	{
		private double _sale_price;
		private double _total;

		public double sale_price
		{
			// For this method name, the database naming convention was used to preserve consistency with the base class.
			get { return _sale_price; }
		}

		public double total
		{
			// For this method name, the database naming convention was used to preserve consistency with the base class.
			get { return _total; }
		}

		public override Int64 tax
		{
			get
			{
				return base.tax;
			}
			set
			{
				base.tax = value;

				_sale_price = base.unit_price;

				if (tax > 0)
					_sale_price += Convert.ToDouble(base.tax) / 100 * base.unit_price;

				_total = _sale_price * base.existence;
			}
		}

		public override double unit_price
		{
			get
			{
				return base.unit_price;
			}
			set
			{
				_sale_price = base.unit_price = value;

				if (tax > 0)
					_sale_price += Convert.ToDouble(base.tax) / 100 * base.unit_price;

				_total = _sale_price * base.existence;
			}
		}

		public override Int64 existence
		{
			get
			{
				return base.existence;
			}
			set
			{
				base.existence = value;
				_total = _sale_price * base.existence;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
