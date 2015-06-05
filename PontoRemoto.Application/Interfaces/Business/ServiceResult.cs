using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Easy.Application.Interfaces.Business
{
    [ExcludeFromCodeCoverage]
    public class ServiceResult<TResult> where TResult : class
    {
        private readonly bool _suceeded;
        private readonly TResult _result;
        private readonly IEnumerable<string> _errors;

        protected ServiceResult(TResult result)
        {
            this._suceeded = true;
            this._result = result;
        }

        protected ServiceResult(IEnumerable<string> errors)
        {
            this._suceeded = false;
            this._errors = errors;
        }

        public bool Succeeded
        {
            get { return this._suceeded; }
        }

        public TResult Result
        {
            get { return this._result; }
        }

        public IEnumerable<string> Errors
        {
            get { return this._errors; }
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
