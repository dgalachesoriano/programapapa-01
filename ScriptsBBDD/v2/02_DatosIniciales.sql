/*
    02_DatosIniciales.sql (modelo v2)
    ----------------------------------------------------------------
    Datos mínimos imprescindibles para que el flujo funcione.

    Los cinco estados (Registrado / En proceso / Pendiente / Facturado
    / Cancelado) son estrictamente obligatorios: el flujo los usa
    explícitamente en cada paso, Y SE INSERTAN EN ESTE ORDEN a
    propósito, porque el código C# (Datos/EstadosTareaConocidos.cs)
    referencia cada uno por su ID en vez de por su texto -así se evita
    depender de una comparación de texto que negocio puede cambiar en
    cualquier momento, que es justo lo que ya pasó una vez (esta tabla
    se sembró originalmente con "REGISTRADO"/"EN_PROCESO"/"BLOQUEADO"/
    "FACTURADO" y se acabó renombrando a los nombres de aquí, rompiendo
    en silencio cualquier comparación de texto)-. Si alguna vez hace
    falta añadir, quitar o reordenar un estado, hay que actualizar
    también esas constantes.

    El resto (segmento, proyecto, moneda, tipo de factura, motivo,
    usuario) son datos de ejemplo: ajústalos a los reales del negocio
    antes de usar la aplicación en serio.
*/

USE GestionFacturas;
GO

-- --------------------------------------------------------
-- TBL_ESTADOS_TAREA (obligatorios: los usa el flujo tal cual;
-- orden de inserción significativo, ver cabecera del script)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Registrado')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'Registrado');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'En proceso')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'En proceso');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Pendiente')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'Pendiente');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Facturado')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'Facturado');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Cancelado')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'Cancelado');
GO

-- --------------------------------------------------------
-- TBL_PROYECTOS (ejemplo orientativo)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_PROYECTOS WHERE DES_PROYECTO = N'General')
    INSERT INTO dbo.TBL_PROYECTOS (DES_PROYECTO) VALUES (N'General');

-- --------------------------------------------------------
-- TBL_SEGMENTOS (ejemplo orientativo)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_SEGMENTOS WHERE DES_SEGMENTO = N'General')
    INSERT INTO dbo.TBL_SEGMENTOS (DES_SEGMENTO) VALUES (N'General');

-- --------------------------------------------------------
-- TBL_DIVISAS (ejemplo orientativo)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_DIVISAS WHERE COD_DIV = N'EUR')
    INSERT INTO dbo.TBL_DIVISAS (COD_DIV) VALUES (N'EUR');

-- --------------------------------------------------------
-- TBL_TIPFACTURAS (ejemplo orientativo)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_TIPFACTURAS WHERE DES_TIP_FACT = N'Estándar')
    INSERT INTO dbo.TBL_TIPFACTURAS (DES_TIP_FACT) VALUES (N'Estándar');

-- --------------------------------------------------------
-- TBL_MOTIVO (ejemplo orientativo, para el bloqueo de tareas)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Pendiente de documentación')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Pendiente de documentación');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Pendiente de validación del cliente')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Pendiente de validación del cliente');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Otros')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Otros');

-- --------------------------------------------------------
-- TBL_USUARIOS (ejemplo orientativo)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_USUARIOS WHERE NOMBRE = N'Administrador')
    INSERT INTO dbo.TBL_USUARIOS (NOMBRE) VALUES (N'Administrador');
GO
