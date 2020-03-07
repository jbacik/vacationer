using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using backend.Data;
using backend.Models;
using MediatR;

namespace backend.Handlers.VacationTimes
{
    public class Edit 
    {
        public class Command : IRequest<int>
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
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
                var vacationTime = await _db.VacationTimes.FindAsync(request.Id);
                if (vacationTime == null || vacationTime.UserId != request.UserId) return 0;

                _mapper.Map<Command, VacationTime>(request, vacationTime);

                await _db.SaveChangesAsync(token);
                return vacationTime.Id;
            }
        }
    }
}