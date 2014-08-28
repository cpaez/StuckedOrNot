using Stucked.Infrastructure;
using System.Web.Http;

namespace Stucked.API.Controllers
{
    public class BaseController : ApiController
    {
        protected static readonly ILogger _logger = LogManager.GetLogger(typeof(CustomLogger));
    }
}
