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
            //"Server=127.0.0.1;Port=3306;Database=DB_OXXO;Uid=root;Password=root1234;"
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=DB_OXXO;Uid=root;Password=root1234;";
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }


        // Método para verificar el login
        public bool LoginUser(string usuario, string contrasena)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                string query = "SELECT COUNT(*) FROM usuarios WHERE nickname = @nickname AND contrasena = @contrasena";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nickname", usuario);
                    cmd.Parameters.AddWithValue("@contrasena", contrasena); 

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; // Retorna `true` si hay una coincidencia
                }
            }
        }

        // Método para registrar un nuevo usuario
        public bool SignUp(string nombre, string nickname, string contrasena, string correoElectronico)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();
                string query = "INSERT INTO Usuarios (nombre, nickname, contrasena, correo_electronico) VALUES (@nombre, @nickname, @contrasena, @correoElectronico)";
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
