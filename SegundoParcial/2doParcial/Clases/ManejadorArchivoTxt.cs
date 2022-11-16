using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial.Clases
{
    public static class ManejadorArchivoTxt
    {

        private static string ruta;
        private static int partidas;

        static ManejadorArchivoTxt()
        {
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ruta += @"/RegistroPartidas";
            Partidas = 0;
        }

        public static int Partidas { get => partidas; set => partidas = value; }

        /// <summary>
        /// Escribe en un archivo .txt que se va a crear dentro de una carpeta en el escritorio el string que le pasemos por parametro
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="nombreArchivo"></param>
        public static void Escribir(string mensaje, string nombreArchivo)
        {
            
            string rutaCompleta = ruta + $@"/{nombreArchivo}" + ".txt";
            try
            {                
                if (!Directory.Exists(ruta)) Directory.CreateDirectory(ruta);
                using (StreamWriter sw = new StreamWriter(rutaCompleta))
                {
                    sw.WriteLine(mensaje);
                }
                Partidas++;
                
            }
            catch(Exception)
            {               
                throw new Exception($"Error al abrir el archivo {nombreArchivo}");
            }            
        }
    }
}
