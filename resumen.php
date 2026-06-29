<?php
session_start();
require_once "conexion.php";

if (!isset($_SESSION["documento"]) || !isset($_SESSION["num_cuenta"])) {
    header("Location: ingreso.html");
    exit;
}

$documento = $_SESSION["documento"];
$numCuenta = $_SESSION["num_cuenta"];

$conexion = obtenerConexion();

$sqlCliente = "SELECT u.nombre, u.apellido, u.documento, u.email,
                      t.num_cuenta, t.numero_tarjeta, t.banco_emisor, t.estado, t.saldo
               FROM usuarios u
               INNER JOIN tarjetas t ON t.dni_titular = u.documento
               WHERE u.documento = ?";

$stmtCliente = $conexion->prepare($sqlCliente);
$stmtCliente->bind_param("s", $documento);
$stmtCliente->execute();
$cliente = $stmtCliente->get_result()->fetch_assoc();
$stmtCliente->close();

$sqlUltima = "SELECT periodo, fecha_vencimiento, total_a_pagar, pago_minimo
              FROM liquidaciones
              WHERE num_cuenta = ?
              ORDER BY periodo DESC, id_liquidacion DESC
              LIMIT 1";

$stmtUltima = $conexion->prepare($sqlUltima);
$stmtUltima->bind_param("i", $numCuenta);
$stmtUltima->execute();
$ultimaLiquidacion = $stmtUltima->get_result()->fetch_assoc();
$stmtUltima->close();

$sqlHistorial = "SELECT periodo, fecha_vencimiento, total_a_pagar, pago_minimo
                 FROM liquidaciones
                 WHERE num_cuenta = ?
                 ORDER BY periodo DESC, id_liquidacion DESC";

$stmtHistorial = $conexion->prepare($sqlHistorial);
$stmtHistorial->bind_param("i", $numCuenta);
$stmtHistorial->execute();
$historial = $stmtHistorial->get_result();
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Resumen de Cuenta</title>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <main class="page-shell">
        <div class="summary-container">
            <header class="page-header">
                <div>
                    <h1>Mis Tarjetas</h1>
                    <p class="welcome-text">Bienvenido, <?php echo htmlspecialchars($_SESSION["nombre"] . " " . $_SESSION["apellido"]); ?></p>
                </div>
                <p class="logout-row"><a class="logout-link" href="logout.php">Cerrar sesion</a></p>
            </header>

            <section class="content-section">
                <h2>Datos del Cliente y la Tarjeta</h2>
                <div class="details-grid">
                    <p class="detail-item">Documento: <?php echo htmlspecialchars($cliente["documento"]); ?></p>
                    <p class="detail-item">Email: <?php echo htmlspecialchars($cliente["email"]); ?></p>
                    <p class="detail-item">Numero de Cuenta: <?php echo htmlspecialchars($cliente["num_cuenta"]); ?></p>
                    <p class="detail-item">Numero de Tarjeta: <?php echo htmlspecialchars($cliente["numero_tarjeta"]); ?></p>
                    <p class="detail-item">Banco Emisor: <?php echo htmlspecialchars($cliente["banco_emisor"]); ?></p>
                    <p class="detail-item">Estado: <?php echo htmlspecialchars($cliente["estado"]); ?></p>
                    <p class="detail-item">Saldo: $<?php echo htmlspecialchars($cliente["saldo"]); ?></p>
                </div>
            </section>

            <section class="content-section">
                <h2>Ultima Liquidacion</h2>
                <?php if ($ultimaLiquidacion) { ?>
                    <div class="details-grid">
                        <p class="detail-item">Periodo: <?php echo htmlspecialchars($ultimaLiquidacion["periodo"]); ?></p>
                        <p class="detail-item">Fecha de Vencimiento: <?php echo htmlspecialchars($ultimaLiquidacion["fecha_vencimiento"]); ?></p>
                        <p class="detail-item">Total a Pagar: $<?php echo htmlspecialchars($ultimaLiquidacion["total_a_pagar"]); ?></p>
                        <p class="detail-item">Pago Minimo: $<?php echo htmlspecialchars($ultimaLiquidacion["pago_minimo"]); ?></p>
                    </div>
                <?php } else { ?>
                    <p class="empty-state">No hay liquidaciones cargadas para esta tarjeta.</p>
                <?php } ?>
            </section>

            <section class="content-section">
                <h2>Historial de Liquidaciones</h2>
                <?php if ($historial->num_rows > 0) { ?>
                    <div class="table-wrapper">
                        <table class="history-table">
                            <tr>
                                <th>Periodo</th>
                                <th>Fecha de Vencimiento</th>
                                <th>Total a Pagar</th>
                                <th>Pago Minimo</th>
                            </tr>
                            <?php while ($fila = $historial->fetch_assoc()) { ?>
                                <tr>
                                    <td><?php echo htmlspecialchars($fila["periodo"]); ?></td>
                                    <td><?php echo htmlspecialchars($fila["fecha_vencimiento"]); ?></td>
                                    <td>$<?php echo htmlspecialchars($fila["total_a_pagar"]); ?></td>
                                    <td>$<?php echo htmlspecialchars($fila["pago_minimo"]); ?></td>
                                </tr>
                            <?php } ?>
                        </table>
                    </div>
                <?php } else { ?>
                    <p class="empty-state">No hay historial de liquidaciones para mostrar.</p>
                <?php } ?>
            </section>
        </div>
    </main>
</body>
</html>
<?php
$stmtHistorial->close();
$conexion->close();
?>
