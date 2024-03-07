using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Learning.App.ViewModels;
using AutoMapper;
using Learning.Business.Interfaces;
using Learning.Business.Models;

namespace Learning.App.Controllers
{
    [Route("")]
    [Route("Produtos")]
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;

        }

        // GET: Produtos
        [Route("lista-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.ObterProdutosFornecedores()));
        }

        // GET: Produtos/Details/5
        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoDTO = await ObterProduto(id);

            if (produtoDTO == null)
            {
                return NotFound();
            }

            return View(produtoDTO);
        }

        // GET: Produtos/Create
        [Route("adicionar")]
        public async Task<IActionResult> Create()
        {
            var ProdutoDTO = await PopularFornecedores(new ProdutoDTO());
            return View(ProdutoDTO);
        }

        // POST: Produtos/Create
        [Route("adicionar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoDTO produtoDTO)
        {
            produtoDTO = await PopularFornecedores(produtoDTO);

            if (!ModelState.IsValid)
                return View(produtoDTO);

            var imgPrefixo = Guid.NewGuid() + "_";

            if(!await UploadArquivo(produtoDTO.ImagemUpload, imgPrefixo)){
                return View(produtoDTO);
            }

            produtoDTO.Imagem = imgPrefixo + produtoDTO.ImagemUpload.FileName;

            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoDTO));

            return RedirectToAction(nameof(Index));
        }

        // GET: Produtos/Edit/5
        [Route("editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoDTO = await ObterProduto(id);

            if (produtoDTO == null)
            {
                return NotFound();
            }

            return View(produtoDTO);
        }

        // POST: Produtos/Edit/5
        [Route("editar/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
                return NotFound();

            var produtoAtualizado = await ObterProduto(id);
            produtoDTO.Fornecedor = produtoAtualizado.Fornecedor;
            produtoDTO.Imagem = produtoAtualizado.Imagem;

            if (!ModelState.IsValid)
                return View(produtoDTO);

            if(produtoDTO.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!await UploadArquivo(produtoDTO.ImagemUpload, imgPrefixo))
                {
                    return View(produtoDTO);
                }

                produtoAtualizado.Imagem = imgPrefixo + produtoDTO.ImagemUpload.FileName;
            }

            produtoAtualizado.Nome = produtoDTO.Nome;
            produtoAtualizado.Descricao = produtoDTO.Descricao;
            produtoAtualizado.Valor = produtoDTO.Valor;
            produtoAtualizado.Ativo = produtoDTO.Ativo;

            await _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoAtualizado));

            return RedirectToAction(nameof(Index));
        }

        // GET: Produtos/Delete/5
        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoDTO = await ObterProduto(id);

            if (produtoDTO == null)
            {
                return NotFound();
            }

            return View(produtoDTO);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoDTO = await ObterProduto(id);

            if (produtoDTO == null)
            {
                return NotFound();
            }

            await _produtoRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoDTO> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterProdutoFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos());

            return produto;
        }

        private async Task<ProdutoDTO> PopularFornecedores(ProdutoDTO produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos());

            return produto;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string prefixo)
        {

            if(arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images", prefixo + arquivo.FileName);

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;

        }
    }
}
