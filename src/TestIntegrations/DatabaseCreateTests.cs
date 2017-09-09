using Interface.Repositories;
using SqlRepository;
using System;
using Xunit;

namespace TestIntegrations
{
  public class DatabaseCreateTests
  {
    private readonly IApplicationSetupRepository _applicationSetupRepository;
    public DatabaseCreateTests()
    {
      _applicationSetupRepository = new ApplicationSetupRepository();
    }

    [Fact]
    public void InstallDatabase_CheckConnectivity_GivenNoConenctionString_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.Exists(null));
    }

    [Fact]
    public void InstallDatabase_CheckConnectivity_GivenEmptyConenctionString_ShouldThrowException()
    {
      Exception ex = Assert.Throws<ArgumentNullException>(() => _applicationSetupRepository.Exists(string.Empty));
    }
  }
}
