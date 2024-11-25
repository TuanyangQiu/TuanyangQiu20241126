

using Handler.Service;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace TestProject
{
	public class StorageServiceTests :IDisposable
	{
		private class TestData
		{
			public int Id { get; set; }
			public string Name { get; set; } = string.Empty;
		}

		private IOptions<StorageOptions<TestData>> GetOptions(string filePath)
		{
			return Options.Create(new StorageOptions<TestData> { FilePath = filePath });
		}

		private string GetTestJsonFilePath() {

			return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
		}
		[Fact]
		public async Task ReadAsync_FileDoesNotExist_ReturnsNull()
		{
			var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

			var storageService = new StorageService<TestData>(GetOptions(tempFilePath));

			var result = await storageService.ReadAsync();

			Assert.Null(result);
		}


		[Fact]
		public async Task WriteAsync_WritesDataToFile()
		{
			var tempFilePath = GetTestJsonFilePath();

			var storageService = new StorageService<TestData>(GetOptions(tempFilePath));

			var testData = new TestData { Id = 1, Name = "Test" };

			await storageService.WriteAsync(testData);

 
			var fileContent = await File.ReadAllTextAsync(tempFilePath);
			var dataList = JsonSerializer.Deserialize<List<TestData>>(fileContent);

			Assert.NotNull(dataList);
			Assert.Single(dataList);
			Assert.Equal(testData.Id, dataList[0].Id);
			Assert.Equal(testData.Name, dataList[0].Name);
		}


		[Fact]
		public async Task ReadAsync_AfterWrite_ReturnsData()
		{
			var tempFilePath = GetTestJsonFilePath();

			var storageService = new StorageService<TestData>(GetOptions(tempFilePath));

			var testData = new TestData { Id = 1, Name = "Test" };

			await storageService.WriteAsync(testData);

			var result = await storageService.ReadAsync();

			Assert.NotNull(result);
			Assert.Single(result);
			Assert.Equal(testData.Id, result[0].Id);
			Assert.Equal(testData.Name, result[0].Name);
		}

		[Fact]
		public async Task WriteAsync_MultipleItems_AppendsData()
		{
			var tempFilePath = GetTestJsonFilePath();

			var storageService = new StorageService<TestData>(GetOptions(tempFilePath));

			var testData1 = new TestData { Id = 1, Name = "Test1" };
			var testData2 = new TestData { Id = 2, Name = "Test2" };

			await storageService.WriteAsync(testData1);
			await storageService.WriteAsync(testData2);

			var result = await storageService.ReadAsync();

			Assert.NotNull(result);
			Assert.Equal(2, result.Count);
			Assert.Equal(testData1.Id, result[0].Id);
			Assert.Equal(testData2.Id, result[1].Id);
		}
		[Fact]
		public async Task ConcurrentAccess_DoesNotCauseDataLoss()
		{
			var tempFilePath = GetTestJsonFilePath();

			var storageService = new StorageService<TestData>(GetOptions(tempFilePath));

			var tasks = new List<Task>();

			for (int i = 0; i < 10; i++)
			{
				var testData = new TestData { Id = i, Name = $"Test{i}" };
				tasks.Add(storageService.WriteAsync(testData));
			}

			await Task.WhenAll(tasks);

			var result = await storageService.ReadAsync();

			Assert.NotNull(result);
			Assert.Equal(10, result.Count);
			for (int i = 0; i < 10; i++)
			{
				Assert.Contains(result, x => x.Id == i && x.Name == $"Test{i}");
			}
		}

		public void Dispose()
		{
			// Clean up the temp files here. do it later...
		}
	}
}