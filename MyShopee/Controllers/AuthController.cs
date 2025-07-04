using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShopee.Data;
using MyShopee.Models;

namespace MyShopee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ApiResponse _response;
        public AuthController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _response = new ApiResponse();
        }
    }
}
