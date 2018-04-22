using FBru.Repository.Interfaces;
using FBru.WebAdmin.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FBru.WebAdmin.Controllers.Api
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
            var advertisementsDto = advertisements.ToList();
            advertisementsDto.ForEach(ad => ad.ImageUrl = Constrains.ServerUrl + ad.ImageUrl);
            return Ok(advertisementsDto);
        }
    }
}