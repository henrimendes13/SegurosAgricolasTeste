using System.ComponentModel.DataAnnotations;
namespace SegurosAgricolas.Domain.Entities
{
    public class BeneficiarioEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? CNPJ { get; set; }
        public bool Ativo { get; set; }
    }
}
