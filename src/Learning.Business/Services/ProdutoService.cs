using Learning.Business.Interfaces;
using Learning.Business.Models;
using Learning.Business.Models.Validations;

namespace Learning.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
        }

        public async Task Remover(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
