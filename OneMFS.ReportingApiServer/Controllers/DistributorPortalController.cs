﻿using MFS.DistributionService.Models;
using MFS.ReportingService.Models;
using MFS.ReportingService.Service;
using MFS.ReportingService.Utility;
using Microsoft.Reporting.WebForms;
using OneMFS.ReportingApiServer.Utility;
using OneMFS.SharedResources.CommonService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace OneMFS.ReportingApiServer.Controllers
{
	public class DistributorPortalController : ApiController
	{
		private readonly IDistributorPortalService service;
		private readonly IKycService kycService;
		public DistributorPortalController(IDistributorPortalService _service, IKycService _kycService)
		{
			this.service = _service;
			this.kycService = _kycService;
		}
		[HttpPost]		
		[Route("api/DistributorPortal/GetAgentDsrListByPmphone")]
		public byte[] GetAgentDsrListByPmphone(ReportModel model)
		{
			StringBuilderService builder = new StringBuilderService();
			string mphone = builder.ExtractText(Convert.ToString(model.ReportOption), "mphone", "}");
			
			List<AgentDsrList> registrationReports = service.GetAgentDsrListByPmphone(mphone);
			ReportViewer reportViewer = new ReportViewer();
			reportViewer.LocalReport.ReportPath = HostingEnvironment.MapPath("~/Reports/RDLC/RPTAgentDsr.rdlc");  //Request.RequestUri("");
			reportViewer.LocalReport.SetParameters(GetAgentDsrListByPmphoneRptParameter(mphone));
			ReportDataSource A = new ReportDataSource("AgentDsrList", registrationReports);
			reportViewer.LocalReport.DataSources.Add(A);
			ReportUtility reportUtility = new ReportUtility();
			MFSFileManager fileManager = new MFSFileManager();
			return reportUtility.GenerateReport(reportViewer, model.FileType);
		}

		private IEnumerable<ReportParameter> GetAgentDsrListByPmphoneRptParameter(string mphone)
		{
			var clientInfo = (Reginfo)kycService.GetClientInfoByMphone(mphone);
			List<ReportParameter> paraList = new List<ReportParameter>();
			paraList.Add(new ReportParameter("printDate", DateTime.Now.ToShortDateString()));
			paraList.Add(new ReportParameter("distName", clientInfo.Name));
			paraList.Add(new ReportParameter("distMobile", clientInfo.Mphone));
			return paraList;
		}
	}
}