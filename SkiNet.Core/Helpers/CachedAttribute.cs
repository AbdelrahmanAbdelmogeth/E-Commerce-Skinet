using ECommerceSkinet.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSkinet.Core.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService =
                context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            // generate cache key
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            // check if cache key exists
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                // return cached response
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            // continue with the action
            var executedContext = await next();
            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                // cache the response
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
