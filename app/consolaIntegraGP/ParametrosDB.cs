using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorDeGP;
using System.Xml;

namespace consolaIntegraGP
{
    public struct Empresa
    {
        private string idbd;
        private string nombreBd;
        private string metadataIntegra;
        private string metadataGP;
        private string metadataUIIntegra;

        public string Idbd
        {
            get
            {
                return idbd;
            }

            set
            {
                idbd = value;
            }
        }

        public string NombreBd
        {
            get
            {
                return nombreBd;
            }

            set
            {
                nombreBd = value;
            }
        }

        /// <summary>
        /// metadata de la bd Integra del servicio de integración
        /// </summary>
        public string MetadataIntegra
        {
            get
            {
                return metadataIntegra;
            }

            set
            {
                metadataIntegra = value;
            }
        }

        /// <summary>
        /// metadata de la bd GP del servicio de integración
        /// </summary>
        public string MetadataGP
        {
            get
            {
                return metadataGP;
            }

            set
            {
                metadataGP = value;
            }
        }

        /// <summary>
        /// metadata de la bd Integra de la aplicación winForms
        /// </summary>
        public string MetadataUIIntegra
        {
            get
            {
                return metadataUIIntegra;
            }

            set
            {
                metadataUIIntegra = value;
            }
        }
    }

    public class ParametrosDB:IParametrosDB
    {
        private List<Empresa> _empresas;
        private string nombreArchivoParametros = "ParametrosConsolaIGP.xml";
        private string targetGPDB = "";
        private string _servidor = "";
        private string _seguridadIntegrada = "0";
        private string _usuarioSql = "";
        private string _passwordSql = "";
        private string connStringSourceEFUI = string.Empty;
        private string connectionStringSourceEF = string.Empty;
        private string connectionStringTargetEF = string.Empty;
        private string connStringSource = string.Empty;
        private string connStringTarget = string.Empty;
        private string formatoFechaDB;
        private string rutaLog;
        Dictionary<string, string> idsDocumento;

        private int _facturaSopDeReqShipDate;
        private int _facturaSopDeActlShipDate;
        private int _facturaSopDeCmmttext;
        private string _incluirUserDef;
        private string _usrtab01_predetValue;
        private string _usrtab02_predetValue;

        private string _intEstadoCompletado;
        private string _intEstadosPermitidos;
        private string _emite;
        private string _envia;
        private string _imprime;
        private string _publica;
        private string _zip;
        private string _anula;

        public ParametrosDB()
        {
            //try
            //{
                XmlDocument listaParametros = new XmlDocument();
                listaParametros.Load(new XmlTextReader(nombreArchivoParametros));

                this._servidor = listaParametros.DocumentElement.SelectSingleNode("/listaParametros/servidor/text()").Value;
                this.DefaultDB = listaParametros.DocumentElement.SelectSingleNode("/listaParametros/servidor").Attributes["defaultDB"].Value;
                this._seguridadIntegrada = listaParametros.DocumentElement.SelectSingleNode("/listaParametros/seguridadIntegrada/text()").Value;
                this._usuarioSql = listaParametros.DocumentElement.SelectSingleNode("/listaParametros/usuariosql/text()").Value;
                this._passwordSql = listaParametros.DocumentElement.SelectSingleNode("/listaParametros/passwordsql/text()").Value;

                XmlNodeList empresasNodes = listaParametros.DocumentElement.SelectNodes("/listaParametros/compannia");

                this._empresas = new List<Empresa>();
                foreach (XmlNode empresaNode in empresasNodes)
                {
                    this._empresas.Add(new Empresa()
                    {
                        Idbd = empresaNode.Attributes["bd"].Value,
                        NombreBd = empresaNode.Attributes["nombre"].Value,
                        MetadataIntegra = empresaNode.Attributes["metadataIntegra"].Value,
                        MetadataGP = empresaNode.Attributes["metadataGP"].Value,
                        MetadataUIIntegra = empresaNode.Attributes["metadataUI"].Value
                    });
                }

            //}
            //catch (Exception eprm)
            //{
            //    ultimoMensaje = "Contacte al administrador. No se pudo obtener la configuración general. [Parametros()]" + eprm.Message;
            //}
        }

