using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TuriStore.Entities
{
	internal class Converters
	{
		// Info: https://learn.microsoft.com/es-es/dotnet/api/system.collections.generic.list-1.convertall?view=net-8.0
		public static ItemView ItemToItemView(Item source)
		{
			ItemView target;
			PropertyInfo[] sourceProperties;

			sourceProperties = source.GetType().GetProperties();
			target = new ItemView();

			// Loop over the Item props and set them to the ItemView (an extended class have the same or more properties/methods than a derived one).
			foreach (PropertyInfo sourceProperty in sourceProperties)
			{
				PropertyInfo targetProperty = target.GetType().GetProperty(sourceProperty.Name);

				if (targetProperty.CanWrite)
					targetProperty.SetValue(target, sourceProperty.GetValue(source));
			}

			return target;
		}
	}
}
