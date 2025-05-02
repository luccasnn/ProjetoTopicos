using System.ComponentModel.DataAnnotations;

namespace CadastroApi.Models
{
    // Representa uma pessoa no sistema
    public class Pessoa
    {
        // Identificador único da pessoa
        public int Id { get; set; }

        // Nome da pessoa (obrigatório e com limite de 100 caracteres)
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        // Idade da pessoa (deve estar entre 0 e 150)
        [Range(0, 150, ErrorMessage = "A idade deve estar entre 0 e 150.")]
        public int Idade { get; set; }

        // E-mail da pessoa (obrigatório e deve ser um e-mail válido)
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string Email { get; set; } = string.Empty;

        // Identificador da cidade da pessoa (obrigatório)
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public int CidadeId { get; set; }

        // Relacionamento com a cidade onde a pessoa reside
        public Cidade? Cidade { get; set; }
    }
}
