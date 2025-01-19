namespace SegurosAgricolas.Domain.Validator
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; }

        public ValidationResult()
        {
            Errors = new List<string>();
        }

        public ValidationResult(IEnumerable<string> errors)
        {
            Errors = new List<string>(errors);
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}