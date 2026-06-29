# Trabajo Práctico Programación III - Progra3Card

## Sistema de Consulta de Liquidaciones de Tarjetas

Proyecto realizado para la materia *Programación III* de la *Tecnicatura Universitaria en Programación*.

El sistema permite administrar tarjetas de crédito desde una aplicación de consola en C# y consultar liquidaciones desde un portal web desarrollado en PHP. Ambas partes trabajan sobre la misma base de datos MySQL llamada mi_banco_db.

---

## Integrantes

* León Comolli
* Bareuther
* Beltrami

---

## Tecnologías utilizadas

* C#
* .NET 8
* MySQL / MariaDB
* PHP
* HTML
* XAMPP
* phpMyAdmin
* MySql.Data

---

## Estructura del proyecto

text
TP_Progra3_Comolli_Bareuther_Beltrami/
│
├── Progra3card.cs
├── Progra3card.csproj
│
├── mi_banco_db.sql
│
├── ingreso.html
├── registro.html
│
├── conexion.php
├── altas.php
├── ingreso.php
├── resumen.php
├── logout.php
│
├── .gitignore
└── README.md


---

## Base de datos

El sistema utiliza la base de datos:

text
mi_banco_db


La base contiene tres tablas principales:

### usuarios

Guarda los datos personales del cliente y sus credenciales web.

### tarjetas

Guarda los datos de la tarjeta asociada al cliente, como número de cuenta, número de tarjeta, banco emisor, estado y saldo.

### liquidaciones

Guarda los resúmenes mensuales de cada tarjeta, incluyendo período, vencimiento, total a pagar y pago mínimo.

El archivo para crear e importar la base es:

text
mi_banco_db.sql


---

## Funcionamiento general

El proyecto está dividido en dos partes principales.

## 1. Aplicación administrativa en C#

La aplicación de consola está destinada al personal administrativo de la entidad financiera.

Desde la consola se puede:

1. Emitir una nueva tarjeta / alta de cliente.
2. Listar tarjetas existentes.
3. Ver detalle completo de una tarjeta y su cliente.
4. Eliminar una tarjeta.
5. Emitir una nueva liquidación mensual.
6. Salir del sistema.

La aplicación se conecta a la base de datos MySQL usando MySql.Data.MySqlClient.

Archivo principal:

text
Progra3card.cs


Archivo de proyecto:

text
Progra3card.csproj


---

## 2. Portal web de clientes en PHP

El portal web permite que los clientes activen su usuario web, inicien sesión y consulten sus liquidaciones.

Archivos principales:

### registro.html

Formulario de activación de usuario web.

### altas.php

Recibe los datos de registro.html, verifica que el cliente exista en la base y tenga una tarjeta asociada. Luego actualiza el usuario y la contraseña web.

### ingreso.html

Formulario de inicio de sesión.

### ingreso.php

Valida las credenciales del cliente e inicia la sesión.

### resumen.php

Muestra los datos del cliente, la tarjeta, la última liquidación y el historial de liquidaciones.

### logout.php

Cierra la sesión del usuario.

### conexion.php

Contiene la conexión centralizada a la base de datos.

---

## Requisitos para ejecutar el proyecto

Para probar el sistema se necesita:

* XAMPP instalado.
* Apache iniciado desde XAMPP.
* MySQL iniciado desde XAMPP.
* .NET SDK instalado.
* Navegador web.
* Visual Studio Code o terminal.

---

## Importar la base de datos

### Opción 1: usando phpMyAdmin

1. Abrir XAMPP.
2. Iniciar Apache y MySQL.
3. Ir a:

text
http://localhost/phpmyadmin


4. Crear o seleccionar la base de datos mi_banco_db.
5. Ir a la pestaña *Importar*.
6. Seleccionar el archivo:

text
mi_banco_db.sql


7. Ejecutar la importación.

---

### Opción 2: usando terminal

Desde la carpeta donde está el archivo mi_banco_db.sql, ejecutar:

bash
C:\xampp\mysql\bin\mysql.exe -uroot mi_banco_db < mi_banco_db.sql


---

## Ejecutar la parte web

Copiar la carpeta del proyecto dentro de:

text
C:\xampp\htdocs\


Por ejemplo:

text
C:\xampp\htdocs\tp_progra3\


Luego abrir en el navegador:

text
http://localhost/tp_progra3/ingreso.html


---

## Usuarios de prueba

Se pueden probar los siguientes accesos:

text
Tipo documento: DNI
Documento: 20123456
Usuario: carlos85
Contraseña: clave123


text
Tipo documento: DNI
Documento: 30987654
Usuario: anamar
Contraseña: clave123


text
Tipo documento: DNI
Documento: 40111222
Usuario: luciaweb
Contraseña: clave123


---

## Ejecutar la aplicación C#

Desde una terminal ubicada en la carpeta del proyecto, ejecutar:

bash
dotnet restore


Luego:

bash
dotnet build


Y finalmente:

bash
dotnet run --project Progra3card.csproj


Al ejecutar, se mostrará el menú principal de la aplicación administrativa.

---

## Pruebas realizadas

Se verificó el funcionamiento de:

* Importación de la base de datos.
* Conexión de PHP con MySQL.
* Login de clientes desde ingreso.html.
* Creación y validación de sesión.
* Visualización de resumen de cuenta.
* Visualización de historial de liquidaciones.
* Cierre de sesión.
* Compilación de la aplicación C#.
* Ejecución del menú administrativo.
* Listado de tarjetas.
* Consulta de detalle de tarjeta y cliente.
* Emisión de liquidación desde C# y visualización posterior desde el portal web.

---

## Notas importantes

Al compilar la aplicación C# pueden generarse automáticamente carpetas como:

text
bin/
obj/


Estos archivos son generados por .NET y no forman parte del código fuente del trabajo práctico.

Por eso se incluyó un archivo .gitignore para evitar subir archivos innecesarios al repositorio.

---

## Archivos principales entregados

text
README.md
mi_banco_db.sql
Progra3card.cs
Progra3card.csproj
ingreso.html
registro.html
conexion.php
altas.php
ingreso.php
resumen.php
logout.php
.gitignore


---

## Estado del proyecto

Proyecto funcional para la entrega del Trabajo Práctico Integrador de Programación III.

El sistema permite administrar tarjetas y liquidaciones desde consola C#, y consultar la información desde un portal web PHP conectado a la misma base de datos MySQL.