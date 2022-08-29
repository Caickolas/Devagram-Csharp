using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devagram_Csharp.Controllers
{
    [Authorize]
    public class BaseController :ControllerBase
    {
        
    }
}
