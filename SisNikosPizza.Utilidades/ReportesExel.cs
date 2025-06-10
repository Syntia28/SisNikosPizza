using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SisNikosPizza.Utilidades;

// Ya tienes esta clase, no necesitas modificarla
public class ReportesExel
{
    public static byte[] FromList<T>(IEnumerable<T> data, string sheetName = "Reporte")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        if (!data.Any()) return Guardar(workbook);

        var propiedades = typeof(T).GetProperties();

        for (int col = 0; col < propiedades.Length; col++)
        {
            worksheet.Cell(1, col + 1).Value = propiedades[col].Name;
            worksheet.Cell(1, col + 1).Style.Font.Bold = true;
        }

        int fila = 2;
        foreach (var item in data)
        {
            for (int col = 0; col < propiedades.Length; col++)
            {
                var valor = propiedades[col].GetValue(item);
                worksheet.Cell(fila, col + 1).Value = valor?.ToString() ?? "";
            }
            fila++;
        }

        worksheet.Columns().AdjustToContents();
        return Guardar(workbook);
    }

    private static byte[] Guardar(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
