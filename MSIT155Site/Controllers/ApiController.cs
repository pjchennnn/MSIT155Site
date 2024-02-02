using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using MSIT155Site.Models;
using MSIT155Site.Models.DTO;
using NuGet.Protocol.Plugins;
using System.Security.Policy;
using System.Text;

namespace MSIT155Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _webHost;
        public ApiController(MyDBContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
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

        public IActionResult Register(user_DTO user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                user.Name = "guest";
            }

            string filePath = "";
            if (user.Avatar != null)
            {
                filePath = Path.Combine(_webHost.WebRootPath, "uploads", user.Avatar.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    user.Avatar.CopyTo(fileStream);
                }
            }
            return Content($"Hello {user.Name}, {user.Age}歲了，電子郵件為:{user.Email}，頭像路徑為:{filePath}", "text/plain", Encoding.UTF8);
        }

        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }

        public IActionResult District(string city)
        {
            var district = _context.Addresses.Where(a => a.City == city).Select(c=> c.SiteId).Distinct();
            return Json(district);
        }

        public IActionResult Road(string district)
        {
            var road = _context.Addresses.Where(a => a.SiteId == district).Select(c => c.Road);
            return Json(road);
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

        public IActionResult Address()
        {
            return View();
        }

        public IActionResult search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Spots([FromBody] searchDTO search)
        {
            //搜尋
            var spots = search.categoryId == 0? _context.SpotImagesSpots : _context.SpotImagesSpots.Where(s => s.CategoryId == search.categoryId);
            
            if (!string.IsNullOrEmpty(search.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(search.keyword) || s.SpotDescription.Contains(search.keyword));
            }
            
            //排序
            switch(search.sortBy)
            {
                case "spotTitle":
                    spots = search.sortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending(s => s.SpotTitle);
                    break;
                default:
                    spots = search.sortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending(s => s.SpotId);
                    break;
            }

            //分頁
            int TotalCount = spots.Count();
            int PageSize = search.pageSize ?? 10;
            int currentPage = search.page ?? 1;
            int TotalPage = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
            spots = spots.Skip((currentPage-1) * PageSize).Take(PageSize);

            //API回傳資料
            SpotPagingDTO sp = new SpotPagingDTO();
            sp.TotalPages = TotalPage;
            sp.TotalCount = TotalCount;
            sp.SpotResult = spots.ToList();

            return Json(sp);
        }

        public IActionResult spotTitleSearch(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var titles = _context.SpotImagesSpots.Where(s => s.SpotTitle.Contains(keyword)).Select(s => s.SpotTitle).Take(8);
                return Json(titles);
            }
            return Json(null);
        }

        public IActionResult Cors()
        {
            return View();
        }
    }
}
