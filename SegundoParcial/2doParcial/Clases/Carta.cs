using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial
{
    public class Carta
    {
        private string tipo;
        private int numero;
        private int valor;

        public Carta(int numero,string tipo)
        {
            this.tipo = tipo;
            this.numero = numero;
            this.valor = this.rankingCartas();
        }

 
        public int Numero { get => numero; set => numero = value; }
        public int Valor { get => valor; set => valor = value; }
        public string Tipo { get => tipo; set => tipo = value; }

        public static bool operator == (Carta c1, Carta c2)
        {
            return c1.Valor == c2.Valor;
        }

        public static bool operator !=(Carta c1, Carta c2)
        {
            return !(c1.Valor == c2.Valor);    
        }

        public override bool Equals(object obj)
        {
            bool ret = false;
            if(obj is Carta)
            {
                ret = this == ((Carta)obj);
            }
            return ret;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.numero}-{this.Tipo.ToUpper()}";
        }


        /// <summary>
        /// Calcula el ranking de cartas segun el truco. del 0(los 4) al 13(ancho de espada) 
        /// </summary>
        /// <returns></returns>
        public int rankingCartas()
        {
            switch (this.numero)
            {
                case 1:
                    if (tipo.Equals("oro") || tipo.Equals("copa"))
                        return 7;
                    else if (tipo.Equals("basto")) return 12;
                    else return 13; //ESPADA
                case 2:
                    return 8;
                case 3:
                    return 9;
                case 4: 
                    return 0;
                case 5: 
                    return 1;
                case 6: 
                    return 2;
                case 7:
                    if (Tipo.Equals("basto") || Tipo.Equals("copa")) return 3;
                    if (Tipo.Equals("oro")) return 10;
                    else return 11; //ESPADA
                case 10:
                    return 4;
                case 11: 
                    return 5;
                case 12:
                    return 6;
            }
            return -1;
        }



        
    }
}
