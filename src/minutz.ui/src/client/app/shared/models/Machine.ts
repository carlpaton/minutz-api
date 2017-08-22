export class Machine  {
    public DestinationServerUrl: string;;
    public DestinationServerApikey: string;
    public Id: string;
    public Name: string;
    public Thumbprint: string;
    public Uri : string;
    public IsDisabled : boolean;
    public EnvironmentIds : string [];
    public EnvironmentName : string;
    public Roles : string[];
    public MachinePolicyId : string;
    public TenantIds : string[];
    public TenantTags : string[];
    public Status : string;
    public HealthStatus : string;
    public HasLatestCalamari : boolean;
    public StatusSummary : string;
    public IsInProcess : boolean;
   constructor() {
     this.DestinationServerApikey = '';
     this.DestinationServerUrl = '';
     this.Id = '';
     this.Name = '';
     this.Thumbprint = '';
     this.Uri = '';
     this.MachinePolicyId = '';
     this.Status = '';
     this.HealthStatus = '';
     this.StatusSummary = '';
   }
 }
