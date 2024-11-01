using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Tools
{
    public static class ModelValidator
    {
        public static bool Validate<T>(T obj)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }
    }
}
