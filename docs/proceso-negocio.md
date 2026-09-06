# Proceso de negocio

## Objetivo de la aplicación

GestionFacturas apoya el proceso de **registro y seguimiento de
facturas** dentro de un departamento (p. ej. Calidad/Compras). Cada
factura se gestiona internamente como una **tarea**: tiene una
cabecera con los datos administrativos y un detalle de líneas
(recepciones/albaranes) asociadas a esa factura.

## Entidades del proceso

- **Factura / Tarea**: unidad central del proceso. Identificada por
  su número de `Registro`. Tiene un `Documento`, una `Sociedad`, un
  `Proyecto`, fechas de entrada en calidad y de registro, un importe
  estimado, un `Segmento` de negocio, un `Estado` y, opcionalmente,
  un `Usuario` asignado.
- **Detalle de factura**: una o varias líneas por factura, cada una
  con Referencia de Control (RC), unidades, precio de inspección,
  precio de compra, precio de venta y número de albarán.
- **Estado**: fase del ciclo de vida de la factura (tabla
  `Estados_Facturas`). Toda factura nueva se crea automáticamente en
  el estado **"Registrado"**.
- **Usuario**: persona a la que se puede asignar el tratamiento de
  una factura (tabla `Usuarios_Facturas`, solo los marcados como
  activos aparecen seleccionables).
- **Segmento de negocio**: clasificación de la factura (tabla
  `Segmento_Negocio`); es obligatorio indicarlo al registrar la
  tarea.

## Flujo del proceso

```
 1) Registrar Tarea
    │  El usuario introduce documento, sociedad, proyecto,
    │  fechas, importe estimado, segmento y (opcionalmente)
    │  el usuario asignado, más las líneas de detalle.
    │  Al grabar: alta en estado "Registrado".
    ▼
 2) Tratar Registro (búsqueda)
    │  Cualquier usuario puede buscar facturas ya registradas
    │  filtrando por estado, usuario asignado, documento o
    │  proyecto.
    ▼
 3) Abrir tarea (edición)
    │  Se abre la factura seleccionada con sus datos y su
    │  detalle, y se puede modificar cualquier campo de
    │  cabecera y volver a grabar el detalle completo.
    ▼
   (Pool de Tareas: pantalla reservada para una futura vista
    de reparto/priorización de tareas pendientes; hoy no
    implementa lógica).
```

### 1. Registro de una tarea nueva

Pantalla: **FrmRegistrarTarea** (modo alta), accesible desde
*Registrar Tarea* en el menú principal.

- Campos obligatorios: Documento, Sociedad, Proyecto, Segmento.
- Campos opcionales: fecha de entrada en calidad, fecha de registro
  (llevan un valor por defecto del propio control), importe
  estimado, usuario asignado (por defecto "Sin asignar").
- El detalle admite tantas líneas como haga falta; se puede rellenar
  a mano o pegando datos copiados de una hoja de cálculo (Ctrl+V)
  directamente sobre la rejilla.
- Al grabar, tras confirmación del usuario, la factura se crea con
  estado **"Registrado"** y se guardan cabecera y detalle en una
  única operación (transacción): si algo falla, no se guarda nada.

### 2. Búsqueda y tratamiento

Pantalla: **FrmTratarTarea**, accesible desde *Tratar Registro*.

- Filtros disponibles: Estado (o "Todos"), Usuario asignado (o
  "Todos"), Documento (coincidencia parcial) y Proyecto
  (coincidencia parcial).
- El resultado se muestra en una rejilla con toda la información
  relevante de cada factura, incluidos campos que no se editan desde
  esta aplicación (`Num_Factura`, `F_Factura`, `Importe_Total`): ver
  nota en [modelo-datos.md](modelo-datos.md) sobre estos campos.
- Seleccionando una fila y pulsando *Abrir* se accede a la edición
  completa de esa factura (reutiliza FrmRegistrarTarea en modo
  edición).

### 3. Edición de una tarea existente

Misma pantalla que el alta (**FrmRegistrarTarea**), pero
precargada con los datos existentes (cabecera + detalle). Al grabar:

- Se actualiza la cabecera.
- Se sustituye por completo el detalle anterior por el que haya en
  pantalla en ese momento (borrado + reinserción), por lo que
  eliminar una línea en pantalla y grabar equivale a eliminarla en
  base de datos.
- El estado de la factura **no se puede cambiar** desde esta
  pantalla; no existe en la aplicación actual una acción para mover
  una factura a un estado distinto de "Registrado" (ver propuestas
  de mejora en la carpeta `work/`).

### 4. Pool de Tareas

Pantalla: **FrmPoolTareas**, accesible desde el menú, pero sin
lógica implementada todavía (formulario vacío). Se documenta aquí
como punto de extensión conocido del proceso.

## Reglas de negocio identificadas en el código

- Una factura nueva siempre nace en el estado cuyo nombre exacto en
  `Estados_Facturas` es `"Registrado"`; si esa fila no existe, el
  alta falla explícitamente.
- Una factura sin usuario asignado se representa como `NULL` en base
  de datos (opción "Sin asignar"/"Todos" = id 0 en la interfaz).
- Una línea de detalle completamente vacía (todas las columnas en
  blanco) no se guarda.
- El importe estimado y las unidades se interpretan con formato
  numérico español (`es-ES`, coma decimal); si el texto no es un
  número válido, el campo se guarda como `NULL` en vez de bloquear
  el guardado.
