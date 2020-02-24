using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

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
            public async Task<QueryResult> Handle(Query request, CancellationToken cancellationToken)
            {
                return new QueryResult{
                    Id = 3,
                    FirstName = "Static",
                    LastName = "Data",
                    UserName = "staticapi",
                    VacationPoolStartDate = new DateTime(2020,01,01),
                    VacationPoolHours = 100
                };
            }
        }
    }
}