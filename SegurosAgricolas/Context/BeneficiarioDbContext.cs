using Microsoft.EntityFrameworkCore;
using SegurosAgricolas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SegurosAgricolas.Context
{
    public class BeneficiarioDbContext : DbContext
    {
        
        public BeneficiarioDbContext(DbContextOptions<BeneficiarioDbContext> options) : base(options)
        {
        }
        public virtual DbSet<BeneficiarioEntity> Beneficiarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BeneficiarioEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired();
                entity.Property(e => e.CNPJ).IsRequired();
                entity.Property(e => e.Ativo).IsRequired();
            });

        }


    }
}
