using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Bol3_Ejer1
{
    class Program
    {
        static void Main(string[] args)
        {
            int opcion = 0;
            bool seguir = false;
            bool correcto = false;
            Hashtable ipes = new Hashtable();
            //ipes.Add("1.1.1.1", 23);
            //ipes.Add("1.2.1.1", 233);
            //ipes.Add("1.3.1.1", 231);
            //ipes.Add("1.4.1.1", 123);
            string ip = "";
            string[] datosRecogidos;
            uint gb = 0;
            string linea;
            StreamWriter escribir;
            StreamReader leer;

            //CARGAMOS EL ARCHIVO CON LOS DATOS ==================================================================
            if (File.Exists("ipes.txt"))
            {
                using (leer = new StreamReader("ipes.txt"))
                {
                    linea = leer.ReadLine();
                    while (linea != null)
                    {
                        datosRecogidos = linea.Split(',');//si todo bien 2 datos, 1 ip y un tamaño

                        if (datosRecogidos.Length == 2)
                        {
                            if (comprobarIp(datosRecogidos[0]) == true)
                            {
                                try
                                {
                                    gb = Convert.ToUInt32(datosRecogidos[1]);
                                    try
                                    {
                                        ipes.Add(datosRecogidos[0], datosRecogidos[1]);
                                    }
                                    catch (ArgumentException)
                                    {
                                        Console.WriteLine("El indice está repetido");
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Solo son admitidos datos numéricos");
                                }
                            }
                        }
                        linea = leer.ReadLine();

                    }
                }

            }


            //MENU================================================================================================

            while (opcion != 5)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1-   Meter datos");
                Console.WriteLine("2-   Eliminar datos");
                Console.WriteLine("3-   Mostrar colección");
                Console.WriteLine("4-   Mostrar Elementos");
                Console.WriteLine("5-   Salir");
                try
                {
                    opcion = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Solo son admitidos datos numéricos");
                }
                switch (opcion)
                {
                    case 1:
                        while (seguir == false)
                        {
                            Console.WriteLine("Introduce la ip");
                            ip = Console.ReadLine();
                            seguir = comprobarIp(ip);
                        }
                        while (correcto == false)
                        {
                            correcto = true;
                            Console.WriteLine("Indica la memoria de tu ordenador en Gb");
                            try
                            {
                                gb = Convert.ToUInt32(Console.ReadLine());
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Solo son admitidos datos numéricos");
                                correcto = false;
                            }
                            catch (OverflowException)
                            {
                                Console.WriteLine("Número incorrecto");
                                correcto = false;
                            }
                        }
                        seguir = false;
                        correcto = false;
                        try
                        {
                            ipes.Add(ip, gb);
                            Console.WriteLine("Los datos fueron introducidos adecuadamente");
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Ya existe un registro con esa IP");
                        }
                        break;
                    case 2:
                        string ipecita = "";
                        mostrarDatos(ipes);
                        Console.WriteLine("Que ip deseas borrar");
                        ipecita = Console.ReadLine();
                        if (ipes.Contains(ipecita))
                        {
                            ipes.Remove(ipecita);
                            Console.WriteLine("Eliminada adecuadamente");
                        }
                        else
                        {
                            Console.WriteLine("No existe esa IP");
                        }

                        break;
                    case 3:
                        mostrarDatos(ipes);
                        break;
                    case 4:
                        string ipecita2 = "";
                        bool Noexiste = false;
                        Console.WriteLine("Que ip deseas buscar");
                        ipecita2 = Console.ReadLine();
                        foreach  (DictionaryEntry item in ipes)
                        {
                            if (item.Key.Equals(ipecita2))
                            {
                                Console.WriteLine("IP:{0} Tiene un PC de {1}GB",item.Key, item.Value);
                                Noexiste = true;
                            }
                        }
                        if (Noexiste == false)
                        {
                            Console.WriteLine("No existe esa IP");
                        }
                        break;
                    case 5:
                        Console.WriteLine("Hasta luego");
                        //para evitar problemas reescribimos el archivo
                        //al ser en debug, con poner el nombre del archivo llega, lo guarda ahi automáticamente
                        using (escribir = new StreamWriter("ipes.txt"))
                        {
                            foreach (DictionaryEntry item in ipes)
                            {
                                escribir.WriteLine("{0},{1}",item.Key,item.Value);
                            }
                            
                        }
                        Console.ReadLine();
                        break;
                    default:
                        break;
                }
            }

        }

        //FUNCIONES ==============================================================================================
        public static bool comprobarIp(string ip)
        {
            string[] numeros;
            int cont = 0;
            numeros = ip.Split('.');
            for (int i = 0; i < numeros.Length; i++)
            {

                try
                {
                    if (Convert.ToInt32(numeros[i]) >= 0 && Convert.ToInt32(numeros[i]) <= 255)
                    {
                        cont++;
                    }
                }
                catch (FormatException)
                {
                    cont--;
                    Console.WriteLine("Tipo de dato incorrecto");
                }
            }
            if (cont == 4)
            {
                return true;
            }
            return false;
        }
        public static void mostrarDatos(Hashtable ipes)
        {
            int contador = 1;
            foreach (DictionaryEntry item in ipes)
            {
                Console.WriteLine("_________________________________________\n{0}=> IP:{1} Tiene un PC de {2}GB\n_________________________________________", contador, item.Key, item.Value);
                contador++;
            }
        }


    }
}
