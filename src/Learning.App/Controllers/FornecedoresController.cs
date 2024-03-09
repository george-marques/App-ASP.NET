using Microsoft.AspNetCore.Mvc;
using Learning.App.ViewModels;
using Learning.Business.Interfaces;
using AutoMapper;
using Learning.Business.Models;

namespace Learning.App.Controllers
{
    [Route("")]
    [Route("Fornecedores")]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;


        public FornecedoresController(
            IFornecedorRepository fornecedorRepository, 
            IFornecedorService fornecedorService, 
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        // GET: Fornecedores
        [Route("lista-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos()));
        }

        // GET: Fornecedores/Details/id
        [Route("detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorDTO = await ObterFornecedorEndereco(id);

            if (fornecedorDTO == null)
            {
                return NotFound();
            }

            return View(fornecedorDTO);
        }

        // GET: Fornecedores/Create
        [Route("adicionar")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedores/Create
        [Route("adicionar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorDTO fornecedorDTO)
        {
            if (!ModelState.IsValid)
                return View(fornecedorDTO);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);
            await _fornecedorService.Adicionar(fornecedor);

            if (!OperacaoValida()) return View(fornecedorDTO);

            TempData["Success"] = "Fornecedor cadastrado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Fornecedores/Edit/5
        [Route("editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorDTO = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorDTO == null)
            {
                return NotFound();
            }

            return View(fornecedorDTO);
        }

        // POST: Fornecedores/Edit/5
        [Route("editar/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorDTO fornecedorDTO)
        {
            if (id != fornecedorDTO.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(fornecedorDTO);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);
            await _fornecedorService.Atualizar(fornecedor);

            if (!OperacaoValida()) return View(await ObterFornecedorProdutosEndereco(id));

            TempData["Success"] = "Produto editado com sucesso.";

            return RedirectToAction(nameof(Index));


        }

        // GET: Fornecedores/Delete/5
        [Route("excluir/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorDTO = await ObterFornecedorEndereco(id);

            if (fornecedorDTO == null)
                return NotFound();

            return View(fornecedorDTO);
        }

        // POST: Fornecedores/Delete/5
        [Route("excluir/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorDTO = await ObterFornecedorEndereco(id);

            if (fornecedorDTO == null)
                return NotFound();

            await _fornecedorService.Remover(id);
            if (!OperacaoValida()) return View(fornecedorDTO);

            return RedirectToAction(nameof(Index));
        }

        // GET: Endereco
        [Route("atualizar-endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
                return NotFound();

            return PartialView("_ModalEditEndereco", new FornecedorDTO { Endereco = fornecedor.Endereco });
        }

        [Route("atualizar-endereco/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarEndereco(FornecedorDTO fornecedorDTO)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");
            ModelState.Remove("Produtos");

            if (!ModelState.IsValid)
                return PartialView("_ModalEditEndereco", fornecedorDTO);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorDTO.Endereco));
            
            if (!OperacaoValida()) return PartialView("_ModalEditEndereco", fornecedorDTO);

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorDTO.Endereco.FornecedorId });
            return Json(new { success = true, url });
        }

        [Route("obter-endereco/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
                return NotFound();

            return PartialView("_DetailsEndereco", fornecedor);
        }

        private async Task<FornecedorDTO> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        private async Task<FornecedorDTO> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}
