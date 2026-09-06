/*
    00_CrearBaseDatos.sql
    ----------------------------------------------------------------
    Crea la base de datos de la aplicación GestionFacturas si todavía
    no existe.

    NOTA: App.config trae de fábrica "Database=master" en la cadena
    de conexión (solo como placeholder de desarrollo). Tras ejecutar
    este script, actualiza App.config para apuntar a la base de datos
    real, por ejemplo:

        Server=localhost\SQLEXPRESS02;Database=GestionFacturas;Trusted_Connection=True;
*/

IF DB_ID(N'GestionFacturas') IS NULL
BEGIN
    CREATE DATABASE GestionFacturas;
END
GO
