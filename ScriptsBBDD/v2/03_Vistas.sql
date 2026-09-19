/*
    03_Vistas.sql (modelo v2)
    ----------------------------------------------------------------
    Como TBL_CONTROL es un histórico (una fila por evento, no una
    fila por tarea — ver nota 4 de 01_CrearTablas.sql), casi toda
    pantalla necesita "el estado actual de cada tarea", es decir, su
    fila más reciente en TBL_CONTROL. Esta vista centraliza esa
    consulta para no repetirla (con CROSS APPLY + TOP 1) en cada
    pantalla o repositorio.

    Pensada para:
      - Pool de Tareas: rejilla superior (tarea + estado + usuario +
        motivo de bloqueo), con filtro por estado/fechas/usuario.
      - Pantalla de Facturación: selección de tareas pendientes de
        facturar (DES_ESTADO = 'EN_PROCESO', por ejemplo).
      - Pantalla de Detalle de Tarea (FrmDetalleTarea): además de lo
        anterior, necesita los campos de TBL_FACTURADO/TBL_DIVISAS/
        TBL_TIPFACTURAS (COD_ENT_SAL, COD_SEQ_MON/COD_DIV,
        COD_SEQ_TPF/DES_TIP_FACT) para poder mostrar y editar los
        datos de facturación de la tarea, no solo el importe.
*/

USE GestionFacturas;
GO

CREATE OR ALTER VIEW dbo.VW_TAREAS_ESTADO_ACTUAL
AS
SELECT
    T.ID                    AS ID_TAREA,
    T.DES_DOC,
    T.FEC_ENT_CAL,
    T.FEC_REG,
    T.DES_ORG_VENTAS,
    T.IMP_ESTIMADO,

    T.COD_SEQ_PRY,
    PRY.DES_PROYECTO,

    T.COD_SEQ_SEG,
    SEG.DES_SEGMENTO,

    C.ID                    AS ID_CONTROL,
    C.FEC_ALTA              AS FEC_ULTIMO_CAMBIO,

    C.COD_SEQ_USER,
    USR.NOMBRE              AS NOMBRE_USUARIO,

    C.COD_SEQ_EST,
    EST.DES_ESTADO,

    C.COD_SEQ_MOTIVO,
    MOT.DES_MOTIVO,

    C.COD_SEQ_FAC,
    FAC.COD_ENT_SAL,
    FAC.FEC_FACT,
    FAC.COD_FACT,
    FAC.IMP_FACT,
    FAC.COD_SEQ_MON,
    DIV.COD_DIV,
    FAC.COD_SEQ_TPF,
    TPF.DES_TIP_FACT

FROM dbo.TBL_TAREAS AS T

CROSS APPLY (
    SELECT TOP (1) CC.*
    FROM dbo.TBL_CONTROL AS CC
    WHERE CC.COD_SEQ_TAR = T.ID
    ORDER BY CC.FEC_ALTA DESC, CC.ID DESC
) AS C

LEFT JOIN dbo.TBL_PROYECTOS AS PRY ON PRY.ID = T.COD_SEQ_PRY
LEFT JOIN dbo.TBL_SEGMENTOS AS SEG ON SEG.ID = T.COD_SEQ_SEG
LEFT JOIN dbo.TBL_USUARIOS  AS USR ON USR.ID = C.COD_SEQ_USER
LEFT JOIN dbo.TBL_ESTADOS_TAREA AS EST ON EST.ID = C.COD_SEQ_EST
LEFT JOIN dbo.TBL_MOTIVO    AS MOT ON MOT.ID = C.COD_SEQ_MOTIVO
LEFT JOIN dbo.TBL_FACTURADO AS FAC ON FAC.ID = C.COD_SEQ_FAC
LEFT JOIN dbo.TBL_DIVISAS   AS DIV ON DIV.ID = FAC.COD_SEQ_MON
LEFT JOIN dbo.TBL_TIPFACTURAS AS TPF ON TPF.ID = FAC.COD_SEQ_TPF;
GO
