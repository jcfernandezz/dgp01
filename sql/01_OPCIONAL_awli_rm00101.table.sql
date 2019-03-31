
IF not EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = OBJECT_ID(N'AWLI_RM00101') AND OBJECTPROPERTY(id,N'IsTable') = 1)
begin
	CREATE TABLE [dbo].AWLI_RM00101(
		CUSTNMBR [char](15) NOT NULL,
		RESP_TYPE char(3)
		)

end
go

