using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tulahack.Dtos;

namespace Tulahack.UI.Services;

public interface ITaskboardService
{
    Task<List<ContestCaseDto>> GetContestCases();
}

public class TaskboardService() : ITaskboardService
{
    public Task<List<ContestCaseDto>> GetContestCases() =>
        Task.FromResult<List<ContestCaseDto>>([
            new()
            {
                Id = Guid.NewGuid(),
                Number = 1,
                Description = "Task description #1",
                Complexity = ContestCaseComplexityDto.Normal,
                Title = "Task title #1",
                TechStack = new[]
                {
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        SkillName = "JS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        SkillName = "JS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        SkillName = "JS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        SkillName = "JS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        SkillName = "JS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    },
                },
                AcceptanceCriteria = new[]
                {
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        Criteria = "WebRTC used"
                    },
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Green",
                        Criteria = "Auth used"
                    },
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        Criteria = "Deployed to Internet"
                    }
                },
                Company = new CompanyDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Company description #1",
                    Name = "Mock inc.",
                    Experts = new[]
                    {
                        new ExpertDto
                        {
                            Firstname = "Ivan",
                            Lastname = "Ivanov"
                        }
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Number = 2,
                Description = "Task description #2",
                Complexity = ContestCaseComplexityDto.Beginner,
                Title = "Task title #2",
                TechStack = new[]
                {
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        SkillName = "JS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    }
                },
                AcceptanceCriteria = new[]
                {
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        Criteria = "WebRTC used"
                    },
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Green",
                        Criteria = "Auth used"
                    },
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        Criteria = "Deployed to Internet"
                    }
                },
                Company = new CompanyDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Company description #2",
                    Name = "Mock inc.",
                    Experts = new[]
                    {
                        new ExpertDto
                        {
                            Firstname = "Ivan",
                            Lastname = "Ivanov"
                        }
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Number = 3,
                Description = "Task description #3",
                Complexity = ContestCaseComplexityDto.Insane,
                Title = "Task title #3",
                TechStack = new[]
                {
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        SkillName = "JS"
                    },
                    new SkillDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        SkillName = "CSS"
                    }
                },
                AcceptanceCriteria = new[]
                {
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Red",
                        Criteria = "WebRTC used"
                    },
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Green",
                        Criteria = "Auth used"
                    },
                    new AcceptanceCriteriaDto
                    {
                        Id = Guid.NewGuid(),
                        Color = "Blue",
                        Criteria = "Deployed to Internet"
                    }
                },
                Company = new CompanyDto
                {
                    Id = Guid.NewGuid(),
                    Description = "Company description #3",
                    Name = "Mock inc.",
                    Experts = new[]
                    {
                        new ExpertDto
                        {
                            Firstname = "Ivan",
                            Lastname = "Ivanov"
                        }
                    }
                }
            },
        ]);
}
