using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations.AddMessage;
using Aes.Communication.Application.Validation;
using FluentValidation;

namespace Aes.Communication.Application.Conversations.AddMessageByConversationSubject
{
    public class AddMessageByConversationSubjectValidator : AbstractValidator<AddMessageByConversationSubjectRequest>
    {
        public AddMessageByConversationSubjectValidator()
        {
            RuleFor(x => x.ConversationSubject).NotEmpty().WithErrorCode("3001").WithMessage("required");
            RuleFor(x => x.ConversationSubject.Id).NotEmpty().When(x => x.ConversationSubject != null).WithErrorCode("3002").WithMessage("required");
            RuleFor(x => x.ConversationSubject.Type).Must(ValidatorHelpers.IsValidEntityType).When(x => x.ConversationSubject != null).WithErrorCode("3003").WithMessage("invalid type");

            RuleFor(x => x.Body).NotEmpty().WithErrorCode("2001").WithMessage("required");
            RuleFor(x => x.Organization).NotNull().WithErrorCode("2002").WithMessage("required");
            RuleFor(x => x.Organization.Id).NotEmpty().When(x => x.Organization != null).WithErrorCode("2003").WithMessage("required and not zero");
            RuleFor(x => x.User).NotNull().WithErrorCode("2004").WithMessage("required");
            RuleFor(x => x.User.UserId).NotEmpty().When(x => x.User != null).WithErrorCode("2005").WithMessage("required and not zero");
        }
    }
}
