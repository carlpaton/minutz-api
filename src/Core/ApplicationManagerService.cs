using Interface.Repositories;
using Interface.Services;

namespace Core
{
  public class ApplicationManagerService
  {
    private readonly IApplicationSetupRepository _applicationSetupRepository;
    private readonly IConnectionStringBuilder _connectionStringBuilder;
    private readonly IApplicationSetting _applicationSetting;
    public ApplicationManagerService(IApplicationSetupRepository applicationSetupRepository, 
                                     IConnectionStringBuilder connectionStringBuilder, 
                                     IApplicationSetting applicationSetting)
    {
    }
  }
}


//check exists
//create db
//create schema

//create instance table
//create person table
// sp for instanceuser


//create meeting table
//create 