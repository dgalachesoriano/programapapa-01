/*
    01_CrearTablas.sql (modelo v2 - orientado a flujo con tabla de control)
    ----------------------------------------------------------------
    Nuevo modelo de datos de GestionFacturas, sustituyendo al modelo
    de Facturas/Detalle_Facturas de ../01_CrearTablas.sql por un
    proceso explícito de 3 pasos (Registrar Tarea -> Pool de Tareas ->
    Facturado) controlado por TBL_CONTROL.

    Todos los nombres de tabla y de columna están en MAYÚSCULAS, tal
    y como se ha pedido. Este script NO modifica ni elimina el modelo
    anterior (Facturas/Detalle_Facturas/...): créalo en la misma base
    de datos o en una nueva según convenga para la migración; ver
    README.md de esta carpeta.

    Diferencias respecto al modelo de entidades que se ha pedido
    textualmente, con la justificación de cada una (ver también el
    análisis en la conversación / README.md):

      1. Se añade TBL_SEGMENTOS. Se menciona COD_SEQ_SEG como lista
         sobre "tabla TBL_SEGMENTOS", pero esa tabla no estaba en el
         listado de entidades; se crea con la misma forma que
         TBL_PROYECTOS (catálogo con ID/descripción/activo).

      2. TBL_USUARIOS incorpora XTI_ACTIVO. No estaba en la definición
         original, pero se usa como lista desplegable (para asignar
         tarea) igual que el resto de catálogos, y la propia petición
         indica que todo campo de lista debe tener en cuenta
         XTI_ACTIVO para decidir si se muestra.

      3. TBL_CONTROL.ESTADO pasa de texto libre a COD_SEQ_EST, clave
         foránea a un nuevo catálogo TBL_ESTADOS_TAREA. Es el único
         campo "categórico" del modelo pedido que no seguía el patrón
         COD_SEQ_xxx -> tabla catálogo (que sí se usa para proyecto,
         segmento, usuario, moneda, tipo de factura y motivo); se
         unifica por consistencia y para poder añadir o desactivar
         estados sin tocar código ni arriesgar valores de texto
         inconsistentes ("Bloqueado" vs "bloqueado"...).

      4. TBL_CONTROL es una tabla de HISTÓRICO (una fila nueva por
         cada cambio de estado/asignación/bloqueo/facturación), no una
         única fila por tarea que se va actualizando. Con esto:
           - Queda registrado quién y cuándo hizo cada cambio
             (auditoría), sin columnas de auditoría adicionales en
             TBL_TAREAS.
           - El estado "actual" de una tarea es, simplemente, su fila
             más reciente en TBL_CONTROL (ver vista
             VW_TAREAS_ESTADO_ACTUAL en 03_Vistas.sql).
         Si se prefiriera una única fila mutable por tarea, el cambio
         es mínimo: añadir CONSTRAINT UQ_TBL_CONTROL_COD_SEQ_TAR
         UNIQUE (COD_SEQ_TAR) y hacer UPDATE en vez de INSERT desde la
         aplicación.

    Decisiones confirmadas tras revisar las preguntas abiertas de la
    primera versión de este script:

      - TBL_FACTURADO.COD_FACT NO lleva UNIQUE: un número de factura
        puede repetirse (p. ej. entre tipos/series distintos).
      - TBL_DETALLETAREA se queda sin equivalente a la antigua "RC"
        (Referencia de Control): confirmado que su desaparición es
        intencionada.
      - TBL_CONTROL NO lleva un campo de observaciones en texto
        libre: el motivo de bloqueo es únicamente COD_SEQ_MOTIVO,
        una lista (TBL_MOTIVO) de la que hay que seleccionar un
        valor obligatoriamente; no se ofrece texto libre.

    El resto de campos, tipos y relaciones siguen literalmente lo
    especificado.
*/

USE GestionFacturas;
GO

-- ========================================================
-- Catálogos (sin dependencias)
-- ========================================================

