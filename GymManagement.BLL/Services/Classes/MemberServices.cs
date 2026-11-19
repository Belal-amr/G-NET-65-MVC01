using GymManagement.BLL.Services.Ineterfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    
    public class MemberServices : IMemberService
    {
        private readonly IGenericRepository<Member> memberRepository;
        private readonly IGenericRepository<Membership> membershipRepository;
        private readonly IGenericRepository<Plan> planRepository;
        private readonly IGenericRepository<HealthRecord> healthRecordRepository;
        private readonly IGenericRepository<Booking> bookingRepository;

        public MemberServices(IGenericRepository<Member> memberRepository , IGenericRepository<Membership> membershipRepository ,IGenericRepository<Plan> planRepository, IGenericRepository<HealthRecord> HealthRecordRepository, IGenericRepository<Booking> BookingRepository)
        {
            this.memberRepository = memberRepository;
            this.membershipRepository = membershipRepository;
            this.planRepository = planRepository;
            healthRecordRepository = HealthRecordRepository;
            bookingRepository = BookingRepository;
        }

        

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await memberRepository.GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            //List<MemberViewModel> memberViewModels = new List<MemberViewModel>();

            //foreach (var item in members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Email = item.Email,
            //        Gender = item.Gender.ToString(),
            //        Phone = item.Phone,
            //        Photo = item.Photo
            //    };
            //    memberViewModels.Add(memberViewModel);
            //}
            var memberViewModel = members.Select(item => new MemberViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Email = item.Email,
                Gender = item.Gender.ToString(),
                Phone = item.Phone,
                Photo = item.Photo
            });
            return memberViewModel;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            //Check Email
            //var members = await memberRepository.GetAllAsync(ct: ct);
            //var emailExist = members.Any(m => m.Email == member.Email);

            var emailExist = await memberRepository.AnyAsync(m => m.Email == member.Email, ct: ct);

            //Check Phone
            var phoneExist = await memberRepository.AnyAsync(m => m.Phone == member.Phone, ct: ct);
            //Emil or Phone exist return false
            if(emailExist || phoneExist) return false;

            //else create member and return true
            var memberEntity = new Member()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,         
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = member.BuildingNumber,
                    City = member.City,
                    Street = member.Street
                },
                HealthRecord = new HealthRecord()
                {
                    Height = member.HealthRecordViewModel.Height,
                    Weight = member.HealthRecordViewModel.Weight,
                    BloodType = member.HealthRecordViewModel.BloodType,
                    Note = member.HealthRecordViewModel.Note
                }
            };
            var result = await memberRepository.AddAsync(memberEntity, ct: ct);
            return result > 0;

        }

        public async Task<MemberViewModel?> GetMemberByIdAsync(int id, CancellationToken ct = default)
        {
            var member = await memberRepository.GetByIdAsync(id,ct);
            if (member == null) return null;
            var model = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Gender = member.Gender.ToString(),
                Photo = member.Photo,
                Address = $"{member.Address?.BuildingNumber}, {member.Address?.Street}, {member.Address?.City}"
            };
            //var memberships = await membershipRepository.GetAllAsync();
            var Activemembership = await membershipRepository.FirstOrDefaultAsync(m => m.MemberId == member.Id && m.EndDate > DateTime.Now);
            if(Activemembership is not null)
            {
                var activplan = await planRepository.GetByIdAsync(Activemembership.PlanId ,ct);
                model.PlanName = activplan?.Name;
                model.MemberShipStartDate = Activemembership.CreatedAt.ToString("dd/MM/yyyy");
                model.MemberShipEndDate = Activemembership.EndDate.ToString("dd/MM/yyyy");
            }
            return model;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int id, CancellationToken ct = default)
        {
            var record = await healthRecordRepository.FirstOrDefaultAsync(hr => hr.MemberId == id,ct: ct);
            if(record is null) return null;
            else
                return new HealthRecordViewModel()
                {
                    Height = record.Height,
                    Weight = record.Weight,
                    BloodType = record.BloodType,
                    Note = record.Note
                };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var Member = await memberRepository.GetByIdAsync(id, ct);
            if (Member is null) return null;
            else
            {
                return new MemberToUpdateViewModel()
                {
                    Name = Member.Name,
                    Email = Member.Email,
                    Phone = Member.Phone,
                    BuildingNumber = Member.Address?.BuildingNumber ?? 0,
                    City = Member.Address?.City ?? string.Empty,
                    Street = Member.Address?.Street ?? string.Empty,
                    Photo = Member.Photo
                };
            }
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await memberRepository.GetByIdAsync(id, ct);
            if (member is null) return false;
            var emailExist = await memberRepository.AnyAsync(x=> x.Email == model.Email && x.Id != id);
            var PhoneExist = await memberRepository.AnyAsync(x=> x.Email == model.Phone && x.Id != id);
            if(emailExist || PhoneExist) return false;
            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            var result = await memberRepository.UpdateAsync(member, ct);
            return result > 0;

        }

        public async Task<bool> RemoveMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await memberRepository.GetByIdAsync(id, ct);
            if (member is null) return false;
            var hasBooking = await bookingRepository.AnyAsync(b => b.MemberId == id && b.Session.StartDate > DateTime.Now, ct);
            if (hasBooking) return false;
            var result = await memberRepository.DeleteAsync(member, ct);
            return result > 0;
        }
    }
}
