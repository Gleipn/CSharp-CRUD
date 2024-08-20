using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class FornecedorDto
    {

        [Required, StringLength(100)]
        public string Nome { get; set; } = "";

        [Required, StringLength(14)]
        public string Cnpj { get; set; } = "";
        
        [Required]
        public string Segmento { get; set; } = "";

        [Required, StringLength(8)]
        public string Cep { get; set; } = "";

        [Required, StringLength(255)]
        public string Endereco { get; set; } = "";

        public IFormFile? Imagem { get; set; }

    }
}
