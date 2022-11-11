using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial
{
    public class Mazo<T> where T: Carta
    {
        private List<T> mazo;
        private string tipo;

        private Mazo()
        {
            mazo = new List<T>();
        }
        
        public Mazo(string tipo) : this()
        {
            this.tipo = tipo;
        }

        public static bool operator ==(Mazo<T> mazo, T carta)
        {
            bool rtn = false;
            foreach (T c in mazo.mazo)
            {
                if (c == carta) rtn =true;
            }
            return rtn;
        }

        public static bool operator !=(Mazo<T> mazo, T carta)
        {
            return !(mazo == carta);
        }

        public static bool operator + (Mazo<T> mazo, T carta)
        {
            if((mazo is not null && carta is not null) && mazo != carta)
            {
                mazo.mazo.Add(carta);
                return true;
            }
            return false;
        }

        public static bool operator -(Mazo<T> mazo, T carta)
        {
            if ((mazo is not null && carta is not null) && mazo == carta)
            {
                mazo.mazo.Remove(carta);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Tipo de mazo:{this.tipo} - Cantidad de cartas: {this.mazo.Count()}";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    

}
