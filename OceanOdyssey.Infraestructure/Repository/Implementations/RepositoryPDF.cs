using OceanOdyssey.Infraestructure.Data;
using OceanOdyssey.Infraestructure.Models;
using OceanOdyssey.Infraestructure.Repository.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Globalization;
using System.IO;
using System.Linq;



using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using System.Linq;
using System.Threading.Tasks;
using System;


namespace OceanOdyssey.Infraestructure.Repository.Implementations
{
    public class RepositoryPDF : IRepositoryPDF
    {
        public async Task CrearResumenReservacionPdfAsync(ResumenReservacion reserva, string rutaPdf)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var totalComplementos = reserva.ReservaComplemento.Sum(c => c.Cantidad * (c.IdcomplementoNavigation?.Precio ?? 0));

            await Task.Run(() =>
            {
                var rutaLogo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logoFactura.png");

                var pdfDocument = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(595f, 842f);
                        page.Margin(30);

                        page.Content().Column(col =>
                        {
                            col.Item().AlignCenter().Text($"Resumen de Reserva – Crucero #{reserva.Id}")
                                .FontSize(20).Bold().FontColor(Colors.Red.Medium);

                            if (File.Exists(rutaLogo))
                            {
                                col.Item().Element(container =>
                                    container.AlignCenter().Height(80).Image(rutaLogo, ImageScaling.FitHeight)
                                );
                            }

                            col.Item().AlignCenter().Text($"Fecha de emisión: {DateTime.Now:dd/MM/yyyy}")
                                .FontSize(10).FontColor(Colors.Grey.Darken2);

                            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            col.Item().Element(box =>
                            {
                                box.Border(1)
                                    .BorderColor(Colors.Red.Lighten1)
                                    .Background(Colors.Red.Lighten5)
                                    .Padding(10)
                                    .Column(info =>
                                    {
                                        info.Item().Text("Información del Crucero")
                                            .FontSize(14).Bold().FontColor(Colors.Black);
                                        info.Item().PaddingTop(5).Text($"Nombre del Crucero: {reserva.IdcruceroNavigation.Nombre}")
                                            .FontSize(11);
                                        info.Item().Text($"Puerto de Salida: {reserva.IdcruceroNavigation.Itinerario.FirstOrDefault()?.IdpuertoNavigation?.Nombre}")
                                            .FontSize(11);
                                        info.Item().Text($"Puerto de Regreso: {reserva.IdcruceroNavigation.Itinerario.LastOrDefault()?.IdpuertoNavigation?.Nombre}")
                                            .FontSize(11);
                                        info.Item().Text($"Fecha de Inicio: {reserva.FechaCruceroNavigation.FechaInicio:dd/MM/yyyy}")
                                            .FontSize(11);
                                        info.Item().Text($"Fecha de Fin: {reserva.FechaCruceroNavigation.FechaInicio.AddDays(reserva.IdcruceroNavigation.Duracion):dd/MM/yyyy}")
                                            .FontSize(11);
                                    });
                            });

                            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            col.Item().PaddingTop(10).Text("Habitaciones Reservadas")
                                .FontSize(14).Bold().FontColor(Colors.Black);

                            col.Item().PaddingBottom(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3); 
                                    columns.RelativeColumn(2); 
                                    columns.RelativeColumn(3); 
                                    columns.RelativeColumn(1); 
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignCenter().Text("Nombre")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignRight().Text("Precio")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignLeft().Text("Pasajeros")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignCenter().Text("Cantidad")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                });

                                bool alternate = false;
                                foreach (var item in reserva.ReservaHabitacion.GroupBy(h => h.IdhabitacionNavigation.Id))
                                {
                                    var habitacion = item.First();
                                    var cantidad = item.Count();
                                    var bgColor = alternate ? Colors.Grey.Lighten4 : Colors.White;

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).PaddingLeft(5).AlignLeft().Text(habitacion.IdhabitacionNavigation.Nombre).FontSize(10);

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).PaddingRight(5).AlignRight().Text(habitacion.IdhabitacionNavigation.PrecioHabitacion.First().Costo.ToString("C")).FontSize(10);

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).PaddingLeft(5).AlignLeft().Text(string.Join(", ", habitacion.IdhabitacionNavigation.Pasajero.Select(p => p.Nombre))).FontSize(10);

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).AlignCenter().Text(cantidad.ToString()).FontSize(10);

