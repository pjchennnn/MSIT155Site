using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT155Site.Models;
using System.Text;

namespace MSIT155Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        public ApiController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return Content("哈哈","text/plain", Encoding.UTF8);
        }

        public IActionResult First()
        {
            return View();
        }

        public IActionResult r()
        {
            int x = 0;
            int y = 0;
            int z = x / y;
            return Content(z.ToString());
        }

        public IActionResult Register(string name, int age = 28)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "guest";
            }
            return Content($"Hello {name}, {age}歲了", "text/plain", Encoding.UTF8);
        }

        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }
        public IActionResult Avatar(int id = 1)
        {
            Member? member = _context.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                if (img != null)
                {
                    return File(img, "image/jpeg");
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> CheckAccountAsync(string n)
        {
            if (string.IsNullOrEmpty(n))
            {
                return Content("未填寫姓名", "text/plain", Encoding.UTF8);
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Name == n);
            if (member == null)
            {
                return Content("帳號可使用", "text/plain", Encoding.UTF8);
            }
            return Content("帳號已存在", "text/plain", Encoding.UTF8);
        }
    }
}
