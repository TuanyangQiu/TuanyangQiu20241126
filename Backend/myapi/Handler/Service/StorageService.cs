
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Handler.Service
{
	public class StorageService<T> : IStorage<T>
	{
		private readonly string _filePath;
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

		public StorageService(IOptions<StorageOptions<T>> options)
		{
			_filePath = options.Value.FilePath;

			if (!string.IsNullOrWhiteSpace(_filePath))
			{
				var directory = Path.GetDirectoryName(_filePath);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
			}
		}


		public async Task<List<T>?> ReadAsync()
		{
			await _semaphore.WaitAsync();
			try
			{
				if (File.Exists(_filePath))
				{
					var existingData = await File.ReadAllTextAsync(_filePath);
					return JsonSerializer.Deserialize<List<T>>(existingData) ?? new List<T>();
				}
				return null;
			}
			finally
			{
				_semaphore.Release();
			}
		}

		//no lock
		private async Task<List<T>?> InternalReadAsync()
		{

			if (File.Exists(_filePath))
			{
				var existingData = await File.ReadAllTextAsync(_filePath);
				return JsonSerializer.Deserialize<List<T>>(existingData) ?? new List<T>();
			}
			return null;

		}


		public async Task WriteAsync(T items)
		{
			await _semaphore.WaitAsync();
			try
			{
				var existData = await InternalReadAsync() ?? new List<T>();
				existData.Add(items);
				var jsonData = JsonSerializer.Serialize(existData, new JsonSerializerOptions { WriteIndented = true });
				await File.WriteAllTextAsync(_filePath, jsonData);
			}
			finally
			{
				_semaphore.Release();
			}
		}
	}
}
