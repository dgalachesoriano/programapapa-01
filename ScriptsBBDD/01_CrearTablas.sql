/*
    01_CrearTablas.sql
    ----------------------------------------------------------------
    Crea el modelo de datos de la aplicación GestionFacturas.

    El esquema se ha reconstruido a partir del código de acceso a
    datos (carpeta Datos/) y de los modelos (carpeta Modelos/), ya
    que no existía ningún script de base de datos versionado. Ver
    también docs/modelo-datos.md.

    Orden de creación: primero las tablas "catálogo" (sin
    dependencias), después Facturas (que las referencia) y por
    último Detalle_Facturas (que referencia a Facturas).

    El script es re-ejecutable: cada CREATE TABLE está protegido con
    una comprobación de existencia previa.
*/

USE GestionFacturas;
GO

-- ========================================================
-- Estados_Facturas
-- Catálogo de estados del ciclo de vida de una factura/tarea.
-- Debe existir obligatoriamente una fila con Estado = 'Registrado':
-- es el estado que la aplicación asigna a toda factura nueva
-- (ver FacturaRepositorio.EstadoInicial).
-- ========================================================
IF OBJECT_ID(N'dbo.Estados_Facturas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Estados_Facturas
    (
        IdEstado    INT IDENTITY(1,1) NOT NULL,
        Estado      NVARCHAR(50)      NOT NULL,

        CONSTRAINT PK_Estados_Facturas PRIMARY KEY CLUSTERED (IdEstado),
        CONSTRAINT UQ_Estados_Facturas_Estado UNIQUE (Estado)
    );
END
GO

-- ========================================================
-- Usuarios_Facturas
-- Usuarios a los que se les pueden asignar facturas.
-- ========================================================
IF OBJECT_ID(N'dbo.Usuarios_Facturas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios_Facturas
    (
        IdUsuario   INT IDENTITY(1,1) NOT NULL,
        Usuario     NVARCHAR(50)      NOT NULL,
        Nombre      NVARCHAR(100)     NOT NULL,
        Activo      BIT               NOT NULL CONSTRAINT DF_Usuarios_Facturas_Activo DEFAULT (1),

        CONSTRAINT PK_Usuarios_Facturas PRIMARY KEY CLUSTERED (IdUsuario),
        CONSTRAINT UQ_Usuarios_Facturas_Usuario UNIQUE (Usuario)
    );
END
GO

-- ========================================================
-- Segmento_Negocio
-- Catálogo de segmentos de negocio a los que puede pertenecer
-- una factura.
-- ========================================================
IF OBJECT_ID(N'dbo.Segmento_Negocio', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Segmento_Negocio
    (
        IdSegmento  INT IDENTITY(1,1) NOT NULL,
        Segmento    NVARCHAR(100)     NOT NULL,

        CONSTRAINT PK_Segmento_Negocio PRIMARY KEY CLUSTERED (IdSegmento),
        CONSTRAINT UQ_Segmento_Negocio_Segmento UNIQUE (Segmento)
    );
END
GO

-- ========================================================
-- Facturas
-- Cabecera de cada factura/tarea.
--
-- Num_Factura, F_Factura e Importe_Total solo se leen desde esta
-- aplicación (se muestran en la búsqueda) pero ninguna pantalla los
-- escribe hoy; se asume que los completa un proceso externo
-- (ver docs/modelo-datos.md), por lo que se dejan opcionales.
-- ========================================================
IF OBJECT_ID(N'dbo.Facturas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Facturas
    (
        Registro            INT IDENTITY(1,1)  NOT NULL,
        Documento           NVARCHAR(100)       NOT NULL,
        F_Ent_Calidad       DATETIME            NOT NULL,
        F_Registro          DATETIME            NOT NULL,
        Sociedad            NVARCHAR(100)       NOT NULL,
        Proyecto            NVARCHAR(100)       NOT NULL,
        Importe_Estimado    DECIMAL(18, 2)      NULL,
        IdUsuarioAsignado   INT                 NULL,
        IdEstado            INT                 NOT NULL,
        IdSegmento          INT                 NOT NULL,

        -- Completados por un proceso externo a esta aplicación:
        Num_Factura         NVARCHAR(50)        NULL,
        F_Factura           DATETIME            NULL,
        Importe_Total       DECIMAL(18, 2)      NULL,

        CONSTRAINT PK_Facturas PRIMARY KEY CLUSTERED (Registro),

        CONSTRAINT FK_Facturas_Estados_Facturas
            FOREIGN KEY (IdEstado)
            REFERENCES dbo.Estados_Facturas (IdEstado),

        CONSTRAINT FK_Facturas_Usuarios_Facturas
            FOREIGN KEY (IdUsuarioAsignado)
            REFERENCES dbo.Usuarios_Facturas (IdUsuario),

        CONSTRAINT FK_Facturas_Segmento_Negocio
            FOREIGN KEY (IdSegmento)
            REFERENCES dbo.Segmento_Negocio (IdSegmento)
    );

    CREATE NONCLUSTERED INDEX IX_Facturas_IdEstado ON dbo.Facturas (IdEstado);
    CREATE NONCLUSTERED INDEX IX_Facturas_IdUsuarioAsignado ON dbo.Facturas (IdUsuarioAsignado);
    CREATE NONCLUSTERED INDEX IX_Facturas_IdSegmento ON dbo.Facturas (IdSegmento);
END
GO

-- ========================================================
-- Detalle_Facturas
-- Líneas de detalle (recepción/albarán) de una factura.
-- Se borran e insertan por completo en cada actualización
-- (ver FacturaRepositorio.Actualizar), de ahí el ON DELETE CASCADE.
-- ========================================================
IF OBJECT_ID(N'dbo.Detalle_Facturas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Detalle_Facturas
    (
        RegistroDetalle INT IDENTITY(1,1) NOT NULL,
        RegistroRN      INT               NOT NULL,
        RC              NVARCHAR(50)      NULL,
        Unidades        DECIMAL(18, 2)    NULL,
        PInspeccion     NVARCHAR(50)      NULL,
        PCompra         NVARCHAR(50)      NULL,
        PVenta          NVARCHAR(50)      NULL,
        Albaran         NVARCHAR(50)      NULL,

        CONSTRAINT PK_Detalle_Facturas PRIMARY KEY CLUSTERED (RegistroDetalle),

        CONSTRAINT FK_Detalle_Facturas_Facturas
            FOREIGN KEY (RegistroRN)
            REFERENCES dbo.Facturas (Registro)
            ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_Detalle_Facturas_RegistroRN ON dbo.Detalle_Facturas (RegistroRN);
END
GO
