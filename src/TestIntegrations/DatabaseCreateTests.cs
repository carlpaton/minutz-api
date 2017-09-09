using Interface.Repositories;
using Interface.Services;
using SqlRepository;
using System;
using Xunit;

namespace TestIntegrations
{
  public class DatabaseCreateTests
  {
    private readonly IApplicationSetupRepository _applicationSetupRepository;
    private readonly string _connectionString;
    private readonly string _falseConnectionString;
    public DatabaseCreateTests()
    {
      _applicationSetupRepository = new ApplicationSetupRepository();
      _connectionString = "Server=tcp:127.0.0.1,1433;User ID=sa;pwd=password1234$;database=minutz;";
      _falseConnectionString = "Server=tcp:127.0.0.1,1433;User ID=sa;pwd=password234$;database=minutz;";
    }

    [Fact]
    public void InstallDatabase_CheckConnectivity_GivenNoConnectionString_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.Exists(null));
    }

    [Fact]
    public void InstallDatabase_CheckConnectivity_GivenEmptyConnectionString_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.Exists(string.Empty));
    }

    [Fact]
    public void InstallDatabase_CheckConnectivity_GivenConnectionStringWithFalseDetails_ShouldReturnFalse()
    {
      var result = _applicationSetupRepository.Exists(_falseConnectionString);
      Assert.False(result);
    }

    [Fact]
    public void InstallDatabase_CheckConnectivity_GivenConnectionStringWithValidDetails_ShouldReturnTrue()
    {
      var result = _applicationSetupRepository.Exists(_connectionString);
      Assert.True(result);
    }

    [Fact]
    public void InstallDatabase_CreateApplicationCatalogue_GivenEmptyConnectionStringAndEmptyCaltalogueName_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.CreateApplicationCatalogue(string.Empty, string.Empty));
    }

    [Fact]
    public void InstallDatabase_CreateApplicationCatalogue_GivenConnectionStringAndCaltalogueName_ShouldThrowException()
    {
      var result = _applicationSetupRepository.CreateApplicationCatalogue(_connectionString, "minutztest");
      Assert.True(result);
    }

    [Fact]
    public void InstallDatabase_CreateApplicationSchema_GivenEmptyConnectionStringAndEmptyCaltalogueNameAndSchema_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.CreateApplicationSchema(string.Empty, string.Empty, string.Empty));
    }

    [Fact]
    public void InstallDatabase_CreateApplicationSchema_GivenConnectionStringAndCaltalogueNameAndSchema_ShouldThrowException()
    {
      var result = _applicationSetupRepository.CreateApplicationSchema(_connectionString, "minutztest", "app");
      Assert.True(result);
    }

    [Fact]
    public void InstallDatabase_CreateApplicationInstance_GivenEmptyConnectionStringAndEmptyCaltalogueNameAndSchema_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.CreateApplicationInstance(string.Empty, string.Empty, string.Empty));
    }

    [Fact]
    public void InstallDatabase_CreateApplicationInstance_GivenConnectionStringAndCaltalogueNameAndSchema_ShouldThrowException()
    {
      var result = _applicationSetupRepository.CreateApplicationInstance(_connectionString, "minutztest", "app");
      Assert.True(result);
    }

    [Fact]
    public void InstallDatabase_CreateApplicationPerson_GivenEmptyConnectionStringAndEmptyCaltalogueNameAndSchema_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.CreateApplicationPerson(string.Empty, string.Empty, string.Empty));
    }

    [Fact]
    public void InstallDatabase_CreateApplicationPerson_GivenConnectionStringAndCaltalogueNameAndSchema_ShouldThrowException()
    {
      var result = _applicationSetupRepository.CreateApplicationPerson(_connectionString, "minutztest", "app");
      Assert.True(result);
    }
  }
}
