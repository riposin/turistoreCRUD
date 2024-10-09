using System.Linq;
using System.Collections.Generic;
using TuriStore.Entities;
using System;

namespace TuriStore.Models
{
	internal class ItemModel : Model
	{
		public ItemModel()
		{
			table = "items";
			primaryKey = "sku";
			returnType = typeof(Item);
		}

		public new List<Item> GetAll()
		{
			return base.GetAll().Cast<Item>().ToList();
		}

		public Item FindByPrimaryKey(string id)
		{
			return (Item)base.FindByPrimaryKey(id);
		}

		public bool Delete(string id)
		{
			Item itemToDelete = FindByPrimaryKey(id);
			if (itemToDelete == null)
			{
				// Consider to fire: System.Collections.Generic.KeyNotFoundException
				throw new ArgumentException("No se encontró el SKU para proceder con la eliminación");
			}
			if (itemToDelete.existence > 0)
			{
				throw new ArgumentException("No se permite eliminar cuando la existencia es mayor a cero");
			}
			return base.DeleteByPrimaryKey(id);
		}

		public string Save(Item item)
		{
			if (string.IsNullOrEmpty(item.sku))
			{
				throw new ArgumentException("El SKU debe definirse");
			}
			if (item.unit_price <= 0)
			{
				throw new ArgumentException("El precio unitario debe ser mayor a cero");
			}
			if (item.existence < 0)
			{
				throw new ArgumentException("La existencia no puede ser negativa");
			}

			Item itemToSave = FindByPrimaryKey(item.sku);
			if (itemToSave == null)
			{
				itemToSave = item;
				// Since it is a new Item, a GUID is needed.
				// Because it is a GUID(it is globally unique), the value is set when the the instance is created(check Item class).
				// itemToSave.guid = Guid.NewGuid().ToByteArray();
			}
			else
			{
				// Prevent the modification of restricted fields when the item already exists.
				itemToSave.description = item.description;
				itemToSave.tax = item.tax;
				itemToSave.unit_price = item.unit_price;
			}

			return base.Save(itemToSave);
		}
	}
}
