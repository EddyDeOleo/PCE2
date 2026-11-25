namespace PCE2.DataStructures;

// Contrato del TAD Cola (FIFO).
public interface ICola<T> : IEnumerable<T>
{
    int Cantidad { get; }
    int Capacidad { get; }
    bool EstaVacia { get; }

    // Inserta por el final.
    void Enqueue(T elemento);

    // Quita y devuelve el frente. Lanza ColaVaciaException si esta vacia.
    T Dequeue();

    // Devuelve el frente sin quitarlo. Lanza ColaVaciaException si esta vacia.
    T Peek();

    // Variantes seguras que no lanzan excepcion.
    bool TryDequeue(out T? elemento);
    bool TryPeek(out T? elemento);
}
