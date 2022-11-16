using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial.Clases
{
    public class Usuario : IDataAccess<Usuario>
    {
        private string nickName;
        private int id;
        private int victorias;
        private int derrotas;
        private SqlCommand command;
        private SqlConnection connection;
        private string connectionString;
        private bool jugando;

        public Usuario(int id, string NickName, int victorias, int derrotas) : this()
        {
            this.nickName = NickName;
            this.id = id;
            this.Victorias = victorias;
            this.Derrotas = derrotas;
        }

        public Usuario() 
        {
            
        }

        public string NickName { get => nickName; set => nickName = value; }
        public int Id { get => id; set => id = value; }
        public int Victorias { get => victorias; set => victorias = value; }
        public int Derrotas { get => derrotas; set => derrotas = value; }
        public bool Jugando { get => jugando; set => jugando = value; }

        public override string ToString()
        {
            return this.NickName;
        }


        /// <summary>
        /// Iniciliza la conexion con la base de datos.
        /// </summary>
        private void InicializarConexion()
        {
            connectionString = @"Server = MATIAS ; Database = Parcial2 ; Trusted_Connection=True; ";
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
        }
        #region #DataAcces

        /// <summary>
        /// Obtiene la lista de usuarios de la tabla Usuarios de la base de datos
        /// </summary>
        /// <returns></returns>
        public List<Usuario> Obtener()
        {
            InicializarConexion();
            List<Usuario> listaUsuariosSQL = new();
            try
            {
                connection.Open();
                command.CommandText = "SELECT * FROM dbo.Usuarios";

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nick = reader.GetString(1);
                    int victorias = reader.GetInt32(2);
                    int derrotas = reader.GetInt32(3);

                    Usuario usuarioSQL = new(id, nick, victorias, derrotas);
                    listaUsuariosSQL.Add(usuarioSQL);
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
            return listaUsuariosSQL;
        }

        /// <summary>
        /// Agrega un usuario a la tabla con los valores del usuario que le pasamos por parametro
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool Agregar(Usuario usuario)
        {
            InicializarConexion();
            bool todoOk = true;
            try
            {
                connection.Open();

                command.CommandText = "INSERT INTO Usuarios VALUES (@Nick,@Victorias,@Derrotas)";
                command.Parameters.AddWithValue("@Nick", usuario.NickName);
                command.Parameters.AddWithValue("@Victorias", usuario.Victorias);
                command.Parameters.AddWithValue("@Derrotas", usuario.Derrotas);

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
        /// Elimina al usuario que le pasamos por parametro de la tabla, retorna true si pudo y retorna false si no
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool Eliminar(Usuario usuario)
        {
            InicializarConexion();
            bool todoOk = true;
            try
            {
                connection.Open();
                command.CommandText = "DELETE Usuarios WHERE Id = @id";
                command.Parameters.AddWithValue("@id", usuario.Id);
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
        /// Modifica los datos del usuario que le pasamos por parametro. Retorna true si pudo modificarlo o false si no pudo.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool Modificar(Usuario usuario)
        {
            InicializarConexion();
            bool todoOk = true;
            try
            {
                connection.Open();
                command.CommandText = "UPDATE Usuarios SET Nick = @Nick, Victorias = @Victorias, Derrotas = @Derrotas WHERE Id = @id";
                command.Parameters.AddWithValue("@Nick", usuario.NickName);
                command.Parameters.AddWithValue("@Victorias", usuario.Victorias);
                command.Parameters.AddWithValue("@Derrotas", usuario.Derrotas);
                command.Parameters.AddWithValue("@id", usuario.Id);

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
