using velowrench.Repository.Models;
using velowrench.Utils.Results;

namespace velowrench.Repository.Interfaces;

public interface IUserPreferenceRepository
{
    Task<OperationResult<UserPreferenceModel>> LoadAsync();
    Task SaveAsync(UserPreferenceModel userPreference);
}