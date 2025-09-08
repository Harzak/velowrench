using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using velowrench.Repository.Services;
using velowrench.Utils.Results;

namespace velowrench.Repository.Tests.Services;

[TestClass]
public class JsonStorageTests
{
    private ILogger<JsonStorage<TestData>> _logger;
    private string _testFileName;
    private string _testDirectory;
    private string _testFilePath;
    private JsonStorage<TestData> _jsonStorage;
    private readonly JsonSerializerOptions _jsonOptions;

    public TestContext TestContext { get; set; }

    public JsonStorageTests(TestContext context)
    {
        this.TestContext = context;
        _jsonOptions  = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [TestInitialize]
    public void Initialize()
    {
        _logger = A.Fake<ILogger<JsonStorage<TestData>>>();
        _testFileName = "test.json";
        _testDirectory = Path.Combine(Path.GetTempPath(), "velowrench-tests", Guid.NewGuid().ToString());
        _testFilePath = Path.Combine(_testDirectory, _testFileName);

        _jsonStorage = new JsonStorage<TestData>(_testDirectory, _testFileName, _logger);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }

        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [TestMethod]
    public async Task SaveAsync_WithValidData_ShouldCreateFileAndSaveData()
    {
        // Arrange
        TestData testData = new()
        {
            Id = 1,
            Name = "Test",
            Value = 42.5
        };

        // Act
        await _jsonStorage.SaveAsync(testData).ConfigureAwait(false);

        // Assert
        File.Exists(_testFilePath).Should().BeTrue();

        string jsonContent = await File.ReadAllTextAsync(_testFilePath, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);
        jsonContent.Should().NotBeNullOrWhiteSpace();

        TestData? deserializedData = JsonSerializer.Deserialize<TestData>(jsonContent, _jsonOptions);
        deserializedData.Should().NotBeNull();
        deserializedData.Id.Should().Be(testData.Id);
        deserializedData.Name.Should().Be(testData.Name);
        deserializedData.Value.Should().Be(testData.Value);
    }

    [TestMethod]
    public async Task SaveAsync_WhenDirectoryDoesNotExist_ShouldCreateDirectoryAndSaveData()
    {
        // Arrange
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }

        TestData testData = new()
        {
            Id = 2,
            Name = "DirectoryTest",
            Value = 100.0
        };

        // Act
        await _jsonStorage.SaveAsync(testData).ConfigureAwait(false);

        // Assert
        Directory.Exists(_testDirectory).Should().BeTrue();
        File.Exists(_testFilePath).Should().BeTrue();

        string jsonContent = await File.ReadAllTextAsync(_testFilePath, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);
        TestData? deserializedData = JsonSerializer.Deserialize<TestData>(jsonContent, _jsonOptions);
        deserializedData.Should().NotBeNull();
        deserializedData.Id.Should().Be(testData.Id);
    }

