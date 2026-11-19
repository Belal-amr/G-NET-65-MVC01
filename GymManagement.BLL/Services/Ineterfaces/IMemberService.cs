using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Ineterfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberByIdAsync(int id, CancellationToken ct = default);

        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel member, CancellationToken ct = default);
        Task<bool> RemoveMemberAsync(int id, CancellationToken ct = default);
    }
}
