using TransactionManager.Components.Models;
using System.Threading.Tasks;

namespace TransactionManager.Components.Validators
{
    public interface IValidator<in T> where T: class
    {
        Task<ValidationResult> Validate(T target);
    }
}
