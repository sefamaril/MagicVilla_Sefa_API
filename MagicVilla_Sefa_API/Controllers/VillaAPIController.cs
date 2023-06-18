using MagicVilla_Sefa_API.Data;
using MagicVilla_Sefa_API.Logging;
using MagicVilla_Sefa_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Sefa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController] //özniteliği(attribute), bir Controller sınıfının API controller'ı olarak davranmasını sağlar ve API geliştirme için bir dizi kullanışlı özellik sağlar.
                    //Bu özniteliği eklemesekte methodlarımız çalışır; aşağıdaki gibi önemli avantajlar sağlar
                    //Model Doğrulama: [ApiController] özniteliği, otomatik model durum doğrulamasını etkinleştirir. Yani, bir HTTP isteği geldiğinde ve bu istek modelle eşleştirildiğinde, modelin durumu otomatik olarak doğrulanır ve eğer model durumu geçerli değilse,
                    //HTTP 400 Bad Request yanıtı otomatik olarak döndürülür.Bu özellik, if (!ModelState.IsValid) kontrolünü yapmanızı gereksiz kılar.

    //Ayrıca Modelimizdeki(DTO) MaxLength ve Required gibi özellikleri tanımasını sağlar [ApiController]

    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;


        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.Log("Getting all villas","");
            return Ok(VillaStore.villaList);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
               _logger.Log("Get Villa Error with Id " + id,"error");
                return BadRequest();
            }
               
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
                return NotFound();

            return Ok(villa);
        }

        [HttpPost]
        public ActionResult<VillaDto> CreateVilla(VillaDto villaDto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
                return BadRequest(villaDto);
            if (villaDto.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);
            villaDto.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);

            return Ok(villaDto);
        }
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
                return NotFound();

            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "DeleteVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
                return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDto.Name;
            villa.Sqft = villaDto.Sqft;
            villa.Occupancy = villaDto.Occupancy;
            return NoContent();
        }


        //Test comment added














        /*[HttpPost]
        public ActionResult<VillaDto> CreateVilla(VillaDto villaDto)

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)

        ActionResult<VillaDto>: Bu ifade, API çağrısının bir VillaDto nesnesi döndüreceğini belirtir. Bu, genellikle bir GET veya POST işlemi sonrasında geri döndürülen veri tipini belirtmek için kullanılır. 
        ActionResult<T> kullanmak, bir model nesnesini ve durum kodunu birlikte döndürmeyi kolaylaştırır.
        Örneğin, bir POST isteğinde (CreateVilla), genellikle başarılı bir işlem sonrası oluşturulan yeni veriyi (bu durumda bir VillaDto nesnesi) ve 201 Created HTTP durum kodunu döndürmek istersiniz.
        
        IActionResult: Bu ifade, API çağrısının herhangi bir türde bir nesne döndürebileceğini belirtir. Bu, çeşitli durum kodlarını ve içerik türlerini döndürme esnekliği sağlar.
        Örneğin, bir DELETE isteğinde (DeleteVilla), genellikle silme işlemi başarılı olduktan sonra bir içerik döndürmeye ihtiyaç duymazsınız, sadece durum kodunu (örneğin, 204 No Content) döndürürsünüz. Bu durumda, IActionResult kullanmak daha uygundur.
        */
    }
}
