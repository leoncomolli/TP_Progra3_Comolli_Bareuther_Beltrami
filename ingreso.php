<?php
session_start();
require_once "conexion.php";

if ($_SERVER["REQUEST_METHOD"] !== "POST") {
    header("Location: ingreso.html");
    exit;
}

$tipoDoc = trim($_POST["tipo_doc"]);
$documento = trim($_POST["documento"]);
$usuario = trim($_POST["usuario"]);
$password = trim($_POST["password"]);

$conexion = obtenerConexion();

$sql = "SELECT u.documento, u.nombre, u.apellido, t.num_cuenta
        FROM usuarios u
        INNER JOIN tarjetas t ON t.dni_titular = u.documento
        WHERE u.documento = ?
          AND u.tipo_doc = ?
          AND u.usuario = ?
          AND u.password = ?";

$stmt = $conexion->prepare($sql);
$stmt->bind_param("ssss", $documento, $tipoDoc, $usuario, $password);
$stmt->execute();
$resultado = $stmt->get_result();

if ($resultado->num_rows === 1) {
    $fila = $resultado->fetch_assoc();

    $_SESSION["documento"] = $fila["documento"];
    $_SESSION["nombre"] = $fila["nombre"];
    $_SESSION["apellido"] = $fila["apellido"];
    $_SESSION["num_cuenta"] = $fila["num_cuenta"];

    $stmt->close();
    $conexion->close();

    header("Location: resumen.php");
    exit;
}

$stmt->close();
$conexion->close();
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Error de Ingreso</title>
</head>
<body>
    <h1>Ingreso al Portal</h1>
    <p>Los datos ingresados no son correctos.</p>
    <p><a href="ingreso.html">Volver</a></p>
</body>
</html>
