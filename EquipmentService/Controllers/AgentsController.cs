using Microsoft.AspNetCore.Mvc;

namespace EquipmentService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class AgentsController : ControllerBase {
    public AgentsController() {
        
    }
    
    [HttpPost]
    public ActionResult testInboundConnection() {
        Console.WriteLine("--> received inbound connection from POST equipment service");
        return Ok("--> inbound test of from agents controller");
    }
}