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

    public class DeleteTestsShould
    {
        [Fact]
        public async Task DeleteExistingVacationtime()
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

            var deleteCommand = new Delete.Command() { Id = resultId, UserId = user.Id };
            var isSuccess = await SendAsync(deleteCommand);

            var vacationTime = await ExecuteDbContextAsync(db => db.VacationTimes.Where(t => t.Id == resultId).SingleOrDefaultAsync());

            isSuccess.ShouldBeTrue();
            vacationTime.ShouldBeNull();
        }

        [Fact]
        public async Task ReturnZeroGivenInvalidVacationTimeId()
        {
            int? lastVacationTimeId = await ExecuteDbContextAsync(db => db.VacationTimes.OrderBy(u => u.Id).Select(u => u.Id).LastOrDefaultAsync());
            int nextId = (lastVacationTimeId ?? 0) + 1;

            var deleteCommand = new Delete.Command { Id = nextId };

            var isSuccess = await SendAsync(deleteCommand);
            isSuccess.ShouldBeFalse();
        }

        [Fact]
        public async Task ReturnZeroGivenInvalidUserId()
        {
            var user = new User
            {
                FirstName = "Norman",
                LastName = "Newman",
                UserName = $"usps_{Guid.NewGuid()}",
                IsActive = false
            };

            var vacationTimeId = 0;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                await ctxt.Users.AddAsync(user);
                var createCommand = new Create.Command
                {
                    UserId = user.Id,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date,
                    Hours = 8
                };
                vacationTimeId = await mediator.Send(createCommand);
            });

            var deleteCommand = new Delete.Command() 
            { 
                Id = vacationTimeId + 1,
                UserId = user.Id 
            };
            var isSuccess = await SendAsync(deleteCommand);

            var vacationTime = await ExecuteDbContextAsync(db => db.VacationTimes.Where(t => t.Id == vacationTimeId).SingleOrDefaultAsync());

            isSuccess.ShouldBeFalse();
            vacationTime.ShouldBeNull();
        }
    }
}