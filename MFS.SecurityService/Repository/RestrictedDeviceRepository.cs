﻿using MFS.SecurityService.Models;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace MFS.SecurityService.Repository
{
    public interface IRestrictedDeviceRepository : IBaseRepository<RestrictedDevice>
    {

    }

    public class RestrictedDeviceRepository : BaseRepository<RestrictedDevice>, IRestrictedDeviceRepository
    {

    }    
}
