namespace HitThatLine.Infrastructure.Validation
{
    public interface IValidatedCommand
    {
        object TransferToOnFailed { get; }
    }
}