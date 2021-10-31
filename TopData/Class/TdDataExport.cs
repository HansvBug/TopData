namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Export the query reesult to Excel or csv format.
    /// </summary>
    public class TdDataExport : IDisposable
    {
        private readonly SaveFileDialog saveDataFileDialog = new ();

        #region properties

        /// <summary>
        /// Gets or sets the datatable which will be exported.
        /// </summary>
        public DataTable Datatable { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        private bool Filtered { get; set; }

        private bool SuppressDialogExportReady { get; set; } // Avoid ready message when multiple query's are executed

        private string CsvSeparator { get; set; }

        private string CSVstartEnd { get; set; }

        private string TextSeparator { get; set; }

        private string TextstartEnd { get; set; }

        private string DefaultExportExtension { get; set; }

        private bool SdoGeometryColumnVisible { get; set; }

        private readonly FormMain formMain;

        private bool UseSameFile { get; set; }

        private string WorksheetName { get; set; } // The Excel worksheet name.

        #endregion properties
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdDataExport"/> class.
        /// Constructor for full datatable Export.
        /// </summary>
        /// <param name="aDataTable">The datatabel which will be exported.</param>
        /// <param name="filterd">Is there a filter active. Only the filtered records will be exporterted.</param>
        /// <param name="suppressDialogExportReady">Show Export ready dialog (or not).</param>
        /// <param name="formMain">Reference to the main form.</param>
        /// <param name="sdoGeomColumnVisible">is the geometry column visssible?.</param>
        public TdDataExport(DataTable aDataTable, bool filterd, bool suppressDialogExportReady, FormMain formMain, bool sdoGeomColumnVisible)
        {
            this.GetSettings();

            if (aDataTable != null && aDataTable.Rows.Count > 0)
            {
                this.Datatable = aDataTable.Copy();     // Make a copy from org datatable so it won't be changed as the geometry column is removed for the export
                this.Filtered = filterd;
                this.SuppressDialogExportReady = suppressDialogExportReady;

                this.CsvSeparator = this.JsonObjSettings.ExportParam[0].CSVExportChar;
                this.TextSeparator = this.JsonObjSettings.ExportParam[0].TextExportChar;
                this.DefaultExportExtension = this.JsonObjSettings.ExportParam[0].DefaultExportFileExtension;

                this.formMain = formMain;
                this.SdoGeometryColumnVisible = sdoGeomColumnVisible;
                this.RemoveSdoGeomColumn(this.Datatable);
                this.PrepareFilteredDatatabe(this.Datatable);

                if (this.CsvSeparator[0] == '"')
                {
                    this.CSVstartEnd = @"""";
                }
                else
                {
                    this.CSVstartEnd = string.Empty;
                }

                if (this.TextSeparator[0] == '"')
                {
                    this.TextstartEnd = @"""";
                }
                else
                {
                    this.TextstartEnd = string.Empty;
                }
            }
        }

        // Constructor helper
        private void GetSettings()
        {
            try
            {
                using TdSettingsManager set = new ();
                set.LoadSettings();

                if (set.JsonObjSettings != null)
                {
                    this.JsonObjSettings = set.JsonObjSettings;
                }
                else
                {
                    MessageBox.Show(MB_Text.Settings_File_Not_Found, MB_Title.Attention, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (AccessViolationException)
            {
                // Logging is not available here
                MessageBox.Show("Fout bij het laden van de instellingen. " + Environment.NewLine + "De default instellingen worden ingeladen.", MB_Title.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion constructor

        private void RemoveSdoGeomColumn(DataTable aDataTable) // Remove the sdogeometry column when this is checked in the main form
        {
            if (aDataTable != null && aDataTable.Rows.Count > 0)
            {
                if (!this.SdoGeometryColumnVisible)
                {
                    int indexSdoGeometryColumn = 0;
                    bool foundGeomColumn = false;

                    for (int j = 0; j < aDataTable.Columns.Count; j++)
                    {
                        if (aDataTable.Columns[j].DataType.Name.ToString(CultureInfo.InvariantCulture) == "SdoGeometry")
                        {
                            indexSdoGeometryColumn = j;
                            foundGeomColumn = true;
                        }
                    }

                    if (foundGeomColumn)
                    {
                        this.Datatable.Columns.RemoveAt(indexSdoGeometryColumn);
                    }
                }
            }
        }

        private void PrepareFilteredDatatabe(DataTable aDataTable)
        {
            if (aDataTable != null && aDataTable.Rows.Count > 0)
            {
                if (this.Filtered)
                {
                    aDataTable.DefaultView.RowFilter = this.formMain.Filters.FilterQuery;

                    DataView dv = aDataTable.DefaultView;
                    aDataTable.DefaultView.RowFilter = this.formMain.Filters.FilterQuery;
                    this.Datatable = dv.ToTable();
                }
            }
        }

        /// <summary>
        /// Export the query result.
        /// </summary>
        public void ExportData()
        {
            // If datatable is empty.
            if (this.Datatable.Rows.Count == 0)
            {
                MessageBox.Show("Geen data beschikbaar, voer eerst een query uit.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;

                this.saveDataFileDialog.Filter = "Excel bestanden (*.xlsx)|*.xlsx|Komma gescheiden bestanden (*.csv)|*.csv|Tekst bestanden (*.txt)|*.txt|Alle formaten (*.*)|*.*";

                switch (this.DefaultExportExtension)
                {
                    case "*.xlsx":
                        this.saveDataFileDialog.FilterIndex = 1;
                        break;
                    case "*.csv":
                        this.saveDataFileDialog.FilterIndex = 2;
                        break;
                    case "*.txt":
                        this.saveDataFileDialog.FilterIndex = 3;
                        break;
                    default:
                        this.saveDataFileDialog.FilterIndex = 2;
                        break;
                }

                this.saveDataFileDialog.AddExtension = true;
                this.saveDataFileDialog.CheckPathExists = true;
                this.saveDataFileDialog.OverwritePrompt = false;

                Cursor.Current = Cursors.Default;

                if (this.saveDataFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.SaveDataFileDialog_FileOk(this.saveDataFileDialog);
                }
                else
                {
                    return;
                }
            }
        }

        private void SaveDataFileDialog_FileOk(SaveFileDialog saveDataFileDialog) // Object sender
        {
            string folderAndFile = string.Empty;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string fileName = saveDataFileDialog.FileName; // Get the file name from saveFileDialog
                folderAndFile = fileName;

                switch (saveDataFileDialog.FilterIndex)
                {
                    case 1: // Excel
                        if (!this.Filtered)
                        {
                            this.formMain.ToolStripStatusLabel1.Text = "Bezig met het exporteren van : " + fileName;
                            this.formMain.Refresh();

                            this.ExportToExcel(this.Datatable, fileName);  // Full export from table
                        }
                        else
                        {
                            this.formMain.ToolStripStatusLabel1.Text = "Bezig met het exporteren van : " + fileName;
                            this.formMain.Refresh();
                            this.ExportFilterdToExcel(this.Datatable, fileName);  // Export from datagridview filterd
                        }

                        Cursor.Current = Cursors.Default;
                        break;

                    case 2: // CSV
                        if (!this.Filtered)
                        {
                            this.formMain.ToolStripStatusLabel1.Text = "Bezig met het exporteren van : " + fileName;
                            this.formMain.Refresh();
                            this.DataTableToCSV(this.Datatable, this.CsvSeparator, fileName);     // Write to the file name selected.
                        }
                        else
                        {
                            this.formMain.ToolStripStatusLabel1.Text = "Bezig met het exporteren van : " + fileName;
                            this.formMain.Refresh();
                            this.ExportFilterdToCSV(this.CsvSeparator, fileName); // Write to the file name selected.
                        }

                        Cursor.Current = Cursors.Default;
                        break;

                    case 3: // Txt
                        if (!this.Filtered)
                        {
                            this.formMain.ToolStripStatusLabel1.Text = "Bezig met het exporteren van : " + fileName;
                            this.formMain.Refresh();
                            this.DataTableToText(this.Datatable, this.TextSeparator, fileName);     // Write to the file name selected.
                        }
                        else
                        {
                            this.formMain.ToolStripStatusLabel1.Text = "Bezig met het exporteren van : " + fileName;
                            this.formMain.Refresh();
                            this.ExportFilterdToText(this.TextSeparator, fileName); // Write to the file name selected.
                        }

                        Cursor.Current = Cursors.Default;
                        break;

                    default:
                        break;
                }
            }
            catch (IOException ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(
                    "Opslaan bestand is mislukt. Is het bestand is geopend in een ander programma?" + Environment.NewLine +
                    Environment.NewLine +
                    "Sla het bestand op onder een andere naam.",
                    "Informatie.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TdLogging.WriteToLogError("Opslaan bestand is mislukt.");
                TdLogging.WriteToLogError("Melding : ");
                TdLogging.WriteToLogError(ex.Message);

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                TdLogging.WriteToLogError("Betreft bestand: " + folderAndFile);
            }
        }

        private void ExportFilterdToExcel(DataTable aDatatable, string excelFilePath)
        {
            if (!File.Exists(excelFilePath))
            {
                this.StartTheFilteredExcelExport(aDatatable, excelFilePath);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Het bestand bestaat al. Overschijven?", "Opslaan bestand.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    this.StartTheFilteredExcelExport(aDatatable, excelFilePath);
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Het bestand is niet opgeslagen.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void StartTheFilteredExcelExport(DataTable aDataTable, string excelFilePath)
        {
            if (aDataTable.Columns.Count != 0 && aDataTable.Rows.Count != 0)
            {
                try
                {
                    // Construct the datatable
                    DataTable exportDataTable = this.MakeFilterdDataTable(aDataTable);

                    this.ExportDataSetToXLSX(exportDataTable, excelFilePath);

                    if (!this.SuppressDialogExportReady)
                    {
                        MessageBox.Show("Opslaan Excel bestand is gereed.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    TdLogging.WriteToLogInformation("Opslaan Excel bestand is gereed.");
                    exportDataTable.Dispose();
                }
                catch (Exception ex)
                {
                    this.formMain.ProgressBarExport.Visible = false;

                    TdLogging.WriteToLogError("Export naar Excel is mislukt.");
                    TdLogging.WriteToLogError("Melding");
                    TdLogging.WriteToLogError(ex.Message);

                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }

                    MessageBox.Show(
                        "Opslaan Excel bestand is mislukt." + Environment.NewLine +
                        Environment.NewLine +
                        "Controleer het log bestand.",
                        "Informatie.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private DataTable MakeFilterdDataTable(DataTable aDataTable)
        {
            DataTable dt = new ("Blad1");
            DataColumn column;
            DataRow row;

            // Adding the Columns
            foreach (DataColumn col in aDataTable.Columns)
            {
                column = new DataColumn
                {
                    ColumnName = col.ColumnName,
                };
                dt.Columns.Add(column);
            }

            row = dt.NewRow();
            dt.Rows.Add(row);

            this.formMain.ProgressBarExport.Value = 0;
            this.formMain.ProgressBarExport.Maximum = aDataTable.Rows.Count;
            this.formMain.ProgressBarExport.Step = 1;

            // Adding the Rows
            foreach (DataRow rw in aDataTable.Rows)
            {
                row = dt.NewRow();

                foreach (DataRow dr in this.Datatable.Rows)
                {
                    for (int i = 0; i < this.Datatable.Columns.Count; ++i)
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = dr[i].ToString();
                    }
                }

                dt.Rows.Add(row);
                this.formMain.ProgressBarExport.PerformStep();
            }

            return dt;
        }

        private void DataTableToCSV(DataTable datatable, string seperator, string fileName)
        {
            if (datatable != null)
            {
                try
                {
                    using StreamWriter sw = new(new FileStream(fileName, FileMode.Create));
                    string aLine = string.Empty;
                    aLine = this.CSVstartEnd;

                    // Header
                    for (int i = 0; i < datatable.Columns.Count; ++i)
                    {
                        aLine += datatable.Columns[i].ToString();
                        if (i < datatable.Columns.Count - 1)
                        {
                            aLine += seperator;
                        }
                    }

                    aLine += this.CSVstartEnd;     // end row with "
                    sw.WriteLine(aLine);  // Write the header
                    aLine = string.Empty;

                    this.formMain.ProgressBarExport.Maximum = datatable.Rows.Count;
                    this.formMain.ProgressBarExport.Step = 1;
                    this.formMain.ProgressBarExport.Visible = true;
                    this.formMain.Refresh();

                    foreach (DataRow dr in datatable.Rows)
                    {
                        aLine = this.CSVstartEnd; // Start row with "
                        for (int i = 0; i < datatable.Columns.Count; ++i)
                        {
                            aLine += dr[i].ToString();
                            if (i < datatable.Columns.Count - 1)
                            {
                                aLine += seperator;
                            }
                        }

                        aLine += this.CSVstartEnd; // end row with "
                        sw.WriteLine(aLine);
                        aLine = string.Empty;
                        this.formMain.ProgressBarExport.PerformStep();
                    }

                    if (!this.SuppressDialogExportReady)
                    {
                        MessageBox.Show("Opslaan bestand is gereed.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    sw.Flush();
                    this.formMain.ProgressBarExport.Visible = false;
                }
                catch (IOException ex)
                {
                    TdLogging.WriteToLogError("Exporteren naar CSV bestand is mislukt.");
                    TdLogging.WriteToLogError("Melding : ");
                    TdLogging.WriteToLogError(ex.Message);

                    if (TdDebugMode.DebugMode)
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
            }
        }

        private void ExportToExcel(DataTable datatable, string excelFilePath)
        {
            // New file
            if (!File.Exists(excelFilePath))
            {
                this.StartTheExcelExport(datatable, excelFilePath);
            }
            else if (this.UseSameFile)
            {
                // use the same Excel file and ad e worksheet to the existing file
                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug("Opslaan zelfde bestand, nieuw werkblad.");
                }

                this.StartTheExcelExport(datatable, excelFilePath);
            }
            else
            {
                this.StartTheExcelExport(datatable, excelFilePath);
            }
        }

        private void StartTheExcelExport(DataTable datatable, string excelFilePath)
        {
            int columnsCount = datatable.Columns.Count;

            if (datatable == null || columnsCount == 0)
            {
                return;  // Exit function when the table is empty
            }

            try
            {
                this.formMain.ProgressBarExport.Maximum = datatable.Rows.Count;
                this.formMain.ProgressBarExport.Step = 1;
                this.formMain.ProgressBarExport.Value = 0; // Reset the progressbar
                this.formMain.ProgressBarExport.Visible = true;

                // Construct the datatable
                DataTable exportDataTable = this.MakeDataTable(datatable);

                this.ExportDataSetToXLSX(exportDataTable, excelFilePath);
                this.formMain.ProgressBarExport.Visible = false;
                TdLogging.WriteToLogInformation("Opslaan Excel bestand is gereed. (" + excelFilePath + ")");

                if (!this.SuppressDialogExportReady)
                {
                    MessageBox.Show("Opslaan Excel bestand is gereed.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                exportDataTable.Dispose();
                this.formMain.Refresh();
            }
            catch (Exception ex)
            {
                this.formMain.ProgressBarExport.Visible = false;
                this.formMain.Refresh();
                TdLogging.WriteToLogError("Export naar Excel is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);

                if (TdDebugMode.DebugMode)
                {
                    TdLogging.WriteToLogDebug(ex.ToString());
                }

                MessageBox.Show(
                    "Opslaan Excel bestand is mislukt." + Environment.NewLine +
                    Environment.NewLine +
                    "Controleer het log bestand.",
                    "Informatie.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private DataTable MakeDataTable(DataTable aDataTable)
        {
            int columnsCount = aDataTable.Columns.Count;
            System.Data.DataTable table = new ("Blad1");
            DataColumn column;
            DataRow row;

            if (aDataTable != null || columnsCount != 0)
            {
                columnsCount = aDataTable.Columns.Count;

                // column headings
                for (int i = 0; i < columnsCount; i++)
                {
                    // Create new DataColumn, set DataType, ColumnName and add to DataTable.
                    column = new DataColumn
                    {
                        ColumnName = aDataTable.Columns[i].ColumnName,
                    };

                    // Add the Column to the DataColumnCollection.
                    table.Columns.Add(column);
                }

                this.formMain.ProgressBarExport.PerformStep();

                // Fill the rows
                foreach (DataRow rw in aDataTable.Rows)
                {
                    row = table.NewRow();
                    for (int i = 0; i < aDataTable.Columns.Count; ++i)
                    {
                        row[i] = rw[i].ToString();
                    }

                    // TODO Disable SplitContainerQueryTree_Paint
                    this.formMain.ProgressBarExport.PerformStep();

                    // TODO enable SplitContainerQueryTree_Paint
                    table.Rows.Add(row);
                }

                return table;
            }

            return table;
        }

        private void ExportDataSetToXLSX(DataTable table, string excelFileName)
        {
            // ExcelFileName may be empty. (Then the file will be created).
            if (!this.UseSameFile)
            {
                this.ExportDataSetToNewXLSX(table, excelFileName);
            }
            else
            {
                this.ExportDataSetToOpenXLSX(table, excelFileName);
            }
        }

        private void ExportDataSetToNewXLSX(DataTable table, string excelFileName)
        {
            string folder = Path.GetDirectoryName(excelFileName);
            if (Directory.Exists(folder))
            {
                try
                {
                    using var workbook = SpreadsheetDocument.Create(excelFileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
                    var workbookPart = workbook.AddWorkbookPart();

                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook
                    {
                        Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets(),
                    };

                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;

                    // if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Any())
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    if (string.IsNullOrEmpty(this.WorksheetName))
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new () { Id = relationshipId, SheetId = sheetId, Name = table.TableName };  // There is no tableName !!! --> result = Blad1 as worksheetname
                        sheets.Append(sheet);
                    }
                    else
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new () { Id = relationshipId, SheetId = sheetId, Name = this.WorksheetName };
                        sheets.Append(sheet);
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new ();

                    // Construct column names
                    List<string> columns = new ();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new ()
                        {
                            DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String,
                            CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName),
                        };

                        headerRow.AppendChild(cell);
                    }

                    // Add the row values to the excel sheet
                    sheetData.AppendChild(headerRow);

                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new ();
                        foreach (string col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new ()
                            {
                                DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String,
                                CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()),
                            };
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                        this.formMain.ProgressBarExport.PerformStep();
                    }
                }
                catch (IOException ex)
                {
                    TdLogging.WriteToLogWarning("Fout opgetreden bij het uitvoeren van meerdere query's. Het Export bestand was geopend oor een ander programma.");
                    TdLogging.WriteToLogWarning("Melding:");
                    TdLogging.WriteToLogWarning(ex.Message);

                    MessageBox.Show(
                        "Het bestand is geopend door een ander programma. " + Environment.NewLine +
                        "Sluit het programma en start het uitvoeren opnieuw",
                        "Waarschuwing.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    TdLogging.WriteToLogError("Betreft bestand: " + excelFileName);
                }
            }
            else
            {
                MessageBox.Show("Opslaan Excel bestand is gereed.", "Waarschuwing.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExportDataSetToOpenXLSX(DataTable table, string excelFileName)
        {
            if (Directory.Exists(Path.GetDirectoryName(excelFileName)))
            {
                try
                {
                    using var workbook = SpreadsheetDocument.Open(excelFileName, true);

                    // Add a blank WorksheetPart.
                    var newWorksheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    newWorksheetPart.Worksheet = new Worksheet(sheetData);

                    var sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                    var relationshipId = workbook.WorkbookPart.GetIdOfPart(newWorksheetPart);

                    // Get a unique ID for the new worksheet.
                    uint sheetId = 1;
                    if (sheets.Elements<Sheet>().Any())
                    {
                        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    // Give the new worksheet a name.
                    var sheetName = string.Empty;

                    // Check for double scheetname
                    if (!string.IsNullOrEmpty(this.WorksheetName))
                    {
                        if (CheckWorksheetNames(workbook, this.WorksheetName))
                        {
                            sheetName = "Blad" + sheetId.ToString(CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            sheetName = this.WorksheetName;
                        }
                    }
                    else
                    {
                        sheetName = "Blad" + sheetId.ToString(CultureInfo.InvariantCulture);
                    }

                    // Append the new worksheet and associate it with the workbook.
                    var sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
                    sheets.Append(sheet);
                    Row headerRow = new ();

                    // Construct column names
                    var columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);
                        var cell = new Cell
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue(column.ColumnName),
                        };
                        headerRow.AppendChild(cell);
                    }

                    // Add the row values to the excel sheet
                    sheetData.AppendChild(headerRow);

                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        Row newRow = new ();
                        foreach (var col in columns)
                        {
                            var cell = new Cell
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(dsrow[col].ToString()),
                            };
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                        this.formMain.ProgressBarExport.PerformStep();
                    }
                }
                catch (IOException ex)
                {
                    TdLogging.WriteToLogWarning("Fout opgetreden bij het uitveoren van meerdere query's. Het Export bestand was geopend oor een ander programma.");
                    TdLogging.WriteToLogWarning("Melding:");
                    TdLogging.WriteToLogWarning(ex.Message);

                    MessageBox.Show(
                        "Het bestand is geopend door een ander programma. " + Environment.NewLine +
                        "Sluit het programma en start het uitvoeren opnieuw", "Waarschuwing.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    TdLogging.WriteToLogError("Betreft bestand: " + excelFileName);
                }
            }
        }

        // Get the Excel worksheet names.
        private static bool CheckWorksheetNames(SpreadsheetDocument aWorkbook, string aSheetName)
        {
            int sheetIndex = 0;
            bool isDouble = false;
            var sheets = aWorkbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();

            uint sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max();

            foreach (WorksheetPart worksheetpart in aWorkbook.WorkbookPart.WorksheetParts)
            {
                Worksheet worksheet = worksheetpart.Worksheet;
                if (sheetIndex < sheetId)
                {
                    // Grab the sheet name each time through your loop
                    if (sheets.Descendants<Sheet>().ElementAt(sheetIndex).Name == aSheetName)
                    {
                        sheetIndex++;
                        isDouble = true;
                        break;
                    }
                    else
                    {
                        sheetIndex++;
                        isDouble = false;
                    }
                }
            }

            return isDouble;
        }

        private void ExportFilterdToText(string seperator, string fileName)
        {
            try
            {
                using StreamWriter sw = new (new FileStream(fileName, FileMode.Create));

                string aLine = string.Empty;
                aLine = this.TextstartEnd;

                // Header
                for (int i = 0; i < this.Datatable.Columns.Count; ++i)
                {
                    aLine += this.Datatable.Columns[i].ColumnName;
                    if (i < this.Datatable.Columns.Count - 1)
                    {
                        aLine += seperator;
                    }
                }

                aLine += this.TextstartEnd;
                sw.WriteLine(aLine);  // Write the header

                this.formMain.ProgressBarExport.Step = 1;

                aLine = string.Empty;

                foreach (DataRow dr in this.Datatable.Rows)
                {
                    aLine = this.TextstartEnd;
                    for (int i = 0; i < this.Datatable.Columns.Count; ++i)
                    {
                        aLine += dr[i].ToString();
                        if (i < this.Datatable.Columns.Count - 1)
                        {
                            aLine += seperator;
                        }
                    }

                    aLine += this.TextstartEnd;
                    sw.WriteLine(aLine);
                    this.formMain.ProgressBarExport.PerformStep();
                }

                this.formMain.ProgressBarExport.Visible = false;
                TdLogging.WriteToLogInformation("Opslaan Tekst bestand is gereed.");

                if (!this.SuppressDialogExportReady)
                {
                    MessageBox.Show("Opslaan bestand is gereed.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                this.formMain.ProgressBarExport.Visible = false;
                TdLogging.WriteToLogError("Exporteren naar Tekst bestand is mislukt.");
                TdLogging.WriteToLogError("Melding:");
                TdLogging.WriteToLogError(ex.Message);

                if (TdDebugMode.DebugMode)
                {
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
            }
        }

        private void DataTableToText(DataTable datatable, string seperator, string fileName)
        {
            if (datatable != null)
            {
                try
                {
                    using StreamWriter sw = new (new FileStream(fileName, FileMode.Create));
                    string aLine = string.Empty;
                    aLine = this.TextstartEnd;

                    // Header
                    for (int i = 0; i < datatable.Columns.Count; ++i)
                    {
                        aLine += datatable.Columns[i].ToString();
                        if (i < datatable.Columns.Count - 1)
                        {
                            aLine += seperator;
                        }
                    }

                    aLine += this.TextstartEnd;     // end row with "
                    sw.WriteLine(aLine);  // Write the header
                    aLine = string.Empty;

                    this.formMain.ProgressBarExport.Maximum = datatable.Rows.Count;
                    this.formMain.ProgressBarExport.Step = 1;
                    this.formMain.ProgressBarExport.Visible = true;
                    this.formMain.Refresh();

                    foreach (DataRow dr in datatable.Rows)
                    {
                        aLine = this.TextstartEnd; // start row with "
                        for (int i = 0; i < datatable.Columns.Count; ++i)
                        {
                            aLine += dr[i].ToString();
                            if (i < datatable.Columns.Count - 1)
                            {
                                aLine += seperator;
                            }
                        }

                        aLine += this.TextstartEnd; // end row with "
                        sw.WriteLine(aLine);
                        aLine = string.Empty;
                        this.formMain.ProgressBarExport.PerformStep();
                    }

                    if (!this.SuppressDialogExportReady)
                    {
                        MessageBox.Show("Opslaan bestand is gereed.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    sw.Flush();
                    this.formMain.ProgressBarExport.Visible = false;
                }
                catch (IOException ex)
                {
                    TdLogging.WriteToLogError("Exporteren van de gegevens naar een tekst bestand is mislukt.");
                    TdLogging.WriteToLogError("Melding:");
                    TdLogging.WriteToLogError(ex.Message);

                    if (TdDebugMode.DebugMode)
                    {
                        {
                            TdLogging.WriteToLogDebug(ex.ToString());
                        }
                    }
                }
            }
        }

        private void ExportFilterdToCSV(string seperator, string fileName)
        {
            try
            {
                using StreamWriter sw = new (new FileStream(fileName, FileMode.Create));

                string aLine = string.Empty;
                aLine = this.CSVstartEnd;

                // Header
                for (int i = 0; i < this.Datatable.Columns.Count; ++i)
                {
                    aLine += this.Datatable.Columns[i].ColumnName;
                    if (i < this.Datatable.Columns.Count - 1)
                    {
                        aLine += seperator;
                    }
                }

                aLine += this.CSVstartEnd;
                sw.WriteLine(aLine);  // Write the header

                this.formMain.ProgressBarExport.PerformStep();

                aLine = string.Empty;

                foreach (DataRow dr in this.Datatable.Rows)
                {
                    aLine = this.CSVstartEnd;
                    for (int i = 0; i < this.Datatable.Columns.Count; ++i)
                    {
                        aLine += dr[i].ToString();
                        if (i < this.Datatable.Columns.Count - 1)
                        {
                            aLine += seperator;
                        }
                    }

                    aLine += this.CSVstartEnd;
                    sw.WriteLine(aLine);
                    this.formMain.ProgressBarExport.PerformStep();
                }

                this.formMain.ProgressBarExport.Visible = false;
                TdLogging.WriteToLogInformation("Opslaan CSV bestand is gereed.");
                if (!this.SuppressDialogExportReady)
                {
                    MessageBox.Show("Opslaan bestand is gereed.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                this.formMain.ProgressBarExport.Visible = false;
                TdLogging.WriteToLogError("Exporteren naar CSV bestand is mislukt.");
                TdLogging.WriteToLogError("Melding : ");
                TdLogging.WriteToLogError(ex.Message);

                if (TdDebugMode.DebugMode)
                {
                    {
                        TdLogging.WriteToLogDebug(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Export the query result. Used by exporting multiple query's.
        /// </summary>
        /// <param name="fileName">Export file name.</param>
        /// <param name="worksheetName">Excel worksheet name.</param>
        /// <param name="useSameFile">Use the same file or not.</param>
        public void ExportData(string fileName, string worksheetName, bool useSameFile)
        {
            if (this.Datatable == null)
            {
                return;
            }

            TdLogging.WriteToLogDebug("ExportData.FileName = " + fileName);
            TdLogging.WriteToLogDebug("ExportData.WorksheetName = " + worksheetName);
            TdLogging.WriteToLogDebug("ExportData.UseSameFile = " + useSameFile.ToString());

            if (!string.IsNullOrEmpty(fileName))
            {
                Cursor.Current = Cursors.WaitCursor;
                this.WorksheetName = worksheetName;   // the worksheetname in a Excel file

                this.UseSameFile = useSameFile;       // if UseSameFile=true then a worksheet is added to the Excel document

                string extension = Path.GetExtension(fileName);
                switch (extension)
                {
                    case ".xlsx":
                        this.ExportToExcel(this.Datatable, fileName);
                        break;
                    case ".csv":
                        this.DataTableToCSV(this.Datatable, this.CsvSeparator, fileName);     // Write to the file name selected.
                        break;
                    case ".txt":
                        this.DataTableToCSV(this.Datatable, this.TextSeparator, fileName);     // Write to the file name selected.
                        break;
                }

                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Bestandsnaam ontbreekt.", "Fout.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region Dispose
        private readonly SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        private bool disposed;

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Has Dispose already been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.safeHandle?.Dispose();

                    // Free other state (managed objects).
                    this.Datatable = null;
                    this.saveDataFileDialog.Dispose();
                }

                // Free your own state (unmanaged objects). Set large fields to null.
                this.disposed = true;
            }
        }
        #endregion Dispose
    }
}
