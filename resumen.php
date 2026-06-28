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
</head>
<body>
    <h1>Mis Tarjetas</h1>
    <p>Bienvenido, <?php echo htmlspecialchars($_SESSION["nombre"] . " " . $_SESSION["apellido"]); ?></p>
    <p><a href="logout.php">Cerrar sesion</a></p>

    <h2>Datos del Cliente y la Tarjeta</h2>
    <p>Documento: <?php echo htmlspecialchars($cliente["documento"]); ?></p>
    <p>Email: <?php echo htmlspecialchars($cliente["email"]); ?></p>
    <p>Numero de Cuenta: <?php echo htmlspecialchars($cliente["num_cuenta"]); ?></p>
    <p>Numero de Tarjeta: <?php echo htmlspecialchars($cliente["numero_tarjeta"]); ?></p>
    <p>Banco Emisor: <?php echo htmlspecialchars($cliente["banco_emisor"]); ?></p>
    <p>Estado: <?php echo htmlspecialchars($cliente["estado"]); ?></p>
    <p>Saldo: $<?php echo htmlspecialchars($cliente["saldo"]); ?></p>

    <h2>Ultima Liquidacion</h2>
    <?php if ($ultimaLiquidacion) { ?>
        <p>Periodo: <?php echo htmlspecialchars($ultimaLiquidacion["periodo"]); ?></p>
        <p>Fecha de Vencimiento: <?php echo htmlspecialchars($ultimaLiquidacion["fecha_vencimiento"]); ?></p>
        <p>Total a Pagar: $<?php echo htmlspecialchars($ultimaLiquidacion["total_a_pagar"]); ?></p>
        <p>Pago Minimo: $<?php echo htmlspecialchars($ultimaLiquidacion["pago_minimo"]); ?></p>
    <?php } else { ?>
        <p>No hay liquidaciones cargadas para esta tarjeta.</p>
    <?php } ?>

    <h2>Historial de Liquidaciones</h2>
    <?php if ($historial->num_rows > 0) { ?>
        <table border="1" cellpadding="6" cellspacing="0">
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
    <?php } else { ?>
        <p>No hay historial de liquidaciones para mostrar.</p>
    <?php } ?>
</body>
</html>
<?php
$stmtHistorial->close();
$conexion->close();
?>
