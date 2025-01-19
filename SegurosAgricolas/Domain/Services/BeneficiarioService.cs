using Microsoft.EntityFrameworkCore;
using SegurosAgricolas.Context;
using SegurosAgricolas.Domain.Entities;
using SegurosAgricolas.Domain.Services.Interfaces;
using SegurosAgricolas.Domain.Validator;

namespace SegurosAgricolas.Domain.Services
{
    public class BeneficiarioService : IBeneficiarioService
    {
        private readonly BeneficiarioDbContext _context;
        private readonly BeneficiarioValidator _validator;

        public BeneficiarioService(BeneficiarioDbContext context, BeneficiarioValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<IEnumerable<BeneficiarioEntity>> GetAllBeneficiariosAsync()
        {
            var beneficiarios = await _context.Beneficiarios
                .Where(b => b.Ativo)
                .ToListAsync();

            return beneficiarios;
        }

        public async Task<BeneficiarioEntity> GetBeneficiarioByIdAsync(Guid id)
        {
            var beneficiario = await _context.Beneficiarios
                .FirstOrDefaultAsync(b => b.Id == id);
            return beneficiario!;
        }

        public async Task<BeneficiarioEntity> AddBeneficiarioAsync(BeneficiarioEntity entity)
        {
            entity.Ativo = true;

            var validationResult = _validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                entity.Ativo = false;
                throw new ArgumentException($"Erro ao adicionar beneficiário: {string.Join(", ", validationResult.Errors)}");
            }

            var cnpjExists = await _context.Beneficiarios.AnyAsync(b => b.CNPJ!.Replace(".", "").Replace("-", "") == entity.CNPJ);

            if (cnpjExists)
            {
                entity.Ativo = false;
                throw new ArgumentException("Erro ao adicionar beneficiário", "CNPJ já cadastrado");
            }

            await _context.Beneficiarios.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteBeneficiarioAsync(Guid id)
        {
            var beneficiario = await _context.Beneficiarios
                .FirstOrDefaultAsync(b => b.Id == id);

            if (beneficiario != null)
            {
                _context.Beneficiarios.Remove(beneficiario);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<BeneficiarioEntity> UpdateBeneficiarioAsync(BeneficiarioEntity entity)
        {
            var validationResult = _validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Erro ao atualizar beneficiário", string.Join(", ", validationResult.Errors));
            }

            var existingEntity = await _context.Beneficiarios
                .FirstOrDefaultAsync(b => b.Id == entity.Id);

            if (existingEntity == null)
            {
                throw new ArgumentException("Beneficiário não encontrado");
            }

            var cnpjExists = await _context.Beneficiarios
                .AnyAsync(b => b.CNPJ!.Replace(".", "").Replace("-", "") == entity.CNPJ && b.Id != entity.Id);

            if (cnpjExists)
            {
                throw new ArgumentException("Erro ao atualizar beneficiário", "CNPJ já cadastrado para outro beneficiário");
            }

            existingEntity.Nome = entity.Nome;
            existingEntity.CNPJ = entity.CNPJ;
            existingEntity.Ativo = entity.Ativo;

            _context.Beneficiarios.Update(existingEntity);
            await _context.SaveChangesAsync();

            return existingEntity;
        }
    }
}
