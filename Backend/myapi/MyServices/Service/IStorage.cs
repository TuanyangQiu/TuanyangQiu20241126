namespace myapi.Service
{
	public interface IStorage<T>
	{
		Task<List<T>?> ReadAsync();
		Task WriteAsync(T items);
	}
}
