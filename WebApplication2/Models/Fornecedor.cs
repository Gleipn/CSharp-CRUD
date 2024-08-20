using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; } = "";

        [StringLength(14)]
        public string Cnpj { get; set; } = "";

        public string Segmento { get; set; } = "";

        [StringLength(8)]
        public string Cep { get; set; } = "";

        [StringLength(255)]
        public string Endereco { get; set; } = "";

        [StringLength(255)]
        public string ImagemCaminho { get; set; } = "";

        public DateTime CreatedAt {  get; set; }
    }
}
