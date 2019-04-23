using IntegradorDeGP.BLL;
using Microsoft.Dynamics.GP.eConnect.Serialization;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorDeGP
{
    public class Cliente
    {
        //public int iError = 0;
        //public string sMensaje = "";
        private string _DatosConexionDB;
        private int _colIdImpuestoCliente = 0;
        private int _colCUSTNAME = 0;
        private string _ClienteDefaultCUSTCLAS;
        //private Parametros _param;
        private taUpdateCreateCustomerRcd _Customer;
        private RMCustomerMasterType _CustomerType;
        private RMCustomerMasterType[] _arrCustomerType;
        private int _colClienteAddress1;

        public RMCustomerMasterType[] ArrCustomerType
        {
            get
            {
                return _arrCustomerType;
            }

            set
            {
                _arrCustomerType = value;
            }
        }

        public Cliente(string DatosConexionDB, string cFacturaSopTXRGNNUM, string cFacturaSopCUSTNAME, string ClienteDefaultCUSTCLAS, string cClienteAddress1)
        {
            _DatosConexionDB = DatosConexionDB;
            _ClienteDefaultCUSTCLAS = ClienteDefaultCUSTCLAS;

            if (!int.TryParse(cFacturaSopTXRGNNUM, out _colIdImpuestoCliente))
                throw new NullReferenceException("No ha definido la columna del Id de impuestos del cliente (facturaSopCa.TXRGNNUM). Revise el archivo de configuración de la aplicación. ");
            if (!int.TryParse(cFacturaSopCUSTNAME, out _colCUSTNAME))
                throw new NullReferenceException("No ha definido la columna del nombre del cliente (facturaSopCa.CUSTNAME). Revise el archivo de configuración de la aplicación. ");
            if (!int.TryParse(cClienteAddress1, out _colClienteAddress1))
                throw new NullReferenceException("No ha definido la columna de la dirección 1 del cliente (facturaSopCa.direccion1). Revise el archivo de configuración de la aplicación. ");

        }

        private string existeIdImpuestoCliente(string txrgnnum)
        {
            int n = 0;
            string cliente = string.Empty;
            using (BLL.DynamicsGPEntities gp = new BLL.DynamicsGPEntities(_DatosConexionDB))
            {
                var c = gp.vwRmClientes.Where(w => w.txrgnnum.Equals(txrgnnum.Trim()) && w.inactive == 0)
                                    .Select(s => s.custnmbr.Trim());
                n = c.Count();
                //foreach (var r in c)
                //    cliente = r.custnmbr;
                cliente = c.FirstOrDefault();
            }

            return (cliente);
        }

        /// <summary>
        /// Revisa datos del cliente.
        /// </summary>
        /// <param name="hojaXl"></param>
        /// <param name="filaXl"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public void validaDatosDeIngreso(ExcelWorksheet hojaXl, int filaXl)
        {
            if (hojaXl.Cells[filaXl, _colIdImpuestoCliente].Value == null || hojaXl.Cells[filaXl, _colIdImpuestoCliente].Value.ToString().Equals(""))
            {
               throw new NullReferenceException( "El ID de impuesto está en blanco.");
            }
            if (hojaXl.Cells[filaXl, _colCUSTNAME].Value == null || hojaXl.Cells[filaXl, _colCUSTNAME].Value.ToString().Equals(""))
            {
                throw new NullReferenceException( "El nombre del cliente está en blanco. Ingrese un nombre en la columna Nombre del cliente.");
            }

        }

        public void armaClienteEconn(ExcelWorksheet hojaXl, int fila, string custnmbr)
        {
            try
            {
                _Customer = new taUpdateCreateCustomerRcd();
                _CustomerType = new RMCustomerMasterType();

                _Customer.CUSTNMBR = custnmbr == null? hojaXl.Cells[fila, _colIdImpuestoCliente].Value.ToString().Trim().Replace(".", String.Empty).Replace("-", String.Empty) : custnmbr;
                _Customer.CUSTNAME = hojaXl.Cells[fila, _colCUSTNAME].Value.ToString().Trim();
                _Customer.CUSTCLAS = _ClienteDefaultCUSTCLAS;
                _Customer.ADRSCODE = "MAIN";
                _Customer.ADDRESS1 = hojaXl.Cells[fila, _colClienteAddress1].Value.ToString().Trim();
                if (_colIdImpuestoCliente>0)
                    _Customer.TXRGNNUM = hojaXl.Cells[fila, _colIdImpuestoCliente].Value.ToString().Trim();

                _Customer.UpdateIfExists = 1;
                _Customer.UseCustomerClass = custnmbr == null ? Convert.ToInt16(1) : Convert.ToInt16(0);

                _CustomerType.taUpdateCreateCustomerRcd = _Customer;
                _arrCustomerType = new RMCustomerMasterType[] { _CustomerType};

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Crea el xml de un cliente a partir de una fila de datos en una hoja excel.
        /// </summary>
        /// <param name="hojaXl">Hoja excel</param>
        /// <param name="filaXl">Fila de la hoja excel a procesar</param>
        public void preparaClienteEconn(ExcelWorksheet hojaXl, int filaXl)
        {
                validaDatosDeIngreso(hojaXl, filaXl);
                string custnmbr = existeIdImpuestoCliente(hojaXl.Cells[filaXl, _colIdImpuestoCliente].Value.ToString().Trim());
                armaClienteEconn(hojaXl, filaXl, custnmbr);

                //if (!existeIdImpuestoCliente(hojaXl.Cells[filaXl, _colIdImpuestoCliente].Value.ToString().Trim()))
                //{
                //    armaClienteEconn(hojaXl, filaXl);
                //    integrar = true;
                //}
        }

    }
}
