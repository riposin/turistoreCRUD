using System;

namespace TuriStore.Entities
{
	internal class Item : Entity
	{
		public virtual byte[] guid { get; set; } = Guid.NewGuid().ToByteArray();
		public virtual string sku { get; set; } = string.Empty;
		public virtual string description { get; set; } = string.Empty;
		public virtual Int64 tax { get; set; } = 0;
		public virtual double unit_price { get; set; } = 0;
		public virtual Int64 existence { get; set; } = 0;
	}
}
