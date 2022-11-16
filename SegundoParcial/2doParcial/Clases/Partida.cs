using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial.Clases
{
    public class Partida : IDataAccess<Partida>
    {
        string ganador;
        string perdedor;
        DateTime fecha;
        string puntaje;
        private SqlCommand command;
        private SqlConnection connection;
        private string connectionString;
        int id;
        int manos;

        public string Ganador { get => ganador; set => ganador = value; }
        public string Perdedor { get => perdedor; set => perdedor = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
        public string Puntaje { get => puntaje; set => puntaje = value; }
        public int Id { get => id; set => id = value; }
        public int Manos { get => manos; set => manos = value; }

        public Partida()
        {
            
        }

        public Partida(int id, string ganador, string perdedor, string puntaje, DateTime fecha, int manos)
        {
            this.Id = id;
            this.Ganador = ganador;
            this.Perdedor = perdedor;
            this.Fecha = fecha;
            this.Puntaje = puntaje;
            this.Manos = manos;
        }


        /// <summary>
        /// Inicializa la conexion con la base de datos y define el CommandType
        /// </summary>
        private void InicializarConexion()
        {
            connectionString = @"Server = MATIAS ; Database = Parcial2 ; Trusted_Connection=true; ";
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
        }

        #region #DataAcces
        /// <summary>
        /// Obtiene toda la tabla de Partidas de la base de datos.
        /// </summary>
        /// <returns></returns>
        public List<Partida> Obtener()
        {
            InicializarConexion();
            List<Partida> listaPartidasSQL = new();
            try
            {
                connection.Open();
                command.CommandText = "SELECT * FROM Partidas";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string Ganador = reader.GetString(1);
                    string Perdedor = reader.GetString(2);
                    string puntaje = reader.GetString(3);
                    DateTime fecha = reader.GetDateTime(4);
                    int manos = reader.GetInt32(5);

                    Partida partidaSQL = new(id, Ganador, Perdedor, puntaje, fecha, manos);
                    listaPartidasSQL.Add(partidaSQL);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return listaPartidasSQL;
        }


        /// <summary>
        /// Agrega una partida a la tabla de Partidas en la base de datos
        /// </summary>
        /// <param name="partida"></param>
        /// <returns></returns>
        public bool Agregar(Partida partida)
        {
            InicializarConexion();
            bool todoOk = true;
            try
            {
                connection.Open();

                command.CommandText = "INSERT INTO Partidas VALUES (@Ganador,@Perdedor,@puntaje, @fecha,@manos)";
                command.Parameters.AddWithValue("@Ganador", partida.Ganador);
                command.Parameters.AddWithValue("@Perdedor", partida.Perdedor);
                command.Parameters.AddWithValue("@puntaje", partida.Puntaje);
                command.Parameters.AddWithValue("@fecha", partida.Fecha);
                command.Parameters.AddWithValue("@manos", partida.Manos);


                int lineasAfectadas = command.ExecuteNonQuery();

                if (lineasAfectadas == 0) todoOk = false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Parameters.Clear();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return todoOk;
        }


        /// <summary>
        /// Elimina una partida de la tabla de Partidas de la base de datos
        /// </summary>
        /// <param name="partida"></param>
        /// <returns></returns>
        public bool Eliminar(Partida partida)
        {
            InicializarConexion();
            bool todoOk = true;
            try
            {
                connection.Open();
                command.CommandText = "DELETE Partidas WHERE Id = @id";
                command.Parameters.AddWithValue("@id", partida.Id);
                int lineasAfectadas = command.ExecuteNonQuery();

                if (lineasAfectadas == 0) todoOk = false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Parameters.Clear();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return todoOk;
        }


        /// <summary>
        /// Modifica los datos de la partida en la tabla.
        /// </summary>
        /// <param name="partida"></param>
        /// <returns></returns>
        public bool Modificar(Partida partida)
        {
            InicializarConexion();
            bool todoOk = true;
            try
            {
                connection.Open();
                command.CommandText = "UPDATE Partidas SET Ganador= @Ganador, Perdedor = @Perdedor, Puntaje = @puntaje, fecha = @fecha, Manos = manos WHERE Id = @id";
                command.Parameters.AddWithValue("@Ganador", partida.Ganador);
                command.Parameters.AddWithValue("@Perdedor", partida.Perdedor);
                command.Parameters.AddWithValue("@puntaje", partida.Puntaje);
                command.Parameters.AddWithValue("@fecha", partida.Fecha);
                command.Parameters.AddWithValue("@manos", partida.Manos);
                command.Parameters.AddWithValue("@id", partida.Id);

                int lineasAfectadas = command.ExecuteNonQuery();
                if (lineasAfectadas == 0) todoOk = false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Parameters.Clear();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            return todoOk;
        }
        #endregion




    }


}
