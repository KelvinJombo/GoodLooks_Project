using Cart.Api.CQRS;
using Cart.Api.Data;
using Cart.Api.Models;
using FluentValidation;
using System.Windows.Input;

namespace Cart.Api.ShoppingCart.SaveCart
{

    public record SaveCartCommand(ShopCart Cart) : ICommand<SaveCartResult>;
    public record SaveCartResult(string UserName);

    public class SaveCartCommandValidator : AbstractValidator<SaveCartCommand>
    {
        public SaveCartCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class SaveCartCommandHandler(ICartRepository repository) : ICommandHandler<SaveCartCommand, SaveCartResult>
    {
        public async Task<SaveCartResult> Handle(SaveCartCommand command, CancellationToken cancellationToken)
        {
            ShopCart cart = command.Cart;

            await repository.StoreCart(command.Cart, cancellationToken);

            return new SaveCartResult(command.Cart.UserName);
        }
    }
}
