namespace EstoqueNutris.Interfaces
{
    public interface IValidatable
    {
        bool IsValid();
        IEnumerable<string> GetValidationErrors();
    }
} 