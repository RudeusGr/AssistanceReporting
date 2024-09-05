using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace AssistanceReporting
{
    public partial class FormInit : Form
    {
        public FormInit()
        {
            InitializeComponent();
            ClearDataBiotimer();
            ClearDataAssistance();
            ButtonFileAssistance.Enabled = false;
            ButtonGenerateReport.Enabled = false;
        }

        private String? FilePathBiotimer = null;
        private String? FilePathAssistance = null;
        private string DateInitial;
        private string DateFinal;
        private readonly int DaysWorked = 6;
        private Dictionary<int, Employee> DicEmployees = new();
        private readonly String PathDir = "C:\\Users\\sistemas\\Documents\\ReportingAssistance\\";

        private void ClearDataBiotimer()
        {
            DataGridViewFileBiotimer.Rows.Clear();
            DataGridViewFileBiotimer.ColumnCount = 5;
            DataGridViewFileBiotimer.Columns[0].HeaderText = "ID Empleado";
            DataGridViewFileBiotimer.Columns[0].Width = 100;
            DataGridViewFileBiotimer.Columns[1].HeaderText = "Nombre";
            DataGridViewFileBiotimer.Columns[1].Width = 300;
            DataGridViewFileBiotimer.Columns[2].HeaderText = "Fecha";
            DataGridViewFileBiotimer.Columns[2].Width = 100;
            DataGridViewFileBiotimer.Columns[3].HeaderText = "Hora Entrada";
            DataGridViewFileBiotimer.Columns[3].Width = 100;
            DataGridViewFileBiotimer.Columns[4].HeaderText = "Hora Salida";
            DataGridViewFileBiotimer.Columns[4].Width = 100;
        }

        private void ClearDataAssistance()
        {
            DataGridViewFileAssistance.Rows.Clear();
            DataGridViewFileAssistance.ColumnCount = 3;
            DataGridViewFileAssistance.Columns[0].HeaderText = "Fecha";
            DataGridViewFileAssistance.Columns[0].Width = 200;
            DataGridViewFileAssistance.Columns[1].HeaderText = "Ruta";
            DataGridViewFileAssistance.Columns[1].Width = 200;
            DataGridViewFileAssistance.Columns[2].HeaderText = "ID de Empleado";
            DataGridViewFileAssistance.Columns[2].Width = 300;
        }

        private void CheckBiotimer()
        {
            ClearDataBiotimer();
            ClearDataAssistance();
            DicEmployees.Clear();
            ButtonFileAssistance.Enabled = true;
            ButtonGenerateReport.Enabled = false;
        }

        private void ErrorUploadBiotimer()
        {
            ClearDataBiotimer();
            ClearDataAssistance();
            DicEmployees.Clear();
            ButtonFileAssistance.Enabled = false;
            ButtonGenerateReport.Enabled = false;
        }

        private void CheckAssistance()
        {
            ClearDataAssistance();
            foreach (var employee in DicEmployees)
            {
                employee.Value.DicRouteDate.Clear();
            }
            ButtonGenerateReport.Enabled = true;
        }

        private void ErrorUploadAssistance()
        {
            ClearDataAssistance();
            foreach (var employee in DicEmployees)
            {
                employee.Value.DicRouteDate.Clear();
            }
            ButtonGenerateReport.Enabled = false;
        }

        private Boolean SearchFile(int option)
        {
            Boolean fileExist = false;
            var browseableOpenFileDialog = new OpenFileDialog();
            if (browseableOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileExtension = browseableOpenFileDialog.FileName.Split('.');
                if (!fileExtension[^1].Equals("xlsx"))
                {
                    MessageBox.Show("El archivo enviado no es un archivo de Excel valido, los archivos de Excel tiene la extencion 'xlsx'", "Tipo de archivo incorrecto");
                }
                else
                {
                    switch (option)
                    {
                        case 1:
                            this.FileNameBiotimer.Text = browseableOpenFileDialog.FileName.Split('\\').Last();
                            FilePathBiotimer = browseableOpenFileDialog.FileName;
                            fileExist = true;
                            break;
                        case 2:
                            this.FileNameAssistance.Text = browseableOpenFileDialog.FileName.Split('\\').Last();
                            FilePathAssistance = browseableOpenFileDialog.FileName;
                            fileExist = true;
                            break;
                    }
                }
            }
            browseableOpenFileDialog.Dispose();
            return fileExist;
        }

        private void UploadDataBiotimer()
        {
            if (FilePathBiotimer is null)
            {
                MessageBox.Show("No es posible encontrar el archivo, asegurese de haberlo seleccionado.", "Archivo no encontrado");
                return;
            }

            try
            {
                CheckBiotimer();
                using XLWorkbook workbook = new(FilePathBiotimer);
                IXLWorksheet sheet = workbook.Worksheet(1);

                var lastRow = sheet.LastRowUsed().RangeAddress.LastAddress.RowNumber;
                DateInitial = sheet.Row(2).Cell(4).GetString().Remove(10);
                DateFinal = sheet.Row(2).Cell(4).GetString().Remove(10);

                for (int i = 2; i <= lastRow; i++)
                {
                    IXLRow currentRow = sheet.Row(i);

                    int idEmployee = currentRow.Cell(1).GetValue<int>();
                    string nameEmployee = currentRow.Cell(2).GetString() + " " + currentRow.Cell(3).GetString();
                    string dateEmployee = currentRow.Cell(4).GetString().Remove(10);
                    string[] hourEmployee = currentRow.Cell(6).GetString().Split(',');
                    string currentHourCompare = dateEmployee + " " + "07:06:00";

                    if (DateTime.Parse(dateEmployee) < DateTime.Parse(DateInitial))
                    {
                        DateInitial = dateEmployee;
                    }

                    if (DateTime.Parse(dateEmployee) > DateTime.Parse(DateFinal))
                    {
                        DateFinal = dateEmployee;
                    }

                    if (hourEmployee.Length == 1)
                    {
                        hourEmployee[0] = hourEmployee[0].Replace("31/12/1899", dateEmployee);
                    }
                    else
                    {
                        Array.Sort(hourEmployee);
                        hourEmployee[0] = dateEmployee + " " + hourEmployee[0];
                        hourEmployee[^1] = dateEmployee + " " + hourEmployee[^1];
                    }

                    if (!DicEmployees.TryGetValue(idEmployee, out Employee? employee))
                    {
                        employee = new Employee(idEmployee,nameEmployee);
                        DicEmployees.Add(idEmployee, employee);
                    }

                    if (DateTime.Parse(hourEmployee[0]) >= DateTime.Parse(currentHourCompare))
                    {
                        employee.DelaysIncremente();
                    }

                    employee.AssistancesIncremente();

                    DataGridViewFileBiotimer.Rows.Add(new string[] {
                        idEmployee + "",
                        nameEmployee,
                        dateEmployee,
                        hourEmployee[0].Replace(dateEmployee,"").Replace("a. m.","").Replace("p. m.","").Trim(),
                        hourEmployee[hourEmployee.Length == 1 ? 0 : hourEmployee.Length-1].Replace(dateEmployee,"").Replace("a. m.","").Replace("p. m.","").Trim()
                    });
                }
                workbook.Dispose();
            }
            catch (OpenXmlPackageException openXmlEx)
            {
                ErrorUploadBiotimer();
                MessageBox.Show($"Ocurrio un error inesperado con la libreria CloseXML al intentar abrir el archivo, por favor notificar al departamento de sistemas.\n Error: {openXmlEx.Message}", "Error de CloseXML");
            }
            catch (ArgumentNullException argNullEx)
            {
                ErrorUploadBiotimer();
                MessageBox.Show($"Ocurrio un error inesperado, existe una referencia nula, por favor notificar al departamento de sistemas.\n Error: {argNullEx.Message}", "Error de ArgumentException");
            }
            catch (IOException IOEx)
            {
                ErrorUploadBiotimer();
                MessageBox.Show($"Actualmente se esta utilizando el archivo, por favor cierrelo y vuelva a intentar cargarlo.\n Error: {IOEx.Message}", "Advertencia Archivo Ocupado");
            }
        }

        private void UploadDataAssistance()
        {
            if (FilePathAssistance is null)
            {
                MessageBox.Show("No es posible encontrar el archivo, asegurese de haberlo seleccionado.", "Archivo no encontrado");
                return;
            }

            try
            {
                CheckAssistance();
                XLWorkbook workbook = new(FilePathAssistance);
                IXLWorksheet sheet = workbook.Worksheet(1);

                var lastRow = sheet.LastRowUsed().RangeAddress.LastAddress.RowNumber;

                string connectionString = "Server=SOPORTETI\\SQLEXPRESS;Database=employees ;Trusted_Connection=SSPI;MultipleActiveResultSets=true;Trust Server Certificate=true";

                using SqlConnection DBConnection = new(connectionString);

                DBConnection.Open();
                string sqlQuery = "SELECT [cverut],[fecalt],[venta] FROM [employees].[dbo].[bulktoroute] WHERE (fecalt BETWEEN @DateInitial AND @DateFinal)";
                var registers = DBConnection.Query(sqlQuery, new {DateInitial = DateTime.Parse(DateInitial), DateFinal = DateTime.Parse(DateFinal) });

                for (int i = 1; i <= lastRow; i++)
                {
                    IXLRow currentRow = sheet.Row(i);

                    string currentDateInsert = currentRow.Cell(1).GetString().Remove(10);
                    int currentEmployeeInsert = currentRow.Cell(2).GetValue<int>();
                    int currentRouteInsert = currentRow.Cell(3).GetValue<int>();

                    DataGridViewFileAssistance.Rows.Add(new string[] {
                        currentDateInsert,
                        currentRouteInsert + "",
                        currentEmployeeInsert + ""
                    });

                    if (DicEmployees.ContainsKey(currentEmployeeInsert))
                    {
                        DicEmployees[currentEmployeeInsert].DicRouteDate.Add(currentDateInsert, currentRouteInsert);
                        var rowDatabase = registers.Where(row => row.fecalt == DateTime.Parse(currentDateInsert) && row.cverut == currentRouteInsert);
                        if (!rowDatabase.IsNullOrEmpty())
                        {
                            DicEmployees[currentEmployeeInsert].Bulk += rowDatabase.First().venta;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Se encontro al empleado {currentEmployeeInsert}, quien no aparece en el archivo de BioTimer, no sera contempleado para la generacion del archivo final, por favor darlo de alta en el checador.", "Empleado No Encntrado");
                    }
                }
                workbook.Dispose();
                DBConnection.Close();
                DBConnection.Dispose();
                DicEmployees = DicEmployees.Where(emp => emp.Value.DicRouteDate.IsNullOrEmpty() == false).ToDictionary(emp => emp.Key, emp => emp.Value);
            }
            catch (OpenXmlPackageException openXmlEx)
            {
                ErrorUploadAssistance();
                MessageBox.Show($"Ocurrio un error inesperado con la libreria CloseXML al intentar abrir el archivo, por favor notificar al departamento de sistemas.\n Error: {openXmlEx.Message}", "Error de CloseXML");
            }
            catch (ArgumentNullException argNullEx)
            {
                ErrorUploadAssistance();
                MessageBox.Show($"Ocurrio un error inesperado, existe una referencia nula, por favor notificar al departamento de sistemas.\n Error: {argNullEx.Message}", "Error de ArgumentException");
            }
        }

        private void ButtomFileBiotimer_Click(object sender, EventArgs e)
        {
            if (SearchFile(1))
            {
                UploadDataBiotimer();
            }
        }

        private void ButtonFileAssistance_Click(object sender, EventArgs e)
        {
            if (SearchFile(2))
            {
                UploadDataAssistance();
            }
        }

        private void ButtonGenerateReport_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(PathDir))
            {
                Directory.CreateDirectory(PathDir);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Asistencias - Ventas");
                worksheet.Cell("A1").Value = $"Reporte de Asistencias de personal de ruta del dia {DateInitial} al {DateFinal}";
                worksheet.Range("A1:F1").Row(1).Merge();
                worksheet.Cell("A2").Value = "ID Empleado";
                worksheet.Cell("B2").Value = "Nombre";
                worksheet.Cell("C2").Value = "Dias trabajados";
                worksheet.Cell("D2").Value = "Retrasos";
                worksheet.Cell("E2").Value = "Bono puntualidad";
                worksheet.Cell("F2").Value = "Bultos";
                worksheet.Cell("G2").Value = "Bono venta";

                int row = 3;

                foreach (var employee in DicEmployees)
                {
                    worksheet.Cell("A" + row).Value = employee.Value.Id;
                    worksheet.Cell("B" + row).Value = employee.Value.Name;
                    worksheet.Cell("C" + row).Value = employee.Value.Assistance;
                    worksheet.Cell("D" + row).Value = employee.Value.Delays;
                    if (employee.Value.Assistance < DaysWorked || employee.Value.Delays >= 2)
                    {
                        worksheet.Cell("E" + row).Value = 0;
                    }
                    else
                    {
                        worksheet.Cell("E" + row).Value = 7 * 50;
                    }

                    worksheet.Cell("F" + row).Value = employee.Value.Bulk;

                    decimal commission = (decimal)(employee.Value.Bulk * .15);
                    
                    if (commission >= 300)
                    {
                        worksheet.Cell("G" + row).Value = 300;
                    } else if (commission <= 150)
                    {
                        worksheet.Cell("G" + row).Value = 150;
                    } else
                    {
                        worksheet.Cell("G" + row).Value = commission;
                    }

                    row++;
                }

                worksheet.Column(1).AdjustToContents();
                worksheet.Column(2).AdjustToContents();
                worksheet.Column(3).AdjustToContents();
                worksheet.Column(4).AdjustToContents();
                worksheet.Column(5).AdjustToContents();
                worksheet.Column(6).AdjustToContents();
                worksheet.Column(7).AdjustToContents();

                workbook.SaveAs(PathDir + $"Reporte Asistencia {DateTime.Now:yyyy-MM-dd HH.mm.ss}.xlsx");
                workbook.Dispose();
            }
        }
    }
}
