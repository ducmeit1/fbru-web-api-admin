using FBru.Repository.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;
using WebGrease.Css.Extensions;

namespace FBru.WebAPI.Controllers
{
    [RoutePrefix("api")]
    public class AdvertisementsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdvertisementsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("advertisements")]
        public async Task<IHttpActionResult> Get()
        {
            var advertisements = await _unitOfWork.Advertisements.GetAdvertisementsWithSimpleDetailOrdered();
            advertisements.ForEach(c => c.ImageUrl = Constants.FBruServer + c.ImageUrl);
            return Ok(advertisements);
        }
    }
}