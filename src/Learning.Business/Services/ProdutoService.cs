using Learning.Business.Interfaces;
using Learning.Business.Models;
using Learning.Business.Models.Validations;

namespace Learning.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(
            IProdutoRepository produtoRepository,
            INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
        }

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

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
