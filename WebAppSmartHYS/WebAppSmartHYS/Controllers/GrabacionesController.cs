using WebAppSmartHYS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebAppSmartHYS.Class;
using System;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using System.IO;


using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Reflection.Metadata;
using Document = QuestPDF.Fluent.Document;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace WebAppSmartHYS.Controllers
{

    [AllowAnonymous]
    public class GrabacionesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _configuration;


        public GrabacionesController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        [HttpPost]
        public JsonResult GrabarPDF(string Empleado, string Documento, string Capacitacion, string TipoCapacitacion, string PorcentajeCalificacion, string FirmaEmpleado)
        {
            var binarySignature = new byte[7000];
            var base64Signature = "";
            System.IO.Stream StreamPdf = new System.IO.MemoryStream();

            var sServidorFTP = _configuration.GetValue<string>("FTP:Servidor");
            var sUsaurioFTP = _configuration.GetValue<string>("FTP:Usuario");
            var sPasswordFTP = _configuration.GetValue<string>("FTP:Password");

            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            var webRoot = _webHostEnvironment.WebRootPath;
            
            //var sPdfFile = Empleado + "_" + Documento + "_" + TipoCapacitacion + "_" + DateTime.Now.Date.ToString("yyyyMMdd") + ".pdf";
            var sPdfFile = Empleado + "_" + Documento + ".pdf";
           
            byte[] FirmaResponsableEmpresa = System.IO.File.ReadAllBytes(contentRootPath + "\\Images\\FirmaResponsableEmpresa.jpg");
            byte[] FirmaResponsableHYS = System.IO.File.ReadAllBytes(contentRootPath + "\\Images\\FirmaResponsableHYS.jpg");


            if (!String.IsNullOrWhiteSpace(FirmaEmpleado))
            {
                base64Signature = FirmaEmpleado.Split(",")[1];
                binarySignature = Convert.FromBase64String(base64Signature);
            }

            

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=app.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                DataTable dt = new DataTable();
                var ListCapacitaciones = "";

                using (var ds = new DataSet())
                {
                    using (var da = new SQLiteDataAdapter("SELECT * FROM Capacitaciones WHERE documento = '" + Documento + "' AND TipoCapacitacion='" + TipoCapacitacion + "'", "Data Source=app.db; Version = 3; New = True; Compress = True; "))
                    {
                        using (var tran = new TransactionScope())
                        {
                            ds.Clear();
                            da.Fill(ds);
                            dt = ds.Tables[0];
                            tran.Complete();     
                        }

                        if (dt.Rows.Count == 0)
                        {
                            sqlite_conn.Open();

                            SQLiteCommand sqlite_cmd;
                            sqlite_cmd = sqlite_conn.CreateCommand();
                            sqlite_cmd.CommandText = "INSERT INTO Capacitaciones VALUES (" + Documento + ", '" + Empleado + "', '" + TipoCapacitacion + "', " + PorcentajeCalificacion + ", '" + base64Signature + "', DATE('now') ); ";
                            sqlite_cmd.ExecuteNonQuery();
                        }
                    }
                }

                using (var ds = new DataSet())
                {
                    using (var da = new SQLiteDataAdapter("SELECT * FROM Capacitaciones WHERE documento = '" + Documento + "'", "Data Source=app.db; Version = 3; New = True; Compress = True; "))
                    {
                        using (var tran = new TransactionScope())
                        {
                            ds.Clear();
                            da.Fill(ds);
                            dt = ds.Tables[0];
                            tran.Complete();

                            if (dt.Rows.Count > 0)
                            {
                                foreach(var row in dt.Rows)
                                {
                                    if (ListCapacitaciones != "")
                                        ListCapacitaciones += ",";

                                    ListCapacitaciones += ((System.Data.DataRow)row).ItemArray[2].ToString();
                                }
                            }
                        }
                    }
                }                

                Settings.License = LicenseType.Community;
                Document document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));

                        /*
                        page.Background()
                            .AlignCenter()
                            .AlignMiddle()
                            .Image(FirmaResponsableHYS);
                        */

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                // by using custom 'Element' method, we can reuse visual configuration
                                table.Cell().ColumnSpan(3).Row(1).Column(1).Element(Block).Text("CONSTANCIA DE CAPACITACION").FontSize(14);
                                table.Cell().ColumnSpan(3).Row(2).Column(1).Element(Block2).Text("DIAGNOSTICO Y SOLUCIONES S.A.").FontSize(11);
                                table.Cell().Row(3).Column(1).Element(Block3).Text("  FECHA: " + DateTime.Now.Date.ToString("dd/MM/yyyy")).FontSize(11);
                                table.Cell().ColumnSpan(2).Row(3).Column(2).Element(Block3).Text("  HORA: " + DateTime.Now.ToShortTimeString()).FontSize(11);
                                table.Cell().Row(4).Column(1).Element(Block3).Text("  LUGAR: D+S").FontSize(11);
                                table.Cell().ColumnSpan(2).Row(4).Column(2).Element(Block3).Text("  CUIT: 30-70820747-7").FontSize(11);
                                table.Cell().ColumnSpan(3).Row(5).Column(1).Element(Block3).Text("  TEMA: " + ListCapacitaciones).FontSize(11);
                                table.Cell().ColumnSpan(3).Row(6).Column(1).Element(Block4).Text("Se dicta el siguiente curso de capacitación referido a la prevención de accidentes en el trabajo y enfermedades profesionales al personal más abajo detallado, cumplimentando la Ley N° 19.587/72, Dec. 351/79, Capitulo 21, Art. 208 al 214. ").FontSize(8);
                                table.Cell().ColumnSpan(3).Row(7).Column(1).Element(Block4).Text("").FontSize(8);
                                table.Cell().ColumnSpan(3).Row(7).Column(1).Element(Block4).Text("").FontSize(8);

                                table.Cell().Row(8).Column(1).Element(Block).Text("NOMBRE Y APELLIDO").FontSize(12);
                                table.Cell().Row(8).Column(2).Element(Block).Text("DNI").FontSize(12);
                                table.Cell().Row(8).Column(3).Element(Block).Text("FIRMA").FontSize(12);

                                table.Cell().Row(9).Column(1).Element(Block3).Text(Empleado).FontSize(11);
                                table.Cell().Row(9).Column(2).Element(Block3).Text(Documento).FontSize(11);
                                table.Cell().Row(9).Column(3).Element(Block3).Width(80).AlignCenter().Image(binarySignature);

                                table.Cell().ColumnSpan(3).Row(10).Column(1).Element(Block4).Text("").FontSize(8);
                                table.Cell().ColumnSpan(3).Row(11).Column(1).Element(Block4).Text("").FontSize(8);

                                table.Cell().Row(12).Column(1).Element(BlockFooter).Width(100).AlignCenter().Image(FirmaResponsableEmpresa).FitArea();
                                table.Cell().ColumnSpan(2).Row(12).Column(2).Element(BlockFooter2).Width(100).Image(FirmaResponsableHYS).FitArea();
                                table.Cell().Row(13).Column(1).Element(BlockFooter).Text("Firma Responsable Empresa").FontSize(11);
                                table.Cell().ColumnSpan(2).Row(13).Column(2).Element(BlockFooter2).Text("Firma Responsable de HyS").FontSize(11);




                                // for simplicity, you can also use extension method described in the "Extending DSL" section
                                static IContainer Block(IContainer container)
                                {
                                    return container
                                        .Border(1)
                                        .Background(Colors.LightBlue.Lighten3)
                                        .ShowOnce()
                                        .MinWidth(50)
                                        .MinHeight(20)
                                        .AlignCenter()
                                        .AlignMiddle();
                                }
                                static IContainer Block2(IContainer container)
                                {
                                    return container
                                        .Border(1)
                                        .ShowOnce()
                                        .MinWidth(50)
                                        .MinHeight(20)
                                        .AlignCenter()
                                        .AlignMiddle();
                                }
                                static IContainer Block3(IContainer container)
                                {
                                    return container
                                        .Border(1)
                                        .ShowOnce()
                                        .MinWidth(50)
                                        .MinHeight(20)
                                        .AlignLeft();
                                }
                                static IContainer Block4(IContainer container)
                                {
                                    return container
                                        .ShowOnce()
                                        .MinWidth(50)
                                        .MinHeight(20)
                                        .AlignCenter()
                                        .AlignMiddle();
                                }

                                static IContainer BlockFooter(IContainer container)
                                {
                                    return container
                                        .ShowOnce()
                                        .MinWidth(20)
                                        .MinHeight(20)
                                        .AlignLeft()
                                        .AlignMiddle();
                                }

                                static IContainer BlockFooter2(IContainer container)
                                {
                                    return container
                                        .ShowOnce()
                                        .MinWidth(50)
                                        .MinHeight(20)
                                        .AlignRight()
                                        .AlignMiddle();
                                }
                            });
                    });
                });

                byte[] pdfBytes = document.GeneratePdf();
                MemoryStream ms = new MemoryStream(pdfBytes);

                if ((sServidorFTP != null) && (sServidorFTP != ""))
                {
                    var url = sServidorFTP + sPdfFile;
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                    request.Credentials = new NetworkCredential(sUsaurioFTP, sPasswordFTP);
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    using (Stream ftpStream = request.GetRequestStream())
                    {
                        ms.CopyTo(ftpStream);
                    }
                }
                
                             

                return Json("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public IActionResult Index()
        {
            return View();
        }

        private IActionResult View()
        {
            throw new NotImplementedException();
        }
    }
}
