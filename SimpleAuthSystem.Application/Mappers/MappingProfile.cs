namespace SimpleAuthSystem.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, UserDTO>();
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
