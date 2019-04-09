--Integraciones GP
--Propósito. Rol que da accesos a objetos de integraciones GP
--Requisitos. Ejecutar en la bd de la compañía
--09/04/19 JCF Creación

use gbra 
go

IF DATABASE_PRINCIPAL_ID('rol_integracionesGP') IS NULL
	create role rol_integracionesGP;

grant select on dbo.vwSopFacturasCabezaTH to rol_integracionesGP;
grant select on dbo.vwRmClientes to rol_integracionesGP;
grant insert, update, delete on dbo.sop10100 to rol_integracionesGP;
grant insert, update, delete on dbo.sop10200 to rol_integracionesGP;
grant insert, update, delete on dbo.sop10102 to rol_integracionesGP;
grant insert, update, delete on dbo.sop10106 to rol_integracionesGP;
grant insert, update, delete on dbo.sop10105 to rol_integracionesGP;
grant insert, update, delete on dbo.rm00101 to rol_integracionesGP;

