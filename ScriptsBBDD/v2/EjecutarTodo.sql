/*
    EjecutarTodo.sql (modelo v2)
    ----------------------------------------------------------------
    Lanzador de conveniencia: ejecuta los scripts del modelo v2 en
    orden usando SQLCMD (:r).

    Requiere lanzarlo en modo SQLCMD (en SSMS: Consulta > Modo
    SQLCMD). Si tu cliente no soporta SQLCMD, ejecuta manualmente, en
    este orden:
        1. 01_CrearTablas.sql
        2. 02_DatosIniciales.sql
        3. 03_Vistas.sql

    Nota: la base de datos GestionFacturas debe existir ya (ver
    ../00_CrearBaseDatos.sql).
*/

:r .\01_CrearTablas.sql
:r .\02_DatosIniciales.sql
:r .\03_Vistas.sql
