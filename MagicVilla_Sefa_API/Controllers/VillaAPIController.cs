using MagicVilla_Sefa_API.Data;
using MagicVilla_Sefa_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Sefa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }
        [HttpGet("{id:int}")]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
                return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
                return NotFound();

            return Ok(villa);
        }
    }
}
