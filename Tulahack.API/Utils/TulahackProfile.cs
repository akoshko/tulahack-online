using AutoMapper;
using Tulahack.Dtos;
using Tulahack.Model;

namespace Tulahack.API.Utils;

public class TulahackProfile : Profile
{
    public TulahackProfile()
    {
        CreateMap<PersonBase, Contestant>(MemberList.None);
        CreateMap<PersonBase, Expert>(MemberList.None);
        CreateMap<PersonBase, Moderator>(MemberList.None);
        CreateMap<PersonBase, Superuser>(MemberList.None);

        CreateMap<Team, TeamDto>(MemberList.None);
        CreateMap<PersonBase, PersonBaseDto>();
        CreateMap<Contestant, ContestantDto>();
        CreateMap<Expert, ExpertDto>();
        CreateMap<Moderator, ModeratorDto>();
        CreateMap<ContestApplication, ContestApplicationDto>()
            .ForMember(dest => dest.StatusJustification, opt => opt
                .MapFrom(src => src.Justification));
    }
}