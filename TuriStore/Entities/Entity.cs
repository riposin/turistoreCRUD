namespace TuriStore.Entities
{
	/// <summary>
	/// Base class to map the first-class citizens against the repository.
	/// Each citizen class must inherit from this.
	/// Entity properties in the citizen must marked as virtual for flexibility in derived classes.
	/// Names of the properties of citizen entities must exactly match table column names, including case.
	/// In reference to the above comment, one must decide between the C# property naming convention or the database column naming convention.
	/// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names
	/// https://github.com/RootSoft/Database-Naming-Convention
	/// </summary>
	internal abstract class Entity
	{
	}
}
