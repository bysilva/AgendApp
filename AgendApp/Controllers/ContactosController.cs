using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AgendApp.Models;
using AgendApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AgendApp.Controllers
{
    public class ContactosController : Controller
    {
        IContactos contactoService;
        public ContactosController(IConfiguration configuration)
        {
            contactoService = new ContactoService(configuration["ConnectionStrings:DefaultConnection"]);
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetContactos()
        {
            return Json(contactoService.GetContactos());
        }
        public JsonResult AddContacto(ContactoModel contacto)
        {
            try
            {
                contactoService.AddContacto(contacto);
                return Json("ok");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
        }
    }
}