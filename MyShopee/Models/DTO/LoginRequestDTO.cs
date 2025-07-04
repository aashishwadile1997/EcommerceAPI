using Microsoft.Extensions.Primitives;

namespace MyShopee.Models.DTO
{
    public class LoginRequestDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
