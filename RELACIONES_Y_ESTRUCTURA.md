# Arquitectura y Relaciones de Objetos - StockBaseApp

Este documento detalla la jerarquía de clases, la responsabilidad de cada objeto y cómo interactúan entre sí dentro del sistema.

---

## 1. Jerarquía de Herencia (Dispositivos)

El sistema utiliza **herencia** para gestionar los diferentes tipos de hardware, compartiendo atributos comunes en una clase base.

### [Base] `Gailua` (Dispositivo)
- **Función**: Define el "contrato" básico y las propiedades comunes de cualquier hardware del inventario (ID, Marca, Modelo, Fecha de Compra, Estado, Ubicación).
- **Relación**: Es la clase madre. No se suele instanciar directamente si existen tipos específicos.
- **Método Virtual**: `LortuInformazioa()`. Permite que las clases hijas extiendan la descripción del objeto.

### [Hija] `Ordenagailua` (Ordenador)
- **Herencia**: `Ordenagailua : Gailua`
- **Extensión**: Añade propiedades técnicas específicas: `Prozesagailua` (CPU) y `RamGB`.
- **Relación**: "Es un" `Gailua`. Sobrescribe `LortuInformazioa()` para incluir los datos del procesador y la RAM.

### [Hija] `Inprimagailua` (Impresora)
- **Herencia**: `Inprimagailua : Gailua`
- **Extensión**: Añade la propiedad booleana `Koloretakoa` (Color/BN).
- **Relación**: "Es un" `Gailua`. Sobrescribe `LortuInformazioa()` para indicar si imprime a color.

---

## 2. Modelos de Organización y Usuarios

### `Mintegia` (Departamento/Taller)
- **Función**: Agrupación lógica de recursos y personas.
- **Relación (Composición)**: Contiene una `List<Gailua>`. Un taller "posee" múltiples dispositivos.
- **Uso**: Sirve como filtro principal en la base de datos para separar el inventario por departamentos (Informática, Mecánica, etc.).

### `Erabiltzailea` (Usuario)
- **Función**: Representa a la persona física que opera el software.
- **Relación (Asociación)**: Tiene un objeto `Mintegia` asociado (`MintegiJabea`).
- **Impacto**: El `Rola` (Rol) del usuario y su `Mintegia` determinan qué botones del `MainForm` están habilitados y qué datos puede ver en el `ViewDevicesForm`.

### `EzabatutakoGailua` (Dispositivo Eliminado)
- **Función**: Objeto de transferencia de datos (DTO) para el historial.
- **Relación**: Independiente de la jerarquía `Gailua`. Se utiliza cuando un objeto es borrado de las tablas activas pero su rastro debe persistir en la base de datos de auditoría.

---

## 3. Controladores y Lógica de Conexión

### `Konexioa` (Gestión de Conexión)
- **Responsabilidad**: Única fuente de verdad para los parámetros de red (IP del servidor, BD, credenciales).
- **Relación**: Es una **dependencia** de `InbentarioSistema`. Lee la configuración de un archivo externo `config.txt`.

### `InbentarioSistema` (Orquestador / Fachada)
- **Responsabilidad**: Es el objeto más complejo. Actúa como mediador entre los modelos de C# y las tablas de MySQL.
- **Interacciones Clave**:
  - **Con `Konexioa`**: Llama a `LortuKonexioa()` cada vez que necesita ejecutar un comando SQL.
  - **Con `Gailua` (Polimorfismo)**: El método `GailuaGehitu(Gailua g)` recibe cualquier objeto que herede de `Gailua`. Mediante *pattern matching* (`if g is Ordenagailua`), decide en qué tabla específica de la base de datos debe insertar los datos adicionales.
  - **Con `DataTable`**: Transforma los objetos del dominio en tablas de datos compatibles con los controles `DataGridView` de la interfaz.

---

## 4. Relación con la Interfaz de Usuario (Forms)

Los formularios no contienen lógica de base de datos, sino que **delegan** en el controlador.

1.  **Inyección de Dependencia (Manual)**: El `MainForm` recibe el objeto `Erabiltzailea` tras el login y lo propaga a todos los demás formularios (`AddComputerForm`, `ViewDevicesForm`, etc.) para mantener el contexto de quién está operando.
2.  **Validación Cruzada**: Los formularios de alta (`AddComputerForm`) consultan a `InbentarioSistema` para obtener las listas de `Mintegia` y `Kokalekua` (Ubicaciones) disponibles, asegurando que el usuario solo elija opciones válidas existentes en la base de datos.
3.  **Lógica de Negocio Automática**: 
    - Cuando un usuario cambia el estado de un dispositivo a "Roto" en `EzabatuakForm`, el controlador `InbentarioSistema` actualiza automáticamente la propiedad `Kokalekua` a "IKT Tailerra", reflejando el movimiento físico del objeto en el modelo digital.

---

## 5. Resumen de Relaciones (Diagrama Conceptual)

- `Ordenagailua` ----|> `Gailua` (Herencia)
- `Inprimagailua` ---|> `Gailua` (Herencia)
- `Mintegia` *-------o `Gailua` (Agregación: 1 Mintegia tiene N Gailuak)
- `Erabiltzailea` ---o `Mintegia` (Asociación: 1 Usuario pertenece a 1 Mintegia)
- `InbentarioSistema` --> `Konexioa` (Uso/Dependencia)
- `Forms` --> `InbentarioSistema` (Uso para persistencia)
