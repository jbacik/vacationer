using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Data;
using backend.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Handlers.VacationTimes
{
    public class List
    {
        public class Query : IRequest<QueryResult>
        {
            public int UserId { get; set; }
        }

        public class QueryResult
        {
            public int UserId { get; set; }
            public List<VacationTimeModel> VacationTimes { get; set; } = new List<VacationTimeModel>();

            public class VacationTimeModel
            {
                public int Id { get; set; }
                public DateTime VacationStartDate { get; set; }
                public DateTime VacationEndDate { get; set; }
                public bool IsPlanned { get; set; }
                public double VacationHours { get; set; }
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<VacationTime, QueryResult.VacationTimeModel>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, QueryResult>
        {
            private readonly VacationContext _db;
            private readonly IConfigurationProvider _mapperConfig;

            public QueryHandler(VacationContext db, IConfigurationProvider mapperConfig)
            {
                _db = db;
                _mapperConfig = mapperConfig;
            }

            public async Task<QueryResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = new QueryResult {
                    UserId = request.UserId
                };

                var user = await _db.Users.FindAsync(request.UserId);
                if (user == null || !user.IsActive) return null;

                result.VacationTimes = await _db.VacationTimes.Where(v => v.UserId == user.Id)
                                            .ProjectTo<QueryResult.VacationTimeModel>(_mapperConfig)
                                            .ToListAsync();                
                return result;
            }
        }
    }
}