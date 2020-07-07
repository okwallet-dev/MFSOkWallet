﻿using MFS.SecurityService.Models;
using MFS.SecurityService.Repository;
using OneMFS.SharedResources;
using OneMFS.SharedResources.CommonService;
using OneMFS.SharedResources.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Service
{
    public interface IApplicationUserService : IBaseService<ApplicationUser>
    {
        AuthUserModel login(LoginModel model);
        string GetTransAmtLimit(string createUser);
		object IsProceedToController(List<string> userInfos);
		object GetAppUserListDdl();
	}

    public class ApplicationUserService : BaseService<ApplicationUser>, IApplicationUserService
    {
        public IApplicationUserRepository usersRepo;
        public IFeatureService featureService;
        public ApplicationUserService(IApplicationUserRepository _usersRepo, IFeatureService _featureService)
        {
            usersRepo = _usersRepo;
            featureService = _featureService;
        }

        public AuthUserModel login(LoginModel model)
        {
            ApplicationUser user = validateLogin(model);
            return BuildAuthUserModel(user);
        }

        private ApplicationUser validateLogin(LoginModel model)
        {
            StringBuilderService stringBuilderService = new StringBuilderService();

            return usersRepo.validateLogin(model.UserName, stringBuilderService.GenerateSha1Hash(model.Password));
        }

        private AuthUserModel BuildAuthUserModel(ApplicationUser model)
        {
            AuthUserModel authUserModel = new AuthUserModel();

            authUserModel.User = model;
            if (authUserModel.User.Is_validated)
            {
                authUserModel.IsAuthenticated = true;
                authUserModel.FeatureList = featureService.GetAuthFeatureList(authUserModel.User.Id);
                authUserModel.BearerToken = Guid.NewGuid().ToString();
            }
            else
            {
                authUserModel.IsAuthenticated = false;
            }

            return authUserModel;
        }

        public string GetTransAmtLimit(string createUser)
        {
            try
            {
                return usersRepo.GetTransAmtLimit(createUser);
            }
            catch (Exception)
            {

                throw;
            }
        }

		public object IsProceedToController(List<string> userInfos)
		{
			var userId = userInfos[0];
			var roleId = userInfos[1];
			var userInfo = (Tuple<string,string>) usersRepo.IsProceedToController(userInfos);
			if(userInfo.Item1 != roleId || userInfo.Item2 == "Y")
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public object GetAppUserListDdl()
		{
			return usersRepo.GetAppUserListDdl();
		}
	}
}
