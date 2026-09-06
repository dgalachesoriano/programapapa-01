/*
    EjecutarTodo.sql
    ----------------------------------------------------------------
    Lanzador de conveniencia: ejecuta los tres scripts del modelo de
    datos en orden usando SQLCMD (:r), tal y como se ejecutarían a
    mano uno a uno.

    Requiere lanzarlo en modo SQLCMD (en SSMS: Consulta > Modo
    SQLCMD). Si tu cliente no soporta SQLCMD, ejecuta manualmente,
    en este orden:
        1. 00_CrearBaseDatos.sql
        2. 01_CrearTablas.sql
        3. 02_DatosIniciales.sql
*/

:r .\00_CrearBaseDatos.sql
:r .\01_CrearTablas.sql
:r .\02_DatosIniciales.sql
