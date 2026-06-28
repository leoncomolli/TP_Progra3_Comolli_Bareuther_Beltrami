using System;
using MySql.Data.MySqlClient; 

namespace Progra3Card.Administrativo
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=mi_banco_db;Uid=root;Pwd=;";

        static void Main(string[] args)
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    SISTEMA ADMINISTRATIVO PROGRA3CARD   ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Emitir Nueva Tarjeta (Alta de Cliente)");
                Console.WriteLine("2. Listar Tarjetas");
                Console.WriteLine("3. Ver Detalle de una Tarjeta / Cliente");
                Console.WriteLine("4. Eliminar Tarjeta (Baja de Sistema)");
                Console.WriteLine("5. Emitir Nueva Liquidación Mensual");
                Console.WriteLine("6. Salir");
                Console.WriteLine("========================================");
                Console.Write("Seleccione una opción: ");

                switch (Console.ReadLine())
                {
                    case "1": MenuEmitirTarjeta(); break;
                    case "2": MenuListarTarjetas(); break;
                    case "3": MenuVerDetalleTarjeta(); break;
                    case "4": MenuEliminarTarjeta(); break;
                    case "5": MenuEmitirLiquidacion(); break;
                    case "6": salir = true; break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Funciones a completar:

        static void MenuEmitirTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA TARJETA / ALTA DE CLIENTE ---");

            Console.Write("Documento: ");
            string documento = Console.ReadLine();

            Console.Write("Tipo de Documento (DNI/PASAPORTE): ");
            string tipoDoc = Console.ReadLine().ToUpper();

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string apellido = Console.ReadLine();

            Console.Write("Fecha de Nacimiento (YYYY-MM-DD): ");
            string fechaNacimientoTexto = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Número de Tarjeta (16 dígitos): ");
            string numeroTarjeta = Console.ReadLine();

            Console.Write("Banco Emisor: ");
            string bancoEmisor = Console.ReadLine();

            Console.Write("Saldo Inicial: ");
            string saldoTexto = Console.ReadLine();

            DateTime fechaNacimiento;
            decimal saldoInicial;

            if (!DateTime.TryParse(fechaNacimientoTexto, out fechaNacimiento))
            {
                Console.WriteLine("\nFecha inválida.");
                Console.WriteLine("\nPresione una tecla para volver al menú...");
                Console.ReadKey();
                return;
            }

            if (!decimal.TryParse(saldoTexto, out saldoInicial))
            {
                Console.WriteLine("\nSaldo inválido.");
                Console.WriteLine("\nPresione una tecla para volver al menú...");
                Console.ReadKey();
                return;
            }

            bool exito = EmitirNuevaTarjeta(documento, tipoDoc, nombre, apellido, fechaNacimiento, email, numeroTarjeta, bancoEmisor, saldoInicial);

            if (exito)
                Console.WriteLine("\nTarjeta emitida correctamente.");
            else
                Console.WriteLine("\nNo se pudo emitir la tarjeta. Verifique los datos ingresados.");

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuListarTarjetas()
        {
            Console.Clear();
            Console.WriteLine("--- LISTADO GENERAL DE TARJETAS ---");
            Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", "Nro Cuenta", "Nro Tarjeta", "Banco Emisor", "DNI Titular");
            Console.WriteLine("----------------------------------------------------------------------");

            // === A realizar ===
            // Aquí deben implementar un SELECT sobre la tabla 'tarjetas'
            // para recorrer las filas e imprimirlas en la consola.
            
            ObtenerYMostrarTarjetas();

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuVerDetalleTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- DETALLE DE TARJETA Y CLIENTE ---");
            Console.Write("Ingrese el Número de Cuenta a consultar: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            // === A realizar ===
            // Aquí deben realizar un SELECT con un JOIN entre 'tarjetas' y 'usuarios' 
            // filtrando por el numCuenta para traer todos los campos (Nombre, Apellido, Email, Saldo, etc.)
            
            MostrarDetalleCompleto(numCuenta);

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEliminarTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR TARJETA DEL SISTEMA ---");
            Console.Write("Ingrese el Número de Cuenta de la tarjeta a dar de baja: ");
            int numCuenta = Convert.ToInt32(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️ ADVERTENCIA: Se eliminará la tarjeta, sus liquidaciones y los datos de acceso web vinculados.");
            Console.ResetColor();
            Console.Write("¿Está seguro de continuar? (S/N): ");

            if (Console.ReadLine().ToUpper() == "S")
            {
                // === A realizar ===
                // Aquí deben ejecutar un DELETE sobre la tabla 'tarjetas' donde num_cuenta = numCuenta.
                // Como definimos ON DELETE CASCADE en la base de datos, las liquidaciones se borrarán solas.
                // Opcional: Evaluar si también eliminan al usuario de la tabla 'usuarios' o si lo mantienen.
                
                bool exito = DarDeBajaTarjeta(numCuenta);

                if (exito)
                    Console.WriteLine("\nTarjeta eliminada correctamente del sistema.");
                else
                    Console.WriteLine("\nError al intentar eliminar la tarjeta. Verifique el número de cuenta.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }

        static void MenuEmitirLiquidacion()
        {
            Console.Clear();
            Console.WriteLine("--- EMITIR NUEVA LIQUIDACIÓN MENSUAL ---");

            Console.Write("Número de Cuenta: ");
            string cuentaTexto = Console.ReadLine();

            Console.Write("Período (YYYY-MM): ");
            string periodo = Console.ReadLine();

            Console.Write("Fecha de Vencimiento (YYYY-MM-DD): ");
            string fechaVencimientoTexto = Console.ReadLine();

            Console.Write("Total a Pagar: ");
            string totalTexto = Console.ReadLine();

            Console.Write("Pago Mínimo: ");
            string pagoMinimoTexto = Console.ReadLine();

            int numCuenta;
            DateTime fechaVencimiento;
            decimal totalAPagar;
            decimal pagoMinimo;

            if (!int.TryParse(cuentaTexto, out numCuenta))
            {
                Console.WriteLine("\nNúmero de cuenta inválido.");
                Console.WriteLine("\nPresione una tecla para volver al menú...");
                Console.ReadKey();
                return;
            }

            if (!DateTime.TryParse(fechaVencimientoTexto, out fechaVencimiento))
            {
                Console.WriteLine("\nFecha de vencimiento inválida.");
                Console.WriteLine("\nPresione una tecla para volver al menú...");
                Console.ReadKey();
                return;
            }

            if (!decimal.TryParse(totalTexto, out totalAPagar) || !decimal.TryParse(pagoMinimoTexto, out pagoMinimo))
            {
                Console.WriteLine("\nLos importes ingresados no son válidos.");
                Console.WriteLine("\nPresione una tecla para volver al menú...");
                Console.ReadKey();
                return;
            }

            bool exito = EmitirNuevaLiquidacion(numCuenta, periodo, fechaVencimiento, totalAPagar, pagoMinimo);

            if (exito)
                Console.WriteLine("\nLiquidación emitida correctamente.");
            else
                Console.WriteLine("\nNo se pudo emitir la liquidación. Verifique el número de cuenta.");

            Console.WriteLine("\nPresione una tecla para volver al menú...");
            Console.ReadKey();
        }


        // =========================================================================
        // MÉTODOS BASE QUE DEBEN COMPLETAR CON LA LÓGICA 
        // =========================================================================

        static void ObtenerYMostrarTarjetas()
        {
            // Completar 
            // Ejemplo de impresión dentro del bucle: 
            // Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", reader["num_cuenta"], reader["numero_tarjeta"], ...);

            string query = "SELECT num_cuenta, numero_tarjeta, banco_emisor, dni_titular FROM tarjetas ORDER BY num_cuenta";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        bool hayRegistros = false;

                        while (reader.Read())
                        {
                            hayRegistros = true;
                            Console.WriteLine(
                                "{0,-12} {1,-18} {2,-20} {3,-15}",
                                reader["num_cuenta"],
                                reader["numero_tarjeta"],
                                reader["banco_emisor"],
                                reader["dni_titular"]
                            );
                        }

                        if (!hayRegistros)
                            Console.WriteLine("No hay tarjetas registradas.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar tarjetas: " + ex.Message);
            }
        }

        static void MostrarDetalleCompleto(int cuenta)
        {
            // Completar haciendo un SELECT con JOIN de usuarios y tarjetas WHERE num_cuenta = @cuenta

            string query = @"SELECT t.num_cuenta, t.numero_tarjeta, t.banco_emisor, t.estado, t.saldo,
                                    u.documento, u.tipo_doc, u.nombre, u.apellido, u.fecha_nacimiento, u.email, u.usuario
                             FROM tarjetas t
                             INNER JOIN usuarios u ON t.dni_titular = u.documento
                             WHERE t.num_cuenta = @cuenta";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cuenta", cuenta);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Console.WriteLine("\nNro Cuenta: " + reader["num_cuenta"]);
                                Console.WriteLine("Nro Tarjeta: " + reader["numero_tarjeta"]);
                                Console.WriteLine("Banco Emisor: " + reader["banco_emisor"]);
                                Console.WriteLine("Estado: " + reader["estado"]);
                                Console.WriteLine("Saldo: " + reader["saldo"]);
                                Console.WriteLine("Documento: " + reader["documento"]);
                                Console.WriteLine("Tipo de Documento: " + reader["tipo_doc"]);
                                Console.WriteLine("Nombre: " + reader["nombre"]);
                                Console.WriteLine("Apellido: " + reader["apellido"]);
                                Console.WriteLine("Fecha de Nacimiento: " + Convert.ToDateTime(reader["fecha_nacimiento"]).ToString("yyyy-MM-dd"));
                                Console.WriteLine("Email: " + reader["email"]);
                                Console.WriteLine("Usuario Web: " + (reader["usuario"] == DBNull.Value ? "Sin activar" : reader["usuario"].ToString()));
                            }
                            else
                            {
                                Console.WriteLine("\nNo se encontró una tarjeta con ese número de cuenta.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar el detalle: " + ex.Message);
            }
        }

        static bool DarDeBajaTarjeta(int cuenta)
        {
            // Completar usando un DELETE FROM tarjetas WHERE num_cuenta = @cuenta
            string query = "DELETE FROM tarjetas WHERE num_cuenta = @cuenta";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cuenta", cuenta);
                        int filasAfectadas = command.ExecuteNonQuery();

                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar la tarjeta: " + ex.Message);
                return false;
            }
        }

        static bool EmitirNuevaTarjeta(string documento, string tipoDoc, string nombre, string apellido, DateTime fechaNacimiento, string email, string numeroTarjeta, string bancoEmisor, decimal saldoInicial)
        {
            string queryExisteUsuario = "SELECT COUNT(*) FROM usuarios WHERE documento = @documento";
            string queryExisteTarjeta = "SELECT COUNT(*) FROM tarjetas WHERE dni_titular = @documento";
            string queryInsertUsuario = @"INSERT INTO usuarios (documento, tipo_doc, nombre, apellido, fecha_nacimiento, email, usuario, password)
                                          VALUES (@documento, @tipoDoc, @nombre, @apellido, @fechaNacimiento, @email, NULL, NULL)";
            string queryInsertTarjeta = @"INSERT INTO tarjetas (numero_tarjeta, banco_emisor, estado, saldo, dni_titular)
                                          VALUES (@numeroTarjeta, @bancoEmisor, 'Activa', @saldo, @documento)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        bool usuarioExiste;

                        using (MySqlCommand commandExisteUsuario = new MySqlCommand(queryExisteUsuario, connection, transaction))
                        {
                            commandExisteUsuario.Parameters.AddWithValue("@documento", documento);
                            usuarioExiste = Convert.ToInt32(commandExisteUsuario.ExecuteScalar()) > 0;
                        }

                        using (MySqlCommand commandExisteTarjeta = new MySqlCommand(queryExisteTarjeta, connection, transaction))
                        {
                            commandExisteTarjeta.Parameters.AddWithValue("@documento", documento);

                            if (Convert.ToInt32(commandExisteTarjeta.ExecuteScalar()) > 0)
                            {
                                transaction.Rollback();
                                Console.WriteLine("\nEl cliente ya posee una tarjeta registrada.");
                                return false;
                            }
                        }

                        if (!usuarioExiste)
                        {
                            using (MySqlCommand commandInsertUsuario = new MySqlCommand(queryInsertUsuario, connection, transaction))
                            {
                                commandInsertUsuario.Parameters.AddWithValue("@documento", documento);
                                commandInsertUsuario.Parameters.AddWithValue("@tipoDoc", tipoDoc);
                                commandInsertUsuario.Parameters.AddWithValue("@nombre", nombre);
                                commandInsertUsuario.Parameters.AddWithValue("@apellido", apellido);
                                commandInsertUsuario.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                                commandInsertUsuario.Parameters.AddWithValue("@email", email);
                                commandInsertUsuario.ExecuteNonQuery();
                            }
                        }

                        using (MySqlCommand commandInsertTarjeta = new MySqlCommand(queryInsertTarjeta, connection, transaction))
                        {
                            commandInsertTarjeta.Parameters.AddWithValue("@numeroTarjeta", numeroTarjeta);
                            commandInsertTarjeta.Parameters.AddWithValue("@bancoEmisor", bancoEmisor);
                            commandInsertTarjeta.Parameters.AddWithValue("@saldo", saldoInicial);
                            commandInsertTarjeta.Parameters.AddWithValue("@documento", documento);
                            commandInsertTarjeta.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al emitir la tarjeta: " + ex.Message);
                return false;
            }
        }

        static bool EmitirNuevaLiquidacion(int numCuenta, string periodo, DateTime fechaVencimiento, decimal totalAPagar, decimal pagoMinimo)
        {
            string queryExisteTarjeta = "SELECT COUNT(*) FROM tarjetas WHERE num_cuenta = @cuenta";
            string queryInsertLiquidacion = @"INSERT INTO liquidaciones (num_cuenta, periodo, fecha_vencimiento, total_a_pagar, pago_minimo)
                                              VALUES (@cuenta, @periodo, @fechaVencimiento, @totalAPagar, @pagoMinimo)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand commandExisteTarjeta = new MySqlCommand(queryExisteTarjeta, connection))
                    {
                        commandExisteTarjeta.Parameters.AddWithValue("@cuenta", numCuenta);

                        if (Convert.ToInt32(commandExisteTarjeta.ExecuteScalar()) == 0)
                            return false;
                    }

                    using (MySqlCommand commandInsertLiquidacion = new MySqlCommand(queryInsertLiquidacion, connection))
                    {
                        commandInsertLiquidacion.Parameters.AddWithValue("@cuenta", numCuenta);
                        commandInsertLiquidacion.Parameters.AddWithValue("@periodo", periodo);
                        commandInsertLiquidacion.Parameters.AddWithValue("@fechaVencimiento", fechaVencimiento);
                        commandInsertLiquidacion.Parameters.AddWithValue("@totalAPagar", totalAPagar);
                        commandInsertLiquidacion.Parameters.AddWithValue("@pagoMinimo", pagoMinimo);
                        return commandInsertLiquidacion.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al emitir la liquidacion: " + ex.Message);
                return false;
            }
        }
    }
}
