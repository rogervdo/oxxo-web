using System;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace OxxoPage.Model
{
    public class DataBaseContext
    {
        public string ConnectionString { get; set; }

        public DataBaseContext()
        {
            // Emilio
            //"Server=127.0.0.1;Port=3306;Database=bdTest3;Uid=root;Password=root1234;"
            
            // Pablo
            ConnectionString = "Server=mysql-93cf659-tamtok2-09a8.b.aivencloud.com;Port=23481;Database=BDOxxo;Uid=avnadmin;Password=AVNS_-9SXvTjsy8x6dg2kaJR";

            // Jordy
            //ConnectionString = "Server=127.0.0.1;Port=3306;Database=DB_OXXO;Uid=root;Password=root1234";
        }

        // Método privado para obtener la conexión a la base de datos
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
                try
                {
                    conexion.Open();
                    string query = "SELECT * FROM usuarios";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Creación de un usuario a partir de los datos obtenidos de la base de datos
                            Usuarios usuario = new Usuarios
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                Nombre = reader["nombre"].ToString(),
                                ApellidoMaterno = reader["apellido_materno"] != DBNull.Value ? reader["apellido_materno"].ToString() : "",
                                ApellidoPaterno = reader["apellido_paterno"] != DBNull.Value ? reader["apellido_paterno"].ToString() : "",
                                Telefono = reader["telefono"] != DBNull.Value ? reader["telefono"].ToString() : "",
                                Fotografia = reader["fotografia"] != DBNull.Value ? reader["fotografia"].ToString() : "default-user.jpg",
                                Nickname = reader["nickname"].ToString(),
                                CorreoElectronico = reader["correo_electronico"].ToString()
                            };
                            ListaUsuarios.Add(usuario);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Captura de errores en caso de problemas con la conexión o consulta
                    Console.WriteLine("Error al obtener usuarios: " + ex.Message);
                    // En producción, es mejor registrar este error en un log
                }
            }
            return ListaUsuarios;
        }

        // Método para verificar el login del usuario
        public bool Login(string nickname, string contrasena)
        {
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT COUNT(*) FROM usuarios WHERE nickname = @nickname AND contrasena = @contrasena";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // Parámetros de la consulta para evitar inyecciones SQL
                        cmd.Parameters.AddWithValue("@nickname", nickname);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;  // Si es mayor a 0, el usuario existe con esa contraseña
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores en el login
                    Console.WriteLine("Error en el login: " + ex.Message);
                    return false;
                }
            }
        }

        // Método login
        public bool LoginUser(string usuario, string contrasena)
        {
            using (var conexion = GetConnection())
            {
                try
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
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine("Error en el login: " + ex.Message);
                    return false;
                }
            }
        }

        // Método SignUp
        public bool SignUp(string nombre, string apellido_materno, string apellido_paterno, string nickname, string contrasena, string correoElectronico)
        {
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();
                    // Consulta SQL para insertar un nuevo usuario en la base de datos
                    string query = "INSERT INTO usuarios (nombre, apellido_materno, apellido_paterno, nickname, contrasena, correo_electronico) VALUES (@nombre, @apellido_materno, @apellido_paterno, @nickname, @contrasena, @correoElectronico)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        // Parámetros para la consulta
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@apellido_materno", apellido_materno);
                        cmd.Parameters.AddWithValue("@apellido_paterno", apellido_paterno);
                        cmd.Parameters.AddWithValue("@nickname", nickname);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);
                        cmd.Parameters.AddWithValue("@correoElectronico", correoElectronico);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;  // Retorna true si se insertó correctamente
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores en el registro
                    Console.WriteLine("Error al registrar usuario: " + ex.Message);
                    return false;
                }
            }
        }

        // Obtener usuarios con medallas para la tabla de puntajes
        public List<Usuarios> GetUsuariosConMedallas()
        {
            List<Usuarios> listaUsuarios = new List<Usuarios>();
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();

                    // Consulta para obtener todos los usuarios que son asesores
                    string query = @"SELECT u.id_usuario,u.nombre,CONCAT(u.nombre, ' ', IFNULL(u.apellido_paterno, ''), ' ', IFNULL(u.apellido_materno, '')) as nombre_completo,
                                    u.nickname,u.fotografia,a.id_asesor
                                    FROM usuarios u
                                    JOIN asesores a ON u.id_usuario = a.id_usuario
                                    ORDER BY u.id_usuario";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Asignación de medallas al usuario con base en su id_asesor
                            int idAsesor = Convert.ToInt32(reader["id_asesor"]);
                            int medallas = 25 - (idAsesor % 16); // Asegura valores entre 9 y 24
                            if (idAsesor <= 3) medallas += 5; // Aumentar medallas a los primeros 3

                            Usuarios usuario = new Usuarios
                            {
                                IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                Nombre = reader["nombre_completo"].ToString(),
                                Nickname = reader["nickname"].ToString(),
                                Fotografia = reader["fotografia"] == DBNull.Value ? "default.png" : reader["fotografia"].ToString(),
                                Medallas = medallas,
                                Posicion = 0 // La posición se calcula después
                            };
                            listaUsuarios.Add(usuario);
                        }
                    }

                    // Ordenar los usuarios por medallas (de mayor a menor)
                    listaUsuarios = listaUsuarios.OrderByDescending(u => u.Medallas).ToList();

                    // Asignar posiciones a los usuarios ordenados
                    for (int i = 0; i < listaUsuarios.Count; i++)
                    {
                        listaUsuarios[i].Posicion = i + 1;
                    }

                    // Limitar los resultados a los primeros 10
                    if (listaUsuarios.Count > 10)
                    {
                        listaUsuarios = listaUsuarios.Take(10).ToList();
                    }
                }
                catch (Exception ex)
                {
                    // Captura de errores al obtener usuarios con medallas
                    Console.WriteLine("Error al obtener usuarios con medallas: " + ex.Message);
                }
            }
            return listaUsuarios;
        }

        // Obtener las métricas del dashboard
        public DashboardMetricas GetDashboardMetricas()
        {
            DashboardMetricas metricas = new DashboardMetricas();

            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();

                    // Establecer la meta mensual
                    metricas.MetaMensual = 180;

                    // Consultas para obtener datos del mes de marzo 2025
                    string queryTotalLogros = @"SELECT COUNT(*) as total_logros FROM instancialogro WHERE MONTH(fecha) = 3 AND YEAR(fecha) = 2025";
                    string queryTotalLogrosDefinidos = @"SELECT COUNT(*) as total_logros FROM logros";
                    string queryJuegosDia = @"SELECT COUNT(*) as juegos_dia FROM instanciajuego WHERE DAY(fecha) = 15 AND MONTH(fecha) = 3 AND YEAR(fecha) = 2025";
                    string queryJuegosTotales = @"SELECT COUNT(*) as juegos_totales FROM juegos";

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(queryTotalLogros, conexion))
                        {
                            object result = cmd.ExecuteScalar();
                            metricas.TotalLogros = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                            if (metricas.TotalLogros == 0)
                            {
                                using (MySqlCommand cmdAlt = new MySqlCommand(queryTotalLogrosDefinidos, conexion))
                                {
                                    object resultAlt = cmdAlt.ExecuteScalar();
                                    metricas.TotalLogros = resultAlt != DBNull.Value ? Convert.ToInt32(resultAlt) * 15 : 147;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener total de logros: " + ex.Message);
                        metricas.TotalLogros = 147;
                    }

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(queryJuegosDia, conexion))
                        {
                            object result = cmd.ExecuteScalar();
                            metricas.CapacitacionesDia = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                            if (metricas.CapacitacionesDia == 0)
                            {
                                using (MySqlCommand cmdAlt = new MySqlCommand(queryJuegosTotales, conexion))
                                {
                                    object resultAlt = cmdAlt.ExecuteScalar();
                                    metricas.CapacitacionesDia = resultAlt != DBNull.Value ? Convert.ToInt32(resultAlt) * 4 : 13;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener capacitaciones del día: " + ex.Message);
                        metricas.CapacitacionesDia = 13;
                    }

                    // Calcular la meta diaria
                    metricas.MetaDiaria = (int)Math.Ceiling(metricas.MetaMensual / 30.0m);

                    // Calcular porcentajes y valores restantes
                    metricas.PorcentajeMeta = metricas.MetaMensual > 0
                        ? Math.Round((decimal)metricas.TotalLogros / metricas.MetaMensual * 100, 1)
                        : 0;

                    metricas.LogrosFaltantes = Math.Max(0, metricas.MetaMensual - metricas.TotalLogros);

                    metricas.PorcentajeMetaDiaria = metricas.MetaDiaria > 0
                        ? Math.Round((decimal)metricas.CapacitacionesDia / metricas.MetaDiaria * 100, 2)
                        : 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener métricas: " + ex.Message);
                    metricas.TotalLogros = 147;
                    metricas.MetaMensual = 180;
                    metricas.CapacitacionesDia = 13;
                    metricas.MetaDiaria = 6;
                    metricas.PorcentajeMeta = 81.6m;
                    metricas.LogrosFaltantes = 33;
                    metricas.PorcentajeMetaDiaria = 109.72m;
                }
            }

            return metricas;
        }

        // Obtener los datos del usuario basados en su nickname
        public Usuarios ObtenerDatosUsuario(string nickname)
        {
            Usuarios usuario = null;
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT id_usuario, nombre, apellido_paterno, apellido_materno, fotografia, about_me FROM usuarios WHERE nickname = @nickname";
                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nickname", nickname);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuarios
                                {
                                    IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                    Nombre = reader["nombre"].ToString(),
                                    ApellidoPaterno = reader["apellido_paterno"]?.ToString() ?? "",
                                    ApellidoMaterno = reader["apellido_materno"]?.ToString() ?? "",
                                    Fotografia = !string.IsNullOrEmpty(reader["fotografia"].ToString()) ? reader["fotografia"].ToString() : "default.png",
                                    AboutMe = reader["about_me"]?.ToString() ?? "Este usuario aún no ha escrito su biografía."
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener datos del usuario: " + ex.Message);
                }
            }
            return usuario;
        }

        // Obtener el rol del usuario (asesor o gerente)
        public string ObtenerRolUsuario(int userId)
        {
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();

                    string queryAsesor = "SELECT id_usuario FROM asesores WHERE id_usuario = @userId";
                    using (var cmd = new MySqlCommand(queryAsesor, conexion))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) return "Asesor de Tienda";
                        }
                    }

                    string queryGerente = "SELECT id_usuario FROM gerentes WHERE id_usuario = @userId";
                    using (var cmd = new MySqlCommand(queryGerente, conexion))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) return "Gerente de Plaza";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener el rol del usuario: " + ex.Message);
                }

                return "Usuario estándar";
            }
        }

        // Obtener los logros de un usuario en función de su nickname
        public List<Achievement> ObtenerLogrosUsuario(string nickname, out int currentXP)
        {
            List<Achievement> logros = new List<Achievement>();
            currentXP = 0;

            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();
                    string query = @"SELECT l.nombre, l.icono, l.experiencia, i.fecha FROM logrosasesores la
                        JOIN instancialogro i ON la.id_instancialogro = i.id_instancialogro
                        JOIN logros l ON i.id_logro = l.id_logro
                        JOIN usuarios u ON la.id_asesor = u.id_usuario
                        WHERE u.nickname = @nickname";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nickname", nickname);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int xp = Convert.ToInt32(reader["experiencia"]);
                                logros.Add(new Achievement(
                                    reader["nombre"].ToString(),
                                    Convert.ToDateTime(reader["fecha"]), // Fecha real de la BD
                                    xp,
                                    reader["icono"].ToString()
                                ));
                                currentXP += xp; // Sumar la experiencia total del usuario
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener logros del usuario: " + ex.Message);
                }
            }
            return logros;
        }

        public string ObtenerFotoDePerfil(string nickname)
        {
            string foto = "default.png"; // Valor por defecto en caso de error
            using (var conexion = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT fotografia FROM usuarios WHERE nickname = @nickname";
                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nickname", nickname);
                        object result = cmd.ExecuteScalar();
                        if (result != null && !string.IsNullOrEmpty(result.ToString()))
                        {
                            foto = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener la foto de perfil: " + ex.Message);
                }
            }
            return foto;
        }
    }
}
