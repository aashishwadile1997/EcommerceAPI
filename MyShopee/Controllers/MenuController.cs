using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShopee.Data;
using MyShopee.Models;
using MyShopee.Models.DTO;
using System.Net;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace MyShopee.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ApiResponse _response;
        public MenuItemController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            _response.result = _context.MenuItems;
            _response.HttpStatusCode = HttpStatusCode.OK;
            return Ok(_response);


        }
        [HttpGet("{Id:int}", Name = "GetMenuItem")]
        public async Task<IActionResult> GetMenuItem(int Id)
        {
            if (Id == 0)
            {
                _response.HttpStatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.result = _context.MenuItems.FirstOrDefault(s => s.Id == Id);

            if (_response.result == null)
            {
                _response.HttpStatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.HttpStatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpPost]

        public async Task<ActionResult<ApiResponse>> CreateMenu([FromForm] MenuItemCreateDTO menuItemCreateDTO)
        {
            try
            {
                if (ModelState.IsValid) {

                    if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
                    {
                        _response.HttpStatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }


                    string fileName = Path.GetFileName(menuItemCreateDTO.File.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await menuItemCreateDTO.File.CopyToAsync(stream);
                    }







                    MenuItem menuItem = new MenuItem()
                    {
                        Name = menuItemCreateDTO.Name,
                        Price = menuItemCreateDTO.Price,
                        Category = menuItemCreateDTO.Category,
                        SpecialTag = menuItemCreateDTO.SpecialTag,
                        Description = menuItemCreateDTO.Description,
                        Image = filePath
                    };
                    _context.Add(menuItem);
                    _context.SaveChanges();
                    _response.HttpStatusCode = HttpStatusCode.Created;
                    _response.result = menuItem;

                    return CreatedAtRoute("GetMenuItem", new { id = menuItem.Id }, _response);





                }
                else
                {
                    _response.IsSuccess = false;
                }


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }


        [HttpPut("{Id:int}")]

        public async Task<ActionResult<ApiResponse>> UpdateMenu(int Id, [FromForm] MenuUpdateDTO menuItemCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (menuItemCreateDTO == null || Id != menuItemCreateDTO.Id)

                    {
                        _response.HttpStatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }

                    var menuItemFromDb = await _context.MenuItems.FindAsync(Id);
                    if (menuItemFromDb == null)
                    {
                        _response.HttpStatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }


                    string fileName = Path.GetFileName(menuItemCreateDTO.File.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await menuItemCreateDTO.File.CopyToAsync(stream);
                    }



                    menuItemFromDb.Name = menuItemCreateDTO.Name;
                    menuItemFromDb.Price = menuItemCreateDTO.Price;
                    menuItemFromDb.Description = menuItemCreateDTO.Description;
                    menuItemFromDb.Category = menuItemCreateDTO.Category;
                    menuItemFromDb.SpecialTag = menuItemCreateDTO.SpecialTag;
                    menuItemFromDb.Image = filePath;





                    _context.Update(menuItemFromDb);
                    _context.SaveChanges();
                    _response.HttpStatusCode = HttpStatusCode.Created;
                    _response.result = menuItemFromDb;

                    return CreatedAtRoute("GetMenuItem", new { id = menuItemFromDb.Id }, _response);





                }
                else
                {
                    _response.IsSuccess = false;
                }


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.HttpStatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }

                MenuItem menuItemFromDb = await _context.MenuItems.FindAsync(id);
                if (menuItemFromDb == null)
                {
                    _response.HttpStatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                //await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(), SD.SD_Storage_Container);
                //int milliseconds = 2000;
                //Thread.Sleep(milliseconds);

                _context.MenuItems.Remove(menuItemFromDb);
                _context.SaveChanges();
                _response.HttpStatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage
                     = new List<string>() { ex.ToString() };
            }

            return _response;
        }

    }
    
}

