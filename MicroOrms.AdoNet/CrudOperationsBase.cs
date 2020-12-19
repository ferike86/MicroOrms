using System.Data;

namespace MicroOrms.AdoNet
{
    public abstract class CrudOperationsBase
    {
        protected IDbDataParameter CreateParameter(IDbCommand command, string parameterName, object parameterValue)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }
    }
}
