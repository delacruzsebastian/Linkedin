using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public enum Results : byte
    {
        Success = 0,
        Error,
        ValidationErrors,
        NoData
    }

    public class ServiceResult<T>
    {
        public ServiceResult()
        {
            this.Result = Results.Success;
            this.ValidationErrors = new List<string>();
        }

        public ServiceResult(T defaultValue)
        {
            this.ResultData = defaultValue;
            this.Result = Results.Success;
            this.ValidationErrors = new List<string>();
        }

        public Results Result { get; internal set; }

        public T ResultData { get; internal set; }

        public string ResultErrorMessage { get; private set; }

        public List<string> ValidationErrors { get; }

        public bool HasValidationError
        {
            get
            {
                return this.ValidationErrors.Count > 0;
            }
        }

        public void SetError(string errorMessage)
        {
            this.Result = Results.Error;
            this.ResultErrorMessage = errorMessage;
        }

        public void SetError(string errorMessage, T resultData)
        {
            this.ResultData = resultData;
            this.SetError(errorMessage);
        }

        public void SetValidationError(string validationErrorMessage)
        {
            this.Result = Results.ValidationErrors;
            this.ValidationErrors.Add(validationErrorMessage);
        }

        public void SetValidationError(string validationErrorMessage, T resultData)
        {
            this.ResultData = resultData;
            this.SetValidationError(validationErrorMessage);
        }
    }
}
