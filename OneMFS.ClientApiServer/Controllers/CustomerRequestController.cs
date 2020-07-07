﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.ClientService.Models;
using MFS.ClientService.Service;
using MFS.SecurityService.Models;
using MFS.SecurityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneMFS.ClientApiServer.Controllers
{
	[Authorize]
    [Produces("application/json")]
    [Route("api/CustomerRequest")]
    public class CustomerRequestController : Controller
    {
        public ICustomerReqLogService customerReqLogService;
        public ICustomerRequestService customerRequestService;
		public IAuditTrailService auditTrailService;
		private IErrorLogService errorLogService;
		public CustomerRequestController(IErrorLogService _errorLogService, IAuditTrailService _auditTrailService, ICustomerReqLogService _customerReqLogService, ICustomerRequestService _customerRequestService)
        {
			auditTrailService = _auditTrailService;
            customerReqLogService = _customerReqLogService;
            customerRequestService = _customerRequestService;
			errorLogService = _errorLogService;
        }

        [HttpPost]
        [Route("Save")]
        public object Save([FromBody]CustomerRequest model)
        {
            try
            {
                if (model.ReqDate != null)
                {
                    switch (model.Status)
                    {
                        case "C":
                            customerReqLogService.updateRequestLog(model);
                            break;
						case "P":
							customerReqLogService.updateRequestLog(model);
							break;
						case "O":
							customerReqLogService.updateRequestLog(model);
							break;
						case "Y":
                            customerReqLogService.deleteRequestLog(model);
							model.CheckedBy = model.HandledBy;
                            customerRequestService.Add(model);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    CustomerReqLog reqModel = new CustomerReqLog()
                    {
                        ReqDate = DateTime.Now,
                        CheckedBy = model.CheckedBy,
                        Mphone = model.Mphone,
                        Remarks = model.Remarks,
                        Status = model.Status,
                        Request = model.Request,
						Gid = Guid.NewGuid().ToString()

				};
					AuditTrail auditTrail = new AuditTrail();
					auditTrail.Who = model.CheckedBy;
					auditTrail.WhatActionId = 3;
					auditTrail.WhichParentMenuId = 2;
					auditTrail.WhichMenu = "Client Profile";
					auditTrail.InputFeildAndValue = auditTrailService.GetAuditTrialFeildBySingleObject(model);
					auditTrailService.InsertIntoAuditTrail(auditTrail);
					return customerReqLogService.Add(reqModel);
                }
                return model;
            }
            catch (Exception ex)
            {
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}

        [HttpGet]
        [Route("GetCustomerRequestHistory")]
        public object GetCustomerRequestHistory(string status, string mphone = null)
        {
            try
            {
                switch (status)
                {
                    case "Pending":
                        if (!string.IsNullOrWhiteSpace(mphone))
                        {
                            return customerReqLogService.GetAllByCustomField(mphone, "mphone", new CustomerReqLog());
                        }
                        else 
                        {
                            return customerReqLogService.GetAll(new MFS.ClientService.Models.CustomerReqLog());
                        }
                    case "Resolved":
                        if (!string.IsNullOrWhiteSpace(mphone))
                        {
                            return customerRequestService.GetAllByCustomField(mphone, "mphone", new CustomerRequest());
                        }
                        else
                        {
                            return customerRequestService.GetAll(new CustomerRequest());
                        }
					case "OnProcess":
						if (!string.IsNullOrWhiteSpace(mphone))
						{
							return customerReqLogService.GetAllOnProcessRequestByCustomer(mphone);
						}
						else
						{
							return customerReqLogService.GetAll(new CustomerReqLog());
						}
					default:
                        return null;
                }
            }
            catch (Exception ex)
            {
				return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
			}
		}
    }
}