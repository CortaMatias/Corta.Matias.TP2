using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2doParcial.Clases
{   

    public class DataAccess_Usuarios
    {
        private SqlCommand command;
        private SqlConnection connection;
        private string connectionString;       
        
        public DataAccess_Usuarios()
        {
            connectionString = @"Server = MATIAS ; Database = Parcial2 ; Trusted_Connection=true; ";
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;       
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> listaUsuariosSQL = new();
            try
            {              
                connection.Open();
                command.CommandText = "SELECT * FROM Usuarios";

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


        public bool AgregarUsuario(Usuario usuario)
        {
            bool todoOk = true;
            try
            {
                connection.Open();

                command.CommandText = "INSERT INTO Usuarios VALUES (@Nick,@Victorias,@Derrotas)";
                command.Parameters.AddWithValue("@Nick", usuario.NickName);
                command.Parameters.AddWithValue("@Victorias", usuario.Victorias);
                command.Parameters.AddWithValue("@Derrotas", usuario.Derrotas);

              int lineasAfectadas =  command.ExecuteNonQuery();

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

        public bool ModificarUsuario(Usuario usuario)
        {
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

                if (lineasAfectadas == 0 ) todoOk = false;
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

        public bool BorrarUsuario(Usuario usuario)
        {
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
        



    }
}
