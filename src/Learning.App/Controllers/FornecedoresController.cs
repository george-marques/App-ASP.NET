using Microsoft.AspNetCore.Mvc;
using Learning.App.ViewModels;
using Learning.Business.Interfaces;
using AutoMapper;
using Learning.Business.Models;

namespace Learning.App.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;


        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        // GET: Fornecedores
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos()));
        }

        // GET: Fornecedores/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorDTO fornecedorDTO)
        {
            if (!ModelState.IsValid)
                return View(fornecedorDTO);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);
            await _fornecedorRepository.Adicionar(fornecedor);

            return RedirectToAction(nameof(Index));
        }

        // GET: Fornecedores/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorDTO fornecedorDTO)
        {
            if (id != fornecedorDTO.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(fornecedorDTO);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);
            await _fornecedorRepository.Atualizar(fornecedor);

            return RedirectToAction(nameof(Index));


        }

        // GET: Fornecedores/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorDTO = await ObterFornecedorEndereco(id);

            if (fornecedorDTO == null)
                return NotFound();

            return View(fornecedorDTO);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorDTO = await ObterFornecedorEndereco(id);
           
            if (fornecedorDTO == null)
                return NotFound();

            await _fornecedorRepository.Remover(id);
            
            return RedirectToAction(nameof(Index));
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
