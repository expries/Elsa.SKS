using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.DataAccess.Sql.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }

        public DataAccessException(string message) : base(message)
        {
        }

        public DataAccessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}