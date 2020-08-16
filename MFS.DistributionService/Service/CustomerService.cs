﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MFS.CommunicationService.Service;
using MFS.DistributionService.Models;
using MFS.DistributionService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.Utility;

namespace MFS.DistributionService.Service
{
	public interface ICustomerSevice : IBaseService<Reginfo>
	{
		object GetCustomerGridList();
		object SaveCustomer(Reginfo aReginfo, bool isEdit, string evnt);
		object GetCustomerByMphone(string mPhone);
	}
	public class CustomerService : BaseService<Reginfo>, ICustomerSevice
	{
		private ICustomerRepository _customerRepository;
		private IKycService kycService;
		public CustomerService(ICustomerRepository customerRepository, IKycService  _kycService)
		{
			_customerRepository = customerRepository;
			kycService = _kycService;
		}

		public object GetCustomerGridList()
		{
			return _customerRepository.GetCustomerGridList();
		}

		public object SaveCustomer(Reginfo aReginfo, bool isEdit, string evnt)
		{
			int fourDigitRandomNo = new Random().Next(1000, 9999);
			try
			{
				if (isEdit != true)
				{
					aReginfo.CatId = "C";
					aReginfo.PinStatus = "N";
					aReginfo.AcTypeCode = 2;
					aReginfo.RegSource = "P";
					//aReginfo.RegDate = aReginfo.RegDate + DateTime.Now.TimeOfDay;

					try
					{
						_customerRepository.Add(aReginfo);
						kycService.UpdatePinNo(aReginfo.Mphone, fourDigitRandomNo.ToString());
						kycService.InsertModelToAuditTrail(aReginfo, aReginfo.EntryBy, 3, 3, "Customer",aReginfo.Mphone, "Save successfully");
						MessageService service = new MessageService();
						service.SendMessage(new MessageModel()
						{
							Mphone = aReginfo.Mphone,
							MessageId = "999",
							MessageBody = "Congratulations! Your OK wallet has been opened successfully." + " Your Pin is "
							+ fourDigitRandomNo.ToString() + ", please change PIN to activate your account, "
						});
					}
					catch (Exception ex)
					{

						throw ex;
					}

					return HttpStatusCode.OK;

				}
				else
				{
					if (evnt == "reject")
					{
						aReginfo.UpdateDate = System.DateTime.Now;
						aReginfo.RegStatus = "R";
						var prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
						_customerRepository.UpdateRegInfo(aReginfo);
						kycService.InsertUpdatedModelToAuditTrail(aReginfo, prevModel, aReginfo.UpdateBy, 3, 4, "Customer", aReginfo.Mphone, "Reject successfully");
						return HttpStatusCode.OK;
					}
					else if (evnt == "edit")
					{						
						aReginfo.UpdateDate = System.DateTime.Now;
						var prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
						_customerRepository.UpdateRegInfo(aReginfo);
						kycService.InsertUpdatedModelToAuditTrail(aReginfo, prevModel, aReginfo.UpdateBy, 3, 4, "Customer",aReginfo.Mphone, "Update successfully");				
						return HttpStatusCode.OK;

					}
					else
					{
						var checkStatus = kycService.CheckPinStatus(aReginfo.Mphone);

						if (checkStatus.ToString() != "P")
						{
							aReginfo.RegStatus = "P";
							aReginfo.AuthoDate = System.DateTime.Now;
							//aReginfo.RegDate = kycService.GetRegDataByMphoneCatID(aReginfo.Mphone, "C");
							var prevModel = kycService.GetRegInfoByMphone(aReginfo.Mphone);
							_customerRepository.UpdateRegInfo(aReginfo);
							kycService.InsertUpdatedModelToAuditTrail(aReginfo, prevModel, aReginfo.UpdateBy, 3, 4, "Customer", aReginfo.Mphone, "Register successfully");
							MessageService service = new MessageService();
							service.SendMessage(new MessageModel()
							{
								Mphone = aReginfo.Mphone,
								MessageId = "999",
								MessageBody = "Dear Customer, Your OK wallet has been Activated successfully. For query, please call at OBL Call Centre: 16269, "
							});

							return HttpStatusCode.OK;
						}
						else
						{
							return HttpStatusCode.OK;
						}
						
					}

				}
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public object GetCustomerByMphone(string mPhone)
		{
			try
			{
				return _customerRepository.GetCustomerByMphone(mPhone);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
