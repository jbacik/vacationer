namespace backend.integrationtests.Handlers.VacationTimes
{
    using backend.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using static SliceFixture;

    public class ListTestsShould
    {
        [Fact]
        public async Task ReturnAllActualTimes()
        {
            var user = new User
            {
                FirstName = "George",
                LastName = "Costanza",
                UserName = $"tbone{Guid.NewGuid()}",
                IsActive = true
            };

            await InsertAsync(user);

            var springPTO = new VacationTime
            {
                UserId = user.Id,
                StartDate = new DateTime(2020, 03, 03),
                EndDate = new DateTime(2020,03,03),
                Hours = 8,
                IsPlanned = false
            };

            var summerPTO = new VacationTime
            {
                UserId = user.Id,
                StartDate = new DateTime(2020, 06, 15),
                EndDate = new DateTime(2020, 06, 18),
                Hours = 20,
                IsPlanned = false
            };

            await InsertAsync(springPTO, summerPTO);

            var query = new backend.Handlers.VacationTimes.List.Query() 
            {
                UserId = user.Id,
                IsPlanned = false
            };
            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.VacationTimes.Count.ShouldBeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ReturnAllPlannedTimes()
        {
            var user = new User
            {
                FirstName = "George",
                LastName = "Costanza",
                UserName = $"tbone{Guid.NewGuid()}",
                IsActive = true
            };

            await InsertAsync(user);

            var fallPTO = new VacationTime
            {
                UserId = user.Id,
                StartDate = new DateTime(2020, 10, 05),
                EndDate = new DateTime(2020, 10, 09),
                Hours = 40,
                IsPlanned = true
            };

            var winterPTO = new VacationTime
            {
                UserId = user.Id,
                StartDate = new DateTime(2020, 12, 24),
                EndDate = new DateTime(2020, 12, 24),
                Hours = 4,
                IsPlanned = true
            };

            await InsertAsync(fallPTO, winterPTO);

            var query = new backend.Handlers.VacationTimes.List.Query() 
            {
                UserId = user.Id,
                IsPlanned = true
            };
            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.VacationTimes.Count.ShouldBeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task ReturnNullGivenInvalidUserId()
        {
            int? lastUserId = await ExecuteDbContextAsync(db => db.Users.OrderBy(u => u.Id).Select(u => u.Id).LastOrDefaultAsync());
            int nextUserId = (lastUserId ?? 0) + 1;

            var query = new backend.Handlers.VacationTimes.List.Query() 
            {
                UserId = nextUserId,
                IsPlanned = true
            };
            var result = await SendAsync(query);

            result.ShouldBeNull();
        }

        [Fact]
        public async Task ReturnNoVacationTimesGivenNewUser()
        {
            var newUser = new User
            {
                FirstName = "George",
                LastName = "Costanza",
                UserName = $"tbone{Guid.NewGuid()}",
                IsActive = true
            };

            await InsertAsync(newUser);

            var query = new backend.Handlers.VacationTimes.List.Query() 
            {
                UserId = newUser.Id,
                IsPlanned = true
            };
            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.VacationTimes.ShouldBeEmpty();

        }

        [Fact]
        public async Task ReturnNullGivenInactiveUser()
        {
            var user = new User
            {
                FirstName = "Norman",
                LastName = "Newman",
                UserName = $"usps_{Guid.NewGuid()}",
                IsActive = false
            };

            await InsertAsync(user);

            var pto = new VacationTime
            {
                UserId = user.Id,
                StartDate = new DateTime(2020, 12, 24),
                EndDate = new DateTime(2020, 12, 24)
            };

            await InsertAsync(pto);

            var query = new backend.Handlers.VacationTimes.List.Query()
            {
                UserId = user.Id
            };
            var result = await SendAsync(query);

            result.ShouldBeNull();
        }
    }
}