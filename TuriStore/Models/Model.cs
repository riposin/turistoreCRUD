using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TuriStore.Data;

namespace TuriStore.Models
{
	internal abstract class Model : DataBase
	{
		protected string table;
		protected Type returnType;
		protected string primaryKey;

		protected string Save(Object row)
		{
			PropertyInfo[] oReturnTypeProps;
			Dictionary<string, object> dataInsert;
			Dictionary <string, object> dataUpdate;
			object existingRow;
			PropertyInfo pkProperty;
			string pkValue;
			object originalValue, newValue;

			// Check if the pk already exists
			oReturnTypeProps = returnType.GetProperties();
			pkProperty = Array.Find(oReturnTypeProps, p => p.Name == primaryKey);
			pkValue = pkProperty.GetValue(row).ToString();
			existingRow = FindByPrimaryKey(pkValue);

			if(existingRow == null)
			{
				// Insert
				dataInsert = new Dictionary<string, object>();
				foreach (PropertyInfo prop in oReturnTypeProps)
				{
					dataInsert.Add(prop.Name, prop.GetValue(row));
				}
				Insert(table, dataInsert);
			}
			else
			{
				// Update
				dataUpdate = new Dictionary<string, object>();
				foreach (PropertyInfo prop in oReturnTypeProps)
				{
					if (prop.GetValue(row).GetType() == typeof(byte[]))
					{
						// This is because IDKW SQLite returns a Byte[36] instead of Byte[16], so the Guid constructor fails because it expects a 16-bit array.
						//newValue = new Guid((byte[])prop.GetValue(row)).ToString();
						newValue = Encoding.ASCII.GetString((byte[])prop.GetValue(row));
						originalValue = Encoding.ASCII.GetString((byte[])prop.GetValue(existingRow));
					}
					else
					{
						newValue = prop.GetValue(row).ToString();
						originalValue = prop.GetValue(existingRow).ToString();
					}

					if ((string)newValue != (string)originalValue)
					{
						dataUpdate.Add(prop.Name, prop.GetValue(row));
					}
				}
				if (dataUpdate.Count > 0)
				{
					Update(table, dataUpdate, primaryKey + "=\"" + pkValue + "\"");
				}
			}

			return pkValue;
		}

		protected List<object> GetAll()
		{
			List<Dictionary<string, object>> rows = Select(new List<string>(), table);

			return ConvertToReturnType(rows);
		}

		protected object FindByPrimaryKey(object id)
		{
			string pkID;
			List<Dictionary<string, object>> rows;
			List<object> result;

			if (id.GetType() == typeof(byte[]))
			{
				pkID = new Guid((byte[])id).ToString();
			}
			else
			{
				pkID = id.ToString();
			}

			rows = Select(new List<string>(), table, primaryKey + "=\"" + pkID + "\"");
			result = ConvertToReturnType(rows);

			return result.Count > 0 ? result[0] : null;
		}

		protected bool DeleteByPrimaryKey(object id)
		{
			string pkID;

			if (id.GetType() == typeof(byte[]))
			{
				pkID = new Guid((byte[])id).ToString();
			}
			else
			{
				pkID = id.ToString();
			}
			
			return Delete(table, primaryKey + "=\"" + pkID + "\"") > 0;
		}
		
		/// <summary>
		/// Manually convert each dictionary in the list to objects of the returnType Type.
		/// On the other hand this can be done y using the JsonConvert.DeserializeObject method from Newtonsoft.Json dll.
		/// </summary>
		/// <param name="rows">The rows that represent each record.</param>
		/// <returns>A list of objects of returnType Type.</returns>
		private List<object> ConvertToReturnType(List<Dictionary<string, object>> rows)
		{
			List<object> result;
			PropertyInfo[] returnTypeProps;
			object newEntity;

			result = new List<object>();

			if (rows.Count > 0)
			{
				returnTypeProps = returnType.GetProperties();

				foreach (Dictionary<string, object> row in rows)
				{
					newEntity = Activator.CreateInstance(returnType);

					foreach (PropertyInfo prop in returnTypeProps)
					{
						if (row.ContainsKey(prop.Name))
						{
							prop.SetValue(newEntity, row[prop.Name]);
						}
					}

					result.Add(newEntity);
				}
			}

			return result;
		}
	}
}
