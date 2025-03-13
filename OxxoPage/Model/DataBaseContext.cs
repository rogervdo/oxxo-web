using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace OxxoPage.Model
{
    public class DataBaseContext
    {
        public string ConnectionString { get; set; }

        public DataBaseContext()
        {
            //"Server=127.0.0.1;Port=3306;Database=bdTest3;Uid=root;Password=root1234;"
            ConnectionString = "";
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        // Obtener todos los usuarios
        public List<Usuarios> GetAllUsers()
        {
            List<Usuarios> ListaUsuarios = new List<Usuarios>();
            using (var conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT * FROM Usuarios";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuarios usuario = new Usuarios
                        {
                            Nombre = reader["nombre"].ToString(),
                            Nickname = reader["nickname"].ToString(),
                            CorreoElectronico = reader["correo_electronico"].ToString()
                        };
                        ListaUsuarios.Add(usuario);
                    }
                }
            }
            return ListaUsuarios;
        }

        // Método para verificar el login
        public bool Login(string nickname, string contrasena)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE nickname = @nickname AND contraseña = @contrasena";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nickname", nickname);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);
                    
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;  // Si es mayor a 0, el usuario existe con esa contraseña
                }
            }
        }

        // Método para registrar un nuevo usuario
        public bool SignUp(string nombre, string nickname, string contrasena, string correoElectronico)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                string query = "INSERT INTO Usuarios (nombre, nickname, contraseña, correo_electronico) VALUES (@nombre, @nickname, @contrasena, @correoElectronico)";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@nickname", nickname);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena);
                    cmd.Parameters.AddWithValue("@correoElectronico", correoElectronico);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;  // Retorna true si se insertó correctamente
                }
            }
        }
    }
}
