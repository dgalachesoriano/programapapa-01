# Documentación — GestionFacturas

Índice de la documentación técnica y funcional de la aplicación
**GestionFacturas**, una aplicación de escritorio (WinForms, .NET
Framework 4.7.2) para el registro y seguimiento de facturas/tareas.

## Contenido

| Documento | Contenido |
|---|---|
| [proceso-negocio.md](proceso-negocio.md) | El proceso de negocio que cubre la aplicación: ciclo de vida de una factura/tarea, estados, roles y flujo entre pantallas. |
| [pantallas.md](pantallas.md) | Manual funcional de cada pantalla: qué hace, qué campos tiene y qué validaciones aplica. |
| [arquitectura.md](arquitectura.md) | Arquitectura técnica del código: capas, estructura de carpetas y por qué está organizado así tras la refactorización. |
| [modelo-datos.md](modelo-datos.md) | Esquema de base de datos inferido a partir de las consultas SQL usadas por la aplicación. |
| [instalacion.md](instalacion.md) | Requisitos, configuración de la cadena de conexión y puesta en marcha. |

## Resumen rápido

GestionFacturas gestiona **facturas** que se tramitan como **tareas**:
se registran con unos datos de cabecera y unas líneas de detalle, se
asignan opcionalmente a un usuario, y se pueden volver a buscar y
editar más adelante desde la pantalla de tratamiento.

```
FrmPrincipal (menú)
 ├─ Registrar Tarea  → FrmRegistrarTarea (alta)
 ├─ Tratar Registro  → FrmTratarTarea (búsqueda) → FrmRegistrarTarea (edición)
 └─ Pool de Tareas   → FrmPoolTareas (pantalla aún sin implementar)
```

Ver el detalle completo en [proceso-negocio.md](proceso-negocio.md).
