using EduTek.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTek.Application.Services
{
    public interface IAdminService
    {

        Task<List<PendingUserDto>> GetPendingRegistrationsAsync();

        Task<bool> ApproveRegistrationAsync(ApprovalDto dto);
        Task<bool> BulkApproveRegistrationsAsync(BulkApprovalDto dto);
    }
}
