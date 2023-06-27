using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Application;
using Tracker.Domain;
//using static Tracker.Features.StudentsService;

namespace Tracker.Features
{
    //Services.Repositories;
    //public class StudentsRepository : IStudentsRepository
    //{
    //    private readonly IDataContext _context;

    //    public StudentsRepository(IDataContext context)
    //    {
    //        _context = context ?? throw new ArgumentNullException(nameof(context));
    //    }

    //    public async Task<IList<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    //    {
    //        IList<Student> result = await _context
    //                       .Student
    //                       .Find(p => true)
    //                       .ToListAsync(cancellationToken).ConfigureAwait(false);
    //        return result;
    //    }

    //}
    //public interface IStudentsRepository //: IBaseRepository<Student>
    //{
    //    Task<IList<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    //}
    //public interface IStudentsService
    //{

    //    Task<IList<StudentEntity>> GetAll();
    //}
    //public class StudentsService : IStudentsService
    //{
    //    private readonly IMediator _mediator;

    //    public StudentsService(IMediator mediator)
    //    {
    //        _mediator = mediator;
    //    }
    //    public class GetAllStudentsQuery : IRequest<IList<StudentEntity>>
    //    {
    //    }
    //    public async Task<IList<StudentEntity>> GetAll()
    //    {
    //        var command = new GetAllStudentsQuery();
    //        IList<StudentEntity> students = await _mediator.Send(command);
    //        return students;
    //    }
    //}
    //public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IList<StudentEntity>>
    //{
    //    private readonly IStudentsRepository _studentsRepository;

    //    public GetAllStudentsQueryHandler(IStudentsRepository studentsRepository)
    //    {
    //        _studentsRepository = studentsRepository;
    //    }

    //    public async Task<IList<StudentEntity>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    //    {
            
    //        var result =await _studentsRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
    //        var s=new List<StudentEntity>();
    //        foreach (var student in result)
    //        {
    //            s.Add(new StudentEntity { Email = student.Email });
    //        }
    //        return s;
    //    }
    //}

}
