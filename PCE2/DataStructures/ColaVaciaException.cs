namespace PCE2.DataStructures;

// Se lanza cuando se intenta Dequeue/Peek sobre una cola vacia (underflow).
public class ColaVaciaException : InvalidOperationException
{
    public ColaVaciaException()
        : base("La cola esta vacia (underflow).") { }
}
