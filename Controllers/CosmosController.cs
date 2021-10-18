using AZURE_FINAL.Models;
using AZURE_FINAL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZURE_FINAL.Controllers
{
    public class CosmosController : Controller
    {
        ServiceCosmosDb service;
        public CosmosController(ServiceCosmosDb service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(String accion)
        {
            await this.service.CrearBbddPeliculaAsync();
            await this.service.CrearColeccionPeliculasAsync();
            List<Pelicula> peliculas = this.service.CrearPeliculas();
            foreach (Pelicula v in peliculas)
            {
                await this.service.InsertarPelicula(v);
            }
            ViewBag.mensaje = "CREADO";
            return View();
        }
        public IActionResult ListPeliculas()
        {
            return View(this.service.GetPeliculas());
        }
         public IActionResult Docente()
        {
            return View(this.service.GetPeliculas());
        }


        
        public async Task<IActionResult> Detalles(String id)
        {
            return View(await this.service.FindPeliculaAsyn(id));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Pelicula peli)
        {
            await this.service.InsertarPelicula(peli);
            return RedirectToAction("Docente");
        }
        public async Task<IActionResult> Delete(String id)
        {
            await this.service.EliminarPelicula(id);
            return RedirectToAction("ListPeliculas");
        }
        public async Task<IActionResult> Editar(String id)
        {
            return View(await this.service.FindPeliculaAsyn(id));
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Pelicula peli)
        {           
            await this.service.ModificarPelicula(peli);
            return RedirectToAction("ListPeliculas");
        }

        


    }
}
