using EventManagement.DTOs;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class IdCardController : ControllerBase
    {
        private readonly IdCardPdfService _pdfService;

        public IdCardController(IdCardPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpGet("test")]
        public IActionResult Test() => Ok("PDF service is working!");

        [HttpPost("generate")]
        public IActionResult GeneratePdf([FromBody] List<IdCardDto> cards)
        {
            if (cards == null || !cards.Any())
                return BadRequest("No cards provided.");

            var items = cards.Select(c => (dto: c, photo: (byte[]?)null)).ToList();
            byte[] pdfBytes = _pdfService.GenerateA3IdCards(items);

            return File(pdfBytes, "application/pdf", "IdCards.pdf");
        }
    }
}
