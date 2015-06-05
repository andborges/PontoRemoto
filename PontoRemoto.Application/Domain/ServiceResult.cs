using System.Collections.Generic;

namespace PontoRemoto.Application.Domain
{
    public class ServiceResult<TResult> where TResult : class
    {
        private readonly bool _suceeded;
        private readonly TResult _result;
        private readonly IEnumerable<string> _errors;

        protected ServiceResult(TResult result)
        {
            _suceeded = true;
            _result = result;
        }

        protected ServiceResult(IEnumerable<string> errors)
        {
            _suceeded = false;
            _errors = errors;
        }

        public bool Succeeded
        {
            get { return _suceeded; }
        }

        public TResult Result
        {
            get { return _result; }
        }

        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }

        public static ServiceResult<TResult> Success(TResult result)
        {
            return new ServiceResult<TResult>(result);
        }

        public static ServiceResult<TResult> Error(IEnumerable<string> errors)
        {
            return new ServiceResult<TResult>(errors);
        }
    }
}
