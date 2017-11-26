namespace Interface.Services
{
    public interface INotify
    {
        string NotifyKey { get; }
        string NotifyDefaultTemplateKey { get; }
        string NotifyUser{ get;}
    }
}