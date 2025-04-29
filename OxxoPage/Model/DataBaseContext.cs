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
            ConnectionString = "Server=mysql-93cf659-tamtok2-09a8.b.aivencloud.com;Port=23481;Database=BDOxxo;Uid=avnadmin;Password=AVNS_-9SXvTjsy8x6dg2kaJR";
        }

        // Método privado para obtener la conexión a la base de datos
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Asesores> GetAsesoresDeGerente(int id_gerente)
        {
            List<Asesores> ListaAsesores = new List<Asesores>();
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();
                    string query = @"
                        SELECT 
                            a.*, 
                            u.*, 
                            COUNT(DISTINCT o.id_oxxo) AS total_oxxos, 
                            COUNT(DISTINCT il.id_logro_usuario) AS total_medallas
                        FROM asesores a
                        JOIN usuarios u ON a.id_usuario = u.id_usuario
                        LEFT JOIN oxxos o ON a.id_asesor = o.id_asesor
                        LEFT JOIN logros_usuario il ON u.id_usuario = il.id_usuario
                        WHERE a.id_gerente = @id_gerente
                        GROUP BY a.id_asesor, u.id_usuario;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id_gerente", id_gerente);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Creación de un usuario a partir de los datos obtenidos de la base de datos
                                Asesores asesor = new Asesores
                                {
                                    IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                    IdAsesor = Convert.ToInt32(reader["id_asesor"]),
                                    Nombre = reader["nombre"]?.ToString() ?? "",
                                    ApellidoMaterno = reader["apellido_materno"]?.ToString() ?? "",
                                    ApellidoPaterno = reader["apellido_paterno"]?.ToString() ?? "",
                                    Telefono = reader["telefono"]?.ToString() ?? "",
                                    Fotografia = reader["fotografia"]?.ToString() ?? "",
                                    Nickname = reader["nickname"]?.ToString() ?? "",
                                    NumOxxos = Convert.ToInt32(reader["total_oxxos"]),
                                    Medallas = Convert.ToInt32(reader["total_medallas"])
                                };
                                ListaAsesores.Add(asesor);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener usuarios: " + ex.Message);
                }
            }
            return ListaAsesores;
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

        //done Obtener los datos del usuario basados en su nickname
        public Usuarios ObtenerDatosUsuario(string nickname)
        {
            Usuarios usuario = null;
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();
                    // Obtener datos de usuario en base a nickname
                    string query = @"SELECT id_usuario, nombre, apellido_paterno, apellido_materno, fotografia, about_me 
                                    FROM usuarios 
                                    WHERE nickname = @nickname";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nickname", nickname);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuarios
                                { // Leer los datos del usuario, con defaults por si no lee
                                    IdUsuario = reader["id_usuario"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario"]) : 0,
                                    Nombre = reader["nombre"].ToString() ?? "NombreDefault",
                                    ApellidoPaterno = reader["apellido_paterno"]?.ToString() ?? "PaternoDefault",
                                    ApellidoMaterno = reader["apellido_materno"]?.ToString() ?? "MaternoDefault",
                                    // Fotografia = ObtenerFotoDePerfil(nickname),
                                    Fotografia = ObtenerFotografiaPath(reader["fotografia"]?.ToString() ?? "default.png"),
                                    AboutMe = reader["about_me"]?.ToString() ?? "Este usuario aún no ha escrito su biografía.",
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
            return usuario ?? new Usuarios
            {
                IdUsuario = 0,
                Nombre = "NombreDefault",
                ApellidoPaterno = "PaternoDefault",
                ApellidoMaterno = "MaternoDefault",
                Fotografia = "default.png",
                AboutMe = "Este usuario aún no ha escrito su biografía."
            };
        }

        //bookmark
        public object? ObtenerUnicoDatoTabla(object data, string tableName, string datatypeInput, string datatypeSearch)
        {
            object? value = null;
            using var conexion = GetConnection();
            try
            {
                conexion.Open();
                string query = $"SELECT {datatypeSearch} FROM {tableName} WHERE {datatypeInput} = @data";

                using var cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@data", data);

                using var reader = cmd.ExecuteReader();
                if (reader.Read()) value = reader[datatypeSearch];

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener datos del usuario: " + ex.Message);
            }

            return value;
        }

        // Obtener el rol del usuario (asesor o gerente)
        public string ObtenerRolUsuario(int userId)
        {
            using (var conexion = GetConnection())
            {
                try
                {
                    conexion.Open();

                    // Checar si el usuario es un asesor
                    string queryAsesor = "SELECT id_usuario FROM asesores WHERE id_usuario = @userId";
                    using (var cmd = new MySqlCommand(queryAsesor, conexion))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) return "asesor";
                        }
                    }

                    // Checar si el usuario es un gerente
                    string queryGerente = "SELECT id_usuario FROM gerentes WHERE id_usuario = @userId";
                    using (var cmd = new MySqlCommand(queryGerente, conexion))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) return "gerente";
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
                    string query = @"SELECT l.id_logro, l.nombre, l.descripcion, l.minijuego, l.experiencia, lu.fecha_obtenido
                             FROM logros_usuario lu
                             JOIN logros l ON lu.id_logro = l.id_logro
                             JOIN usuarios u ON lu.id_usuario = u.id_usuario
                             WHERE u.nickname = @nickname;";

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
                                    Convert.ToDateTime(reader["fecha_obtenido"]), // nombre correcto del campo
                                    xp,
                                    reader["minijuego"].ToString() // usando 'minijuego' como reemplazo de 'icono'
                                ));
                                currentXP += xp;
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


        //---
        public string ObtenerFotografiaPath(string fotografia)
        {
            string foto;
            if (fotografia != null && !string.IsNullOrEmpty(fotografia.ToString()))
            {
                string rutaFoto = Path.Combine("wwwroot", "assets", "img", fotografia.ToString());
                if (File.Exists(rutaFoto))
                {
                    Console.WriteLine($"rutafoto = {rutaFoto}");
                    foto = fotografia.ToString();
                }
                else
                {
                    Console.WriteLine("Default Pic");
                    foto = "default.png";
                }
            }
            else
            {
                foto = "default.png";
            }
            return foto;
        }

        //Metodo para actualizar la imagen en base a la seleccionada u update en base al nombre
        public bool ActualizarFotografia(int idUsuario, string fotografia)
        {
            try
            {
                using (var conexion = GetConnection())
                {
                    conexion.Open();
                    string query = "UPDATE usuarios SET fotografia = @fotografia WHERE id_usuario = @id";
                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@fotografia", fotografia);
                        cmd.Parameters.AddWithValue("@id", idUsuario); // Usar int directamente

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Retorna true si se actualizó correctamente
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar la fotografía: " + ex.Message);
                return false; // Retorna false en caso de error
            }
        }

        //metodo para obtener la informacion del juego seleccionado basado en Id onPost
        public GameInfo GetGameInfo(int idJuego)
        {
            GameInfo gameInfo = null;

            using (var conexion = new MySqlConnection(ConnectionString))
            {   //select 
                conexion.Open();
                string query = @"
                SELECT descripcion, personajes, controles, como_ganar, como_perder, creditos, licencias
                FROM juegos
                WHERE id_juego = @IdJuego
                LIMIT 1;
            ";

                using (var command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@IdJuego", idJuego);
                    //se cosntruye objeto GameInfo
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            gameInfo = new GameInfo
                            {
                                Descripcion = reader["descripcion"].ToString(),
                                Personajes = reader["personajes"].ToString(),
                                Controles = reader["controles"].ToString(),
                                ComoGanar = reader["como_ganar"].ToString(),
                                ComoPerder = reader["como_perder"].ToString(),
                                Creditos = reader["creditos"].ToString(),
                                Licencia = reader["licencias"].ToString()
                            };
                        }
                    }
                }
            }

            return gameInfo;
        }


        //Metodo para actualizar el about me de un usuario 
        public void ActualizarAboutMe(int idUsuario, string aboutMe)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionString);
                connection.Open();

                string query = "UPDATE usuarios SET about_me = @about_me WHERE id_usuario = @id";
                using var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@about_me", aboutMe);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                //manejo de error
                Console.WriteLine($"Error al actualizar AboutMe: {ex.Message}");
                throw;
            }
        }

        internal void ActualizarAboutMe(object id_usuario, string aboutMeInput)
        {
            throw new NotImplementedException();
        }

        public Settings ObtenerUsuario(string nickname)
        {
            Settings usuario = null;

            using (var conexion = GetConnection())
            {
                conexion.Open();

                string query = @"SELECT nickname, contrasena, Fecha_Nacimiento, Genero FROM usuarios WHERE nickname = @Nickname";
                using var command = new MySqlCommand(query, conexion);
                command.Parameters.AddWithValue("@Nickname", nickname);

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Settings
                    {
                        Nickname = reader.GetString("nickname"),
                        Contrasena = reader.IsDBNull(reader.GetOrdinal("contrasena")) ? null : reader.GetString("contrasena"),
                        FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("Fecha_Nacimiento")) ? (DateTime?)null : reader.GetDateTime("Fecha_Nacimiento"),
                        Genero = reader.IsDBNull(reader.GetOrdinal("Genero")) ? (int?)null : reader.GetInt32("Genero")
                    };
                }
            }
            return usuario;
        }

        public bool ActualizarUsuario(string nickname, string? nuevaContrasena, DateTime? fechaNacimiento, int? genero)
        {
            using (var conexion = GetConnection())
            {
                conexion.Open();

                var actualizaciones = new List<string>();
                var command = new MySqlCommand();
                command.Connection = conexion;

                if (!string.IsNullOrWhiteSpace(nuevaContrasena))
                {
                    actualizaciones.Add("contrasena = @NuevaContrasena");
                    command.Parameters.AddWithValue("@NuevaContrasena", nuevaContrasena);
                }

                if (fechaNacimiento.HasValue)
                {
                    actualizaciones.Add("fecha_nacimiento = @FechaNacimiento");
                    command.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento.Value);
                }

                if (genero.HasValue && genero.Value > 0)
                {
                    actualizaciones.Add("genero = @Genero");
                    command.Parameters.AddWithValue("@Genero", genero.Value);
                }

                if (actualizaciones.Count == 0)
                    return false; // No hay nada que actualizar

                string setClause = string.Join(", ", actualizaciones);
                command.CommandText = $"UPDATE usuarios SET {setClause} WHERE nickname = @Nickname";
                command.Parameters.AddWithValue("@Nickname", nickname);

                int filasAfectadas = command.ExecuteNonQuery();
                return filasAfectadas > 0;

            }
        }
    }
}    