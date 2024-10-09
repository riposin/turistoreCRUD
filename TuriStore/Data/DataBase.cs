using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace TuriStore.Data
{
	internal class DataBase
	{
		private readonly SQLiteConnection _conn;

		public DataBase() {
			_conn = new SQLiteConnection
			{
				ConnectionString = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "Data\\" + "turistore.sqlite3.db"
			};
		}

		/// <summary>
		/// Get records.
		/// </summary>
		/// <param name="fields">List of fields/columns to return.</param>
		/// <param name="table">Tale name.</param>
		/// <param name="condition">Pass the condition without the WHERE word.</param>
		/// <returns></returns>
		public List<Dictionary<string, object>> Select(List<string> fields, string table, string condition = "")
		{
			List<Dictionary<string, object>> result;
			SQLiteCommand cmd;
			SQLiteDataReader reader;
			Dictionary<string, object> record;
			string commandTxt;

			result = new List<Dictionary<string, object>>();
			commandTxt = "SELECT " + (fields.Count == 0 ? "*" : String.Join(", ", fields.ToArray())) + " FROM " + table + (condition != "" ? " WHERE " + condition : "") + ";";
			cmd = new SQLiteCommand(commandTxt, _conn);

			_conn.Open();
			reader = cmd.ExecuteReader();

			// Improvement: Validate that all fields actually exist as columns in the table (that is specific to each DBMS).
			while (reader.Read())
			{
				record = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
				{
					record.Add(reader.GetName(i), reader.GetValue(i));
				}
				result.Add(record);
			}
			_conn.Close();
			
			return result;
		}

		public void Insert(string table, Dictionary<string, object> fields)
		{
			SQLiteCommand cmd;
			string commandTxt;
			string columns = "";
			string values = "";

			foreach (KeyValuePair<string, object> kvp in fields)
			{
				columns += kvp.Key + ",";
				if (kvp.Value.GetType() == typeof(string))
				{
					values += "\"" + kvp.Value + "\",";
				}
				else if (kvp.Value.GetType() == typeof(byte[]))
				{
					values += "\"" + System.Text.Encoding.ASCII.GetString((byte[])kvp.Value) + "\",";
					//values += "\"" + new Guid((byte[])kvp.Value).ToString() + "\",";
				}
				else
				{
					values += kvp.Value + ",";
				}
			}

			commandTxt = "INSERT INTO " + table + "(" + columns.Substring(0, columns.Length - 1) + ") VALUES(" + values.Substring(0, values.Length - 1) + ");";
			cmd = new SQLiteCommand(commandTxt, _conn);
			_conn.Open();
			cmd.ExecuteNonQuery();
			_conn.Close();
		}
		public int Update(string table, Dictionary<string, object> fields, string condition = "")
		{
			int affectedRows;
			SQLiteCommand cmd;
			string commandTxt;
			string columnValue = "";

			foreach (KeyValuePair<string, object> kvp in fields)
			{
				columnValue += kvp.Key + "=";

				if (kvp.Value.GetType() == typeof(string))
				{
					columnValue += "\"" + kvp.Value + "\"";
				}
				else if (kvp.Value.GetType() == typeof(byte[]))
				{
					columnValue += "\"" + System.Text.Encoding.ASCII.GetString((byte[])kvp.Value) + "\",";
					//columnValue += "\"" + new Guid((byte[])kvp.Value).ToString() + "\"";
				}
				else
				{ 
					columnValue +=  kvp.Value;
				}
				columnValue += ",";
			}

			commandTxt = "UPDATE " + table + " SET " + columnValue.Substring(0, columnValue.Length - 1) + (condition != "" ? " WHERE " + condition : "") + ";";
			cmd = new SQLiteCommand(commandTxt, _conn);
			_conn.Open();
			cmd.ExecuteNonQuery();
			affectedRows = _conn.Changes;
			_conn.Close();

			return affectedRows;
		}

		public int Delete(string table, string condition = "")
		{
			SQLiteCommand cmd;
			string commandTxt;
			int affectedRows;

			commandTxt = "DELETE FROM " + table + (condition != "" ? " WHERE " + condition : "") + ";";

			cmd = new SQLiteCommand(commandTxt, _conn);
			_conn.Open();
			cmd.ExecuteNonQuery();
			affectedRows = _conn.Changes;
			_conn.Close();

			return affectedRows;
		}
	}
}
