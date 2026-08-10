using CasaBlanca_API.Models.DTO.Ingresos;
using ClosedXML.Excel;

namespace CasaBlanca_API.Tools.Excel
{
    public class ExcelGenerator : IDisposable
    {
        private readonly IConfiguration _config;

        public ExcelGenerator(IConfiguration config)
        {
            _config = config;
        }


        public string? CreaArchivoExcelIngresos(List<IngresoResponse> Ingresos, string nombreArchivo)
        {
            var filePath = _config["ApiSettings:RutaReportesExcel"] + nombreArchivo;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ingresos"); // Agregar una hoja

                worksheet.Row(1).Height = 51;

                var titulo1 = worksheet.Range("A1:AP1");
                titulo1.Style.Fill.BackgroundColor = XLColor.Silver;
                titulo1.Style.Font.Bold = true;

                foreach (var celda in titulo1.Cells())
                {
                    celda.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    celda.Style.Border.OutsideBorderColor = XLColor.Black;

                    celda.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    celda.Style.Border.InsideBorderColor = XLColor.Black;

                    celda.Style.Font.FontColor = XLColor.Black;
                    celda.Style.Font.FontSize = 9;
                    //celda.Style.Font.Bold = true;
                    celda.Style.Alignment.WrapText = true;
                    celda.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    celda.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }


                #region Ancho de las columnas

                worksheet.Column(1).Width = 9.43;
                worksheet.Column(2).Width = 12.86;
                worksheet.Column(3).Width = 10.71;
                worksheet.Column(4).Width = 16;
                worksheet.Column(5).Width = 21.14;

                #endregion

                #region Encabezado

                worksheet.Cell(1, 1).Value = "TITULO 1";
                worksheet.Cell(1, 2).Value = "TITULO 2";
                worksheet.Cell(1, 3).Value = "TITULO 3";
                worksheet.Cell(1, 4).Value = "TITULO 4";
                worksheet.Cell(1, 5).Value = "TITULO 5";

                #endregion

                int renglon = 2;
                int rwIndex = 1;

                var rango = worksheet.Range("A1:AP1");

                #region Llena hoja Salida

                foreach (IngresoResponse _ingreso in Ingresos)
                {

                    worksheet.Cell(renglon, 1).Value = _ingreso.NumeroCasa;
                    worksheet.Cell(renglon, 2).Value = _ingreso.NombreTitular;
                    worksheet.Cell(renglon, 3).Value = _ingreso.NumeroRecibo;
                    worksheet.Cell(renglon, 4).Value = _ingreso.Concepto;
                    worksheet.Cell(renglon, 5).Value = _ingreso.FechaRecepcion;
                    
                    renglon += 1;
                    rwIndex += 1;
                }

                #endregion
                workbook.SaveAs(filePath);
            }

            return filePath;
        }


        #region Disposable 
        private bool _disposed = false;
        //private readonly FileStream _stream;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Liberar recursos administrados
                // _stream?.Dispose();
                Console.WriteLine("Recurso liberado.");
            }

            // Aquí liberarías recursos no administrados si los hubiera

            _disposed = true;
        }

        ~ExcelGenerator()
        {
            Dispose(false);
        }

        #endregion
    }
}