    [TestMethod]
    public async Task LoadAsync_WhenFileDoesNotExist_ShouldReturnFailureResult()
    {
        // Arrange & Act
        OperationResult<TestData> result = await _jsonStorage.LoadAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task LoadAsync_WhenFileIsEmpty_ShouldReturnFailureResult()
    {
        // Arrange
        Directory.CreateDirectory(_testDirectory);
        await File.WriteAllTextAsync(_testFilePath, string.Empty, TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Act
        OperationResult<TestData> result = await _jsonStorage.LoadAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task LoadAsync_WithValidData_ShouldReturnSuccessResultWithData()
    {
        // Arrange
        TestData originalData = new()
        {
            Id = 3,
            Name = "LoadTest",
            Value = 75.25
        };
        await _jsonStorage.SaveAsync(originalData).ConfigureAwait(false);

        // Act
        OperationResult<TestData> result = await _jsonStorage.LoadAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.Id.Should().Be(originalData.Id);
        result.Content.Name.Should().Be(originalData.Name);
        result.Content.Value.Should().Be(originalData.Value);
    }

    [TestMethod]
    public async Task LoadAsync_WithInvalidJson_ShouldReturnFailureResultWithError()
    {
        // Arrange
        Directory.CreateDirectory(_testDirectory);
        await File.WriteAllTextAsync(_testFilePath, "{ invalid json }", TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Act
        OperationResult<TestData> result = await _jsonStorage.LoadAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.HasContent.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        result.ErrorMessage.Should().Contain("Failed to load data from");
    }

    [TestMethod]
    public async Task LoadAsync_WithNullJsonValue_ShouldReturnSuccessResultWithoutContent()
    {
        // Arrange
        Directory.CreateDirectory(_testDirectory);
        await File.WriteAllTextAsync(_testFilePath, "null", TestContext.CancellationTokenSource.Token).ConfigureAwait(false);

        // Act
        OperationResult<TestData> result = await _jsonStorage.LoadAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task SaveAndLoadCycle_WithComplexData_ShouldPreserveAllData()
    {
        // Arrange
        TestData originalData = new()
        {
            Id = 999,
            Name = "Complex Test with Special Characters: הצü@#$%&*()[]{}",
            Value = -123.456789,
            CreatedDate = new DateTime(2024, 12, 25, 10, 30, 45, DateTimeKind.Utc),
            IsActive = true,
            Tags = ["tag1", "tag2", "special-tag"]
        };

        // Act
        await _jsonStorage.SaveAsync(originalData).ConfigureAwait(false);
        OperationResult<TestData> result = await _jsonStorage.LoadAsync().ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().BeEquivalentTo(originalData);
    }

    [TestMethod]
    public async Task SaveAsync_MultipleOperations_ShouldOverwritePreviousData()
    {
        // Arrange
        TestData firstData = new()
        {
            Id = 1,
            Name = "First",
            Value = 100.0
        };
        TestData secondData = new()
        {
            Id = 2,
            Name = "Second",
            Value = 200.0
        };

        // Act
        await _jsonStorage.SaveAsync(firstData).ConfigureAwait(false);
        await _jsonStorage.SaveAsync(secondData).ConfigureAwait(false);
        OperationResult<TestData> result = await _jsonStorage.LoadAsync().ConfigureAwait(false);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().BeEquivalentTo(secondData);
        result.Content.Should().NotBeEquivalentTo(firstData);
    }

    [TestMethod]
    public void FilePath_ShouldReturnCorrectPath()
    {
        // Act & Assert
        _jsonStorage.FilePath.Should().Be(_testFilePath);
    }

    [TestMethod]
    public void Exists_WhenFileDoesNotExist_ShouldReturnFalse()
    {
        // Act & Assert
        _jsonStorage.Exists.Should().BeFalse();
    }

    [TestMethod]
    public async Task Exists_WhenFileExists_ShouldReturnTrue()
    {
        // Arrange
        TestData testData = new() { Id = 1, Name = "Test", Value = 42.5 };
        await _jsonStorage.SaveAsync(testData).ConfigureAwait(false);

        // Act & Assert
        _jsonStorage.Exists.Should().BeTrue();
    }

    [TestMethod]
    public async Task Delete_WhenFileExists_ShouldDeleteFile()
    {
        // Arrange
        TestData testData = new() { Id = 1, Name = "Test", Value = 42.5 };
        await _jsonStorage.SaveAsync(testData).ConfigureAwait(false);
        _jsonStorage.Exists.Should().BeTrue();

        // Act
        _jsonStorage.Delete();

        // Assert
        _jsonStorage.Exists.Should().BeFalse();
        File.Exists(_testFilePath).Should().BeFalse();
    }

    [TestMethod]
    public void Delete_WhenFileDoesNotExist_ShouldNotThrowException()
    {
        // Act & Assert
        Action act = () => _jsonStorage.Delete();
        act.Should().NotThrow();
    }
}

public class TestData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public List<string> Tags { get; set; } = [];
}