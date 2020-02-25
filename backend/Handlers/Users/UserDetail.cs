using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using backend.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Handlers.Users 
{
    public class UserDetail 
    {
        public class Query : IRequest<QueryResult>
        {
            public int Id { get; set; }
        }

        public class QueryResult
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public DateTime VacationPoolStartDate { get; set; }
            public double VacationPoolHours { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, QueryResult>
        {
            private readonly VacationContext _db;

            public QueryHandler(VacationContext db) => _db = db;

            public async Task<QueryResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _db.Users.Where(u => u.Id == request.Id)
                                .Include(v => v.VacationPools)
                                .Where(v => v.IsActive)
                                .FirstOrDefaultAsync();

                if (result == null) return null;

                return new QueryResult{
                    Id = result.Id,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    UserName = result.UserName,
                    VacationPoolHours = result.VacationPools.FirstOrDefault().Hours,
                    VacationPoolStartDate = result.VacationPools.FirstOrDefault().StartDate
                };
            }
        }
    }
}