        public void GetParametros(int idxEmpresa)
        {
            string IdCompannia = this._empresas[idxEmpresa].Idbd;
                XmlDocument listaParametros = new XmlDocument();
                listaParametros.Load(new XmlTextReader(nombreArchivoParametros));
                XmlNode elemento = listaParametros.DocumentElement;


            FormatoFechaDB = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/formatoFechaDB/text()").Value;
            targetGPDB = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/TargetGPDB/text()").Value;
            if (seguridadIntegrada)
            {
                connectionStringSourceEF = this._empresas[idxEmpresa].MetadataIntegra + "provider connection string='data source=" + _servidor + "; initial catalog = " + IdCompannia + "; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework'";
                connectionStringTargetEF = this._empresas[idxEmpresa].MetadataGP + "provider connection string='data source=" + _servidor + "; initial catalog = " + targetGPDB + "; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework'";
                connStringSource = "Initial Catalog=" + IdCompannia + ";Data Source=" + _servidor + ";Integrated Security=SSPI";
                connStringTarget = "Initial Catalog=" + targetGPDB + ";Data Source=" + _servidor + ";Integrated Security=SSPI";
                connStringSourceEFUI = this._empresas[idxEmpresa].MetadataUIIntegra + "provider connection string='data source=" + _servidor + "; initial catalog = " + IdCompannia + "; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework'"; 
            }
            else
            {
                connectionStringSourceEF = this._empresas[idxEmpresa].MetadataIntegra + "provider connection string='data source=" + _servidor + ";initial catalog=" + IdCompannia + ";user id=" + _usuarioSql + ";Password=" + _passwordSql + ";integrated security=False; MultipleActiveResultSets=True;App=EntityFramework'";
                connectionStringTargetEF = this._empresas[idxEmpresa].MetadataGP + "provider connection string='data source=" + _servidor + ";initial catalog=" + targetGPDB + ";user id=" + _usuarioSql + ";Password=" + _passwordSql + ";integrated security=False; MultipleActiveResultSets=True;App=EntityFramework'";
                connStringSource = "User ID=" + _usuarioSql + ";Password=" + _passwordSql + ";Initial Catalog=" + IdCompannia + ";Data Source=" + _servidor;
                connStringTarget = "User ID=" + _usuarioSql + ";Password=" + _passwordSql + ";Initial Catalog=" + targetGPDB + ";Data Source=" + _servidor;
                connStringSourceEFUI = this._empresas[idxEmpresa].MetadataUIIntegra + "provider connection string='data source=" + _servidor + "; initial catalog = " + IdCompannia + ";user id=" + _usuarioSql + ";Password=" + _passwordSql + ";integrated security=False; MultipleActiveResultSets=True;App=EntityFramework'";
            }

            RutaLog = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/RutaLog/text()").Value;
            XmlNodeList idsDocumentoSOP = listaParametros.DocumentElement.SelectNodes("/listaParametros/compannia[@bd='" + IdCompannia + "']/idsDocumentoSOP");
            IdsDocumento = new Dictionary<string, string>();
            foreach (XmlNode n in idsDocumentoSOP)
            {
                try
                {
                    IdsDocumento.Add(n.Attributes["idAriane"].Value, n.Attributes["idGP"].Value);
                }
                catch
                { }
            }

            _facturaSopDeReqShipDate = int.Parse(elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/facturaSopDe/ReqShipDate/text()").Value);
            _facturaSopDeActlShipDate = int.Parse(elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/facturaSopDe/ActlShipDate/text()").Value);
            _facturaSopDeCmmttext = int.Parse(elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/facturaSopDe/CMMTTEXT/text()").Value);

            _incluirUserDef = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/sopUserDefined/incluirUserDef/text()").Value;
            _usrtab01_predetValue = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/sopUserDefined/usrtab01_predetValue/text()").Value;
            _usrtab02_predetValue = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/sopUserDefined/usrtab02_predetValue/text()").Value;

            _intEstadoCompletado = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/intEstadoCompletado/text()").Value;
            _intEstadosPermitidos = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/intEstadosPermitidos/text()").Value;
            _emite = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/emite/text()").Value;
            _envia = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/envia/text()").Value;
            _imprime = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/imprime/text()").Value;
            _publica = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/publica/text()").Value;
            _zip = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/zip/text()").Value;
            _anula = elemento.SelectSingleNode("//compannia[@bd='" + IdCompannia + "']/anula/text()").Value;


        }

        public string servidor
        {
            get { return _servidor; }
            set { _servidor = value; }
        }

        public bool seguridadIntegrada
        {
            get
            {
                return _seguridadIntegrada.Equals("1");
            }
            set
            {
                if (value)
                    _seguridadIntegrada = "1";
                else
                    _seguridadIntegrada = "0";
            }
        }

        public string usuarioSql
        {
            get { return _usuarioSql; }
            set { _usuarioSql = value; }
        }

        public string passwordSql
        {
            get { return _passwordSql; }
            set { _passwordSql = value; }
        }


        public string TargetGPDB
        {
            get
            {
                return targetGPDB;
            }
            set { targetGPDB = value; }

        }


        public List<Empresa> Empresas
        {
            get
            {
                return _empresas;
            }

            set
            {
                _empresas = value;
            }
        }

        public string ConnectionStringSourceEF
        {
            get
            {
                return connectionStringSourceEF;
            }

            set
            {
                connectionStringSourceEF = value;
            }
        }

        public string ConnectionStringTargetEF
        {
            get
            {
                return connectionStringTargetEF;
            }

            set
            {
                connectionStringTargetEF = value;
            }
        }
        public string DefaultDB { get; private set; }

        public string ConnStringSource
        {
            get
            {
                return connStringSource;
            }

            set
            {
                connStringSource = value;
            }
        }

        public string ConnStringTarget
        {
            get
            {
                return connStringTarget;
            }

            set
            {
                connStringTarget = value;
            }
        }

        public string FormatoFechaDB
        {
            get
            {
                return formatoFechaDB;
            }

            set
            {
                formatoFechaDB = value;
            }
        }

        public string RutaLog
        {
            get
            {
                return rutaLog;
            }

            set
            {
                rutaLog = value;
            }
        }

        public string ConnStringSourceEFUI
        {
            get
            {
                return connStringSourceEFUI;
            }

            set
            {
                connStringSourceEFUI = value;
            }
        }

        public Dictionary<string, string> IdsDocumento
        {
            get
            {
                return idsDocumento;
            }

            set
            {
                idsDocumento = value;
            }
        }
        public int FacturaSopDeReqShipDate { get => _facturaSopDeReqShipDate; set => _facturaSopDeReqShipDate = value; }
        public int FacturaSopDeActlShipDate { get => _facturaSopDeActlShipDate; set => _facturaSopDeActlShipDate = value; }
        public int FacturaSopDeCmmttext { get => _facturaSopDeCmmttext; set => _facturaSopDeCmmttext = value; }


        public int intEstadoCompletado { get => int.Parse(_intEstadoCompletado); set => _intEstadoCompletado = value.ToString(); }
        public int intEstadosPermitidos { get => int.Parse(_intEstadosPermitidos); set => _intEstadosPermitidos = value.ToString(); }
        public bool emite { get => _emite.Equals("1"); set => _emite = value.ToString(); }
        public bool envia { get => _envia.Equals("1"); set => _envia = value.ToString(); }
        public bool imprime { get => _imprime.Equals("1"); set => _imprime = value.ToString(); }
        public bool publica { get => _publica.Equals("1"); set => _publica = value.ToString(); }
        public bool zip { get => _zip.Equals("1"); set => _zip = value.ToString(); }
        public bool anula { get => _anula.Equals("1"); set => _anula = value.ToString(); }

        public bool IncluirUserDef { get => _incluirUserDef.ToLower().Equals("true") || _incluirUserDef.Equals("1"); set => _incluirUserDef = value.ToString(); }
        public string Usrtab01_predetValue { get => _usrtab01_predetValue; set => _usrtab01_predetValue = value; }
        public string Usrtab02_predetValue { get => _usrtab02_predetValue; set => _usrtab02_predetValue = value; }

    }

}

