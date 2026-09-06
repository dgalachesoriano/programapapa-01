# Arquitectura técnica

## Stack

- **.NET Framework 4.7.2**, aplicación de escritorio **WinForms**
  (`OutputType = WinExe`).
- Acceso a datos con **ADO.NET** (`System.Data.SqlClient`) contra
  **SQL Server**, sin ORM.
- Sin inyección de dependencias ni tests automatizados (ver
  propuestas de mejora en `work/`).

## Estructura de carpetas

```
GestionFacturas/
├── Program.cs                  Punto de entrada de la aplicación.
├── FrmPrincipal.cs(.Designer)   Formulario de menú principal.
├── FrmTratarTarea.cs(.Designer) Búsqueda/listado de facturas.
├── FrmPoolTareas.cs(.Designer)  Pantalla reservada, sin lógica aún.
├── Formularios/
│   └── FrmRegistrarTarea.cs(.Designer)  Alta y edición de facturas.
├── Modelos/                     Entidades de dominio (POCOs).
│   ├── Factura.cs
│   ├── DetalleFactura.cs
│   ├── EstadoFactura.cs
│   ├── Usuario.cs
│   ├── Segmento.cs
│   └── FiltroBusquedaFacturas.cs
├── Datos/                       Capa de acceso a datos.
│   ├── ConexionBD.cs
│   ├── FacturaRepositorio.cs
│   ├── EstadoFacturaRepositorio.cs
│   ├── UsuarioRepositorio.cs
│   └── SegmentoRepositorio.cs
└── docs/, work/                 Documentación y propuestas de mejora.
```

## Capas y responsabilidades

La refactorización introdujo una separación explícita en tres capas,
donde antes el SQL, la lógica de negocio y el código de interfaz
estaban mezclados dentro de cada formulario:

1. **Presentación (formularios)** — `FrmPrincipal`, `FrmTratarTarea`,
   `FrmRegistrarTarea`, `FrmPoolTareas`. Responsables únicamente de:
   leer/escribir controles, validar campos obligatorios, formatear la
   rejilla, y orquestar llamadas a la capa de datos. **No contienen
   SQL.**
2. **Dominio (Modelos)** — clases POCO que representan las entidades
   del proceso (`Factura`, `DetalleFactura`, `EstadoFactura`,
   `Usuario`, `Segmento`) y un objeto de criterios de búsqueda
   (`FiltroBusquedaFacturas`). Sustituyen el uso disperso de
   `DataTable`/`DataRow` sin tipar que había en el código original.
3. **Datos (Datos/\*Repositorio.cs)** — una clase repositorio por
   entidad, con métodos que encapsulan el SQL correspondiente
   (`ObtenerTodos`, `BuscarPorFiltro`, `Insertar`, `Actualizar`...).
   `FacturaRepositorio` concentra además la gestión de la
   transacción de alta/edición (cabecera + detalle en una única
   unidad atómica) que antes vivía directamente en el evento de clic
   del botón Grabar.

`ConexionBD` es la única clase que conoce la cadena de conexión; la
lee de `App.config` (sección `<connectionStrings>`) en lugar de
tenerla escrita en el código, para poder cambiar de servidor/entorno
sin recompilar.

## Por qué este diseño y no otro

Se ha optado por **repositorios concretos sin interfaces ni
contenedor de inyección de dependencias**, en lugar de una solución
más "enterprise" (repositorios genéricos, Unit of Work, DI
container, Entity Framework...). Motivo: el tamaño y alcance actual
de la aplicación (4 pantallas, 5 tablas) no lo justifica, y añadir
esa infraestructura sin necesidad real dificultaría el mantenimiento
en vez de ayudarlo. La carpeta `work/` documenta esta y otras
decisiones como propuestas a valorar si la aplicación crece.

## Compatibilidad con el diseñador de Windows Forms

Los archivos `*.Designer.cs` (generados por el diseñador visual de
Visual Studio) **no se han modificado**: conservan los mismos
nombres de controles y el mismo cableado de eventos
(`button.Click += ...`) que antes de la refactorización. Esto
garantiza que el formulario se siga pudiendo abrir y editar en el
diseñador de VS sin conflictos, y que ningún event handler haya
quedado "huérfano".
