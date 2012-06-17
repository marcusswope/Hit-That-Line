namespace HitThatLine.Infrastructure
{
    public interface IValidatedCommand
    {
        object TransferToOnFailed { get; }
    }
}