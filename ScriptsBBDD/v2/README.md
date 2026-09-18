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
| `DatosPrueba.sql` | Genera 10 tareas por cada usuario activo (30 en total con los 3 usuarios actuales), repartidas en distintos estados (En proceso / Pendiente / Facturado / Cancelado) y combinaciones de proyecto/segmento/divisa/tipo de factura/motivo, usando los catálogos ya cargados. Pensado para tener volumen con el que probar filtros, el coloreado del Pool de Tareas y el informe de facturación. Re-ejecutable: borra primero cualquier carga anterior propia (documentos `DOC-PRUEBA-%`). No se ejecuta como parte de `EjecutarTodo.sql`. |

Usa la misma base de datos `GestionFacturas` que el modelo v1 (ver
`../00_CrearBaseDatos.sql`); solo añade tablas nuevas, no toca las
existentes.

## Tablas

Catálogos (patrón común: `ID` identity, una descripción, y
`XTI_ACTIVO CHAR(1)` con `CHECK IN ('S','N')`, por defecto `'S'`):
`TBL_USUARIOS`, `TBL_PROYECTOS`, `TBL_SEGMENTOS`, `TBL_DIVISAS`,
`TBL_TIPFACTURAS`, `TBL_MOTIVO`, `TBL_ESTADOS_TAREA`.

Entidades del proceso: `TBL_TAREAS`, `TBL_DETALLETAREA`,
`TBL_FACTURADO`, `TBL_CONTROL`.

```
TBL_PROYECTOS ──┐
TBL_SEGMENTOS ──┼──► TBL_TAREAS ──1───*── TBL_DETALLETAREA
                │         ▲
                │         │ COD_SEQ_TAR
                │         │
TBL_USUARIOS ───┼──► TBL_CONTROL ◄──── COD_SEQ_FAC ──── TBL_FACTURADO ──► TBL_DIVISAS
TBL_ESTADOS_TAREA┤         │                                          └─► TBL_TIPFACTURAS
TBL_MOTIVO ──────┘   (1 fila por evento:
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

**Sigue abierta** (no bloquea nada, es una mejora opcional):
`TBL_TAREAS.DES_ORG_VENTAS` (equivalente a la antigua `Sociedad`) se
mantiene como texto libre, tal y como se pidió. Si el conjunto de
organizaciones de venta es cerrado y estable, se podría normalizar
igual que `TBL_PROYECTOS`/`TBL_SEGMENTOS` más adelante.
