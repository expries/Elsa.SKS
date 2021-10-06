using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
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