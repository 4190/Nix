using FluentValidation;
using Shortener.Dtos;
using System.Text.RegularExpressions;
using System;

namespace Shortener.Validators
{
    public class ShortenedLinkValidator : AbstractValidator<ShortenedLinkDto>
    {
        public ShortenedLinkValidator()
        {
            RuleFor(x => x.OriginalLink).Custom((str, context) =>
            {
                string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
                Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                if(!Rgx.IsMatch(str))
                {
                    context.AddFailure("Please provide correct link to shorten");
                }
            });
        }
    }
}

