# Instalación y configuración

## Requisitos

- Windows con **.NET Framework 4.7.2** (o superior compatible)
  instalado.
- **Visual Studio 2019/2022** (o MSBuild equivalente) para compilar
  el proyecto `GestionFacturas.csproj`.
- Acceso a una instancia de **SQL Server** con la base de datos del
  proyecto (ver [modelo-datos.md](modelo-datos.md)).

> Nota: este proyecto es una aplicación de escritorio WinForms para
> Windows; no se puede compilar ni ejecutar en macOS/Linux.

## Configuración de la cadena de conexión

Desde la refactorización, la cadena de conexión **ya no está
hardcodeada en el código**. Se configura en `App.config`:

```xml
<connectionStrings>
    <add name="GestionFacturasConnection"
         connectionString="Server=TU_SERVIDOR;Database=TU_BASE_DE_DATOS;Integrated Security=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

Para apuntar a otro servidor o base de datos (por ejemplo, al pasar
de un entorno de pruebas a producción), basta con editar el atributo
`connectionString` de esa entrada — no hace falta tocar ni
recompilar el código. Tras publicar la aplicación, el archivo
equivalente es `GestionFacturas.exe.config`, generado a partir de
`App.config`.

## Puesta en marcha

1. Abrir `GestionFacturas.sln` en Visual Studio.
2. Restaurar paquetes NuGet si el IDE lo solicita (carpeta
   `packages/`).
3. Ajustar `App.config` con la cadena de conexión del entorno.
4. Compilar y ejecutar (F5). El punto de entrada es
   `Program.Main`, que abre `FrmPrincipal`.

## Requisitos de datos mínimos para poder usar la aplicación

- La tabla `Estados_Facturas` debe tener una fila con
  `Estado = 'Registrado'`; si no existe, el alta de tareas nuevas
  fallará con un error explícito.
- Debe existir al menos un `Segmento_Negocio`, ya que es obligatorio
  para registrar una factura.
