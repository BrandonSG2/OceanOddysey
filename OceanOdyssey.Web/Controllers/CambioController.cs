using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.Services.Interfaces;

namespace OceanOdyssey.Web.Controllers
{
    public class CambioController : Controller
    {
        private readonly IServiceCambio _serviceCambio;
        public CambioController(IServiceCambio serviceCambio)
        {
            _serviceCambio = serviceCambio;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pais = await _serviceCambio.ListAsync();
            return View(pais);
        }
    }
}