                                    alternate = !alternate;
                                }
                            });

                            var totalHabitaciones = reserva.ReservaHabitacion
                                .GroupBy(h => h.IdhabitacionNavigation.Id)
                                .Sum(g => g.Count() * g.First().IdhabitacionNavigation.PrecioHabitacion.First().Costo);

                            col.Item().AlignRight().PaddingTop(5).Text($"Costo Total de las Habitaciones: {totalHabitaciones:C}")
                                .FontSize(10).Bold().FontColor(Colors.Red.Darken2);

                            col.Item().PaddingTop(20).Text("Complementos Adicionales")
                                .FontSize(14).Bold().FontColor(Colors.Black);

                            col.Item().PaddingBottom(20).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3); 
                                    columns.RelativeColumn(1.5f); 
                                    columns.RelativeColumn(2); 
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignCenter().Text("Nombre")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignCenter().Text("Cantidad")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignRight().Text("Precio Unitario")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                    header.Cell().Background(Colors.Red.Darken3).Padding(5).AlignRight().Text("Total")
                                        .FontColor(Colors.White).Bold().FontSize(11);
                                });

                                bool alternate = false;
                                foreach (var complemento in reserva.ReservaComplemento)
                                {
                                    var precio = complemento.IdcomplementoNavigation?.Precio ?? 0;
                                    var cantidad = complemento.Cantidad;
                                    var total = precio * cantidad;
                                    var bgColor = alternate ? Colors.Grey.Lighten4 : Colors.White;

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).PaddingLeft(5).AlignLeft().Text(complemento.IdcomplementoNavigation?.Nombre).FontSize(10);

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).AlignCenter().Text(cantidad.ToString()).FontSize(10);

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).PaddingRight(5).AlignRight().Text(precio.ToString("C")).FontSize(10);

                                    table.Cell().Background(bgColor).BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                       .PaddingVertical(8).PaddingRight(5).AlignRight().Text(total.ToString("C")).FontSize(10);

                                    alternate = !alternate;
                                }
                            });

                            col.Item().AlignRight().PaddingTop(5).Text($"Costo Total por Complementos: {totalComplementos:C}")
                                .FontSize(10).Bold().FontColor(Colors.Red.Darken2);

                            col.Item().PaddingTop(20).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                            col.Item().AlignRight().Text($"Subtotal: {reserva.TotalHabitaciones + totalComplementos:C}")
                                .FontSize(11).Bold();
                            col.Item().AlignRight().Text($"Impuestos: {reserva.Impuestos:C}")
                                .FontSize(11).Bold();
                            col.Item().AlignRight().Text($"Precio Total: {reserva.TotalFinal:C}")
                                .FontSize(12).Bold().FontColor(Colors.Red.Darken3);

                            col.Item().PaddingTop(20).Text($"Estado de Pago: {reserva.Estado}")
                                .FontSize(11).Bold().FontColor(reserva.Estado == "Pagado" ? Colors.Green.Medium : Colors.Red.Medium);

                            if (reserva.Estado != "Pagado")
                            {
                                col.Item().Text($"Fecha Límite de Pago: {reserva.FechaCruceroNavigation.FechaInicio.AddDays(reserva.IdcruceroNavigation.Duracion + 10):dd/MM/yyyy}")
                                    .FontSize(10);
                                col.Item().Text($"Monto Restante: {reserva.TotalFinal:C}")
                                    .FontSize(10);
                            }

                            col.Item().PaddingTop(25).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                            col.Item().AlignCenter().Text($"Contacto: info@oceanOdysset.com | Tel: 8385-5898 | www.oceanOdyssey.com")
                                .FontSize(9).FontColor(Colors.Grey.Darken2);
                        });
                    });
                });

                pdfDocument.GeneratePdf(rutaPdf);
            });
        }






    }
}
