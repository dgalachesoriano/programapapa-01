# Modelo de datos (inferido)

> No existe un script de creación de base de datos en el repositorio.
> El esquema siguiente se ha reconstruido a partir de las consultas
> SQL usadas por la aplicación (ahora concentradas en `Datos/`). Se
> recomienda contrastarlo con la base de datos real y, si es
> correcto, añadir un script `.sql` versionado (ver `work/`).

## Tablas

### Facturas

Cabecera de cada factura/tarea.

| Columna | Tipo inferido | Notas |
|---|---|---|
| `Registro` | int, PK, identity | Generado por `SCOPE_IDENTITY()` al insertar. |
| `Documento` | nvarchar | Obligatorio. |
| `F_Ent_Calidad` | date/datetime | Fecha de entrada en calidad. |
| `F_Registro` | date/datetime | Fecha de registro de la tarea. |
| `Sociedad` | nvarchar | Obligatorio. |
| `Proyecto` | nvarchar | Obligatorio. |
| `Importe_Estimado` | decimal, nullable | |
| `IdUsuarioAsignado` | int, nullable, FK → `Usuarios_Facturas.IdUsuario` | |
| `IdEstado` | int, FK → `Estados_Facturas.IdEstado` | Se fija a "Registrado" en el alta; la aplicación no ofrece hoy ninguna acción para cambiarlo. |
| `IdSegmento` | int, FK → `Segmento_Negocio.IdSegmento` | Obligatorio. |
| `Num_Factura` | nvarchar/int | Se lee y se muestra en la búsqueda, pero **ninguna pantalla de la aplicación lo escribe**. |
| `F_Factura` | date/datetime | Igual que `Num_Factura`: solo lectura desde esta app. |
| `Importe_Total` | decimal | Igual que `Num_Factura`: solo lectura desde esta app. |

### Detalle_Facturas

Líneas de detalle de una factura.

| Columna | Tipo inferido | Notas |
|---|---|---|
| `RegistroDetalle` | int, PK, identity (asumido) | Solo se usa para el `ORDER BY` al leer el detalle. |
| `RegistroRN` | int, FK → `Facturas.Registro` | |
| `RC` | nvarchar | |
| `Unidades` | decimal, nullable | |
| `PInspeccion` | nvarchar | A pesar del nombre ("precio"), se trata como texto libre, no como importe. |
| `PCompra` | nvarchar | Igual que `PInspeccion`. |
| `PVenta` | nvarchar | Igual que `PInspeccion`. |
| `Albaran` | nvarchar | |

### Estados_Facturas

| Columna | Tipo inferido | Notas |
|---|---|---|
| `IdEstado` | int, PK | |
| `Estado` | nvarchar | Debe existir obligatoriamente una fila con el valor exacto `"Registrado"`; si no existe, el alta de facturas falla. |

### Usuarios_Facturas

| Columna | Tipo inferido | Notas |
|---|---|---|
| `IdUsuario` | int, PK | |
| `Usuario` | nvarchar | Login. |
| `Nombre` | nvarchar | Nombre mostrado en los combos. |
| `Activo` | bit | Solo los usuarios con `Activo = 1` son seleccionables. |

### Segmento_Negocio

| Columna | Tipo inferido | Notas |
|---|---|---|
| `IdSegmento` | int, PK | |
| `Segmento` | nvarchar | |

## Relaciones

```
Estados_Facturas 1 ──── * Facturas * ──── 1 Usuarios_Facturas (opcional)
                                │
                                * ──── 1 Segmento_Negocio
                                │
                                1
                                │
                                *
                        Detalle_Facturas
```

## Observaciones para revisar con el equipo de base de datos

- `Num_Factura`, `F_Factura` e `Importe_Total` sugieren que existe un
  proceso posterior (¿otra aplicación? ¿proceso manual/ERP?) que
  completa el ciclo de vida de la factura fuera de esta app. Merece
  la pena documentarlo explícitamente para evitar que quede como
  conocimiento tácito.
- No hay evidencia en el código de claves foráneas declaradas ni de
  restricciones `NOT NULL`/`CHECK`; las validaciones actuales viven
  solo en la aplicación (`ValidarDatos`), lo que las hace saltables
  desde cualquier otro cliente que escriba en la misma base de datos.
