namespace PCE2.Presentation;

// Unico punto que toca Console: facilita pruebas y cambios de interfaz.
public class ConsolaIO
{
    public void Escribir(string texto) => Console.WriteLine(texto);

    public void Linea() => Console.WriteLine();

    public string? Leer(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    public void Pausa()
    {
        Console.Write("\nPresiona Enter para continuar...");
        Console.ReadLine();
    }

    public void Limpiar()
    {
        // Console.Clear falla si la salida esta redirigida.
        if (!Console.IsOutputRedirected)
            Console.Clear();
    }
}
