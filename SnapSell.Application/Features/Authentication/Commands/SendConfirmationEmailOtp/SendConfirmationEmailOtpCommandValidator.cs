﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Authentication.Commands.SendConfirmationEmailOtp
{
    public class SendConfirmationEmailOtpCommandValidator:AbstractValidator<SendConfirmationEmailOtpCommand>
    {
        public SendConfirmationEmailOtpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
