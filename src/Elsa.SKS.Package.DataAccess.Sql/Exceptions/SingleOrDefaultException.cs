using System;

namespace Elsa.SKS.Package.DataAccess.Sql.Exceptions
{
    public class SingleOrDefaultException : DataAccessException
    {
        public SingleOrDefaultException()
        {
        }

        public SingleOrDefaultException(string message) : base(message)
        {
        }

        public SingleOrDefaultException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}