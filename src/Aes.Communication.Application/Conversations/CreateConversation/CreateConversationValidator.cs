using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations.AddMessage;
using Aes.Communication.Application.Validation;
using Aes.Communication.Domain.Messages;
using FluentValidation;

namespace Aes.Communication.Application.Conversations.CreateConversation
{
    public class CreateConversationValidator : AbstractValidator<CreateConversationRequest>
    {
        public CreateConversationValidator()
        {
            RuleFor(x => x.Subject).NotEmpty().WithErrorCode("1001").WithMessage("required");
            RuleFor(x => x.Subject.Id).NotEmpty().When(x=>x.Subject!=null).WithErrorCode("1002").WithMessage("required");
            RuleFor(x => x.Subject.Type).Must(ValidatorHelpers.IsValidEntityType).When(x => x.Subject != null).WithErrorCode("1003").WithMessage("invalid type");
            RuleFor(x => x.CreatedByUserId).NotEqual(0).WithErrorCode("1004").OverridePropertyName("UserId").WithMessage("required and not zero");
            RuleFor(x => x.OrganizationId).NotEqual(0).WithErrorCode("1005").WithMessage("required and not zero");
            RuleFor(x => x.CounterpartyId).NotEqual(0).WithErrorCode("1006").WithMessage("required and not zero");
            RuleFor(x => x.Title).NotEmpty().WithErrorCode("1007").WithMessage("title required");
            RuleForEach(x => x.Messages).SetValidator(new AddConversationMessageValidator());
        }

    }

   

}
