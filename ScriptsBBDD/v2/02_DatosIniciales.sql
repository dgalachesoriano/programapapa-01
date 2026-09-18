/*
    02_DatosIniciales.sql (modelo v2)
    ----------------------------------------------------------------
    Datos mínimos imprescindibles para que el flujo funcione.

    Los tres estados (REGISTRADO / EN_PROCESO / BLOQUEADO / FACTURADO)
    son estrictamente obligatorios: el flujo descrito los usa
    explícitamente en cada paso. El resto (segmento, proyecto, moneda,
    tipo de factura, motivo, usuario) son datos de ejemplo: ajústalos a
    los reales del negocio antes de usar la aplicación en serio.
*/

USE GestionFacturas;
GO

-- --------------------------------------------------------
-- TBL_ESTADOS_TAREA (obligatorios: los usa el flujo tal cual)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'REGISTRADO')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'REGISTRADO');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'EN_PROCESO')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'EN_PROCESO');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'BLOQUEADO')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'BLOQUEADO');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'FACTURADO')
    INSERT INTO dbo.TBL_ESTADOS_TAREA (DES_ESTADO) VALUES (N'FACTURADO');
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
