using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SisNikosPizza.Utilidades;

// Ya tienes esta clase, no necesitas modificarla
public class ReportesPdf
{
    public static byte[] FromList<T>(IEnumerable<T> data, string titulo = "Reporte")
    {
        using var stream = new MemoryStream();
        using var writer = new PdfWriter(stream);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        document.Add(new Paragraph(titulo).SetTextAlignment(TextAlignment.CENTER).SetFontSize(16).SetBold());

        if (!data.Any())
        {
            document.Add(new Paragraph("Sin datos para mostrar."));
            document.Close();
            return stream.ToArray();
        }

        var propiedades = typeof(T).GetProperties();
        var table = new Table(propiedades.Length).UseAllAvailableWidth();

        // Encabezados
        foreach (var prop in propiedades)
        {
            table.AddHeaderCell(new Cell().Add(new Paragraph(prop.Name).SetBold()));
        }

        // Filas de datos
        foreach (var item in data)
        {
            foreach (var prop in propiedades)
            {
                var valor = prop.GetValue(item);
                table.AddCell(new Cell().Add(new Paragraph(valor?.ToString() ?? "")));
            }
        }

        document.Add(table);
        document.Close();
        return stream.ToArray();
    }

    public static byte[] GenerarBoletaPDF(VMDBoleta boleta)
    {

        using var stream = new MemoryStream();
        var writer = new PdfWriter(stream);
        var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        // === CONTORNO DEL VOUCHER ===
        var marco = new Table(1)
            .SetHorizontalAlignment(HorizontalAlignment.CENTER)
            .SetWidth(UnitValue.CreatePercentValue(90))
            // .SetBorder(new DottedBorder(ColorConstants.BLACK, 1))
            .SetBorder(new DashedBorder(new DeviceRgb(209, 213, 219), 1f, 5f))
            .SetPadding(15);

        // === CONTENEDOR INTERNO ===
        var contenido = new Div().SetPadding(20);

        // === TÍTULO ===
        contenido.Add(new Paragraph("NIKOS PIZZA")
            .SetFont(fontBold)
            .SetFontSize(20)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(new DeviceRgb(67, 56, 202)));

        // === SUBTÍTULO ===
        contenido.Add(new Paragraph("Boleta de Pago")
            .SetFont(fontRegular)
            .SetFontSize(11)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(new DeviceRgb(107, 114, 128)));

        // === FECHA ===
        contenido.Add(new Paragraph($"Fecha: {boleta.Fecha.ToString("dd/MM/yyyy HH:mm")}")
            .SetFont(fontRegular)
            .SetFontSize(10.5f)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(new DeviceRgb(156, 163, 175)));

        // === LÍNEA HORIZONTAL ===
        var lineaGris = new SolidLine(0.5f); // grosor 0.5 puntos
        lineaGris.SetColor(new DeviceRgb(229, 231, 235)); // aplica color gris
        contenido.Add(new LineSeparator(lineaGris).SetMarginTop(5));

        // === DATOS CLIENTE ===
        contenido.Add(GenerarTablaUsuario(boleta, fontRegular, fontBold));

        // === LÍNEA HORIZONTAL ===
        contenido.Add(new LineSeparator(lineaGris).SetMarginTop(5));

        // === DETALLE DE PEDIDO ===
        contenido.Add(new Paragraph("Detalle del Pedido:")
            .SetFont(fontBold)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.LEFT)
            .SetMarginTop(10));

        contenido.Add(GenerarTablaDetallePedido(boleta, fontRegular));

        // === TOTAL FINAL (fuente 16, color verde, negrita) ===
        var tablaTotal = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 }))
            .UseAllAvailableWidth();

        tablaTotal.AddCell(new Cell().Add(new Paragraph("Total")
            .SetFont(fontBold)
            .SetFontSize(16))
            .SetTextAlignment(TextAlignment.LEFT)
            .SetBorder(Border.NO_BORDER));
        tablaTotal.AddCell(new Cell().Add(new Paragraph($"S/. {boleta.Total}")
            .SetFont(fontBold)
            .SetFontSize(16)
            .SetFontColor(new DeviceRgb(22, 163, 74)))
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetBorder(Border.NO_BORDER));

        contenido.Add(tablaTotal);

        // === MÉTODO DE ENTREGA (gris, tamaño 11, negrita) ===
        var tablaMetodo = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 }))
            .UseAllAvailableWidth();

        tablaMetodo.AddCell(new Cell().Add(new Paragraph("Metodo")
            .SetFont(fontBold).SetFontSize(10)
            .SetFontColor(new DeviceRgb(156, 163, 175)))
            .SetTextAlignment(TextAlignment.LEFT)
            .SetBorder(Border.NO_BORDER));
        tablaMetodo.AddCell(new Cell().Add(new Paragraph(boleta.MetodoEntrega)
            .SetFont(fontRegular)
            .SetFontSize(10)
            .SetFontColor(new DeviceRgb(75, 85, 99)))
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetBorder(Border.NO_BORDER));

        contenido.Add(tablaMetodo);

        // === Añadir todo dentro del marco ===
        marco.AddCell(new Cell().Add(contenido)
            .SetBorder(Border.NO_BORDER));
        doc.Add(marco);

        doc.Close();
        return stream.ToArray();
    }
    private static Table GenerarTablaUsuario(VMDBoleta datos, PdfFont fuenteC1, PdfFont fuenteC2)
    {
        var tabla = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 }))
            .UseAllAvailableWidth()
            .SetMarginTop(5);

        foreach (var item in datos.DetallesUsuario)
        {
            tabla.AddCell(new Cell().Add(new Paragraph(item.Columna)
                .SetFont(fuenteC2))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetBorder(Border.NO_BORDER));

            tabla.AddCell(new Cell().Add(new Paragraph(item.Contenido)
                .SetFont(fuenteC1))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(Border.NO_BORDER));
        }

        return tabla;
    }
    private static Table GenerarTablaDetallePedido(VMDBoleta datos, PdfFont fuente)
    {
        var tabla = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 2 }))
            .UseAllAvailableWidth();
        // .SetMarginTop(10);

        var detalleList = datos.DetallesPedido.ToList();

        for (int i = 0; i < detalleList.Count; i++)
        {
            var item = detalleList[i];
            bool esUltima = i == detalleList.Count - 1;
            // Border bordeInferior = esUltima ? Border.NO_BORDER : new DottedBorder(1f);
            Border bordeInferior = esUltima ? Border.NO_BORDER : new DottedBorder(new DeviceRgb(229, 231, 235), 1f);


            tabla.AddCell(new Cell().Add(new Paragraph(item.cantidad.ToString()).SetFont(fuente))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetBorder(Border.NO_BORDER)
                .SetBorderBottom(bordeInferior));

            tabla.AddCell(new Cell().Add(new Paragraph(item.producto).SetFont(fuente))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBorder(Border.NO_BORDER)
                .SetBorderBottom(bordeInferior));

            tabla.AddCell(new Cell().Add(new Paragraph($"S/. {item.Total}").SetFont(fuente))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(Border.NO_BORDER)
                .SetBorderBottom(bordeInferior));
        }

        return tabla;
    }
}