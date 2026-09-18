/*
    ResetearDatos.sql (modelo v2)
    ----------------------------------------------------------------
    ¡ATENCIÓN! Script DESTRUCTIVO: borra TODAS las filas de las
    tablas de movimiento del flujo (tareas, su detalle, los eventos
    de control y los datos de facturación) y reinicia sus contadores
    IDENTITY a 0, para que la siguiente fila insertada vuelva a
    empezar por 1. Pensado para dejar la base de datos como recién
    creada durante desarrollo/pruebas.

    NO se ejecuta automáticamente: revísalo y lánzalo tú mismo
    (SSMS o sqlcmd) cuando quieras resetear el estado.

    NO toca los catálogos (TBL_USUARIOS, TBL_PROYECTOS,
    TBL_SEGMENTOS, TBL_DIVISAS, TBL_TIPFACTURAS, TBL_MOTIVO,
    TBL_ESTADOS_TAREA): solo las cuatro tablas de movimiento
    pedidas explícitamente.

    Orden de borrado (obligatorio por las claves foráneas):
      1. TBL_CONTROL      (referencia a TBL_TAREAS y TBL_FACTURADO)
      2. TBL_DETALLETAREA (referencia a TBL_TAREAS)
      3. TBL_TAREAS
      4. TBL_FACTURADO

    TBL_CONTROL y TBL_DETALLETAREA no están referenciadas por
    ninguna otra tabla, así que se pueden truncar directamente
    (TRUNCATE reinicia el IDENTITY por sí solo). TBL_TAREAS y
    TBL_FACTURADO sí están referenciadas por una FOREIGN KEY (desde
    TBL_CONTROL, aunque ya esté vacía) y SQL Server no permite
    truncar una tabla en esa situación; para ellas se usa DELETE +
    DBCC CHECKIDENT para reiniciar el contador a mano.

    Todo dentro de una única transacción: si algo falla a mitad, no
    se queda la base de datos a medio borrar.
*/

USE GestionFacturas;
GO

BEGIN TRANSACTION;

BEGIN TRY

    TRUNCATE TABLE dbo.TBL_CONTROL;

    TRUNCATE TABLE dbo.TBL_DETALLETAREA;

    DELETE FROM dbo.TBL_TAREAS;
    DBCC CHECKIDENT ('dbo.TBL_TAREAS', RESEED, 0);

    DELETE FROM dbo.TBL_FACTURADO;
    DBCC CHECKIDENT ('dbo.TBL_FACTURADO', RESEED, 0);

    COMMIT TRANSACTION;

    PRINT 'Reseteo completado: TBL_CONTROL, TBL_DETALLETAREA, TBL_TAREAS y TBL_FACTURADO están vacías y sus contadores IDENTITY vuelven a empezar en 1.';

END TRY
BEGIN CATCH

    ROLLBACK TRANSACTION;

    PRINT 'Reseteo cancelado, no se ha borrado nada. Error: ' + ERROR_MESSAGE();

    THROW;

END CATCH
GO
