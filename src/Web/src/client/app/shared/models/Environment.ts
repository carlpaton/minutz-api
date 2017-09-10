export class Environment {
    public DestinationServerUrl: string;
    public DestinationServerApikey: string;
    public Id: string;
    public Name: string;
    public Description: string;
    public SortOrder: number;
    public UseGuidedFailure: boolean;
    constructor() {
        this.DestinationServerApikey = '';
        this.DestinationServerUrl = '';
        this.Id = '';
        this.Name = '';
        this.Description = '';
        this.SortOrder = 0;
        this.UseGuidedFailure = false;
    }
}
