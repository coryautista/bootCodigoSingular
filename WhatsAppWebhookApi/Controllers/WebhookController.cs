using Microsoft.AspNetCore.Mvc;

namespace WhatsAppWebhookApi.Controllers;

[ApiController]
public class WebhookController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public WebhookController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    //DENTRO DE LA RUTA webhook
    [Route("webhook")]
    //RECIBIMOS LOS PARAMETROS QUE NOS ENVIA WHATSAPP PARA VALIDAR NUESTRA URL
    public IActionResult Webhook(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.challenge")] string challenge,
        [FromQuery(Name = "hub.verify_token")] string verify_token
    )
    {
        var expectedToken = _configuration["Webhook:VerifyToken"];

        //SI EL TOKEN ES CORRECTO Y EL MODO ES subscribe (VERIFICACION DE WHATSAPP)
        if (mode == "subscribe" && verify_token == expectedToken)
        {
            return Ok(challenge);
        }
        else
        {
            return Forbid();
        }
    }
}
