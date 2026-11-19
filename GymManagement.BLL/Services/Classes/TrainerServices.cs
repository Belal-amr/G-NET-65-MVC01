using GymManagement.BLL.Services.Ineterfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerServices : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;

        public TrainerServices(IGenericRepository<Trainer> TrainerRepository , IGenericRepository<Booking> BookingRepository)
        {
            _trainerRepository = TrainerRepository;
            _bookingRepository = BookingRepository;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct = default)
        {
            var trainers = await _trainerRepository.GetAllAsync(ct: ct);
            return trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialties.ToString()
            });
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExist = await _trainerRepository.AnyAsync(t=> t.Email == model.Email,ct);
            var PhoneExist = await _trainerRepository.AnyAsync(t=> t.Phone == model.Phone,ct);
            if (emailExist || PhoneExist) return false;

            var trainer = new Trainer()
            {
                Name=model.Name,
                Email=model.Email,
                Phone=model.Phone,
                DateOfBirth=model.DateOfBirth,
                Gender=model.Gender,
                Specialties=model.Specialties,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                }

            };

            var result = await _trainerRepository.AddAsync(trainer,ct);
            return result > 0;

        }

        public async Task<TrainerViewModel?> GetAllTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId,ct);
            if (trainer == null) return null;

            return new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId,ct);
            if (trainer is null) return null;
            else
            {
                return new TrainerToUpdateViewModel()
                {
                    Name = trainer.Name,
                    Email = trainer.Email,
                    Phone = trainer.Phone,
                    DateOfBirth = trainer.DateOfBirth,
                    Gender = trainer.Gender,
                    Specialties = trainer.Specialties,
                    BuildingNumber = trainer.Address?.BuildingNumber ?? 0,
                    City = trainer.Address?.City ?? string.Empty,
                    Street = trainer.Address?.Street ?? string.Empty
                };
            }
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId ,ct);
            if (trainer is null) return false;
            var emailExist = await _trainerRepository.AnyAsync(t => t.Email == model.Email && t.Id != trainerId);
            var PhoneExist = await _trainerRepository.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId);
            if(emailExist || PhoneExist) return false;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.City = model.City;
            trainer.Address.Street = model.Street;
            trainer.Specialties = model.Specialties;
            trainer.UpdatedAt = DateTime.Now;

            var result = await _trainerRepository.UpdateAsync(trainer,ct);
            return result > 0;

        }

        public async Task<bool> RemoveTrainerAsync(int id, CancellationToken ct = default)
        {
            var Trainer = await _trainerRepository.GetByIdAsync(id, ct);
            if (Trainer is null) return false;
            var scheduled = await _bookingRepository.AnyAsync(b =>b.Session.StartDate > DateTime.Now, ct);
            if (scheduled) return false;
            var result = await _trainerRepository.DeleteAsync(Trainer, ct);
            return result > 0;
        }
    }
}
