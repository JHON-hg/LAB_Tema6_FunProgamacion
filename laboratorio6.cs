using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static string archivoEquipos = "equipos_fund.txt";
    static string archivoPrestamos = "prestamos_fund.txt";

    static void Main()
    {
        int opc = 0;
        string confirmacion = "";

        Console.WriteLine("===== Laboratorio Universitario =====");
        Console.Write("Es estudiante de la universidad? (s/n): ");
        confirmacion = Console.ReadLine();

        if (confirmacion == "s" || confirmacion == "S")
        {
            do
            {
                MostrarMenuPrincipal();
                Console.Write("Elige una opcion: ");
                opc = int.Parse(Console.ReadLine());

                switch (opc){
                    case 1: RegistrarEquipo(); break;
                    case 2: RegistrarPrestamo(); break;
                    case 3: DevolverEquipo(); break;
                    case 4: ListarDisponibilidad(); break;
                    case 5: ContarEquipos(); break;
                    case 0: Console.WriteLine("programa finalizado..."); break;
                    default: Console.WriteLine("opcion no valida..."); break;
                }
            } while (opc != 0);
        }
        else{
            Console.WriteLine("No puedes usar el programa...");
        }
    }

// =================================================================================
// =================================================================================
    static void MostrarMenuPrincipal(){
        Console.WriteLine("===== Prestamos de Equipos =====");
        Console.WriteLine("1. Registrar equipo");
        Console.WriteLine("2. Registrar un prestamo");
        Console.WriteLine("3. Devolver equipo");
        Console.WriteLine("4. Listar disponibilidad");
        Console.WriteLine("5. Equipos disponibles por categoria");
        Console.WriteLine("0. Salir");
    }
// =================================================================================
// =================================================================================
    static bool ValidarDato(string dato){
        if (dato == null || dato.Trim() == "" || dato.Contains(","))
        {
            return false;
        }
        return true;
    }
// =================================================================================
// =================================================================================
    static string LeerDato(string mensaje){
        string dato = "";
        do{
            Console.Write(mensaje);
            dato = Console.ReadLine();

            if (!ValidarDato(dato)){
                Console.WriteLine("Dato vacio...");
                Console.WriteLine("intente de Nuevo...");
            }
        } while (!ValidarDato(dato));

        return dato.Trim();
    }
// =================================================================================
// =================================================================================
    public static void RegistrarEquipo()
    {
        string id, nombre, categoria, confirmacion = "";

        id = LeerDato("Ingresar Codigo: ");
        nombre = LeerDato("Ingresar Nombre: ");
        categoria = LeerDato("Ingresar Categoria: ");
        Console.Write("Esta disponible? (si/no): ");
        confirmacion = Console.ReadLine();
        while (confirmacion != "si" && confirmacion != "no")
        {
            Console.Write("Escribe si o no: ");
            confirmacion = Console.ReadLine();
        }

        string registro = id + "," + nombre + "," + categoria + "," + confirmacion;

        using (var archivo = new StreamWriter(archivoEquipos, true, Encoding.UTF8))
        {
            archivo.WriteLine(registro);
        }
        Console.WriteLine("equipo guardado...");
    }
// =================================================================================
// =================================================================================
public static void DevolverEquipo()
{
    string id = LeerDato("Codigo del equipo a devolver: ");
    if (!File.Exists(archivoPrestamos)){
        Console.WriteLine("Prestamos no registrados...");
        return;
    }

    if (!File.Exists(archivoEquipos)){
        Console.WriteLine("Equipos no registrados...");
        return;
    }

    string[] prestamos = File.ReadAllLines(archivoPrestamos);
    string[] equipos = File.ReadAllLines(archivoEquipos);
    int posPrestamo = -1;
    int posEquipo = -1;
    for (int i = 0; i < prestamos.Length; i++){
        string[] datos = prestamos[i].Split(',');

        if (datos.Length >= 3 && datos[0] == id){
            posPrestamo = i;
            break;
        }
    }

    if (posPrestamo == -1){
        Console.WriteLine("No hay prestamo activo con ese codigo...");
        return;
    }

    for (int i = 0; i < equipos.Length; i++){
        string[] datos = equipos[i].Split(',');

        if (datos.Length >= 4 && datos[0] == id){
            posEquipo = i;
            break;
        }
    }

    if (posEquipo == -1){
        Console.WriteLine("No se encontro el equipo registrado...");
        return;
    }

    using (var archivo = new StreamWriter(archivoPrestamos, false, Encoding.UTF8)){

        for (int i = 0; i < prestamos.Length; i++){
            if (i != posPrestamo)
            {
                archivo.WriteLine(prestamos[i]);
            }
        }
    }

    string[] datosEquipo = equipos[posEquipo].Split(',');
    datosEquipo[3] = "si";
    equipos[posEquipo] = string.Join(",", datosEquipo);
    File.WriteAllLines(archivoEquipos, equipos, Encoding.UTF8);
    Console.WriteLine("equipo devuelto...");
}
// =================================================================================
// =================================================================================
    public static void ListarDisponibilidad()
    {
        if (!File.Exists(archivoEquipos)){
            Console.WriteLine("No hay equipos registrados...");
            return;
        }

        string[] equipos = File.ReadAllLines(archivoEquipos, Encoding.UTF8);
        if (equipos.Length == 0){
            Console.WriteLine("No hay equipos registrados...");
            return;
        }

        Console.WriteLine("Codigo | Nombre | Categoria | Disponible");
        foreach (string linea in equipos){
            string[] partes = linea.Split(',');
            if (partes.Length >= 4)
            {
                Console.WriteLine($"{partes[0]} | {partes[1]} | {partes[2]} | {partes[3]}");
            }
        }
    }
// =================================================================================
// =================================================================================
    public static void ContarEquipos()
    {
        string categoria = LeerDato("Categoria a consultar: ");
        int cantidad = 0;
        if (!File.Exists(archivoEquipos)){
            Console.WriteLine("No hay equipos registrados...");
            return;
        }

        string[] equipos = File.ReadAllLines(archivoEquipos, Encoding.UTF8);
        foreach (string linea in equipos){
            string[] partes = linea.Split(',');
            if (partes.Length >= 4 && partes[2] == categoria && partes[3] == "si"){
                cantidad++;
            }
        }
        Console.WriteLine($"Equipos disponibles en {categoria}: {cantidad}");
    }
}