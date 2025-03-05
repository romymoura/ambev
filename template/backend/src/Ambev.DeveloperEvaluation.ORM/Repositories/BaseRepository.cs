namespace Ambev.DeveloperEvaluation.ORM.Repositories;


public class BaseRepository
{
    public readonly DefaultContext _context;
    public BaseRepository(DefaultContext context)
    {
        _context = context;
    }
}
