using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Models;
using MediatR;

namespace backend.Handlers.VacationTimes
{
    public class Create 
    {
        public class Command : IRequest<int>
        {
            public int UserId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool IsPlanned { get; set; }
            public double Hours { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Command, VacationTime>();
        }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly VacationContext _db;
            private readonly IMapper _mapper;

            public CommandHandler(VacationContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task<int> Handle(Command request, CancellationToken token)
            {
                var user = await _db.Users.FindAsync(request.UserId);
                if (user == null || !user.IsActive) return 0;

                var vacationTime = _mapper.Map<Command, VacationTime>(request);
                _db.VacationTimes.Add(vacationTime);

                await _db.SaveChangesAsync(token);
                return vacationTime.Id;
            }
        }
    }
}