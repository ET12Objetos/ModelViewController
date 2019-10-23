using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Nomina.Models;

namespace Nomina.Controllers
{
    public class EmpleadoController : Controller
    {
        LiteDatabase db;

        public EmpleadoController()
        {
            db = new LiteDatabase(@"nomina.db");            
        }

        public IActionResult Index()
        {
            var empleados = db.GetCollection<Empleado>("nomina");

            return View("Index", empleados.FindAll());
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            return View("Agregar");
        }

        [HttpPost]
        public IActionResult Agregar(Empleado empleado)
        {            
            var empleados = db.GetCollection<Empleado>("nomina");

            empleados.Insert(empleado);

            return View("Index", empleados.FindAll());
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var empleados = db.GetCollection<Empleado>("nomina").FindAll();

            var empleado = empleados.FirstOrDefault(x => x.id == id);

            return View("Editar", empleado);
        }

        [HttpPost]
        public IActionResult Editar(Empleado empleado)
        {            
            var empleados = db.GetCollection<Empleado>("nomina");

            empleados.Update(empleado);

            return View("Index", empleados.FindAll());
        }



        public IActionResult Eliminar (int id)
        {
            var empleados = db.GetCollection<Empleado>("nomina");

            empleados.Delete(x => x.id == id);

            //return View("Index", empleados.FindAll());
            return RedirectToAction("Index", empleados.FindAll());
        }
    }
}