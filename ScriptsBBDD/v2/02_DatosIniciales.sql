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
    usuario) son los catálogos reales usados en el entorno de trabajo
    actual (volcado desde la base de datos el 2026-09-19). Ajústalos
    si el negocio los cambia.
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
-- TBL_PROYECTOS
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_PROYECTOS WHERE DES_PROYECTO = N'Proyecto1')
    INSERT INTO dbo.TBL_PROYECTOS (DES_PROYECTO) VALUES (N'Proyecto1');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_PROYECTOS WHERE DES_PROYECTO = N'Proyecto2')
    INSERT INTO dbo.TBL_PROYECTOS (DES_PROYECTO) VALUES (N'Proyecto2');

-- --------------------------------------------------------
-- TBL_SEGMENTOS
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_SEGMENTOS WHERE DES_SEGMENTO = N'01 - PIPING')
    INSERT INTO dbo.TBL_SEGMENTOS (DES_SEGMENTO) VALUES (N'01 - PIPING');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_SEGMENTOS WHERE DES_SEGMENTO = N'02 - VALVULAS')
    INSERT INTO dbo.TBL_SEGMENTOS (DES_SEGMENTO) VALUES (N'02 - VALVULAS');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_SEGMENTOS WHERE DES_SEGMENTO = N'03 - CABLES')
    INSERT INTO dbo.TBL_SEGMENTOS (DES_SEGMENTO) VALUES (N'03 - CABLES');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_SEGMENTOS WHERE DES_SEGMENTO = N'04 - ESTRUCTURAS')
    INSERT INTO dbo.TBL_SEGMENTOS (DES_SEGMENTO) VALUES (N'04 - ESTRUCTURAS');

-- --------------------------------------------------------
-- TBL_DIVISAS
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_DIVISAS WHERE COD_DIV = N'EUR')
    INSERT INTO dbo.TBL_DIVISAS (COD_DIV) VALUES (N'EUR');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_DIVISAS WHERE COD_DIV = N'USD')
    INSERT INTO dbo.TBL_DIVISAS (COD_DIV) VALUES (N'USD');

-- --------------------------------------------------------
-- TBL_TIPFACTURAS
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_TIPFACTURAS WHERE DES_TIP_FACT = N'Factura')
    INSERT INTO dbo.TBL_TIPFACTURAS (DES_TIP_FACT) VALUES (N'Factura');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_TIPFACTURAS WHERE DES_TIP_FACT = N'Abono')
    INSERT INTO dbo.TBL_TIPFACTURAS (DES_TIP_FACT) VALUES (N'Abono');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_TIPFACTURAS WHERE DES_TIP_FACT = N'Anticipo')
    INSERT INTO dbo.TBL_TIPFACTURAS (DES_TIP_FACT) VALUES (N'Anticipo');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_TIPFACTURAS WHERE DES_TIP_FACT = N'Retención')
    INSERT INTO dbo.TBL_TIPFACTURAS (DES_TIP_FACT) VALUES (N'Retención');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_TIPFACTURAS WHERE DES_TIP_FACT = N'Proforma')
    INSERT INTO dbo.TBL_TIPFACTURAS (DES_TIP_FACT) VALUES (N'Proforma');

-- --------------------------------------------------------
-- TBL_MOTIVO (usado al bloquear una tarea)
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Activador')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Activador');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Cliente')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Cliente');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Contabilidad')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Contabilidad');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Error SAP')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Error SAP');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Facturación')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Facturación');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Grabación')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Grabación');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Ofertador')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Ofertador');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Proveedor')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Proveedor');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_MOTIVO WHERE DES_MOTIVO = N'Transporte')
    INSERT INTO dbo.TBL_MOTIVO (DES_MOTIVO) VALUES (N'Transporte');

-- --------------------------------------------------------
-- TBL_USUARIOS
-- --------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM dbo.TBL_USUARIOS WHERE NOMBRE = N'JGalache')
    INSERT INTO dbo.TBL_USUARIOS (NOMBRE) VALUES (N'JGalache');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_USUARIOS WHERE NOMBRE = N'NLopez')
    INSERT INTO dbo.TBL_USUARIOS (NOMBRE) VALUES (N'NLopez');

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_USUARIOS WHERE NOMBRE = N'POlaechea')
    INSERT INTO dbo.TBL_USUARIOS (NOMBRE) VALUES (N'POlaechea');
GO
