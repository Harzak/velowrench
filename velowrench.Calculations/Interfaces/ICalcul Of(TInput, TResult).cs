using velowrench.Utils.Results;

namespace velowrench.Calculations.Interfaces;

public interface ICalcul<TInput, TResult> : ICalcul where TInput : class
{
    OperationResult<TResult> Start(TInput input);
}