using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Aes.Communication.Application.Conversations.AddMessage
{
    public class AddConversationMessageValidator : AbstractValidator<AddConversationMessageRequest>
    {
        public AddConversationMessageValidator()
        {
            RuleFor(x=>x.Body).NotEmpty().WithErrorCode("2001").WithMessage("required");
            RuleFor(x => x.Organization).NotNull().WithErrorCode("2002").WithMessage("required");
            RuleFor(x => x.Organization.Id).NotEmpty().When(x => x.Organization != null).WithErrorCode("2003").WithMessage("required and not zero");
            RuleFor(x => x.User).NotNull().WithErrorCode("2004").WithMessage("required");
            RuleFor(x => x.User.UserId).NotEmpty().When(x => x.User != null).WithErrorCode("2005").WithMessage("required and not zero");
        }
    }
}