IF OBJECT_ID(N'dbo.TBL_USUARIOS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_USUARIOS
    (
        ID          INT IDENTITY(1,1) NOT NULL,
        NOMBRE      NVARCHAR(100)     NOT NULL,
        XTI_ACTIVO  CHAR(1)           NOT NULL CONSTRAINT DF_TBL_USUARIOS_XTI_ACTIVO DEFAULT ('S'),

        CONSTRAINT PK_TBL_USUARIOS PRIMARY KEY CLUSTERED (ID),
        CONSTRAINT CK_TBL_USUARIOS_XTI_ACTIVO CHECK (XTI_ACTIVO IN ('S', 'N'))
    );
END
GO

IF OBJECT_ID(N'dbo.TBL_PROYECTOS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_PROYECTOS
    (
        ID              INT IDENTITY(1,1) NOT NULL,
        DES_PROYECTO    NVARCHAR(100)     NOT NULL,
        XTI_ACTIVO      CHAR(1)           NOT NULL CONSTRAINT DF_TBL_PROYECTOS_XTI_ACTIVO DEFAULT ('S'),

        CONSTRAINT PK_TBL_PROYECTOS PRIMARY KEY CLUSTERED (ID),
        CONSTRAINT UQ_TBL_PROYECTOS_DES_PROYECTO UNIQUE (DES_PROYECTO),
        CONSTRAINT CK_TBL_PROYECTOS_XTI_ACTIVO CHECK (XTI_ACTIVO IN ('S', 'N'))
    );
END
GO

-- TBL_SEGMENTOS: no estaba en el listado de entidades pero es
-- necesaria para COD_SEQ_SEG (ver nota 1 de la cabecera).
IF OBJECT_ID(N'dbo.TBL_SEGMENTOS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_SEGMENTOS
    (
        ID              INT IDENTITY(1,1) NOT NULL,
        DES_SEGMENTO    NVARCHAR(100)     NOT NULL,
        XTI_ACTIVO      CHAR(1)           NOT NULL CONSTRAINT DF_TBL_SEGMENTOS_XTI_ACTIVO DEFAULT ('S'),

        CONSTRAINT PK_TBL_SEGMENTOS PRIMARY KEY CLUSTERED (ID),
        CONSTRAINT UQ_TBL_SEGMENTOS_DES_SEGMENTO UNIQUE (DES_SEGMENTO),
        CONSTRAINT CK_TBL_SEGMENTOS_XTI_ACTIVO CHECK (XTI_ACTIVO IN ('S', 'N'))
    );
END
GO

IF OBJECT_ID(N'dbo.TBL_DIVISAS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_DIVISAS
    (
        ID          INT IDENTITY(1,1) NOT NULL,
        COD_DIV     NVARCHAR(10)      NOT NULL,
        XTI_ACTIVO  CHAR(1)           NOT NULL CONSTRAINT DF_TBL_DIVISAS_XTI_ACTIVO DEFAULT ('S'),

        CONSTRAINT PK_TBL_DIVISAS PRIMARY KEY CLUSTERED (ID),
        CONSTRAINT UQ_TBL_DIVISAS_COD_DIV UNIQUE (COD_DIV),
        CONSTRAINT CK_TBL_DIVISAS_XTI_ACTIVO CHECK (XTI_ACTIVO IN ('S', 'N'))
    );
END
GO

IF OBJECT_ID(N'dbo.TBL_TIPFACTURAS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_TIPFACTURAS
    (
        ID              INT IDENTITY(1,1) NOT NULL,
        DES_TIP_FACT    NVARCHAR(100)     NOT NULL,
        XTI_ACTIVO      CHAR(1)           NOT NULL CONSTRAINT DF_TBL_TIPFACTURAS_XTI_ACTIVO DEFAULT ('S'),

        CONSTRAINT PK_TBL_TIPFACTURAS PRIMARY KEY CLUSTERED (ID),
        CONSTRAINT UQ_TBL_TIPFACTURAS_DES_TIP_FACT UNIQUE (DES_TIP_FACT),
        CONSTRAINT CK_TBL_TIPFACTURAS_XTI_ACTIVO CHECK (XTI_ACTIVO IN ('S', 'N'))
    );
END
GO

IF OBJECT_ID(N'dbo.TBL_MOTIVO', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_MOTIVO
    (
        ID          INT IDENTITY(1,1) NOT NULL,
        DES_MOTIVO  NVARCHAR(200)     NOT NULL,
        XTI_ACTIVO  CHAR(1)           NOT NULL CONSTRAINT DF_TBL_MOTIVO_XTI_ACTIVO DEFAULT ('S'),

        CONSTRAINT PK_TBL_MOTIVO PRIMARY KEY CLUSTERED (ID),
        CONSTRAINT UQ_TBL_MOTIVO_DES_MOTIVO UNIQUE (DES_MOTIVO),
        CONSTRAINT CK_TBL_MOTIVO_XTI_ACTIVO CHECK (XTI_ACTIVO IN ('S', 'N'))
    );
END
GO

-- TBL_ESTADOS_TAREA: catálogo de estados del flujo de una tarea
-- (Registrado / En Proceso / Bloqueado / Facturado). Sustituye al
-- campo de texto libre ESTADO de la definición original (ver nota 3
-- de la cabecera).
IF OBJECT_ID(N'dbo.TBL_ESTADOS_TAREA', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_ESTADOS_TAREA
    (
        ID          INT IDENTITY(1,1) NOT NULL,
        DES_ESTADO  NVARCHAR(50)      NOT NULL,
        XTI_ACTIVO  CHAR(1)           NOT NULL CONSTRAINT DF_TBL_ESTADOS_TAREA_XTI_ACTIVO DEFAULT ('S'),

        CONSTRAINT PK_TBL_ESTADOS_TAREA PRIMARY KEY CLUSTERED (ID),
        CONSTRAINT UQ_TBL_ESTADOS_TAREA_DES_ESTADO UNIQUE (DES_ESTADO),
        CONSTRAINT CK_TBL_ESTADOS_TAREA_XTI_ACTIVO CHECK (XTI_ACTIVO IN ('S', 'N'))
    );
END
GO

-- ========================================================
-- TBL_TAREAS
-- Cabecera de cada tarea (paso 1: Registrar Tarea).
-- ========================================================
IF OBJECT_ID(N'dbo.TBL_TAREAS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_TAREAS
    (
        ID              INT IDENTITY(1,1) NOT NULL,
        DES_DOC         NVARCHAR(100)     NOT NULL,
        FEC_ENT_CAL     DATE              NOT NULL,
        FEC_REG         DATE              NOT NULL,
        DES_ORG_VENTAS  NVARCHAR(100)     NOT NULL,
        COD_SEQ_PRY     INT               NOT NULL,
        COD_SEQ_SEG     INT               NOT NULL,
        IMP_ESTIMADO    DECIMAL(18, 2)    NULL,

        CONSTRAINT PK_TBL_TAREAS PRIMARY KEY CLUSTERED (ID),

        -- "no puede ser nulo ni duplicarse", tal cual se ha pedido.
        CONSTRAINT UQ_TBL_TAREAS_DES_DOC UNIQUE (DES_DOC),

        CONSTRAINT FK_TBL_TAREAS_TBL_PROYECTOS
            FOREIGN KEY (COD_SEQ_PRY)
            REFERENCES dbo.TBL_PROYECTOS (ID),

        CONSTRAINT FK_TBL_TAREAS_TBL_SEGMENTOS
            FOREIGN KEY (COD_SEQ_SEG)
            REFERENCES dbo.TBL_SEGMENTOS (ID),

        CONSTRAINT CK_TBL_TAREAS_IMP_ESTIMADO
            CHECK (IMP_ESTIMADO IS NULL OR IMP_ESTIMADO >= 0)
    );

    CREATE NONCLUSTERED INDEX IX_TBL_TAREAS_COD_SEQ_PRY ON dbo.TBL_TAREAS (COD_SEQ_PRY);
    CREATE NONCLUSTERED INDEX IX_TBL_TAREAS_COD_SEQ_SEG ON dbo.TBL_TAREAS (COD_SEQ_SEG);
END
GO

-- ========================================================
-- TBL_DETALLETAREA
-- Líneas de detalle de una tarea (varias por tarea).
-- ========================================================
IF OBJECT_ID(N'dbo.TBL_DETALLETAREA', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_DETALLETAREA
    (
        ID              INT IDENTITY(1,1) NOT NULL,
        COD_SEQ_TAR     INT               NOT NULL,
        COD_PED_VENTA   NVARCHAR(50)      NULL,
        COD_PED_COMPRA  NVARCHAR(50)      NULL,
        COD_PED_INSPEC  NVARCHAR(50)      NULL,
        COD_ENT_ENTR    NVARCHAR(50)      NULL,
        NBR_UNIDADES    DECIMAL(18, 2)    NULL,

        CONSTRAINT PK_TBL_DETALLETAREA PRIMARY KEY CLUSTERED (ID),

        CONSTRAINT FK_TBL_DETALLETAREA_TBL_TAREAS
            FOREIGN KEY (COD_SEQ_TAR)
            REFERENCES dbo.TBL_TAREAS (ID)
            ON DELETE CASCADE,

        CONSTRAINT CK_TBL_DETALLETAREA_NBR_UNIDADES
            CHECK (NBR_UNIDADES IS NULL OR NBR_UNIDADES >= 0)
    );

    CREATE NONCLUSTERED INDEX IX_TBL_DETALLETAREA_COD_SEQ_TAR ON dbo.TBL_DETALLETAREA (COD_SEQ_TAR);
END
GO

-- ========================================================
-- TBL_FACTURADO
-- Datos de facturación (paso 3). Entidad independiente: se
-- relaciona con la tarea únicamente a través de TBL_CONTROL.
-- ========================================================
IF OBJECT_ID(N'dbo.TBL_FACTURADO', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_FACTURADO
    (
        ID              INT IDENTITY(1,1) NOT NULL,
        COD_ENT_SAL     NVARCHAR(50)      NULL,
        FEC_FACT        DATE              NOT NULL,
        COD_FACT        NVARCHAR(50)      NOT NULL,
        IMP_FACT        DECIMAL(18, 2)    NOT NULL,
        COD_SEQ_MON     INT               NOT NULL,
        COD_SEQ_TPF     INT               NOT NULL,

        CONSTRAINT PK_TBL_FACTURADO PRIMARY KEY CLUSTERED (ID),

        CONSTRAINT FK_TBL_FACTURADO_TBL_DIVISAS
            FOREIGN KEY (COD_SEQ_MON)
            REFERENCES dbo.TBL_DIVISAS (ID),

        CONSTRAINT FK_TBL_FACTURADO_TBL_TIPFACTURAS
            FOREIGN KEY (COD_SEQ_TPF)
            REFERENCES dbo.TBL_TIPFACTURAS (ID),

        CONSTRAINT CK_TBL_FACTURADO_IMP_FACT
            CHECK (IMP_FACT >= 0)
    );

    CREATE NONCLUSTERED INDEX IX_TBL_FACTURADO_COD_SEQ_MON ON dbo.TBL_FACTURADO (COD_SEQ_MON);
    CREATE NONCLUSTERED INDEX IX_TBL_FACTURADO_COD_SEQ_TPF ON dbo.TBL_FACTURADO (COD_SEQ_TPF);
END
GO

-- ========================================================
-- TBL_CONTROL
-- Histórico de eventos del flujo de una tarea: alta, asignación,
-- bloqueo y facturación. Una fila nueva por cada evento (ver nota 4
-- de la cabecera); el estado "actual" de una tarea es su fila más
-- reciente (ver vista VW_TAREAS_ESTADO_ACTUAL en 03_Vistas.sql).
--
-- COD_SEQ_MOTIVO se informa solo en los eventos de bloqueo: la
-- aplicación debe exigir seleccionar un valor de TBL_MOTIVO (lista
-- desplegable, sin texto libre) al marcar una tarea como bloqueada.
-- ========================================================
IF OBJECT_ID(N'dbo.TBL_CONTROL', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TBL_CONTROL
    (
        ID              INT IDENTITY(1,1) NOT NULL,
        COD_SEQ_TAR     INT               NOT NULL,
        COD_SEQ_USER    INT               NULL,
        COD_SEQ_FAC     INT               NULL,
        COD_SEQ_EST     INT               NOT NULL,
        COD_SEQ_MOTIVO  INT               NULL,
        FEC_ALTA        DATETIME2(0)      NOT NULL CONSTRAINT DF_TBL_CONTROL_FEC_ALTA DEFAULT (SYSDATETIME()),

        CONSTRAINT PK_TBL_CONTROL PRIMARY KEY CLUSTERED (ID),

        CONSTRAINT FK_TBL_CONTROL_TBL_TAREAS
            FOREIGN KEY (COD_SEQ_TAR)
            REFERENCES dbo.TBL_TAREAS (ID),

        CONSTRAINT FK_TBL_CONTROL_TBL_USUARIOS
            FOREIGN KEY (COD_SEQ_USER)
            REFERENCES dbo.TBL_USUARIOS (ID),

        CONSTRAINT FK_TBL_CONTROL_TBL_FACTURADO
            FOREIGN KEY (COD_SEQ_FAC)
            REFERENCES dbo.TBL_FACTURADO (ID),

        CONSTRAINT FK_TBL_CONTROL_TBL_ESTADOS_TAREA
            FOREIGN KEY (COD_SEQ_EST)
            REFERENCES dbo.TBL_ESTADOS_TAREA (ID),

        CONSTRAINT FK_TBL_CONTROL_TBL_MOTIVO
            FOREIGN KEY (COD_SEQ_MOTIVO)
            REFERENCES dbo.TBL_MOTIVO (ID)
    );

    -- Cubre tanto "histórico de una tarea" como "última fila de una
    -- tarea" (la vista de 03_Vistas.sql se apoya en este índice).
    CREATE NONCLUSTERED INDEX IX_TBL_CONTROL_COD_SEQ_TAR_FEC_ALTA
        ON dbo.TBL_CONTROL (COD_SEQ_TAR, FEC_ALTA DESC, ID DESC);

    CREATE NONCLUSTERED INDEX IX_TBL_CONTROL_COD_SEQ_USER ON dbo.TBL_CONTROL (COD_SEQ_USER);
END
GO
