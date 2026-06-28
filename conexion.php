<?php
function obtenerConexion()
{
    $conexion = new mysqli("localhost", "root", "", "mi_banco_db");

    if ($conexion->connect_error) {
        die("Error de conexion: " . $conexion->connect_error);
    }

    $conexion->set_charset("utf8mb4");

    return $conexion;
}
