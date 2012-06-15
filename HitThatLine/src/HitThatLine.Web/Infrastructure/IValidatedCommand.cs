namespace HitThatLine.Web.Infrastructure
{
    public interface IValidatedCommand
    {
        object TransferOnFailed { get; }
    }
}