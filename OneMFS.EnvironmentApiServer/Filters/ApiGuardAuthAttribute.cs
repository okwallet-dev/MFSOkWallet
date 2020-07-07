﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneMFS.DistributionApiServer.Filters
{
	
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ApiGuardAuthAttribute : Attribute, IAsyncActionFilter
	{
		private const string ApiKeyHeaderName = "ApiKey";

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
			var apiKey = configuration.GetValue<string>("okwallet");

			if (!apiKey.Equals(potentialApiKey))
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			await next();
		}
	}
}
