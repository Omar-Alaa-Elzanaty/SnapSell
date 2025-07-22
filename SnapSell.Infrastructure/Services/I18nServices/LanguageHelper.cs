using Microsoft.AspNetCore.Http;
using SnapSell.Application.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Infrastructure.Services.I18nServices
{
    public class LanguageHelper : ILanguageHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public LanguageHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetLang()
        {
            var lang = _contextAccessor.HttpContext.Request.Headers.AcceptLanguage.FirstOrDefault();

            if (lang == null)
            {
                lang = "en";
            }

            return lang;
        }
    }
}
