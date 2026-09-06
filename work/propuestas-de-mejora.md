# Propuestas de mejora — GestionFacturas

Documento de trabajo con recomendaciones sobre la solución actual,
más allá de la reorganización en capas ya aplicada (ver
[../docs/arquitectura.md](../docs/arquitectura.md)). Se agrupan por
área e incluyen impacto, prioridad orientativa y **estado** actual
para poder priorizarlas con el equipo. El plan de acción para
implementarlas está al final, en la [sección 6](#6-plan-de-acción).

> **Actualizado: 2026-09-06.** Desde la primera versión de este
> documento se han implementado varias propuestas (marcadas ✅ más
> abajo) y han aparecido hallazgos nuevos durante ese trabajo
> (compilación, artefactos de otra máquina, ficheros bloqueados por
> Windows). Ver el resumen de cambios al final de cada sección
> afectada.

## 1. Proceso de negocio

### 1.1 No hay forma de cambiar el estado de una factura
**Prioridad: Alta. Estado: ✅ Implementado (2026-09-06).** Toda
factura nacía en `"Registrado"` y ninguna pantalla permitía moverla a
otro estado. Se ha añadido a `FrmPoolTareas` un cambio de estado
masivo ("Nuevo estado" + botón "Cambiar estado"), reutilizando el
mismo patrón de la asignación de usuario (selección múltiple + acción
en bloque + transacción en `FacturaRepositorio.CambiarEstado`, que
comparte helper con `AsignarUsuario`). **Decisión de negocio
adoptada:** se permite cualquier transición (de cualquier estado a
cualquier otro), sin modelar un flujo restringido — más simple de
entregar; se puede acotar más adelante si aparecen reglas concretas.

### 1.2 Campos `Num_Factura`, `F_Factura`, `Importe_Total` sin origen
**Prioridad: Alta. Estado: pendiente.** La búsqueda los muestra pero
ninguna pantalla los escribe (ver
[../docs/modelo-datos.md](../docs/modelo-datos.md)). O bien los
rellena un proceso externo no documentado, o bien falta una
pantalla/paso del proceso para facturar. Conviene aclarar esto con el
negocio: si el proceso debe completarse en esta aplicación, falta una
funcionalidad completa; si no, documentar la integración. Esto sigue
sin resolverse — es una decisión de negocio, no de código.

### 1.3 Sin control de concurrencia en la edición
**Prioridad: Media. Estado: pendiente.** Si dos personas abren la
misma factura a la vez, la última en grabar sobreescribe
silenciosamente los cambios de la otra (incluida la sustitución
completa del detalle). Propuesta: añadir una columna de control (p.
ej. `RowVersion`/`timestamp`) y comprobarla al actualizar, avisando si
el registro cambió entre la carga y el guardado.

### 1.4 Sin trazabilidad de auditoría
**Prioridad: Media. Estado: pendiente.** No hay columnas de quién y
cuándo creó o modificó una factura ni su detalle. Para un proceso de
gestión documental/financiera, suele ser un requisito razonable
(usuario y fecha de alta/última modificación).

### 1.5 Pool de Tareas
**Prioridad: Alta. Estado: ✅ Implementado (2026-09-06).**
`FrmPoolTareas` ya no es un formulario vacío. Se ha implementado:
- Filtros por Estado, Proyecto (combo con los valores ya existentes en
  `Facturas.Proyecto`), Usuario asignado y rango de fechas
  Desde/Hasta sobre `F_Registro`.
- Vista maestro-detalle: al seleccionar una tarea se cargan sus líneas
  de `Detalle_Facturas` en la rejilla inferior.
- **Asignación masiva**: selección múltiple de tareas + un único
  usuario destino (o "Sin asignar" para desasignar en bloque),
  aplicada en una sola transacción
  (`FacturaRepositorio.AsignarUsuario`).
- "Abrir tarea" para editar la tarea con el foco, igual que en
  "Tratar Tarea".

Ampliado el 2026-09-06 con **cambio de estado en bloque** (ver 1.1).

Pendiente dentro de esta misma pantalla (no bloqueante):
- Paginación/exportación si el volumen de tareas crece (ver 3.2/3.3).
- No hay forma de deshacer una asignación masiva salvo repitiéndola en
  sentido contrario; aceptable por ahora, pero a vigilar si se usa con
  lotes grandes.

## 2. Robustez y calidad del código

### 2.1 Manejo de errores solo con `MessageBox`
**Prioridad: Alta. Estado: 🟡 Parcialmente aplicado (2026-09-06).**
Se ha resuelto la parte de **conexión a base de datos**: antes,
`ConexionBD.CrearConexion()` podía lanzar una `NullReferenceException`
"en crudo" si `App.config` no tenía la cadena de conexión esperada (de
hecho, así se manifestó un bug real: unos artefactos de compilación
obsoletos de otra máquina hacían que el `.exe.config` desplegado no
llevara `<connectionStrings>`). Ahora:
- `ConexionBD` valida la configuración y expone `AbrirConexion()`,
  que envuelve cualquier fallo (configuración ausente, servidor
  caído, credenciales inválidas) en una nueva
  [`ConexionBDException`](../Datos/ConexionBDException.cs) con un
  mensaje claro para el usuario, conservando la excepción original
  como `InnerException`.
- Todos los repositorios (`FacturaRepositorio`,
  `EstadoFacturaRepositorio`, `UsuarioRepositorio`,
  `SegmentoRepositorio`) usan ya `AbrirConexion()`.

Sigue pendiente lo general: las excepciones se siguen mostrando con
`ex.Message` y **no se registra nada en ningún sitio** (no hay
logging real). Propuesta original sigue en pie: introducir un logger
(NLog, Serilog o `System.Diagnostics.Trace` como mínimo).

### 2.2 Excepciones no tipadas
**Prioridad: Baja. Estado: 🟡 Patrón ya iniciado.** Varios puntos
siguen lanzando `Exception`/`InvalidOperationException` genéricas para
errores de negocio (p. ej. "no existe el estado Registrado"). La
`ConexionBDException` introducida en 2.1 es el primer ejemplo de
excepción propia en la base de código; se propone extender el mismo
patrón a otros errores de negocio esperados (p. ej.
`FacturaNoEncontradaException`), para que la interfaz pueda reaccionar
distinto a un error de negocio esperado que a un fallo de
infraestructura.

### 2.3 Ausencia total de pruebas automatizadas
**Prioridad: Alta. Estado: pendiente.** No existe ningún proyecto de
tests. La extracción de la lógica a `Datos/*Repositorio.cs` y a
métodos como `DetalleFactura.EstaVacia()` ya deja partes más fáciles
de testear, pero para poder probar los repositorios sin una base de
datos real haría falta:
- Definir interfaces (`IFacturaRepositorio`, etc.) e inyectarlas en
  los formularios, o
- Adoptar una librería como **Dapper** con soporte de conexión
  sustituible, o pruebas de integración contra una base de datos de
  pruebas (LocalDB) dedicada a CI.

*Nota (2026-09-06):* se intentó, a petición del usuario, verificar el
flujo de "Registrar Tarea" automatizando la interfaz gráfica (UI
Automation de Windows). Resultó frágil y complicado de depurar
(problemas de codificación de acentos al leer scripts desde archivo,
aislamiento de sesión de escritorio al lanzar procesos desde
sub-procesos anidados) y se abandonó el enfoque. **Conclusión
reutilizable para este punto:** cualquier prueba automatizada de esta
aplicación debe hacerse contra la capa de lógica/repositorios
directamente, nunca conduciendo la GUI.

### 2.4 Dependencia NuGet huérfana
**Prioridad: Baja. Estado: pendiente.** La carpeta `packages/` incluye
`Microsoft.Data.SqlClient.Extensions.Abstractions` pero el proyecto no
la referencia en ningún sitio (usa `System.Data.SqlClient`, que
Microsoft mantiene ya solo en modo de mantenimiento). Propuesta:
completar la migración a **`Microsoft.Data.SqlClient`** (sucesor
recomendado, con mejoras de seguridad y rendimiento) y eliminar el
paquete huérfano si no se acaba usando.

### 2.5 Validación duplicada entre alta y edición
**Prioridad: Baja. Estado: pendiente.** `ValidarDatos()` ya está
centralizado en un solo método, lo cual está bien; como siguiente
paso, se podría extraer a una clase `ValidadorFactura` independiente
del formulario, para poder testearla sin instanciar UI.

## 3. Experiencia de usuario

### 3.1 Importe estimado inválido se guarda como vacío sin avisar
**Prioridad: Media. Estado: ✅ Implementado (2026-09-06).** Si el
usuario escribía un importe (o unas unidades de detalle) con formato
incorrecto, se grababa como `NULL` sin ningún aviso. Ahora
`FrmRegistrarTarea.ValidarDatos()` comprueba el Importe Estimado y,
con el nuevo método `ValidarUnidadesDetalle()`, cada fila no vacía del
detalle; si algún valor no es un número válido, se avisa, se pone el
foco en el campo/celda correspondiente y se bloquea el guardado.

### 3.2 Búsqueda sin paginación
**Prioridad: Media (crece con el volumen de datos). Estado:
pendiente; ahora afecta a dos pantallas.** `FrmTratarTarea` y, desde
esta iteración, también `FrmPoolTareas` traen siempre el resultado
completo a un `DataTable`/rejilla en memoria. Con un histórico grande
de facturas, esto degradará el rendimiento de ambas pantallas.
Propuesta: paginación en servidor (`OFFSET/FETCH`) o al menos un
límite razonable de filas con aviso de "hay más resultados, afine el
filtro".

### 3.3 Sin exportación de resultados
**Prioridad: Baja. Estado: pendiente; ahora afecta a dos pantallas.**
Ni "Tratar Tarea" ni "Pool de Tareas" permiten exportar el listado
filtrado (p. ej. a Excel/CSV), algo habitual en pantallas de este tipo
para reporting rápido.

### 3.4 Interfaz no adaptable a distintos tamaños de pantalla
**Prioridad: Media. Estado: ✅ Implementado (2026-09-06).** Las
pantallas usaban posicionamiento absoluto (`Location`/`Size` fijos,
sin `Anchor`/`Dock`), por lo que no se aprovechaba el espacio al
maximizar ni se adaptaban a resoluciones menores; `FrmPoolTareas` era
el caso más grave: sus secciones sumaban más alto del que cabía en su
propio `MinimumSize`. Se ha rediseñado con paneles:
- `FrmRegistrarTarea` y `FrmTratarTarea`: cabecera/filtros en
  `TableLayoutPanel` anclado arriba, contenido a `Dock=Fill`, barra de
  botones anclada abajo.
- `FrmPoolTareas`: además, un `SplitContainer` con separador
  arrastrable entre la rejilla de tareas y la de detalle.

Pendiente (no bloqueante): pulido visual fino (alineaciones, tipografía,
color) queda por revisar directamente en la aplicación por el equipo,
ya que no se puede validar visualmente de forma fiable sin ejecutarla.

## 4. Infraestructura y despliegue

### 4.1 Cadena de conexión por entorno
**Prioridad: Baja. Estado: aplicado parcialmente.** Se ha movido a
`App.config` (ver [../docs/instalacion.md](../docs/instalacion.md)) y
se ha corregido para apuntar a la base de datos real (ver 4.4). Como
siguiente paso, considerar transformaciones de configuración por
entorno (`App.Debug.config`/`App.Release.config` o un `.config`
distinto por entorno) para evitar tener que editar el archivo a mano
en cada despliegue.

### 4.2 Sin control de versiones
**Prioridad: Alta — sigue siendo el bloqueante más importante.
Estado: pendiente.** La carpeta del proyecto sigue sin ser un
repositorio Git. Esto ya ha costado tiempo real esta iteración (ver
4.5) y sigue siendo el mayor riesgo del proyecto: no hay forma de
revertir un cambio, comparar versiones ni recuperar un archivo
sobrescrito por error. Se recomienda inicializar control de versiones
cuanto antes (`git init` + `.gitignore` para `bin/`, `obj/`, `.vs/`)
**antes de seguir acumulando cambios**.

### 4.3 Framework de destino
**Prioridad: A largo plazo. Estado: pendiente.** El proyecto apunta a
.NET Framework 4.7.2 (fuera de soporte de nuevas funcionalidades,
aunque sigue recibiendo soporte de Microsoft). Si el ciclo de vida del
producto es largo, evaluar una migración futura a **.NET 8
(WinForms)**, que permite `Nullable Reference Types`, `async/await` de
forma más idiomática en la UI, y mejor rendimiento — es un cambio de
calado, no urgente, pero conviene tenerlo en el radar.

### 4.4 No existía script de creación de la base de datos
**Prioridad: Alta. Estado: ✅ Implementado (2026-09-05).** Se ha
añadido [../ScriptsBBDD/](../ScriptsBBDD/) con el modelo de datos
completo reconstruido a partir del código (`00_CrearBaseDatos.sql`,
`01_CrearTablas.sql`, `02_DatosIniciales.sql`, más un `README.md` con
las instrucciones y las estimaciones de tipo de columna que conviene
contrastar con negocio). `App.config` se ha corregido para apuntar a
la base de datos `GestionFacturas` creada por estos scripts (antes
apuntaba a `Database=master` como placeholder).

*Pendiente:* actualizar la advertencia inicial de
[../docs/modelo-datos.md](../docs/modelo-datos.md) ("No existe un
script de creación de base de datos..."), que ha quedado desactualizada.

### 4.5 Artefactos de compilación de otra máquina mezclados en el proyecto
**Prioridad: Alta (nuevo hallazgo). Estado: mitigado puntualmente,
riesgo estructural pendiente.** Se ha detectado que `bin/` y `obj/`
contenían artefactos generados en **otros equipos** (rutas como
`C:\Mi_Temp\DesarrolloCSharp\...` y
`C:\Users\JGALACHE\OneDrive - grupocunado\...` aparecían en el
tracking interno de MSBuild). Esto provocó un bug real y difícil de
diagnosticar: `GestionFacturas.exe.config` no se regeneraba con los
cambios de `App.config`, porque MSBuild no detectaba que debía
reemplazar un archivo intermedio que nunca había visto en esa ruta de
usuario. Se resolvió borrando `bin/`/`obj/` y recompilando desde cero,
pero **puede volver a pasar** mientras el proyecto se comparta como
carpeta/zip en lugar de por control de versiones. Ligado directamente
a 4.2: en cuanto haya un repositorio Git, un `.gitignore` que excluya
`bin/`, `obj/` y `.vs/` elimina este riesgo de raíz.

### 4.6 Ficheros bloqueados por Windows ("descargado de Internet")
**Prioridad: Baja (nuevo hallazgo). Estado: mitigado puntualmente.**
Todo el árbol del proyecto tenía la marca de zona de Windows (Zone.Identifier,
"este archivo procede de otro equipo"), probablemente por haberse
distribuido como zip/descarga. Esto bloqueaba una compilación limpia
(`FrmPrincipal.resx` daba error MSB3821 "Please follow MSBuild secure
usage best practices"). Se ha desbloqueado todo el árbol
(`Unblock-File` recursivo). Propuesta: si el proyecto se sigue
distribuyendo así entre máquinas mientras no haya Git, documentar este
paso en
[../docs/instalacion.md](../docs/instalacion.md).

## 5. Resumen priorizado

| # | Propuesta | Prioridad | Estado |
|---|---|---|---|
| 4.2 | Inicializar control de versiones | Alta | ✅ Implementado |
| 1.2 | Aclarar origen de Num_Factura/F_Factura/Importe_Total | Alta | Pendiente (decisión de negocio) |
| 2.3 | Suite de pruebas automatizadas | Alta | Pendiente |
| 4.5 | Evitar artefactos bin/obj de otra máquina | Alta | Mitigado puntualmente (resuelto de raíz al cerrar 4.2) |
| 1.1 | Flujo explícito de cambio de estado | Alta | ✅ Implementado |
| 1.5 | Pool de Tareas | Alta | ✅ Implementado |
| 4.4 | Script de creación de base de datos | Alta | ✅ Implementado |
| 2.1 | Logging real de errores | Alta | ✅ Implementado |
| 1.3 | Control de concurrencia en edición | Media | Pendiente |
| 1.4 | Auditoría (usuario/fecha de alta y modificación) | Media | Pendiente |
| 3.1 | Validar importe/unidades en vez de descartar en silencio | Media | ✅ Implementado |
| 3.2 | Paginación de resultados (Tratar Tarea + Pool de Tareas) | Media | Pendiente |
| 3.4 | Interfaz adaptable a distintos tamaños de pantalla | Media | ✅ Implementado |
| 2.2 | Excepciones de negocio tipadas | Baja | Patrón iniciado |
| 2.4 | Migrar a Microsoft.Data.SqlClient y limpiar dependencias | Baja | Pendiente |
| 2.5 | Extraer validación a clase independiente | Baja | Pendiente |
| 3.3 | Exportación de resultados (ambas pantallas) | Baja | Pendiente |
| 4.1 | Configuración por entorno | Baja | Parcial |
| 4.6 | Documentar desbloqueo de ficheros al distribuir sin Git | Baja | Mitigado puntualmente |
| 4.3 | Migración a .NET 8 WinForms | Largo plazo | Pendiente |

## 6. Plan de acción

Orden propuesto para abordar lo pendiente, agrupado en fases. Cada
fase asume que la anterior está cerrada; dentro de una fase, el orden
de la lista es el orden de ejecución sugerido.

### Fase 0 — Ya completada (contexto)
No requiere acción; se lista para que el plan quede autocontenido.
- ✅ Scripts de base de datos (4.4) y corrección de `App.config`.
- ✅ `ConexionBDException` / `AbrirConexion()` (parte de 2.1).
- ✅ Pool de Tareas: filtros, maestro-detalle, asignación masiva (1.5).
- ✅ Interfaz adaptable con paneles (3.4).

### Fase 1 — Cimientos (bloqueante, antes de tocar más código)
**Objetivo: dejar de arriesgar el trabajo ya hecho.**
1. **4.2 — Inicializar Git.** ✅ Hecho (2026-09-06): `git init`,
   `.gitignore` (`bin/`, `obj/`, `.vs/`, `*.user`, `Logs/`) y primer
   commit con el estado actual. Resuelve de raíz el riesgo descrito
   en 4.5.
2. **2.1 (resto) — Logging real.** ✅ Hecho (2026-09-06): se optó por
   un registrador propio sin dependencias
   (`Servicios/RegistradorErrores.cs`, salida a `Logs/
   GestionFacturas.log`) en vez de añadir NLog/Serilog, para no
   introducir un paquete NuGet nuevo mientras la gestión de paquetes
   del proyecto no esté clarificada (ver 2.4). Conectado en los 15
   puntos que antes solo hacían `MessageBox.Show`.
3. **1.2 — Reunión con negocio** sobre `Num_Factura`/`F_Factura`/
   `Importe_Total`. **Sigue pendiente** — no es una tarea de código:
   es una decisión previa necesaria si en el futuro se quiere ampliar
   el proceso de facturación dentro de la aplicación.

### Fase 2 — Cerrar el proceso de negocio
**Objetivo: que el ciclo de vida de una factura sea completo y
seguro.**
4. **1.1 — Cambio de estado.** ✅ Hecho (2026-09-06): cambio de
   estado masivo en `FrmPoolTareas`, reutilizando el patrón ya
   construido para la asignación (selección múltiple + acción en
   bloque + transacción en `FacturaRepositorio`). **Decisión
   adoptada:** cualquier transición de estado es válida, sin flujo
   restringido.
5. **1.3 — Control de concurrencia** (`RowVersion`/`timestamp` +
   comprobación en `Actualizar`). **Pospuesto explícitamente** — el
   usuario prefirió no tocar el esquema de `Facturas` todavía.
6. **1.4 — Auditoría** (usuario/fecha de alta y modificación en
   `Facturas` y `Detalle_Facturas`). **Pospuesto explícitamente**, por
   el mismo motivo que 1.3 — ambos requieren `ALTER TABLE` sobre una
   base de datos que ya puede estar en uso.
7. **3.1 — Validar importe/unidades.** ✅ Hecho (2026-09-06): en vez
   de descartarlos en silencio, `ValidarDatos()` bloquea el guardado
   con un aviso si el Importe Estimado o alguna fila de Unidades no
   es un número válido.

*Pendiente real de esta fase: solo 1.3 y 1.4, en espera de decidir
cuándo tocar el esquema de la base de datos.*

### Fase 3 — Calidad y mantenibilidad
**Objetivo: poder cambiar código con confianza.**
8. **2.3 — Primeras pruebas automatizadas**, contra la capa de
   repositorios (no contra la UI — ver la nota de la sección 2.3).
   Empezar por lo más crítico: `FacturaRepositorio.Insertar/Actualizar/
   AsignarUsuario` y `ConexionBD`.
9. **2.2 — Extender excepciones tipadas** (p. ej.
   `FacturaNoEncontradaException`) siguiendo el patrón de
   `ConexionBDException`.
10. **2.5 — Extraer `ValidarDatos()`** a una clase `ValidadorFactura`
    testeable de forma aislada.

*Duración orientativa: ~1 semana.*

### Fase 4 — Experiencia de usuario y escalabilidad de datos
**Objetivo: que la aplicación siga siendo usable cuando crezca el
histórico de facturas.**
11. **3.2 — Paginación** en "Tratar Tarea" y "Pool de Tareas".
12. **3.3 — Exportación** de resultados (Excel/CSV) en ambas
    pantallas.
13. Pulido visual fino del redisño adaptable de la Fase 0 (colores,
    tipografía, iconografía), a validar directamente en la aplicación.

*Duración orientativa: ~1 semana.*

### Fase 5 — Infraestructura a largo plazo (sin prisa)
14. **2.4 — Migrar a `Microsoft.Data.SqlClient`** y limpiar el
    paquete NuGet huérfano.
15. **4.1 — Configuración por entorno** (transformaciones de
    `App.config`).
16. **4.6 — Documentar** en `docs/instalacion.md` el desbloqueo de
    ficheros si el proyecto se sigue repartiendo sin Git (debería ser
    innecesario en cuanto la Fase 1 esté cerrada).
17. **4.3 — Evaluar migración a .NET 8 WinForms** como iniciativa
    aparte, con su propio análisis de esfuerzo/riesgo.

*Sin urgencia: revisar cuando el resto del backlog esté cerrado.*
