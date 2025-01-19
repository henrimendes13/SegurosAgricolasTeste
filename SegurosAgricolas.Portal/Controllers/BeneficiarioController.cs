using Microsoft.AspNetCore.Mvc;
using SegurosAgricolas.Domain.Entities;
using SegurosAgricolas.Domain.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SegurosAgricolas.Portal.Controllers
{
    public class BeneficiarioController : Controller
    {
        private readonly IBeneficiarioService _beneficiarioService;

        public BeneficiarioController(IBeneficiarioService beneficiarioService)
        {
            _beneficiarioService = beneficiarioService ?? throw new ArgumentNullException(nameof(beneficiarioService));
        }

        public async Task<IActionResult> Index()
        {
            var beneficiarios = await _beneficiarioService.GetAllBeneficiariosAsync();
            return View(beneficiarios);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beneficiario = await _beneficiarioService.GetBeneficiarioByIdAsync(id.Value);
            if (beneficiario == null)
            {
                return NotFound();
            }

            return View(beneficiario);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CNPJ,Ativo")] BeneficiarioEntity beneficiarioEntity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _beneficiarioService.AddBeneficiarioAsync(beneficiarioEntity);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            return View(beneficiarioEntity);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beneficiario = await _beneficiarioService.GetBeneficiarioByIdAsync(id.Value);
            if (beneficiario == null)
            {
                return NotFound();
            }

            return View(beneficiario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,CNPJ,Ativo")] BeneficiarioEntity beneficiarioEntity)
        {
            if (id != beneficiarioEntity.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _beneficiarioService.UpdateBeneficiarioAsync(beneficiarioEntity);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Erro ao atualizar o beneficiário: {ex.Message}");
                    return View(beneficiarioEntity);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(beneficiarioEntity);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beneficiario = await _beneficiarioService.GetBeneficiarioByIdAsync(id.Value);
            if (beneficiario == null)
            {
                return NotFound();
            }

            return View(beneficiario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _beneficiarioService.DeleteBeneficiarioAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}