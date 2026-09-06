/*
    02_DatosIniciales.sql
    ----------------------------------------------------------------
    Datos mínimos imprescindibles para que la aplicación funcione.

    Solo la fila 'Registrado' de Estados_Facturas es estrictamente
    obligatoria: si no existe, el alta de facturas falla
    (FacturaRepositorio.EstadoInicial / EstadoFacturaRepositorio.
    ObtenerIdPorNombre lanza InvalidOperationException).

    El resto de estados, los segmentos de negocio y los usuarios son
    datos de ejemplo/orientativos: ajústalos a los reales del negocio
    antes de usar la aplicación en serio.
*/

USE GestionFacturas;
GO

-- --------------------------------------------------------
-- Estados_Facturas
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Estados_Facturas WHERE Estado = N'Registrado')
    INSERT INTO dbo.Estados_Facturas (Estado) VALUES (N'Registrado');

IF NOT EXISTS (SELECT 1 FROM dbo.Estados_Facturas WHERE Estado = N'Pendiente')
    INSERT INTO dbo.Estados_Facturas (Estado) VALUES (N'Pendiente');

IF NOT EXISTS (SELECT 1 FROM dbo.Estados_Facturas WHERE Estado = N'Facturado')
    INSERT INTO dbo.Estados_Facturas (Estado) VALUES (N'Facturado');
GO

-- --------------------------------------------------------
-- Segmento_Negocio (ejemplo orientativo, ajustar según negocio)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Segmento_Negocio WHERE Segmento = N'General')
    INSERT INTO dbo.Segmento_Negocio (Segmento) VALUES (N'General');
GO

-- --------------------------------------------------------
-- Usuarios_Facturas (ejemplo orientativo, ajustar según negocio)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios_Facturas WHERE Usuario = N'admin')
    INSERT INTO dbo.Usuarios_Facturas (Usuario, Nombre, Activo)
    VALUES (N'admin', N'Administrador', 1);
GO
