using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.DataAccess.Sql.Exceptions
{
    [ExcludeFromCodeCoverage]
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