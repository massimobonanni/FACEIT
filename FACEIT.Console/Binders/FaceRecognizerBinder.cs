using FACEIT.Core.Interfaces;
using FACEIT.FaceService.Implementations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Console.Binders
{
    internal class FaceRecognizerBinder : BinderBase<IFaceRecognizer>
    {
        private readonly Option<string> _endpointOption;
        private readonly Option<string> _apiKeyOption;

        public FaceRecognizerBinder(Option<string> endpointOption, Option<string> apiKeyOption)
        {
            _endpointOption = endpointOption;
            _apiKeyOption = apiKeyOption;
        }

        protected override IFaceRecognizer GetBoundValue(BindingContext bindingContext)
        {
            var endpoint = bindingContext.ParseResult.GetValueForOption(_endpointOption);
            var apiKey = bindingContext.ParseResult.GetValueForOption(_apiKeyOption);
            var httpClient = new HttpClient();

            using ILoggerFactory loggerFactory = LoggerFactory.Create(
                builder => builder.AddConsole());
            ILogger<FacesManager> logger = loggerFactory.CreateLogger<FacesManager>();

            return new FacesManager(httpClient, endpoint, apiKey, logger);
        }
    }
}
