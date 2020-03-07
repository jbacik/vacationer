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

    public class EditTestsShould
    {
        //[Fact]
        //public async Task QueryForCommand()
        //{

        //}

        [Fact]
        public async Task UpdateVactionTime()
        {
            var user = new User
            {
                FirstName = "George",
                LastName = "Costanza",
                UserName = $"tbone_{Guid.NewGuid()}",
                IsActive = true
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

            var editCommand = new Edit.Command
            {
                Id = vacationTimeId,
                UserId = user.Id,
                StartDate = DateTime.Now.AddDays(1).Date,
                EndDate = DateTime.Now.AddDays(2).Date,
                Hours = 10
            };

            await SendAsync(editCommand);

            var vacationTime = await ExecuteDbContextAsync(db => db.VacationTimes.Where(t => t.Id == vacationTimeId).SingleOrDefaultAsync());
            
            vacationTime.ShouldNotBeNull();
            vacationTime.StartDate.ShouldBe(editCommand.StartDate);
            vacationTime.EndDate.ShouldBe(editCommand.EndDate);
            vacationTime.Hours.ShouldBe(editCommand.Hours);            
        }

        [Fact]
        public async Task ReturnZeroGivenInvalidVacationTimeId()
        {
            int? lastVacationTimeId = await ExecuteDbContextAsync(db => db.VacationTimes.OrderBy(u => u.Id).Select(u => u.Id).LastOrDefaultAsync());
            int nextId = (lastVacationTimeId ?? 0) + 1;

            var editCommand = new Edit.Command
            {
                Id = nextId,
                StartDate = DateTime.Now.AddDays(1).Date,
                EndDate = DateTime.Now.AddDays(2).Date,
                Hours = 10
            };

            var resultId = await SendAsync(editCommand);

            resultId.ShouldBe(0);
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

            var editCommand = new Edit.Command
            {
                Id = vacationTimeId,
                UserId = (user.Id + 1),
                Hours = 0
            };
            
            var resultId = await SendAsync(editCommand);

            resultId.ShouldBe(0);
        }
    }
}
