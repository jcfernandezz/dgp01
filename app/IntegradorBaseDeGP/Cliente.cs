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
        private string _colClienteAddress1;
        private string _colClienteAddress2;
        private string _colClienteAddress3;
        private string _colClienteCiudad;
        private string _colClienteEstado;
        private string _colClienteZipCode;
        private string _colClienteEmail;

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

        public int ColClienteAddress1 {
            get {
                int c;
                if (!int.TryParse(_colClienteAddress1, out c))
                    throw new NullReferenceException("No ha definido la columna de la dirección 1 del cliente (facturaSopCa.cliDireccion1). Revise el archivo de configuración de la aplicación. ");
                return c;
            }
            set => _colClienteAddress1 = value.ToString();
        }
        public int ColClienteAddress2 {
            get
            {
                int c;
                if (!int.TryParse(_colClienteAddress2, out c))
                    throw new NullReferenceException("No ha definido la columna de la dirección 2 del cliente (facturaSopCa.cliDireccion2). Revise el archivo de configuración de la aplicación. ");
                return c;
            }
            set => _colClienteAddress2 = value.ToString();
        }
        public int ColClienteAddress3 {
            get
            {
                int c;
                if (!int.TryParse(_colClienteAddress3, out c))
                    throw new NullReferenceException("No ha definido la columna de la dirección 3 del cliente (facturaSopCa.cliDireccion3). Revise el archivo de configuración de la aplicación. ");
                return c;
            }
            set => _colClienteAddress3 = value.ToString();
        }
        public int ColClienteCiudad {
            get
            {
                int c;
                if (!int.TryParse(_colClienteCiudad, out c))
                    throw new NullReferenceException("No ha definido la columna de la ciudad del cliente (facturaSopCa.cliCiudad). Revise el archivo de configuración de la aplicación. ");
                return c;
            }
            set => _colClienteCiudad = value.ToString();
        }
        public int ColClienteEstado {
            get
            {
                int c;
                if (!int.TryParse(_colClienteEstado, out c))
                    throw new NullReferenceException("No ha definido la columna del estado del cliente (facturaSopCa.cliEstado). Revise el archivo de configuración de la aplicación. ");
                return c;
            }
            set => _colClienteEstado = value.ToString();
        }
        public int ColClienteZipCode {
            get
            {
                int c;
                if (!int.TryParse(_colClienteZipCode, out c))
                    throw new NullReferenceException("No ha definido la columna código postal del cliente (facturaSopCa.cliZipCode). Revise el archivo de configuración de la aplicación. ");
                return c;
            }
            set => _colClienteZipCode = value.ToString();
        }
        public int ColClienteEmail {
            get
            {
                int c;
                if (!int.TryParse(_colClienteEmail, out c))
                    throw new NullReferenceException("No ha definido la columna email del cliente (facturaSopCa.cliEmail). Revise el archivo de configuración de la aplicación. ");
                return c;
            }
            set => _colClienteEmail = value.ToString(); }

        public Cliente(string DatosConexionDB, string cFacturaSopTXRGNNUM, string cFacturaSopCUSTNAME, string ClienteDefaultCUSTCLAS)
        {
            _DatosConexionDB = DatosConexionDB;
            _ClienteDefaultCUSTCLAS = ClienteDefaultCUSTCLAS;

            if (!int.TryParse(cFacturaSopTXRGNNUM, out _colIdImpuestoCliente))
                throw new NullReferenceException("No ha definido la columna del Id de impuestos del cliente (facturaSopCa.TXRGNNUM). Revise el archivo de configuración de la aplicación. ");
            if (!int.TryParse(cFacturaSopCUSTNAME, out _colCUSTNAME))
                throw new NullReferenceException("No ha definido la columna del nombre del cliente (facturaSopCa.CUSTNAME). Revise el archivo de configuración de la aplicación. ");

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

                if (!string.IsNullOrEmpty(_colClienteAddress1) && !_colClienteAddress1.Equals("0"))
                    _Customer.ADDRESS1 = hojaXl.Cells[fila, ColClienteAddress1].Value.ToString().Trim();

                if (!string.IsNullOrEmpty(_colClienteAddress2) && !_colClienteAddress2.Equals("0"))
                    _Customer.ADDRESS2 = hojaXl.Cells[fila, ColClienteAddress2].Value.ToString().Trim();

                if (!string.IsNullOrEmpty(_colClienteAddress3) && !_colClienteAddress3.Equals("0"))
                    _Customer.ADDRESS3 = hojaXl.Cells[fila, ColClienteAddress3].Value.ToString().Trim();

                if (!string.IsNullOrEmpty(_colClienteCiudad) && !_colClienteCiudad.Equals("0"))
                    _Customer.CITY = hojaXl.Cells[fila, ColClienteCiudad].Value.ToString().Trim();

                if (!string.IsNullOrEmpty(_colClienteEmail) && !_colClienteEmail.Equals("0"))
                    _Customer.ToEmail_Recipient = hojaXl.Cells[fila, ColClienteEmail].Value.ToString().Trim();

                if (!string.IsNullOrEmpty(_colClienteEstado) && !_colClienteEstado.Equals("0"))
                    _Customer.STATE= hojaXl.Cells[fila, ColClienteEstado].Value.ToString().Trim();

                if (!string.IsNullOrEmpty(_colClienteZipCode) && !_colClienteZipCode.Equals("0"))
                    _Customer.ZIPCODE = hojaXl.Cells[fila, ColClienteZipCode].Value.ToString().Trim();

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
