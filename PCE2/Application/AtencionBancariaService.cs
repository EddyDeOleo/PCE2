using PCE2.Configuration;
using PCE2.DataStructures;
using PCE2.Domain;

namespace PCE2.Application;

public class AtencionBancariaService
{
    private readonly ICola<Cliente> _fila;
    private readonly OpcionesAtencion _opciones;
    private int _siguienteTurno;

    // Se depende de la abstraccion ICola (inversion de dependencias).
    public AtencionBancariaService(ICola<Cliente> fila, OpcionesAtencion opciones)
    {
        _fila = fila;
        _opciones = opciones;
        _siguienteTurno = opciones.TurnoInicial;
    }

    // Cliente que esta siendo atendido en ventanilla (fuera de la fila).
    public Cliente? ClienteEnAtencion { get; private set; }

    public int Cantidad => _fila.Cantidad;
    public int Capacidad => _fila.Capacidad;

    // Peek: el siguiente en el frente, sin retirarlo de la fila.
    public Cliente? ProximoEnFila =>
        _fila.TryPeek(out var proximo) ? proximo : null;

    public ResultadoOperacion RegistrarLlegada(string? nombre, string? tramite)
    {
        string? error = ValidarTexto(nombre, "nombre", exigirLetra: true)
                        ?? ValidarTexto(tramite, "tramite", exigirLetra: false);
        if (error is not null)
            return new ResultadoOperacion(false, error, null);

        var cliente = new Cliente(_siguienteTurno++, nombre!.Trim(), tramite!.Trim(), DateTime.Now);

        // ENQUEUE: el cliente se coloca al final de la fila (FIFO).
        _fila.Enqueue(cliente);

        return new ResultadoOperacion(true, $"Cliente registrado. Posicion en fila: {_fila.Cantidad}.", cliente);
    }

    public ResultadoOperacion LlamarSiguiente()
    {
        // Underflow controlado: no se intenta Dequeue sobre una fila vacia.
        if (_fila.EstaVacia)
            return new ResultadoOperacion(false, "No hay clientes en espera: la fila esta vacia.", ClienteEnAtencion);

        // Si habia un cliente en ventanilla, se da por terminado al llamar al siguiente.
        string aviso = ClienteEnAtencion is null
            ? string.Empty
            : $"Se finalizo la atencion del turno #{ClienteEnAtencion.Turno}. ";

        // DEQUEUE: sale el cliente que llego primero.
        ClienteEnAtencion = _fila.Dequeue();

        return new ResultadoOperacion(true, $"{aviso}Llamando al siguiente cliente.", ClienteEnAtencion);
    }

    public ResultadoOperacion FinalizarAtencion()
    {
        if (ClienteEnAtencion is null)
            return new ResultadoOperacion(false, "No hay ningun cliente en atencion.", null);

        var terminado = ClienteEnAtencion;
        ClienteEnAtencion = null;

        return new ResultadoOperacion(true, $"Atencion finalizada para el turno #{terminado.Turno}.", terminado);
    }

    // Copia ordenada de frente a final (orden de llegada).
    public IReadOnlyList<Cliente> ObtenerEnEspera() => _fila.ToList();

    private string? ValidarTexto(string? valor, string campo, bool exigirLetra)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return $"El {campo} no puede estar vacio.";

        if (valor.Trim().Length > _opciones.LongitudMaximaTexto)
            return $"El {campo} no puede superar {_opciones.LongitudMaximaTexto} caracteres.";

        if (exigirLetra && !valor.Any(char.IsLetter))
            return $"El {campo} debe contener al menos una letra.";

        return null;
    }
}
