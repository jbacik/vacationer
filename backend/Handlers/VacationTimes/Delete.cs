using System.Threading;
using System.Threading.Tasks;
using backend.Data;
using MediatR;

namespace backend.Handlers.VacationTimes
{
    public class Delete 
    {
        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
            public int UserId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly VacationContext _db;

            public CommandHandler(VacationContext db) => _db = db;

            public async Task<bool> Handle(Command request, CancellationToken token)
            {
                var vacationTime = await _db.VacationTimes.FindAsync(request.Id);
                if (vacationTime == null || vacationTime.UserId != request.UserId) return false;

                _db.VacationTimes.Remove(vacationTime);
                return await _db.SaveChangesAsync(token) >= 0;
            }
        }
    }
}