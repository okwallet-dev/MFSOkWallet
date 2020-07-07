﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.DistributionApiServer.Filters;

namespace OneMFS.DistributionApiServer.Controllers
{
	[Authorize]
	[Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
	    private ICustomerSevice _customerSevice;
		private IErrorLogService errorLogService;
		public CustomerController(ICustomerSevice customerSevice, IErrorLogService _errorLogService)
	    {
		    this._customerSevice = customerSevice;
			this.errorLogService = _errorLogService;
	    }
		[HttpGet]
		[Route("GetCustomerGridList")]
	    public object GetCustomerGridList()
		{
			try
			{
				return _customerSevice.GetCustomerGridList();
			}
			catch(Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}			
		}
		[ApiGuardAuth]
		[HttpPost]
		[Route("SaveCustomer")]
	    public object SaveCustomer(bool isEdit, string evnt,[FromBody] Reginfo aReginfo)
		{
			try
			{
				return _customerSevice.SaveCustomer(aReginfo, isEdit, evnt);
			}
			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}

		}
		[ApiGuardAuth]
		[HttpGet]
		[Route("getCustomerByMphone")]
	    public object GetCustomerByMphone(string mPhone)
		{
			try
			{
				return _customerSevice.GetCustomerByMphone(mPhone);
			}

			catch (Exception ex)
			{
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());

			}
		}
	}
}