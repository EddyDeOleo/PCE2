namespace PCE2.Configuration;

// Parametros configurables del sistema. Ningun servicio ni estructura
// incrusta estos valores: los recibe por constructor.
public class OpcionesAtencion
{
    // Capacidad inicial del arreglo circular de la cola.
    public int CapacidadInicial { get; init; } = 4;

    // Cuando la cola se llena, la capacidad se multiplica por este factor.
    public int FactorCrecimiento { get; init; } = 2;

    // Numero del primer turno que se asigna.
    public int TurnoInicial { get; init; } = 1;

    // Longitud maxima aceptada para nombre y tramite.
    public int LongitudMaximaTexto { get; init; } = 60;
}
