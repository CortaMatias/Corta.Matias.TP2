using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial.Clases
{
    public interface IDataAccess<T> where T : class, new()
    {
        public List<T> Obtener();
        public bool Agregar(T param);
        public bool Eliminar(T param);
        public bool Modificar(T param);
    }
}
