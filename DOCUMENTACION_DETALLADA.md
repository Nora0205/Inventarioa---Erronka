# Documentacion Detallada de StockBaseApp

Esta documentacion describe la estructura, clases y metodos del proyecto **StockBaseApp**, una aplicacion de gestion de inventario desarrollada en C# con Windows Forms y MySQL.

---

## 1. Capa de Modelos (Namespace: StockBaseApp.Modeloak)

Los modelos representan las entidades de datos del sistema.

### Clase: `Gailua` (Dispositivo)
Clase base para todos los dispositivos del inventario.
- **Propiedades:**
  - `IdGailua`: Identificador unico.
  - `Marka`: Marca del dispositivo.
  - `Modeloa`: Modelo especifico.
  - `Kokalekua`: Ubicacion fisica (aula o sala).
  - `Egoera`: Estado actual (Activo, Mantenimiento, Roto, etc.).
  - `ErosketaData`: Fecha de compra.
  - `IdMintegia`: ID del taller/departamento al que pertenece.
- **Metodos:**
  - `LortuInformazioa()`: Retorna una cadena con la informacion basica del dispositivo.

### Clase: `Ordenagailua` (Ordenador)
Hereda de `Gailua`. Añade caracteristicas especificas de PC.
- **Propiedades:**
  - `Prozesagailua`: Modelo de CPU.
  - `RamGB`: Memoria RAM en GB.
- **Metodos:**
  - `LortuInformazioa()`: Sobrescribe el metodo base para incluir CPU y RAM.

### Clase: `Inprimagailua` (Impresora)
Hereda de `Gailua`.
- **Propiedades:**
  - `Koloretakoa`: Booleano que indica si es a color.
- **Metodos:**
  - `LortuInformazioa()`: Sobrescribe el metodo base para incluir si es a color.

### Clase: `Erabiltzailea` (Usuario)
Representa a los usuarios del sistema.
- **Propiedades:**
  - `IdErabiltzailea`, `Izena`, `Email`, `Rola`, `Pasahitza`.
  - `MintegiJabea`: Taller al que pertenece el usuario.

### Clase: `Mintegia` (Taller/Departamento)
- **Propiedades:**
  - `IdMintegia`, `Izena`.
  - `Gailuak`: Lista de dispositivos asociados.

---

## 2. Capa de Controladores (Namespace: StockBaseApp.Kontrolagailuak)

Contiene la logica de negocio y acceso a datos.

### Clase: `Konexioa` (Conexion)
Gestiona la conexion a la base de datos MySQL.
- **Metodos:**
  - `LortuKonexioa()`: Crea y retorna un objeto `MySqlConnection` usando los parametros configurados (servidor, BD, usuario, password). Intenta leer el servidor desde un archivo `config.txt`.

### Clase: `InbentarioSistema` (Sistema de Inventario)
Es el corazon de la logica de la aplicacion.
- **Metodos principales:**
  - `SaioaHasi(izena, pasahitza)`: Valida credenciales y retorna un objeto `Erabiltzailea` si es correcto.
  - `GailuaGehitu(gailua, kokalekua)`: Inserta un dispositivo en la base de datos. Usa transacciones para insertar tanto en la tabla base `Gailua` como en las tablas especificas (`Ordenagailua` o `Inprimagailua`).
  - `LortuGailuakGuztiak(idMintegia?)`: Retorna un `DataTable` con todos los dispositivos activos, uniendo tablas mediante `UNION ALL`.
  - `GailuaEzabatu(gailua)`: Mueve el dispositivo a la tabla de historico `EzabatutakoGailua` y lo borra de las tablas activas.
  - `GailuaEgoeraAldatu(id, egoeraBerria)`: Actualiza el estado de un dispositivo. Si el estado es 'Hautsia' (Roto) o 'Mantentze-lanetan' (Mantenimiento), lo mueve automaticamente al 'IKT Tailerra'.
  - `ErabiltzaileaGehitu(...)`, `ErabiltzaileaEzabatu(...)`, `ErabiltzaileGuztiakLortu()`: CRUD de usuarios.
  - `LortuMintegiak()`, `LortuKokalekuak()`: Obtienen listas de referencia para los desplegables de la UI.

---

## 3. Capa de Interfaz (Forms)

### `LoginForm.cs`
- Pantalla de inicio de sesion.
- Gestiona la entrada de usuario y contraseña.
- Llama a `InbentarioSistema.SaioaHasi`.

### `MainForm.cs`
- Menu principal de la aplicacion.
- Muestra opciones segun el rol del usuario (IKT Arduraduna, Mintegi burua, Irakaslea).
- Permite navegar a la adicion de dispositivos, visualizacion de inventario y gestion de usuarios.

### `AddComputerForm.cs` y `AddPrinterForm.cs`
- Formularios para registrar nuevos dispositivos.
- Incluyen logica para asignar automaticamente el taller (`Mintegia`) basandose en el nombre del aula (`Kokalekua`).

### `ViewDevicesForm.cs`
- Muestra una rejilla (`DataGridView`) con el inventario actual.
- Permite filtrar por taller si el usuario tiene permisos.

### `ManageUsersForm.cs`, `CreateUserForm.cs`, `DeleteUserForm.cs`
- Conjunto de formularios para administrar las cuentas de usuario.
- Restringen acciones segun el nivel de privilegio (ej. un profesor no puede borrar usuarios).

### `EzabatuakForm.cs` (Bajas y Mantenimiento)
- Panel centralizado para gestionar dispositivos que no estan operativos.
- **Funcionalidades:**
  - **Activar**: Marca un dispositivo como reparado y lo devuelve al inventario general.
  - **Mantenimiento**: Cambia el estado a mantenimiento.
  - **Borrado Definitivo**: Elimina el dispositivo de las tablas activas y guarda su registro en el historico.

---

## 4. Flujos Especiales

1. **Asignacion de Taller Automatica**: Al elegir una ubicacion que empieza por prefijos especificos (ej. 'PAAG', 'Mekanika'), el sistema pre-selecciona el taller correspondiente y bloquea el selector para asegurar la integridad de los datos.
2. **Gestion de Estados**: El sistema diferencia entre dispositivos en uso, dispositivos rotos y dispositivos que han sido eliminados permanentemente del stock fisico pero cuyo registro se conserva por trazabilidad.
