# Modelo de datos v2 — flujo con tabla de control

Nuevo modelo de datos, a partir del planteamiento de entidades y flujo
descrito por el negocio: 3 pasos (Registrar Tarea → Pool de Tareas →
Facturado) controlados por una tabla `TBL_CONTROL` en lugar de un
único estado y usuario asignado en la propia tabla de tareas (como en
el [modelo v1](../README.md)).

Este modelo **coexiste** con el v1 en este repositorio; no sustituye
ni borra nada. La migración de la aplicación (formularios, repositorios)
a este esquema es un trabajo posterior, no incluido en estos scripts.

## Contenido

| Script | Qué hace |
|---|---|
| `01_CrearTablas.sql` | Crea las 10 tablas, claves primarias/foráneas, `CHECK` e índices. Re-ejecutable. |
| `02_DatosIniciales.sql` | Inserta los 4 estados del flujo (obligatorios) y datos de ejemplo del resto de catálogos. Re-ejecutable. |
| `03_Vistas.sql` | Crea `VW_TAREAS_ESTADO_ACTUAL`, con el estado/usuario/motivo **actuales** de cada tarea (ver más abajo). |
| `EjecutarTodo.sql` | Lanza los tres anteriores en orden (requiere modo SQLCMD en SSMS). |
| `ResetearDatos.sql` | **Destructivo.** Vacía `TBL_CONTROL`, `TBL_DETALLETAREA`, `TBL_TAREAS` y `TBL_FACTURADO`, y reinicia sus contadores `IDENTITY` a 1. No toca los catálogos. No se ejecuta como parte de `EjecutarTodo.sql`: es una utilidad aparte, para usar manualmente en desarrollo/pruebas cuando se quiera dejar el flujo como recién creado. |
| `AltaConsulta.sql` | Plantilla para dar de alta (o modificar, si ya existe una con el mismo nombre) una consulta de `TBL_CONSULTAS`, usada por la pantalla **Datos > Consultas SQL**. Se rellenan las variables del principio y se ejecuta. No se ejecuta como parte de `EjecutarTodo.sql`. Ver [Consultas SQL](#consultas-sql-tbl_consultas). |
| `DatosPrueba.sql` | Genera 10 tareas por cada usuario activo (30 en total con los 3 usuarios actuales), repartidas en distintos estados (En proceso / Pendiente / Facturado / Cancelado) y combinaciones de proyecto/segmento/divisa/tipo de factura/motivo, usando los catálogos ya cargados. Pensado para tener volumen con el que probar filtros, el coloreado del Pool de Tareas y el informe de facturación. Re-ejecutable: borra primero cualquier carga anterior propia (documentos `DOC-PRUEBA-%`). No se ejecuta como parte de `EjecutarTodo.sql`. |

Usa la misma base de datos `GestionFacturas` que el modelo v1 (ver
`../00_CrearBaseDatos.sql`); solo añade tablas nuevas, no toca las
existentes.

## Tablas

Catálogos (patrón común: `ID` identity, una descripción, y
`XTI_ACTIVO CHAR(1)` con `CHECK IN ('S','N')`, por defecto `'S'`):
`TBL_USUARIOS`, `TBL_ORGANIZACIONES_VENTAS`, `TBL_PROYECTOS`,
`TBL_SEGMENTOS`, `TBL_DIVISAS`, `TBL_TIPFACTURAS`, `TBL_MOTIVO`,
`TBL_ESTADOS_TAREA`.

Consultas predefinidas de la pantalla Datos > Consultas SQL:
`TBL_CONSULTAS` (ver [más abajo](#consultas-sql-tbl_consultas)).

Entidades del proceso: `TBL_TAREAS`, `TBL_DETALLETAREA`,
`TBL_FACTURADO`, `TBL_CONTROL`.

```
TBL_ORGANIZACIONES_VENTAS ─┐
TBL_PROYECTOS ─────────────┤
TBL_SEGMENTOS ─────────────┼──► TBL_TAREAS ──1───*── TBL_DETALLETAREA
TBL_TIPFACTURAS ───────────┤         ▲
TBL_DIVISAS ────────────────┘         │ COD_SEQ_TAR
                                      │
TBL_USUARIOS ───────────────────► TBL_CONTROL ◄──── COD_SEQ_FAC ──── TBL_FACTURADO ──► TBL_DIVISAS
TBL_ESTADOS_TAREA┤                      │
TBL_MOTIVO ──────┘                (1 fila por evento:
                                    alta / asignación /
                                    bloqueo / facturación)
```

## Diferencias respecto a lo pedido, y por qué

El detalle completo está también en la cabecera de `01_CrearTablas.sql`;
resumen:

1. **`TBL_SEGMENTOS` añadida.** `COD_SEQ_SEG` se describe como lista
   sobre "tabla TBL_SEGMENTOS", pero esa tabla no estaba en el listado
   de entidades — se ha creado con la misma forma que `TBL_PROYECTOS`.

2. **`TBL_USUARIOS` incorpora `XTI_ACTIVO`.** No estaba en la
   definición original, pero se usa como lista desplegable (asignar
   tarea) igual que el resto de catálogos, y la instrucción pide que
   todo campo de lista respete `XTI_ACTIVO` para decidir si se
   muestra.

3. **`TBL_CONTROL.ESTADO` pasa de texto libre a `COD_SEQ_EST`**,
   clave foránea a un nuevo catálogo `TBL_ESTADOS_TAREA`. Era el único
   campo categórico del modelo que no seguía el patrón
   `COD_SEQ_xxx → tabla catálogo` que sí se usa para proyecto,
   segmento, usuario, moneda, tipo de factura y motivo. Se unifica por
   consistencia y para poder gestionar los estados (añadir, desactivar)
   sin tocar código ni arriesgar valores inconsistentes
   (`"Bloqueado"` vs `"bloqueado"` vs `"BLOQUEADO"`).

4. **`TBL_CONTROL` es histórico, no una fila mutable por tarea.**
   Cada evento del flujo (alta → `REGISTRADO`, asignación →
   `EN_PROCESO`, bloqueo → `BLOQUEADO`, facturación → `FACTURADO`)
   inserta una fila **nueva**, nunca hace `UPDATE` sobre una anterior.
   Ventajas:
   - Auditoría completa "quién hizo qué y cuándo" sin añadir columnas
     de auditoría a `TBL_TAREAS` (esto era, de hecho, una mejora
     pendiente ya identificada antes de este cambio de modelo).
   - Si una tarea se bloquea y se desbloquea varias veces, queda
     rastro de cada motivo, no solo del último.
   - El "estado actual" de una tarea es, simplemente, su fila más
     reciente — resuelto por la vista `VW_TAREAS_ESTADO_ACTUAL`.

   Si se prefiere una única fila mutable por tarea (más simple de
   consultar, pero sin histórico), el cambio es mínimo: añadir
   `CONSTRAINT UQ_TBL_CONTROL_COD_SEQ_TAR UNIQUE (COD_SEQ_TAR)` y que
   la aplicación haga `UPDATE` en vez de `INSERT` a partir del segundo
   evento. Avisa si prefieres este enfoque y se ajusta el script.

## `VW_TAREAS_ESTADO_ACTUAL`

Como `TBL_CONTROL` es histórico, casi toda pantalla necesita "la fila
más reciente de `TBL_CONTROL` por tarea". La vista lo resuelve una
vez (con `CROSS APPLY` + `TOP 1`, apoyada en el índice
`IX_TBL_CONTROL_COD_SEQ_TAR_FEC_ALTA`) y expone ya los `JOIN` a
proyecto/segmento/usuario/estado/motivo/facturado resueltos a texto,
lista para enlazar directamente a un `DataGridView`:

- **Pool de Tareas** (rejilla superior): filtra por
  `DES_ESTADO`/fechas/`NOMBRE_USUARIO` sobre esta vista.
- **Pantalla de Facturación**: selecciona tareas con
  `DES_ESTADO = 'EN_PROCESO'` (pendientes de facturar) desde esta
  misma vista.

## Decisiones confirmadas

Las tres preguntas abiertas de la primera versión de este modelo ya
están resueltas:

- **`TBL_FACTURADO.COD_FACT` sin `UNIQUE`.** Un número de factura
  puede repetirse (p. ej. entre tipos/series distintos); no lleva
  restricción de unicidad.
- **Sin equivalente a la antigua "RC"** (`Detalle_Facturas.RC` del
  modelo v1) en `TBL_DETALLETAREA`. Confirmado como intencionado: se
  queda solo con los cinco campos pedidos (`COD_PED_VENTA`,
  `COD_PED_COMPRA`, `COD_PED_INSPEC`, `COD_ENT_ENTR`,
  `NBR_UNIDADES`).
- **Bloqueo de una tarea: solo motivo de lista, sin observaciones en
  texto libre.** `TBL_CONTROL` no tiene columna de texto libre; al
  bloquear una tarea hay que seleccionar obligatoriamente un valor de
  `TBL_MOTIVO` (`COD_SEQ_MOTIVO`), mostrado como desplegable
  (respetando `XTI_ACTIVO`, igual que el resto de listas del modelo).
  Es responsabilidad de la pantalla exigir esa selección antes de
  permitir el "OK" del bloqueo — la base de datos deja
  `COD_SEQ_MOTIVO` como `NULL`-able porque solo aplica a los eventos
  de bloqueo, no a los demás (alta, asignación, facturación).

**Resuelta:** `TBL_TAREAS.DES_ORG_VENTAS` (equivalente a la antigua
`Sociedad`) ya no es texto libre: se ha normalizado en
`TBL_ORGANIZACIONES_VENTAS` + `TBL_TAREAS.COD_SEQ_ORG_VTA`, con la
misma forma que `TBL_PROYECTOS`/`TBL_SEGMENTOS` (catálogo con
ID/descripción/activo, mostrado como desplegable en pantalla).

**`TBL_TAREAS` incorpora tipo de documento, moneda estimada y
notas**, las tres informadas al Registrar Tarea:
- `COD_SEQ_TPF` (tipo de documento): antes era
  `TBL_FACTURADO.COD_SEQ_TPF` ("tipo de factura"), informado solo al
  facturar. Es en realidad un dato de la tarea, no de su facturación,
  así que se mueve a `TBL_TAREAS` y `TBL_FACTURADO` deja de tenerlo.
- `COD_SEQ_MON` (moneda estimada): FK a `TBL_DIVISAS`, independiente
  de la divisa real de `TBL_FACTURADO.COD_SEQ_MON` (estimada en el
  alta vs. real al facturar).
- `DES_NOTAS`: texto libre opcional (`NVARCHAR(255) NULL`).

## Consultas SQL (`TBL_CONSULTAS`)

Catálogo de consultas que la pantalla **Datos > Consultas SQL** permite
buscar por nombre, previsualizar (10 primeros registros) y exportar
completas a Excel (como Tabla; requiere Microsoft Excel instalado).

| Columna | Tipo | Uso |
|---|---|---|
| `ID` | `INT IDENTITY` | Clave primaria. |
| `NOM_CONSULTA` | `NVARCHAR(100)`, único | Nombre mostrado en el desplegable. |
| `DES_CONSULTA` | `NVARCHAR(500) NULL` | Descripción mostrada al seleccionarla. |
| `TXT_CONSULTA` | `VARCHAR(8000)` | Texto SQL de la consulta. |
| `XTI_ACTIVO` | `CHAR(1)` `'S'/'N'` | Solo las activas aparecen en pantalla. |

No hay pantalla de mantenimiento: se dan de alta o se modifican con
`AltaConsulta.sql`.

**Parámetros.** Una consulta puede incluir parámetros con la sintaxis
`:Nombre`. Al seleccionarla, la pantalla muestra una etiqueta y una caja
de texto por cada parámetro distinto (si el mismo nombre aparece varias
veces se pide una sola vez, sin distinguir mayúsculas) y la consulta se
lanza con el botón **Ejecutar**; sin parámetros, se previsualiza nada
más seleccionarla. Detalles:

- Los valores viajan como parámetros de SQL Server (`NVARCHAR`), nunca
  concatenados al texto, así que no hay riesgo de inyección por lo que
  se teclee.
- Caja vacía = `NULL`, útil para filtros opcionales:
  `AND (:Usuario IS NULL OR NOMBRE_USUARIO = :Usuario)`.
- Fechas: conviértelas en la consulta indicando el formato, p. ej.
  `CONVERT(DATE, :FechaDesde, 103)` para `dd/mm/aaaa`.
- No se consideran parámetros los `:` dentro de literales (`'10:30'`),
  identificadores entre corchetes, comentarios ni el `::` de métodos
  como `geography::Point`.
