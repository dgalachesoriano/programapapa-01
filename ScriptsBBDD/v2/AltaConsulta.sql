/*
    AltaConsulta.sql (modelo v2)
    ----------------------------------------------------------------
    Plantilla para dar de alta, o modificar, una consulta de
    TBL_CONSULTAS (pantalla Datos > Consultas SQL). No hay pantalla de
    mantenimiento en la aplicación: las consultas se gestionan con
    este script, directamente en la BBDD.

    Uso:
      1. Rellena las cuatro variables del bloque "DATOS DE LA
         CONSULTA".
      2. Ejecuta el script completo.

    Si ya existe una consulta con el mismo NOM_CONSULTA se ACTUALIZA
    (descripción, texto y activo); si no, se INSERTA. Así el mismo
    script sirve para corregir una consulta ya dada de alta.

    Para desactivar una consulta sin borrarla, vuelve a lanzarlo con
    su nombre y @XTI_ACTIVO = 'N' (deja de aparecer en pantalla).

    Reglas para @TXT_CONSULTA:
      - Las comillas simples dentro de la consulta se escriben
        DOBLES (''), igual que en cualquier literal de SQL Server.
        Ejemplo: WHERE DES_ESTADO = ''Facturado''
      - Máximo 8000 caracteres (el script lo comprueba).
      - Parámetros: escribe :Nombre donde quieras que el usuario
        introduzca un valor (p. ej. :FechaDesde). La pantalla
        mostrará una etiqueta y una caja de texto por cada parámetro
        distinto; si el mismo :Nombre aparece varias veces se pide
        una sola vez. No se tienen en cuenta los ":" dentro de
        literales ('10:30') ni de comentarios.
      - Los valores se envían como texto. Si la caja se deja vacía
        el parámetro vale NULL, lo que permite filtros opcionales:
            AND (:Usuario IS NULL OR NOMBRE_USUARIO = :Usuario)
      - Fechas: para no depender del idioma del servidor, conviértelas
        explícitamente indicando el formato que se va a teclear:
            FEC_FACT >= CONVERT(DATE, :FechaDesde, 103)   -- dd/mm/aaaa
*/

USE GestionFacturas;
GO

DECLARE @NOM_CONSULTA NVARCHAR(100);
DECLARE @DES_CONSULTA NVARCHAR(500);
DECLARE @TXT_CONSULTA VARCHAR(MAX);
DECLARE @XTI_ACTIVO   CHAR(1);

-- ========================================================
-- DATOS DE LA CONSULTA (editar aquí)
-- ========================================================
SET @NOM_CONSULTA = N'Tareas facturadas entre fechas';

SET @DES_CONSULTA = N'Tareas en estado Facturado cuya fecha de factura está '
                  + N'entre las dos fechas indicadas (dd/mm/aaaa). '
                  + N'Usuario opcional: déjalo vacío para ver todos.';

SET @TXT_CONSULTA = '
SELECT
    DES_DOC,
    DES_ORG_VENTAS,
    DES_PROYECTO,
    DES_SEGMENTO,
    NOMBRE_USUARIO,
    COD_FACT,
    FEC_FACT,
    IMP_FACT,
    COD_DIV_FAC
FROM dbo.VW_TAREAS_ESTADO_ACTUAL
WHERE DES_ESTADO = ''Facturado''
    AND FEC_FACT >= CONVERT(DATE, :FechaDesde, 103)
    AND FEC_FACT <= CONVERT(DATE, :FechaHasta, 103)
    AND (:Usuario IS NULL OR NOMBRE_USUARIO = :Usuario)
ORDER BY FEC_FACT, DES_DOC';

SET @XTI_ACTIVO = 'S';
-- ========================================================

-- RAISERROR solo admite constantes o variables como argumentos, de
-- ahí la variable intermedia en vez de pasarle LEN(...) directamente.
DECLARE @LONGITUD INT = LEN(@TXT_CONSULTA);

IF @LONGITUD > 8000
BEGIN
    RAISERROR(N'La consulta tiene %d caracteres; el máximo es 8000.', 16, 1, @LONGITUD);
    RETURN;
END

IF EXISTS (SELECT 1 FROM dbo.TBL_CONSULTAS WHERE NOM_CONSULTA = @NOM_CONSULTA)
BEGIN
    UPDATE dbo.TBL_CONSULTAS
    SET DES_CONSULTA = @DES_CONSULTA,
        TXT_CONSULTA = @TXT_CONSULTA,
        XTI_ACTIVO   = @XTI_ACTIVO
    WHERE NOM_CONSULTA = @NOM_CONSULTA;

    PRINT N'Consulta actualizada: ' + @NOM_CONSULTA;
END
ELSE
BEGIN
    INSERT INTO dbo.TBL_CONSULTAS (NOM_CONSULTA, DES_CONSULTA, TXT_CONSULTA, XTI_ACTIVO)
    VALUES (@NOM_CONSULTA, @DES_CONSULTA, @TXT_CONSULTA, @XTI_ACTIVO);

    PRINT N'Consulta dada de alta: ' + @NOM_CONSULTA;
END
GO
