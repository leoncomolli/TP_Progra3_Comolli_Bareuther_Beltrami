<?php
require_once "conexion.php";

if ($_SERVER["REQUEST_METHOD"] !== "POST") {
    header("Location: registro.html");
    exit;
}

$tipoDoc = trim($_POST["tipo_doc"]);
$documento = trim($_POST["documento"]);
$nombre = trim($_POST["nombre"]);
$apellido = trim($_POST["apellido"]);
$fechaNacimiento = trim($_POST["fecha_nacimiento"]);
$email = trim($_POST["email"]);
$usuario = trim($_POST["usuario"]);
$passwordA = trim($_POST["passwordA"]);
$passwordB = trim($_POST["passwordB"]);

$mensaje = "";
$volver = "registro.html";

if ($passwordA !== $passwordB) {
    $mensaje = "Las contrasenas no coinciden.";
} else {
    $conexion = obtenerConexion();

    $sql = "SELECT u.documento, u.usuario
            FROM usuarios u
            INNER JOIN tarjetas t ON t.dni_titular = u.documento
            WHERE u.documento = ?
              AND u.tipo_doc = ?
              AND u.nombre = ?
              AND u.apellido = ?
              AND u.fecha_nacimiento = ?
              AND u.email = ?";

    $stmt = $conexion->prepare($sql);
    $stmt->bind_param("ssssss", $documento, $tipoDoc, $nombre, $apellido, $fechaNacimiento, $email);
    $stmt->execute();
    $resultado = $stmt->get_result();

    if ($resultado->num_rows === 0) {
        $mensaje = "No se encontro un cliente con esos datos o no tiene una tarjeta emitida.";
    } else {
        $fila = $resultado->fetch_assoc();

        if (!is_null($fila["usuario"])) {
            $mensaje = "La cuenta web ya fue activada anteriormente.";
            $volver = "ingreso.html";
        } else {
            $update = $conexion->prepare("UPDATE usuarios SET usuario = ?, password = ? WHERE documento = ?");
            $update->bind_param("sss", $usuario, $passwordA, $documento);

            if ($update->execute()) {
                $mensaje = "Cuenta web activada correctamente.";
                $volver = "ingreso.html";
            } else {
                $mensaje = "No se pudo activar la cuenta. Verifique si el usuario ya existe.";
            }

            $update->close();
        }
    }

    $stmt->close();
    $conexion->close();
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Resultado de Activacion</title>
</head>
<body>
    <h1>Activacion de Usuario</h1>
    <p><?php echo htmlspecialchars($mensaje); ?></p>
    <p><a href="<?php echo htmlspecialchars($volver); ?>">Volver</a></p>
</body>
</html>
