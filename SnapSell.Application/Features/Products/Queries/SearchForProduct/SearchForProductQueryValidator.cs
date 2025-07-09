using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Products.Queries.SearchForProduct
{
    public class SearchForProductQueryValidator:AbstractValidator<SearchForProductQuery>
    {
        public SearchForProductQueryValidator()
        {
            RuleFor(x => x.MinPrice).GreaterThan(9);
            RuleFor(x => x.Filter).IsInEnum();
        }
    }
}
