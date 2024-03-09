using FluentValidation;
using FluentValidation.Results;
using Learning.Business.Models;

namespace Learning.Business.Services
{
    public abstract class BaseService
    {
        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string message)
        {
            //Propagar esse erro até a camada de apresentacao
        }

        protected bool ExecutarValidacao<TV, TE>(TV validation, TE entity) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            Notificar(validator);
            return false;
        }
    }
}
