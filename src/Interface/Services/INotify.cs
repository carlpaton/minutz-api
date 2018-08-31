namespace Interface.Services
{
    public interface INotify
    {
        string NotifyKey { get; }
        string NotifyDefaultTemplateKey { get; }
        string NotifyInvitationAddress { get; }
        string NotifyMinutesAddress { get; }
        string NotifyUser{ get;}
        string DestinationBaseAddress { get; }
    }
}