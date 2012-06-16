namespace HitThatLine.Web.Infrastructure
{
    public interface IValidatedCommand
    {
        object TransferToOnFailed { get; }
    }
}