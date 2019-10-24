using System.Linq;
using ejemplomvc.Models;
using Microsoft.AspNetCore.Mvc;
using Nomina.Models;

namespace Nomina.Controllers
{
    public class EmpleadoController : Controller
    {
        //db es una variable de lectura en donde se almacena la referencia al objeto contexto de la base de datos
        private readonly LiteDbContext db;

        //inyeccion del contexto de la base de datos
        public EmpleadoController(LiteDbContext db)
        {
            this.db = db;
        }

        /*
            Muestra la pagina principal de "Empleados"
            notation: [HttpGet] indica que se realizar una peticion a la aplicacion desde el navegador
         */
        [HttpGet]
        public IActionResult Index()
        {
            /* 
                obtengo la lista de empleados con id "nomina" de la base de datos, 
                ojo que el método GetCollection retorna una coleccion del tipo "LiteCollection" no "IEnumerable"
            */
            var empleados = db.Context.GetCollection<Empleado>("nomina");

            /*
                Guardo en el ViewBag la cantidad de empleados que se encuentra en la lista
                El ViewBag es una memoria compartida entre el Controller y la View, para guardar y acceder se
                debe realizar mediante un identificador, la forma de uso es ViewBag."Identificador" se puede almacenar
                cualquier objeto
             */
            ViewBag.CantidadEmpleados = empleados.Count();

            /*
                Retorna la vista con nombre "Index" con el contenido de la lista empleados con
                tipo de dato IEnumerable
             */
            return View("Index", empleados.FindAll());
        }

        /*
            Muestra pantalla para agregar un nuevo empleado
            notation: [HttpGet] indica que se realiza una peticion a la aplicacion desde el navegador
         */
        [HttpGet]
        public IActionResult Agregar()
        {
             /*
                Retorna la vista con nombre "Agregar" la cual permite la carga de datos de un empleado
             */
            return View("Agregar");
        }

        /*
            Realiza la accion de procesar el "empleado" nuevo que se crea desde la pantalla "Agregar"
            notation: [HttpPost] indica que se realiza una peticion desde el navegador con informacion adicional
            en este caso la informacion será el objeto Empleado, notar que la informacion que carga desde la pantalla "Agregar"
            se encapsula en un objeto de tipo Empleado
         */
        [HttpPost]
        public IActionResult Agregar(Empleado empleado)
        {            
            var empleados = db.Context.GetCollection<Empleado>("nomina");

            /*
                Insert: método propio de la biblioteca LiteDB, permite guardar un objeto de tipo Empleado
                en la base de datos
             */
            empleados.Insert(empleado);

            /*
                Redirecciona la vista "Index" con el contenido de la lista empleados con tipo de datos
                IEnumerable, se usa este método de redireccionamiento en vez de View() para que cambie 
                la refencia en la barra de direcciones
             */
            return RedirectToAction("Index", empleados.FindAll());
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var empleados = db.Context.GetCollection<Empleado>("nomina").FindAll();

            /*
                Con el método FirstOrDefault busco el primer elemento que coicida con el "id" se pasa por parametro
                El "id" viene dado por el enlace que se genera al momento de procesar el listado de empleados
                en la vista "Index"
             */
            var empleado = empleados.FirstOrDefault(x => x.id == id);

            return View("Editar", empleado);
        }

        [HttpPost]
        public IActionResult Editar(Empleado empleado)
        {            
            var empleados = db.Context.GetCollection<Empleado>("nomina");

             /*
                Update: método propio de la biblioteca LiteDB, permite actualizar un objeto de tipo Empleado
                en la base de datos, como el Id viene incluido en el objeto no se necesita buscar por el mismo
                LiteDB automaticamente infiere que se trata de ese objeto y actualiza el resto de campos
             */
            empleados.Update(empleado);

            return RedirectToAction("Index", empleados.FindAll());
        }

        public IActionResult Eliminar (int id)
        {
            var empleados = db.Context.GetCollection<Empleado>("nomina");

             /*
                Delete: método propio de la biblioteca LiteDB, permite eliminar un objeto de tipo Empleado
                en la base de datos, en este caso como se tiene el objeto, pero si el Id y como este es unico
                procedo a borrar todos los objetos que posean el mismo Id.
             */
            empleados.Delete(x => x.id == id);

            return RedirectToAction("Index", empleados.FindAll());
        }
    }
}