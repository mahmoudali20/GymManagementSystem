using AutoMapper;
using GymManagement.BLL.ViewModels.Members;
using GymManagement.BLL.ViewModels.MemberShips;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.BLL.ViewModels.Trainers;
using GymManagement.DAL.Models;

namespace GymManagement.BLL.Profiles
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {

            #region Member Mapping

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s))
                .ForMember(d => d.HealthRecord, o => o.MapFrom(s => s.HealthRecordViewModel));
            CreateMap<CreateMemberViewModel, Address>();
            CreateMap<HealthRecordViewModel, HealthRecord>();

            //--Update;

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForMember(des => des.Name, opt => opt.Ignore())
                .ForMember(des => des.Photo, opt => opt.Ignore());

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
                .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber));


            //--Details
            CreateMap<Member, MemberViewModel>()
                 .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
                 .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToString()))
                 .ForMember(d => d.Address, o => o.MapFrom(s => $"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"));


            //--Health Record
            CreateMap<HealthRecord, HealthRecordViewModel>();
            #endregion

            #region Session Mapping

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>()
                         .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                         .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                         .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            #endregion

            #region Plan Mapping

            CreateMap<Plan, PlanViewModel>();
            CreateMap<UpdatePlanViewModel, Plan>().ReverseMap();

            #endregion

            #region Trainer Mapping


            //Get
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialty.ToString()))
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToString("dd MMM yyyy")))
                .ForMember(d => d.Address, o => o.MapFrom(s => $"{s.Address.BuildingNumber} · {s.Address.Street} · {s.Address.City}"));



            //Create
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s));
            CreateMap<CreateTrainerViewModel, Address>();

            //Update
            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForMember(d => d.Specialty, o => o.MapFrom(s => s.Specialties))
                .ForMember(d => d.Name, o => o.Ignore());

            CreateMap<Trainer, TrainerToUpdateViewModel>()
              .ForPath(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
              .ForPath(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
              .ForPath(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
              .ForMember(d => d.Specialties, o => o.MapFrom(s => s.Specialty));



            #endregion

            #region MemberShipMapping

            CreateMap<Membership, MemberShipViewModel>()
                .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member.Name))
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.CreatedAt));

            CreateMap<CreateMemberShipViewModel, Membership>();

            CreateMap<Plan, PlanSelectListViewModel>();
            CreateMap<Member, MemberSelectListViewModel>();



            #endregion


        }

    }
}
