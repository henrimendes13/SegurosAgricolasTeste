using Microsoft.EntityFrameworkCore;
using Moq;
using SegurosAgricolas.Context;
using SegurosAgricolas.Domain.Entities;
using SegurosAgricolas.Domain.Services;
using SegurosAgricolas.Domain.Validator;

namespace SegurosAgricolas.Tests
{
    public class BeneficiarioServiceTest
    {
        [Fact]
        public async Task BeneficiarioService_GetAllBeneficiariosAsync_Sucesso()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BeneficiarioDbContext>()
                .UseInMemoryDatabase(databaseName: "BeneficiarioService_GetAllBeneficiariosAsync_Sucesso")
                .Options;

            // Seed data
            using (var context = new BeneficiarioDbContext(options))
            {
                context.Beneficiarios.AddRange(
                    new BeneficiarioEntity { Nome = "Beneficiario1", CNPJ = "12345678000100", Ativo = true },
                    new BeneficiarioEntity { Nome = "Beneficiario2", CNPJ = "12345678000200", Ativo = true }
                );
                context.SaveChanges();
            }

            // Create service with the seeded context
            using (var context = new BeneficiarioDbContext(options))
            {
                var mockValidator = new Mock<BeneficiarioValidator>();
                var beneficiarioService = new BeneficiarioService(context, mockValidator.Object);

                // Act
                var result = await beneficiarioService.GetAllBeneficiariosAsync();

                // Assert
                Assert.IsAssignableFrom<IEnumerable<BeneficiarioEntity>>(result);
                Assert.Equal("Beneficiario1", result.ElementAt(0).Nome);
                Assert.Equal("Beneficiario2", result.ElementAt(1).Nome);
            }
        }

        [Fact]
        public async Task BeneficiarioService_GetBeneficiarioByIdAsync_Sucesso()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BeneficiarioDbContext>()
                .UseInMemoryDatabase(databaseName: "BeneficiarioService_GetAllBeneficiariosAsync_Sucesso")
                .Options;

            // Seed data
            Guid beneficiarioId;
            using (var context = new BeneficiarioDbContext(options))
            {
                var beneficiario = new BeneficiarioEntity { Id = Guid.NewGuid(), Nome = "Beneficiario1", CNPJ = "12345678000100", Ativo = true };
                beneficiarioId = beneficiario.Id;
                context.Beneficiarios.Add(beneficiario);

                context.SaveChanges();
            }

            // Create service with the seeded context
            using (var context = new BeneficiarioDbContext(options))
            {
                var mockValidator = new Mock<BeneficiarioValidator>();
                var beneficiarioService = new BeneficiarioService(context, mockValidator.Object);

                // Act
                var result = await beneficiarioService.GetBeneficiarioByIdAsync(beneficiarioId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(beneficiarioId, result.Id);
                Assert.Equal("Beneficiario1", result.Nome);
                Assert.Equal("12345678000100", result.CNPJ);
            }
        }

        [Fact]
        public async Task BeneficiarioService_DeleteBeneficiarioAsync_Sucesso()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BeneficiarioDbContext>()
                .UseInMemoryDatabase(databaseName: "BeneficiarioService_GetAllBeneficiariosAsync_Sucesso")
                .Options;

            // Seed data
            Guid beneficiarioId;
            using (var context = new BeneficiarioDbContext(options))
            {
                var beneficiario = new BeneficiarioEntity { Id = Guid.NewGuid(), Nome = "Beneficiario1", CNPJ = "12345678000100", Ativo = true };
                beneficiarioId = beneficiario.Id;
                context.Beneficiarios.Add(beneficiario);

                context.SaveChanges();
            }

            // Create service with the seeded context
            using (var context = new BeneficiarioDbContext(options))
            {
                var mockValidator = new Mock<BeneficiarioValidator>();
                var beneficiarioService = new BeneficiarioService(context, mockValidator.Object);

                // Act
                var result = await beneficiarioService.DeleteBeneficiarioAsync(beneficiarioId);

                // Assert
                Assert.True(result);
                var deletedBeneficiario = await context.Beneficiarios.FindAsync(beneficiarioId);
                Assert.Null(deletedBeneficiario);
            }
        }

        [Fact]
        public async Task BeneficiarioService_AddBeneficiarioAsync_Sucesso()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BeneficiarioDbContext>()
                .UseInMemoryDatabase(databaseName: "BeneficiarioService_GetAllBeneficiariosAsync_Sucesso")
                .Options;

            // Seed data
            using (var context = new BeneficiarioDbContext(options))
            {
                context.Beneficiarios.AddRange(
                    new BeneficiarioEntity { Nome = "Beneficiario1", CNPJ = "46823975000132", Ativo = true },
                    new BeneficiarioEntity { Nome = "Beneficiario2", CNPJ = "28457962000101", Ativo = true }
                );
                context.SaveChanges();
            }

            // Create service with the seeded context
            using (var context = new BeneficiarioDbContext(options))
            {
                var beneficiarioValidator = new BeneficiarioValidator();
                var beneficiarioService = new BeneficiarioService(context, beneficiarioValidator);

                // Act
                var result = await beneficiarioService.AddBeneficiarioAsync(new BeneficiarioEntity { Nome = "Beneficiario3", CNPJ = "12345678000195" });

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Beneficiario3", result.Nome);
                Assert.Equal("12345678000195", result.CNPJ);

                // Verify that the new beneficiario was added to the context
                var addedBeneficiario = await context.Beneficiarios.FindAsync(result.Id);
                Assert.NotNull(addedBeneficiario);
                Assert.Equal("Beneficiario3", addedBeneficiario.Nome);
                Assert.Equal("12345678000195", addedBeneficiario.CNPJ);
            }
        }

        [Fact]
        public async Task BeneficiarioService_UpdateBeneficiarioAsync_Sucesso()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BeneficiarioDbContext>()
                .UseInMemoryDatabase(databaseName: "BeneficiarioService_GetAllBeneficiariosAsync_Sucesso")
                .Options;

            // Seed data
            Guid beneficiarioId;
            using (var context = new BeneficiarioDbContext(options))
            {
                var beneficiario = new BeneficiarioEntity { Id = Guid.NewGuid(), Nome = "Beneficiario1", CNPJ = "12345678000100", Ativo = true };
                beneficiarioId = beneficiario.Id;
                context.Beneficiarios.Add(beneficiario);

                context.SaveChanges();
            }

            // Create service with the seeded context
            using (var context = new BeneficiarioDbContext(options))
            {
                var beneficiarioValidator = new BeneficiarioValidator();
                var beneficiarioService = new BeneficiarioService(context, beneficiarioValidator);

                // Act
                var updatedBeneficiario = new BeneficiarioEntity { Id = beneficiarioId, Nome = "Beneficiario1 Atualizado", CNPJ = "12345678000195", Ativo = true };
                await beneficiarioService.UpdateBeneficiarioAsync(updatedBeneficiario);

                // Assert
                var result = await context.Beneficiarios.FindAsync(beneficiarioId);
                Assert.NotNull(result);
                Assert.Equal("Beneficiario1 Atualizado", result.Nome);
                Assert.Equal("12345678000195", result.CNPJ);
            }
        }

    }
}