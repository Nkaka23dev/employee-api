using TheEmployeeAPI.Domain.Entities;

namespace Core.Infrastructure.Repositories;

public interface IBenefitRepository: IRepository<Benefit>
{
    Task<List<Benefit>> GetByBenefitIdsAsync(IEnumerable<int> ids);

}
