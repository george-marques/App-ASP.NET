namespace Learning.Business.Models

{
    public class Produto : Entity
    {
        public Guid FornecedorId { get; set; }

        public string Nome { get; set; }
             
        public string Descricao { get; set; }

        public string Imagem { get; set; }
        
        public decimal Valor { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        //Entity Core - Relations
        public Fornecedor Fornecedor { get; set; }
    }
}
