/*
    DatosPrueba.sql (modelo v2)
    ----------------------------------------------------------------
    Carga datos de prueba con volumen: 10 tareas por cada usuario
    activo de TBL_USUARIOS (documentos "DOC-PRUEBA-001".."DOC-PRUEBA-NNN"),
    cada una asignada a ese usuario y repartida entre distintos
    estados/combinaciones para poder probar filtros, el coloreado por
    estado del Pool de Tareas y el informe de facturación con algo
    más de volumen que un par de filas sueltas.

    Usa los catálogos que YA existen en tu base de datos (usuarios,
    proyectos, segmentos, divisas, tipos de factura, motivos, estados)
    tal cual están cargados; no inserta ni modifica ningún catálogo.

    Los nombres de estado se buscan por su texto EXACTO actual en
    TBL_ESTADOS_TAREA: "Registrado", "En proceso", "Pendiente",
    "Facturado", "Cancelado" (el código C# ya no depende de este texto:
    ver Datos/EstadosTareaConocidos.cs, que referencia cada estado por
    su ID).

    Por cada usuario, sus 10 tareas se reparten así:
      - 3 se quedan en "En proceso" (asignadas, sin más eventos).
      - 2 pasan a "Pendiente" (bloqueadas, con un motivo).
      - 4 pasan a "Facturado" (con su fila de TBL_FACTURADO).
      - 1 pasa a "Cancelado".
    Todas nacen en "Registrado" y se asignan a ese usuario como
    segundo evento, igual que haría la aplicación (Registrar Tarea +
    Pool de Tareas > Asignar tarea), antes de progresar al estado
    final que le toque.

    Re-ejecutable: al principio borra cualquier dato de prueba
    anterior generado por este mismo script (documentos
    "DOC-PRUEBA-%" y facturas "FAC-PRUEBA-%"), para no acumular
    duplicados ni chocar con la restricción UNIQUE de DES_DOC.
*/

USE GestionFacturas;
GO

SET NOCOUNT ON;

BEGIN TRANSACTION;

