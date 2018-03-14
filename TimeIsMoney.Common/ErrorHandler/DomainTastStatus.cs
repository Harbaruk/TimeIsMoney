using TimeIsMoney.Common.ErrorHandler;

namespace TimeIsMoney.Common
{
    public class DomainTaskStatus
    {
        private ErrorCollection _errorCollection = new ErrorCollection();

        public ErrorCollection ErrorCollection => _errorCollection;

        public bool HasErrors => _errorCollection.HasErrors;

        public void AddError(string key, string error) => _errorCollection.AddError(key, error);

        public void AddUnkeyedError(string error) => AddError("general", error);
    }
}