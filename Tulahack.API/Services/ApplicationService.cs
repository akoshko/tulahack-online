using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tulahack.API.Context;
using Tulahack.Dtos;
using Tulahack.Model;

namespace Tulahack.API.Services;

public interface IApplicationService
{
    Task<ContestApplicationDto> GetApplication(Guid userId, ContestApplicationDto dto);
    Task<ContestApplicationDto> UpdateApplication(Guid userId, ContestApplicationDto dto);
    Task<ContestApplicationDto> SubmitApplication(Guid userId, ContestApplicationDto dto);
    Task<ContestApplicationDto> AcceptApplication(Guid userId, Guid applicationId, string justification);
    Task<ContestApplicationDto> DeclineApplication(Guid userId, Guid applicationId, string justification);
}

public class ApplicationService : IApplicationService
{
    private readonly TulahackContext _tulahackContext;
    private readonly IMapper _mapper;

    public ApplicationService(
        TulahackContext tulahackContext,
        IMapper mapper)
    {
        _tulahackContext = tulahackContext;
        _mapper = mapper;
    }

    public Task<ContestApplicationDto> GetApplication(Guid userId, ContestApplicationDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<ContestApplicationDto> UpdateApplication(Guid userId, ContestApplicationDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<ContestApplicationDto> SubmitApplication(Guid userId, ContestApplicationDto dto)
    {
        ContestApplication application = _mapper.Map<ContestApplication>(dto);
        PersonBase? account = await _tulahackContext.Accounts.SingleOrDefaultAsync(item => item.Id == userId);

        if (account is null)
        {
            throw new ValidationException("Unable to find user account in DB, re-login and try again");
        }

        account = _mapper.Map<PersonBase>(dto);

        _ = _tulahackContext.Accounts.Update(account);
        _ = _tulahackContext.ContestApplications.Add(application);
        _ = await _tulahackContext.SaveChangesAsync();

        return dto;
    }

    public async Task<ContestApplicationDto> AcceptApplication(
        Guid userId,
        Guid applicationId,
        string justification)
    {
        ContestApplication? application = await _tulahackContext.ContestApplications
            .SingleOrDefaultAsync(item => item.Id == applicationId && item.UserId == userId);

        if (application is null)
        {
            throw new ValidationException("Unable to find application in DB");
        }

        application.Status = ContestApplicationStatus.Accepted;
        application.Justification = justification;
        _ = await _tulahackContext.SaveChangesAsync();

        return _mapper.Map<ContestApplicationDto>(application);
    }

    public async Task<ContestApplicationDto> DeclineApplication(
        Guid userId,
        Guid applicationId,
        string justification)
    {
        ContestApplication? application = await _tulahackContext.ContestApplications
            .SingleOrDefaultAsync(item => item.Id == applicationId && item.UserId == userId);

        if (application is null)
        {
            throw new ValidationException("Unable to find application in DB");
        }

        application.Status = ContestApplicationStatus.Declined;
        application.Justification = justification;
        _ = await _tulahackContext.SaveChangesAsync();

        return _mapper.Map<ContestApplicationDto>(application);
    }
}
