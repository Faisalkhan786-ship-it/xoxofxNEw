using System.Data;

namespace RepositoryContract
{
    public interface IDapperContext
    {
        public IDbConnection createConnection();
    }
}
