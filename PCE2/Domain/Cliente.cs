namespace PCE2.Domain;

// Modelo inmutable de un cliente en fila. Todos los valores provienen
// del usuario o del servicio (el turno), nunca de datos fijos.
public sealed record Cliente(
    int Turno,
    string Nombre,
    string Tramite,
    DateTime HoraLlegada);