BEGIN TRY

    -- --------------------------------------------------------
    -- Limpieza de una carga anterior de este mismo script
    -- --------------------------------------------------------
    DELETE FROM dbo.TBL_CONTROL
    WHERE COD_SEQ_TAR IN (SELECT ID FROM dbo.TBL_TAREAS WHERE DES_DOC LIKE N'DOC-PRUEBA-%');

    DELETE FROM dbo.TBL_FACTURADO
    WHERE COD_FACT LIKE N'FAC-PRUEBA-%';

    DELETE FROM dbo.TBL_DETALLETAREA
    WHERE COD_SEQ_TAR IN (SELECT ID FROM dbo.TBL_TAREAS WHERE DES_DOC LIKE N'DOC-PRUEBA-%');

    DELETE FROM dbo.TBL_TAREAS
    WHERE DES_DOC LIKE N'DOC-PRUEBA-%';

    -- --------------------------------------------------------
    -- Catálogos indexados 1..N, para poder rotar sobre ellos con
    -- módulo y así variar proyecto/segmento/divisa/tipo/motivo de
    -- una tarea a otra.
    -- --------------------------------------------------------
    DECLARE @Usuarios TABLE (Idx INT IDENTITY(1, 1), ID INT);
    INSERT INTO @Usuarios (ID) SELECT ID FROM dbo.TBL_USUARIOS WHERE XTI_ACTIVO = 'S' ORDER BY ID;

    DECLARE @Proyectos TABLE (Idx INT IDENTITY(1, 1), ID INT);
    INSERT INTO @Proyectos (ID) SELECT ID FROM dbo.TBL_PROYECTOS WHERE XTI_ACTIVO = 'S' ORDER BY ID;

    DECLARE @Segmentos TABLE (Idx INT IDENTITY(1, 1), ID INT);
    INSERT INTO @Segmentos (ID) SELECT ID FROM dbo.TBL_SEGMENTOS WHERE XTI_ACTIVO = 'S' ORDER BY ID;

    DECLARE @Divisas TABLE (Idx INT IDENTITY(1, 1), ID INT);
    INSERT INTO @Divisas (ID) SELECT ID FROM dbo.TBL_DIVISAS WHERE XTI_ACTIVO = 'S' ORDER BY ID;

    DECLARE @TiposFactura TABLE (Idx INT IDENTITY(1, 1), ID INT);
    INSERT INTO @TiposFactura (ID) SELECT ID FROM dbo.TBL_TIPFACTURAS WHERE XTI_ACTIVO = 'S' ORDER BY ID;

    DECLARE @Motivos TABLE (Idx INT IDENTITY(1, 1), ID INT);
    INSERT INTO @Motivos (ID) SELECT ID FROM dbo.TBL_MOTIVO WHERE XTI_ACTIVO = 'S' ORDER BY ID;

    DECLARE @NumUsuarios INT = (SELECT COUNT(*) FROM @Usuarios);
    DECLARE @NumProyectos INT = (SELECT COUNT(*) FROM @Proyectos);
    DECLARE @NumSegmentos INT = (SELECT COUNT(*) FROM @Segmentos);
    DECLARE @NumDivisas INT = (SELECT COUNT(*) FROM @Divisas);
    DECLARE @NumTiposFactura INT = (SELECT COUNT(*) FROM @TiposFactura);
    DECLARE @NumMotivos INT = (SELECT COUNT(*) FROM @Motivos);

    IF @NumUsuarios = 0 OR @NumProyectos = 0 OR @NumSegmentos = 0
        OR @NumDivisas = 0 OR @NumTiposFactura = 0 OR @NumMotivos = 0
    BEGIN
        RAISERROR(N'Falta algún catálogo activo (usuarios/proyectos/segmentos/divisas/tipos de factura/motivos); no se puede generar la carga de prueba.', 16, 1);
    END

    -- --------------------------------------------------------
    -- Estados del flujo, por su nombre EXACTO actual (ver aviso de
    -- cabecera: no son los que espera la aplicación C#).
    -- --------------------------------------------------------
    DECLARE @IdEstRegistrado INT = (SELECT ID FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Registrado');
    DECLARE @IdEstEnProceso INT = (SELECT ID FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'En proceso');
    DECLARE @IdEstPendiente INT = (SELECT ID FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Pendiente');
    DECLARE @IdEstFacturado INT = (SELECT ID FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Facturado');
    DECLARE @IdEstCancelado INT = (SELECT ID FROM dbo.TBL_ESTADOS_TAREA WHERE DES_ESTADO = N'Cancelado');

    IF @IdEstRegistrado IS NULL OR @IdEstEnProceso IS NULL OR @IdEstPendiente IS NULL
        OR @IdEstFacturado IS NULL OR @IdEstCancelado IS NULL
    BEGIN
        RAISERROR(N'No se han encontrado en TBL_ESTADOS_TAREA los 5 estados esperados (Registrado/En proceso/Pendiente/Facturado/Cancelado); revisa sus nombres exactos.', 16, 1);
    END

    -- --------------------------------------------------------
    -- Generación: 10 tareas por cada usuario activo
    -- --------------------------------------------------------
    DECLARE @IdxUsuario INT = 1;
    DECLARE @IdxTarea INT;
    DECLARE @Contador INT = 0;

    DECLARE @IdUsuario INT;
    DECLARE @IdProyecto INT, @IdSegmento INT, @IdDivisa INT, @IdTipoFactura INT, @IdMotivo INT;
    DECLARE @OrgVentas NVARCHAR(100);
    DECLARE @ImporteEstimado DECIMAL(18, 2);
    DECLARE @ImporteFactura DECIMAL(18, 2);
    DECLARE @FecReg DATE, @FecEntCal DATE;
    DECLARE @Documento NVARCHAR(100);
    DECLARE @IdTarea INT;
    DECLARE @IdFacturado INT;

    WHILE @IdxUsuario <= @NumUsuarios
    BEGIN
        SELECT @IdUsuario = ID FROM @Usuarios WHERE Idx = @IdxUsuario;

        SET @IdxTarea = 1;

        WHILE @IdxTarea <= 10
        BEGIN
            SET @Contador = @Contador + 1;

            SELECT @IdProyecto = ID FROM @Proyectos WHERE Idx = ((@Contador - 1) % @NumProyectos) + 1;
            SELECT @IdSegmento = ID FROM @Segmentos WHERE Idx = ((@Contador - 1) % @NumSegmentos) + 1;
            SELECT @IdDivisa = ID FROM @Divisas WHERE Idx = ((@Contador - 1) % @NumDivisas) + 1;
            SELECT @IdTipoFactura = ID FROM @TiposFactura WHERE Idx = ((@Contador - 1) % @NumTiposFactura) + 1;
            SELECT @IdMotivo = ID FROM @Motivos WHERE Idx = ((@Contador - 1) % @NumMotivos) + 1;

            SET @OrgVentas = (CASE (@Contador % 3)
                                  WHEN 0 THEN N'ACME Ingeniería SL'
                                  WHEN 1 THEN N'Grupo Industrial Norte SA'
                                  ELSE N'Suministros del Sur SL'
                              END);

            -- Importe entre 150 y 2149, con dos decimales; fecha de
            -- registro escalonada hacia atrás para que el informe de
            -- facturación tenga varios meses con datos.
            SET @ImporteEstimado = CAST(150 + ((@Contador * 137) % 2000) AS DECIMAL(18, 2));
            SET @FecReg = DATEADD(day, -(@Contador * 3), CAST(GETDATE() AS DATE));
            SET @FecEntCal = DATEADD(day, -2, @FecReg);
            SET @Documento = N'DOC-PRUEBA-' + RIGHT('000' + CAST(@Contador AS VARCHAR(3)), 3);

            -- Cabecera de la tarea
            INSERT INTO dbo.TBL_TAREAS
                (DES_DOC, FEC_ENT_CAL, FEC_REG, DES_ORG_VENTAS, COD_SEQ_PRY, COD_SEQ_SEG, IMP_ESTIMADO)
            VALUES
                (@Documento, @FecEntCal, @FecReg, @OrgVentas, @IdProyecto, @IdSegmento, @ImporteEstimado);

            SET @IdTarea = SCOPE_IDENTITY();

            -- Detalle: siempre una línea; las tareas pares llevan una
            -- segunda línea, para variar también el número de líneas.
            INSERT INTO dbo.TBL_DETALLETAREA
                (COD_SEQ_TAR, COD_PED_VENTA, COD_PED_COMPRA, COD_PED_INSPEC, COD_ENT_ENTR, NBR_UNIDADES)
            VALUES
                (@IdTarea,
                 N'PV-' + CAST(@Contador AS VARCHAR(6)),
                 N'PC-' + CAST(@Contador AS VARCHAR(6)),
                 N'PI-' + CAST(@Contador AS VARCHAR(6)),
                 N'EE-' + CAST(@Contador AS VARCHAR(6)),
                 CAST(1 + (@Contador % 20) AS DECIMAL(18, 2)));

            IF @Contador % 2 = 0
            BEGIN
                INSERT INTO dbo.TBL_DETALLETAREA
                    (COD_SEQ_TAR, COD_PED_VENTA, COD_PED_COMPRA, COD_PED_INSPEC, COD_ENT_ENTR, NBR_UNIDADES)
                VALUES
                    (@IdTarea,
                     N'PV-' + CAST(@Contador AS VARCHAR(6)) + N'B',
                     N'PC-' + CAST(@Contador AS VARCHAR(6)) + N'B',
                     N'PI-' + CAST(@Contador AS VARCHAR(6)) + N'B',
                     N'EE-' + CAST(@Contador AS VARCHAR(6)) + N'B',
                     CAST(1 + (@Contador % 15) AS DECIMAL(18, 2)));
            END

            -- Evento 1: alta en "Registrado" (igual que Registrar Tarea)
            INSERT INTO dbo.TBL_CONTROL (COD_SEQ_TAR, COD_SEQ_EST, FEC_ALTA)
            VALUES (@IdTarea, @IdEstRegistrado, CAST(@FecReg AS DATETIME2));

            -- Evento 2: asignación al usuario de este lote (igual que
            -- Pool de Tareas > Asignar tarea), pasa a "En proceso"
            INSERT INTO dbo.TBL_CONTROL (COD_SEQ_TAR, COD_SEQ_USER, COD_SEQ_EST, FEC_ALTA)
            VALUES (@IdTarea, @IdUsuario, @IdEstEnProceso, DATEADD(day, 1, CAST(@FecReg AS DATETIME2)));

            -- Combinación según la posición (1..10) dentro de las
            -- tareas de este usuario:
            --   1-3  -> se queda en "En proceso"
            --   4-5  -> pasa a "Pendiente" (bloqueada), con motivo
            --   6-9  -> pasa a "Facturado"
            --   10   -> pasa a "Cancelado"
            IF @IdxTarea BETWEEN 4 AND 5
            BEGIN
                INSERT INTO dbo.TBL_CONTROL (COD_SEQ_TAR, COD_SEQ_USER, COD_SEQ_EST, COD_SEQ_MOTIVO, FEC_ALTA)
                VALUES (@IdTarea, @IdUsuario, @IdEstPendiente, @IdMotivo, DATEADD(day, 2, CAST(@FecReg AS DATETIME2)));
            END
            ELSE IF @IdxTarea BETWEEN 6 AND 9
            BEGIN
                SET @ImporteFactura = CAST(@ImporteEstimado * (0.90 + ((@Contador % 4) * 0.05)) AS DECIMAL(18, 2));

                INSERT INTO dbo.TBL_FACTURADO
                    (COD_ENT_SAL, FEC_FACT, COD_FACT, IMP_FACT, COD_SEQ_MON, COD_SEQ_TPF)
                VALUES
                    (N'ES-' + CAST(@Contador AS VARCHAR(6)),
                     DATEADD(day, 5, @FecReg),
                     N'FAC-PRUEBA-' + RIGHT('000' + CAST(@Contador AS VARCHAR(3)), 3),
                     @ImporteFactura,
                     @IdDivisa,
                     @IdTipoFactura);

                SET @IdFacturado = SCOPE_IDENTITY();

                INSERT INTO dbo.TBL_CONTROL (COD_SEQ_TAR, COD_SEQ_USER, COD_SEQ_FAC, COD_SEQ_EST, FEC_ALTA)
                VALUES (@IdTarea, @IdUsuario, @IdFacturado, @IdEstFacturado, DATEADD(day, 5, CAST(@FecReg AS DATETIME2)));
            END
            ELSE IF @IdxTarea = 10
            BEGIN
                INSERT INTO dbo.TBL_CONTROL (COD_SEQ_TAR, COD_SEQ_USER, COD_SEQ_EST, FEC_ALTA)
                VALUES (@IdTarea, @IdUsuario, @IdEstCancelado, DATEADD(day, 2, CAST(@FecReg AS DATETIME2)));
            END
            -- (1-3: sin evento adicional, se quedan en "En proceso")

            SET @IdxTarea = @IdxTarea + 1;
        END

        SET @IdxUsuario = @IdxUsuario + 1;
    END

    COMMIT TRANSACTION;

    PRINT N'Datos de prueba cargados: ' + CAST(@Contador AS VARCHAR(10)) +
          N' tareas (10 por cada uno de los ' + CAST(@NumUsuarios AS VARCHAR(10)) + N' usuarios activos).';

END TRY
BEGIN CATCH

    ROLLBACK TRANSACTION;

    PRINT N'Carga de datos de prueba cancelada, no se ha insertado nada. Error: ' + ERROR_MESSAGE();

    THROW;

END CATCH
GO
