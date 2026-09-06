# Manual de pantallas

## FrmPrincipal — Menú principal

Formulario de arranque de la aplicación (`Program.Main`). Contiene un
menú con tres opciones, cada una abre el formulario correspondiente
como ventana modal:

| Opción de menú | Acción |
|---|---|
| Registrar Tarea | Abre `FrmRegistrarTarea` en modo alta (tarea nueva). |
| Tratar Registro | Abre `FrmTratarTarea` (búsqueda de tareas existentes). |
| Pool de Tareas | Abre `FrmPoolTareas` (pantalla aún sin funcionalidad). |

## FrmTratarTarea — Búsqueda y tratamiento de tareas

Al abrirse, carga los combos de filtro y lanza automáticamente una
búsqueda sin filtros (muestra todas las tareas).

**Filtros:**
- `cboEstado` — estado de la factura, incluye "Todos" (id 0).
- `cboUsuario` — usuario asignado, incluye "Todos" (id 0).
- `txtDocumento` — coincidencia parcial por documento.
- `txtProyecto` — coincidencia parcial por proyecto.

**Botones:**
- `btnBuscar` — repite la búsqueda con los filtros actuales.
- `btnAbrir` — requiere una fila seleccionada en `dgvTareas`; abre esa
  factura en `FrmRegistrarTarea` (modo edición) y, al cerrarse,
  refresca la búsqueda.
- `btnCerrar` — cierra la pantalla.

**Rejilla de resultados (`dgvTareas`):** Registro, Documento, Fecha
Entrada Calidad, Fecha Registro, Sociedad, Proyecto, Segmento,
Estado, Usuario, Importe Estimado, Factura, Fecha Factura, Importe
Total. Las columnas de fecha usan formato `dd/MM/yyyy` y las de
importe, `N2`.

## FrmRegistrarTarea — Alta y edición de una tarea

Se usa en dos modos, según el constructor invocado:

- **Alta** (`new FrmRegistrarTarea()`): todos los campos en blanco o
  con su valor por defecto.
- **Edición** (`new FrmRegistrarTarea(registro)`): precarga la
  cabecera y el detalle de la factura indicada; el título de la
  ventana muestra el número de registro.

**Cabecera:**

| Campo | Control | Obligatorio |
|---|---|---|
| Documento | `txtDocumento` | Sí |
| Fecha Entrada Calidad | `dtpFEntCalidad` | No (valor por defecto del control) |
| Fecha Registro | `dtpFRegistro` | No (valor por defecto del control) |
| Sociedad | `txtSociedad` | Sí |
| Proyecto | `txtProyecto` | Sí |
| Importe Estimado | `txtImporteEstimado` | No — si no es numérico válido se guarda vacío |
| Usuario asignado | `cboUsuario` | No — "Sin asignar" = sin usuario |
| Segmento | `cboSegmento` | Sí |

El campo Importe Estimado se reformatea automáticamente (dos
decimales, formato español) al perder el foco.

**Detalle (`dgvDetalle`):** columnas RC, Unidades, P. Inspección,
P. Compra, P. Venta y Albarán. Permite añadir/quitar filas
libremente y admite **pegado especial con Ctrl+V**: al pegar texto
copiado (p. ej. de Excel), cada línea se interpreta como una fila y
cada tabulación (o bloque de 2+ espacios) como una columna, añadiendo
filas nuevas si hacen falta a partir de la celda seleccionada.

**Botones:**
- `btnGrabar` — valida los campos obligatorios, pide confirmación y
  graba (inserta si es alta, actualiza si es edición). El detalle se
  guarda siempre completo: en edición, sustituye por completo el
  detalle anterior.
- `btnCancelar` — cierra sin grabar.

**Navegación:** la tecla Enter mueve el foco al siguiente control del
formulario (salvo dentro de la rejilla de detalle, donde conserva su
comportamiento habitual de edición de celdas).

## FrmPoolTareas — Pool de tareas

Formulario que se abre desde el menú principal pero que, a día de
hoy, no contiene controles ni lógica de negocio. Ver
[proceso-negocio.md](proceso-negocio.md) y las propuestas de mejora
en la carpeta `work/` para su posible alcance futuro.
