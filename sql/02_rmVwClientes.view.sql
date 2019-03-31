IF (OBJECT_ID ('dbo.vwRmClientes', 'V') IS NULL)
   exec('create view dbo.vwRmClientes as SELECT 1 as t');
go

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW dbo.vwRmClientes
--Propósito. Obtiene datos de clientes
--30/03/19 JCF Creación
AS
SELECT	rtrim(ms.custnmbr) custnmbr, rtrim(ms.custname) custname, rtrim(ms.txrgnnum) txrgnnum, ms.inactive, RTRiM(ms.TAXEXMT1) InscricaoMunicipalTomador, RTRiM(ms.TAXEXMT2) InscricaoEstadualTomador,
		RTRiM(ms.ADDRESS1) ADDRESS1, RTRiM(ms.ADDRESS2) ADDRESS2, RTRiM(ms.ADDRESS3) ADDRESS3,	RTRIM(ms.CITY) CITY, RTRIM(ms.STATE) [STATE], RTRIM(ms.ZIP) ZIP,
		isnull(aw.RESP_TYPE, 'DFL') RESP_TYPE
FROM dbo.rm00101 ms
left JOIN dbo.AWLI_RM00101 aw
	ON ms.custnmbr = aw.CUSTNMBR

GO

go
IF (@@Error = 0) PRINT 'Creación exitosa de: vwRmClientes'
ELSE PRINT 'Error en la creación de: vwRmClientes'
GO



