using SegurosAgricolas.Domain.Entities;

namespace SegurosAgricolas.Domain.Services.Interfaces
{
    public interface IBeneficiarioService
    {
        Task<BeneficiarioEntity> AddBeneficiarioAsync(BeneficiarioEntity entity);

        Task<BeneficiarioEntity> GetBeneficiarioByIdAsync(Guid id);

        Task<IEnumerable<BeneficiarioEntity>> GetAllBeneficiariosAsync();

        Task<BeneficiarioEntity> UpdateBeneficiarioAsync(BeneficiarioEntity entity);

        Task<bool> DeleteBeneficiarioAsync(Guid id);
    }
}
