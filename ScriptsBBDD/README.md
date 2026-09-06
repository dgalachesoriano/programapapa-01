# Scripts de base de datos

Scripts T-SQL (SQL Server) para montar el modelo de datos de la
aplicación GestionFacturas. El esquema se ha reconstruido a partir
del código de acceso a datos existente (`Datos/`, `Modelos/`), ya que
no había ningún script de base de datos versionado — ver
[`docs/modelo-datos.md`](../docs/modelo-datos.md) para el detalle y
las dudas pendientes de confirmar con el equipo de base de datos.

## Contenido

| Script | Qué hace |
|---|---|
| `00_CrearBaseDatos.sql` | Crea la base de datos `GestionFacturas` si no existe. |
| `01_CrearTablas.sql` | Crea las tablas, claves primarias/foráneas e índices. Re-ejecutable. |
| `02_DatosIniciales.sql` | Inserta el estado `Registrado` (obligatorio para poder dar de alta facturas) y datos de ejemplo (segmento, usuario). Re-ejecutable. |
| `EjecutarTodo.sql` | Lanza los tres anteriores en orden (requiere modo SQLCMD en SSMS). |

## Cómo ejecutarlos

1. Abre los scripts con SQL Server Management Studio (o `sqlcmd`)
   contra el servidor deseado.
2. Ejecuta en orden `00_CrearBaseDatos.sql`, `01_CrearTablas.sql` y
   `02_DatosIniciales.sql` (o `EjecutarTodo.sql` en modo SQLCMD).
3. Actualiza `App.config` para que la cadena de conexión
   `GestionFacturasConnection` apunte a `Database=GestionFacturas`
   (de fábrica trae `Database=master` como placeholder).

## Notas

- Los estados adicionales (`Pendiente`, `Facturado`), el segmento
  `General` y el usuario `admin` son solo datos de ejemplo: sustitúyelos
  por los reales del negocio antes de usar la aplicación en producción.
- `Num_Factura`, `F_Factura` e `Importe_Total` (tabla `Facturas`) se
  dejan como columnas opcionales porque la aplicación solo las lee;
  se asume que las completa un proceso externo (ver observaciones en
  `docs/modelo-datos.md`).
- No hay evidencia en el código de tipos de columna exactos (longitudes
  de `nvarchar`, `date` vs `datetime`, etc.), por lo que son una
  estimación razonable. Contrástalos con la base de datos real si ya
  existe una en producción.
