using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;
using Microsoft.Dynamics.GP.eConnect;
using Microsoft.Dynamics.GP.eConnect.Serialization;
using System.Globalization;
using System.IO;

namespace IntegradorDeGP
{
    public class FacturaDeVentaSOP
    {
        int cantidadItemsFactura = 1;
        private string connStringEF;
        taSopHdrIvcInsert facturaSopCa;
        SOPTransactionType facturaSop;
        private int _iniciaNuevaFacturaEn;
        //TraceSource trace;
        //TextWriterTraceListener textListener;

        public SOPTransactionType FacturaSop
        {
            get
            {
                return facturaSop;
            }

            set
            {
                facturaSop = value;
            }
        }

        public int IniciaNuevaFacturaEn
        {
            get
            {
                return _iniciaNuevaFacturaEn;
            }

            set
            {
                _iniciaNuevaFacturaEn = value;
            }
        }

        public int CantidadItemsFactura { get => cantidadItemsFactura; set => cantidadItemsFactura = value; }

        public FacturaDeVentaSOP(string DatosConexionDB)
        {
            //Stream outputFile = File.Create(@"C:\GPDocIntegration\traceInterfazGP.txt");
            //textListener = new TextWriterTraceListener(outputFile);
            //trace = new TraceSource("trSource", SourceLevels.All);
            //trace.Listeners.Clear();
            //trace.Listeners.Add(textListener);
            //trace.TraceInformation("integra factura sop");

            connStringEF = DatosConexionDB;
            facturaSopCa = new taSopHdrIvcInsert();
            facturaSop = new SOPTransactionType();
        }

        public List<taSopLineIvcInsert_ItemsTaSopLineIvcInsert> armaFacturaCaEconn(ExcelWorksheet hojaXl, int fila, string sTimeStamp, IParametrosXL param)
        {
            //int idxFila = fila;
            try
            {
                String serie = string.Empty;
                string numFactura = string.Empty;
                string sopnumbe = string.Empty;
                int idxFila = CalculaFilaNuevaFactura(hojaXl, fila, param, out serie, out numFactura, out sopnumbe);
                cantidadItemsFactura = idxFila - fila;

                facturaSopCa.SOPNUMBE = numFactura;
                facturaSopCa.BACHNUMB = sTimeStamp;
                facturaSopCa.SOPTYPE = 3;
                facturaSopCa.DOCID = "SERIE " + serie;
                facturaSopCa.DOCDATE = DateTime.Parse(hojaXl.Cells[fila, param.FacturaSopDocdate].Value.ToString().Trim()).ToString(param.FormatoFechaXL);
                facturaSopCa.DUEDATE = DateTime.Parse(hojaXl.Cells[fila, param.FacturaSopDuedate].Value.ToString().Trim()).ToString(param.FormatoFechaXL);

                String custnmbr = hojaXl.Cells[fila, param.FacturaSopTXRGNNUM].Value == null ? "_enblanco" : hojaXl.Cells[fila, param.FacturaSopTXRGNNUM].Value.ToString().Trim();
                facturaSopCa.CUSTNMBR = getCustomer(custnmbr);
                facturaSopCa.CREATETAXES = 1;   //1:crear impuestos automáticamente
                facturaSopCa.DEFPRICING = 0;    //0:se debe indicar el precio unitario
                facturaSopCa.REFRENCE = "Carga automática";

                List<taSopLineIvcInsert_ItemsTaSopLineIvcInsert> listaDeItemsDeFactura = new List<taSopLineIvcInsert_ItemsTaSopLineIvcInsert>();

                var articuloDeFactura = CreaItemDeFactura(hojaXl, fila, param);
                listaDeItemsDeFactura.Add(articuloDeFactura);
                facturaSopCa.SUBTOTAL = articuloDeFactura.UNITPRCE;
                facturaSopCa.DOCAMNT = facturaSopCa.SUBTOTAL;

                if (param.FacturaSopDeUNITPRCE != 0)
                    for (int i = fila; i < fila + cantidadItemsFactura; i++)
                    {
                        taSopLineIvcInsert_ItemsTaSopLineIvcInsert facturaSopDe = CreaItemsFicticiosDeFactura(hojaXl, i, param);

                        listaDeItemsDeFactura.Add(facturaSopDe);
                    }

                return listaDeItemsDeFactura;
            }
            catch (FormatException fmt)
            {
                throw new FormatException("Formato incorrecto en la fila " + fila.ToString() + " [armaFacturaCaEconn]", fmt);
            }
            catch (OverflowException ovr)
            {
                throw new OverflowException("Monto demasiado grande en la fila " + fila.ToString() + " [armaFacturaCaEconn]", ovr);
            }
            //finally
            //{
            //    //trace.Flush();
            //    //trace.Close();
            //}
        }

        public static int CalculaFilaNuevaFactura(ExcelWorksheet hojaXl, int fila, IParametrosXL param, out string serie, out string numFactura, out string sopnumbe)
        {
            int idxFila = fila;
            sopnumbe = hojaXl.Cells[fila, param.FacturaSopnumbe].Value.ToString().Trim();
            if (param.FacturaSopSerieYNumbeSeparados.ToUpper().Equals("SI"))
            {
                serie = hojaXl.Cells[fila, param.FacturaSopSerie].Value.ToString().Trim();
                numFactura = serie + sopnumbe;
                do
                {
                    idxFila++;
                }
                while (idxFila <= hojaXl.Dimension.Rows &&
                    (serie + sopnumbe).Equals(hojaXl.Cells[idxFila, param.FacturaSopSerie].Value.ToString().Trim() + hojaXl.Cells[idxFila, param.FacturaSopnumbe].Value.ToString().Trim()));
            }
            else
            {
                serie = sopnumbe.Substring(0, 1);
                numFactura = sopnumbe;
                do
                {
                    idxFila++;
                }
                while (idxFila <= hojaXl.Dimension.Rows &&
                    sopnumbe.Equals(hojaXl.Cells[idxFila, param.FacturaSopnumbe].Value.ToString().Trim()));
            }

            return idxFila;
        }

