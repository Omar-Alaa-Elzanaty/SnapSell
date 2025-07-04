using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Features.Products.Queries.SearchForProductVideos
{
    public class SearchForProductVideosQueryValidator:AbstractValidator<SearchForProductVideosQuery>
    {
        public SearchForProductVideosQueryValidator()
        {
            RuleFor(x => x.MinPrice).GreaterThan(9);
            RuleFor(x => x.Filter).IsInEnum();
        }
    }
}
