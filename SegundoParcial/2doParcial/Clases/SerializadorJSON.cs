using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2doParcial.Clases
{
    public class SerializadorJSON<T> where T : class, new()
    {
        public string path;


        public SerializadorJSON(string fileName)
        {
            path += ".\\" + fileName;
        }



        /// <summary>
        /// Deserializa un objeto alojado en el directorio que le indiquemos y lo retorna. 
        /// </summary>
        /// <returns></returns>
        public T DeSerialize()
        {
            T obj = new T();

            try
            {
                string jsonFile = File.ReadAllText(path + ".json");
                obj = JsonSerializer.Deserialize<T>(jsonFile);
            }
            catch (Exception)
            {
                return null;
            }
            return obj;
        }


        /// <summary>
        ///  Serializa un objeto en el archivo que le indiquemos y retorna true si pude hacerlo.
        /// </summary>
        /// <param name="serializar"></param>
        /// <returns></returns>
        public bool Serialize(T serializar)
        {
            bool todoOk = false;

            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;

                string json = JsonSerializer.Serialize(serializar, options);

                File.WriteAllText(path + ".json", json);

                todoOk = true;
            }
            catch (Exception)
            {
                todoOk = false;
            }
            return todoOk;
        }

    }
}