        private taSopLineIvcInsert_ItemsTaSopLineIvcInsert CreaItemsFicticiosDeFactura(ExcelWorksheet hojaXl, int fila, IParametrosXL param)
        {
            taSopLineIvcInsert_ItemsTaSopLineIvcInsert facturaSopDe = new taSopLineIvcInsert_ItemsTaSopLineIvcInsert();
            facturaSopDe.SOPTYPE = facturaSopCa.SOPTYPE;
            facturaSopDe.SOPNUMBE = facturaSopCa.SOPNUMBE;
            facturaSopDe.CUSTNMBR = facturaSopCa.CUSTNMBR;
            facturaSopDe.DOCDATE = facturaSopCa.DOCDATE;
            facturaSopDe.NONINVEN = 1;
            facturaSopDe.ITEMNMBR = hojaXl.Cells[fila, param.FacturaSopItemnmbr].Value.ToString();
            facturaSopDe.ITEMDESC = hojaXl.Cells[fila, param.FacturaSopItemnmbrDescr].Value?.ToString();
            facturaSopDe.QUANTITY = 0;
            facturaSopDe.DEFEXTPRICE = 1;   //1: calcular el precio extendido en base al precio unitario y la cantidad

            decimal unitprice = 0;
            if (Decimal.TryParse(hojaXl.Cells[fila, param.FacturaSopDeUNITPRCE].Value.ToString(), out unitprice))
            {
                facturaSopDe.UNITPRCE = Decimal.Round(unitprice, 2);
            }
            else
                throw new FormatException("El monto es incorrecto en la fila " + fila.ToString() + ", columna " + param.FacturaSopUNITPRCE + " [armaFacturaCaEconn]");
            return facturaSopDe;
        }

        private taSopLineIvcInsert_ItemsTaSopLineIvcInsert CreaItemDeFactura(ExcelWorksheet hojaXl, int fila, IParametrosXL param)
        {
            taSopLineIvcInsert_ItemsTaSopLineIvcInsert facturaSopDe = new taSopLineIvcInsert_ItemsTaSopLineIvcInsert();
            facturaSopDe.SOPTYPE = facturaSopCa.SOPTYPE;
            facturaSopDe.SOPNUMBE = facturaSopCa.SOPNUMBE;
            facturaSopDe.CUSTNMBR = facturaSopCa.CUSTNMBR;
            facturaSopDe.DOCDATE = facturaSopCa.DOCDATE;

            facturaSopDe.ITEMNMBR = facturaSopCa.DOCID;
            facturaSopDe.ITEMDESC = hojaXl.Cells[fila, param.FacturaSopReferencia].Value?.ToString();
            facturaSopDe.QUANTITY = 1;
            facturaSopDe.DEFEXTPRICE = 1;   //1: calcular el precio extendido en base al precio unitario y la cantidad

            decimal unitprice = 0;
            if (Decimal.TryParse(hojaXl.Cells[fila, param.FacturaSopUNITPRCE].Value.ToString(), out unitprice))
            {
                facturaSopDe.UNITPRCE = Decimal.Round(unitprice, 2);
            }
            else
                throw new FormatException("El monto es incorrecto en la fila " + fila.ToString() + ", columna " + param.FacturaSopUNITPRCE + " [armaFacturaCaEconn]");
            return facturaSopDe;
        }

        private string getCustomer(string txrgnnum)
        {
            int n = 0;
            string cliente = string.Empty;
            using (BLL.DynamicsGPEntities gp = new BLL.DynamicsGPEntities(connStringEF))
                {
                //agregar una vista para rm00101
                var c = gp.vwRmClientes.Where(w => w.txrgnnum.Equals(txrgnnum.Trim()) && w.inactive == 0)
                                    .Select(s => new { custnmbr = s.custnmbr.Trim() });
                n = c.Count();
                foreach (var r in c)
                    cliente = r.custnmbr;
            }
            if (n==0)
                    throw new NullReferenceException("Cliente inexistente "+ txrgnnum);
            else if (n>1)
                    throw new InvalidOperationException("Cliente con Id de impuesto duplicado " + txrgnnum);

            return cliente;

        }

        public void preparaFacturaSOP(ExcelWorksheet hojaXl, int filaXl, string sTimeStamp, IParametrosXL param)
        {
            List<taSopLineIvcInsert_ItemsTaSopLineIvcInsert> itemsDeFactura = armaFacturaCaEconn(hojaXl, filaXl, sTimeStamp, param);

            facturaSop.taSopHdrIvcInsert = facturaSopCa;
            int longitud = itemsDeFactura.Count;
            facturaSop.taSopLineIvcInsert_Items = new taSopLineIvcInsert_ItemsTaSopLineIvcInsert[longitud]; //{ facturaSopDe };
            facturaSop.taSopLineIvcInsert_Items = itemsDeFactura.ToArray();

        }
    }
}

