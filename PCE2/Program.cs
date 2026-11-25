using PCE2.Application;
using PCE2.Configuration;
using PCE2.DataStructures;
using PCE2.Domain;
using PCE2.Presentation;

namespace PCE2
{

    public class Program
    {
        public static void Main(string[] args)
        {
            // Program.cs solo compone las dependencias (composition root) y arranca el menu.
            // No contiene logica de negocio.
            var opciones = new OpcionesAtencion();
            var cola = new Cola<Cliente>(opciones.CapacidadInicial, opciones.FactorCrecimiento);
            var servicio = new AtencionBancariaService(cola, opciones);
            var menu = new MenuConsola(servicio, new ConsolaIO());

            menu.Ejecutar();
        }
    }
}