using PCE2.Application;
using PCE2.Domain;

namespace PCE2.Presentation;

public class MenuConsola
{
    private readonly AtencionBancariaService _servicio;
    private readonly ConsolaIO _io;

    public MenuConsola(AtencionBancariaService servicio, ConsolaIO io)
    {
        _servicio = servicio;
        _io = io;
    }

    public void Ejecutar()
    {
        bool salir = false;

        // Bucle principal: se repite hasta que el usuario elige salir.
        while (!salir)
        {
            MostrarMenu();
            string? opcion = _io.Leer("Selecciona una opcion: ")?.Trim();
            _io.Linea();

            switch (opcion)
            {
                case "1": RegistrarLlegada(); break;
                case "2": MostrarResultado(_servicio.LlamarSiguiente()); break;
                case "3": MostrarResultado(_servicio.FinalizarAtencion()); break;
                case "4": MostrarFila(); break;
                case "5": MostrarEstado(); break;
                case "6": salir = true; _io.Escribir("Hasta luego."); continue;
                default: _io.Escribir("Opcion no valida. Elige un numero del 1 al 6."); break;
            }

            _io.Pausa();
        }
    }

    private void MostrarMenu()
    {
        _io.Limpiar();
        _io.Escribir("=== Atencion al cliente bancario (Cola FIFO) ===");
        _io.Escribir($"En atencion : {Formatear(_servicio.ClienteEnAtencion)}");
        _io.Escribir($"Siguiente   : {Formatear(_servicio.ProximoEnFila)}");
        _io.Escribir($"En espera   : {_servicio.Cantidad}");
        _io.Linea();
        _io.Escribir("1. Registrar llegada de cliente");
        _io.Escribir("2. Llamar al siguiente cliente");
        _io.Escribir("3. Finalizar atencion actual");
        _io.Escribir("4. Ver clientes en espera");
        _io.Escribir("5. Ver cliente en atencion y siguiente");
        _io.Escribir("6. Salir");
        _io.Linea();
    }

    private void RegistrarLlegada()
    {
        string? nombre = _io.Leer("Nombre del cliente: ");
        string? tramite = _io.Leer("Tramite: ");

        MostrarResultado(_servicio.RegistrarLlegada(nombre, tramite));
    }

    private void MostrarFila()
    {
        var espera = _servicio.ObtenerEnEspera();

        if (espera.Count == 0)
        {
            _io.Escribir("No hay clientes en fila.");
            return;
        }

        _io.Escribir("Clientes en espera (del frente al final):");
        for (int i = 0; i < espera.Count; i++)
        {
            string marca = i == 0 ? "  <-- FRENTE (proximo)" : string.Empty;
            _io.Escribir($"  {i + 1}. {Formatear(espera[i])}{marca}");
        }

        _io.Linea();
        _io.Escribir($"Clientes: {_servicio.Cantidad} | Capacidad interna: {_servicio.Capacidad}");
    }

    private void MostrarEstado()
    {
        _io.Escribir($"En atencion : {Formatear(_servicio.ClienteEnAtencion)}");
        _io.Escribir($"Siguiente   : {Formatear(_servicio.ProximoEnFila)}");
    }

    private void MostrarResultado(ResultadoOperacion resultado)
    {
        _io.Escribir(resultado.Mensaje);

        if (resultado.Cliente is not null)
            _io.Escribir($"Cliente: {Formatear(resultado.Cliente)}");
    }

    private static string Formatear(Cliente? c) =>
        c is null
            ? "(ninguno)"
            : $"Turno #{c.Turno} | {c.Nombre} | {c.Tramite} | llegada {c.HoraLlegada:HH:mm:ss}";
}
