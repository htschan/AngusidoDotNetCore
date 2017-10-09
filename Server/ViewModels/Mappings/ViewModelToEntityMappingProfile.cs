using AutoMapper;
using Server.Controllers;
using Server.Domain.UserConfiguration;
using Server.ViewModels.Validations;


namespace Server.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            //       <Destination, Source>
            CreateMap<RegisterDto, AppUser>().ForMember(au => au.UserName, map => map.MapFrom<string>(dto => dto.Email));
			CreateMap<AppUser, UserDto>()
                .ReverseMap();
            CreateMap<string, RoleDto>()
                .ForMember(s => s.Name, map => map.MapFrom(vm => vm))
                .ReverseMap();
            CreateMap<RefreshToken, SessionDto>()
                .ForMember(s => s.Userid, map => map.MapFrom(vm => vm.ClientId))
                .ForMember(s => s.Email, map => map.MapFrom(vm => vm.ClientName));
        }
    }
}