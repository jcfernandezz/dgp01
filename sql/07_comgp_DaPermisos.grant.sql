--Integraciones GP
--Propósito. Rol que da accesos a objetos de integraciones GP
--Requisitos. Ejecutar en la bd intermedia
--06/03/18 JCF Creación
--
-----------------------------------------------------------------------------------
--use	tst12	--pro12 --
--go
IF DATABASE_PRINCIPAL_ID('rol_integracionesGP') IS NULL
	create role rol_integracionesGP;
grant select on dbo.comgp_fnDocStatusPreCondiciones to rol_integracionesGP;
grant select on dbo.vwRmClientes to rol_integracionesGP;

go
