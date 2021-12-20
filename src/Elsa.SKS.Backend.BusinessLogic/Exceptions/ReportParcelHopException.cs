using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ReportParcelHopException : BusinessException
    {
        public ReportParcelHopException()
        {
        }

        public ReportParcelHopException(string message) : base(message)
        {
        }

        public ReportParcelHopException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}