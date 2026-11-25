using PCE2.Domain;

namespace PCE2.Application;

// Resultado de una operacion del servicio: la capa de presentacion
// decide como mostrarlo, el servicio no imprime nada.
public sealed record ResultadoOperacion(
    bool Exito,
    string Mensaje,
    Cliente? Cliente);
