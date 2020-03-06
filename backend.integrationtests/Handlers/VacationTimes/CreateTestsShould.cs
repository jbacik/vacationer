namespace backend.integrationtests.Handlers.VacationTimes
{
    using Xunit;
    using Shouldly;
    using backend.Data;
    using Microsoft.EntityFrameworkCore;
    using backend.Models;
    using System;
    using System.Diagnostics;
    using backend.Handlers.VacationTimes;
    using static SliceFixture;
    using System.Linq;
    using System.Threading.Tasks;

    public class CreateTestsShould
    {
        [Fact]
        public async Task CreateNewVacationtime()
        {
            var user = new User
            {
                FirstName = "George",
                LastName = "Costanza",
                UserName = "tbone7",
                IsActive = true
            };

            Create.Command command = null;
            int resultId = 0;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                await ctxt.Users.AddAsync(user);
                command = new Create.Command
                {
                    UserId = user.Id,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date,
                    Hours = 8
                };
                resultId = await mediator.Send(command);
            });

            var created = await ExecuteDbContextAsync(db => db.VacationTimes.Where(t => t.Id == resultId).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.UserId.ShouldBe(user.Id);
            created.StartDate.Date.ShouldBe(command.StartDate.Date);
            created.EndDate.Date.ShouldBe(command.EndDate.Date);
            created.Hours.ShouldBe(command.Hours);
        }

        [Fact]
        public async Task ReturnZeroGivenInvalidUser()
        {
            int? lastUserId = await ExecuteDbContextAsync(db => db.Users.OrderBy(u => u.Id).Select(u => u.Id).LastOrDefaultAsync());
            int nextUserId = (lastUserId ?? 0) + 1;

            Create.Command command = null;
            int resultId = 0;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                command = new Create.Command
                {
                    UserId = nextUserId,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date,
                    Hours = 8
                };
                resultId = await mediator.Send(command);
            });

            resultId.ShouldBe(0);
        }
        
        [Fact]
        public async Task ReturnZeroGivenInactiveUser()
        {
            var user = new User
            {
                FirstName = "Norman",
                LastName = "Newman",
                UserName = $"usps_{Guid.NewGuid()}",
                IsActive = false
            };

            Create.Command command = null;
            int resultId = 0;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                await ctxt.Users.AddAsync(user);
                command = new Create.Command
                {
                    UserId = user.Id,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date,
                    Hours = 8
                };
                resultId = await mediator.Send(command);
            });

            resultId.ShouldBe(0);
        }
    }
}