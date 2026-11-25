using System.Collections;

namespace PCE2.DataStructures;

public class Cola<T> : ICola<T>
{
    /*
     * Cola circular sobre un arreglo dinamico.
     * Invariantes:
     *  - _frente apunta al elemento mas antiguo (el proximo en salir).
     *  - Los datos validos ocupan _cantidad posiciones a partir de _frente,
     *    avanzando con modulo Length (por eso es "circular").
     *  - La posicion del final es (_frente + _cantidad) % Length.
     *  - 0 <= _cantidad <= Length; cola vacia <=> _cantidad == 0.
     * Al sacar un elemento solo se mueve _frente, asi el espacio liberado se
     * reutiliza y no hay que desplazar elementos como en un arreglo lineal.
     */
    private T[] _elementos;
    private int _frente;
    private int _cantidad;
    private readonly int _factorCrecimiento;

    public Cola(int capacidadInicial, int factorCrecimiento)
    {
        if (capacidadInicial <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacidadInicial), "Debe ser mayor que cero.");
        if (factorCrecimiento < 2)
            throw new ArgumentOutOfRangeException(nameof(factorCrecimiento), "Debe ser al menos 2.");

        _elementos = new T[capacidadInicial];
        _factorCrecimiento = factorCrecimiento;
    }

    public int Cantidad => _cantidad;
    public int Capacidad => _elementos.Length;
    public bool EstaVacia => _cantidad == 0;

    public void Enqueue(T elemento)
    {
        // Si el arreglo esta lleno se crece antes de insertar (evita overflow).
        if (_cantidad == _elementos.Length)
            Crecer();

        int posicionFinal = (_frente + _cantidad) % _elementos.Length;
        _elementos[posicionFinal] = elemento;
        _cantidad++;
    }

    public T Dequeue()
    {
        // Proteccion contra underflow.
        if (EstaVacia)
            throw new ColaVaciaException();

        T elemento = _elementos[_frente];

        // Se limpia la posicion para no retener referencias innecesarias.
        _elementos[_frente] = default!;

        // El frente avanza de forma circular: el espacio queda reutilizable.
        _frente = (_frente + 1) % _elementos.Length;
        _cantidad--;
        return elemento;
    }

    public T Peek()
    {
        if (EstaVacia)
            throw new ColaVaciaException();

        return _elementos[_frente];
    }

    public bool TryDequeue(out T? elemento)
    {
        if (EstaVacia)
        {
            elemento = default;
            return false;
        }

        elemento = Dequeue();
        return true;
    }

    public bool TryPeek(out T? elemento)
    {
        if (EstaVacia)
        {
            elemento = default;
            return false;
        }

        elemento = Peek();
        return true;
    }

    private void Crecer()
    {
        // Se copian los elementos en orden FIFO al inicio del nuevo arreglo,
        // de modo que _frente vuelve a 0. Costo amortizado de Enqueue: O(1).
        var nuevo = new T[_elementos.Length * _factorCrecimiento];
        for (int i = 0; i < _cantidad; i++)
            nuevo[i] = _elementos[(_frente + i) % _elementos.Length];

        _elementos = nuevo;
        _frente = 0;
    }

    // Recorre desde el frente hasta el final, es decir, en orden de llegada.
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _cantidad; i++)
            yield return _elementos[(_frente + i) % _elementos.Length];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
