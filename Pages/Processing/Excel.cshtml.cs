using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Printing;
using Web2.Models;
using Microsoft.AspNetCore.Hosting;
using ExcelDataReader;
using System.Data;

namespace Web2.Pages
{
    public class ExcelModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Web2.Data.AppDbContext _context;
        [BindProperty]
        public List<List<string>> Data { get; set; }
        public ExcelModel(Web2.Data.AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            Data = new List<List<string>>();
            _webHostEnvironment = webHostEnvironment;
        }
        // модели для разных таблиц БД
        public patient_archive Patient { get; set; }
        public Analyzes AnalyzesP { get; set; }
        public Examination ExaminationP { get; set; }
        public Archive_Group Archive { get; set; }
        public Patient_Analyzes PA { get; set; }
        public Patient_Examination PE { get; set; }
        // выбор таблицы БД
        public List<SelectListItem>? GetName()
        {
            List<SelectListItem> group = new List<SelectListItem>();
            group.Add(new SelectListItem() { Text = "Архив пациентов", Value = "patient_archive" });
            group.Add(new SelectListItem() { Text = "Анализы", Value = "analyzes" });
            group.Add(new SelectListItem() { Text = "Результаты диспансеризации", Value = "examination" });
            group.Add(new SelectListItem() { Text = "Связь: пациенты-группы", Value = "archive_group" });
            group.Add(new SelectListItem() { Text = "Связь: пациенты-анализы", Value = "patient_analyzes" });
            group.Add(new SelectListItem() { Text = "Связь: пациенты-диспансеризация", Value = "patient_examinations" });
            return group;
        }
        public void OnGet(){}
        // ввод данных из Excel файла в БД
        public async Task<IActionResult> OnPostAdd(string table_name)
        {
            try
            {
                switch (table_name)
                {
                    case "patient_archive":
                        if (Data.Count == 6) {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                Patient = new patient_archive();
                                Patient.name = Data[1][i];
                                Patient.surname = Data[2][i];
                                Patient.patronymic = Data[3][i];
                                var dateTime = DateTime.Parse(Data[4][i]);
                                Patient.birthday = DateOnly.FromDateTime(dateTime);
                                Patient.phone_num = Data[5][i];

                                _context.patient_archive.Add(Patient);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "Неверные данные");
                        break;
                    case "analyzes":
                        if (Data.Count == 8)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                AnalyzesP = new Analyzes();
                                AnalyzesP.serum_calcium = Convert.ToDouble(Data[1][i]);
                                AnalyzesP.serum_phosphorus = Convert.ToDouble(Data[2][i]);
                                AnalyzesP.serum_oxyproline = Convert.ToDouble(Data[3][i]);
                                AnalyzesP.calcium_excretion = Convert.ToDouble(Data[4][i]);
                                AnalyzesP.phosphorus_excretion = Convert.ToDouble(Data[5][i]);
                                AnalyzesP.oxyproline_excretion = Convert.ToDouble(Data[6][i]);
                                AnalyzesP.severity_of_dst = Convert.ToInt32(Data[7][i]);

                                _context.analyzes.Add(AnalyzesP);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "Неверные данные");
                        break;
                    case "examination":
                        if (Data.Count == 13)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                ExaminationP = new Examination();
                                var dateTime = DateTime.Parse(Data[1][i]);
                                ExaminationP.date = DateOnly.FromDateTime(dateTime);
                                ExaminationP.posture_holding_time = Convert.ToDouble(Data[2][i]);
                                ExaminationP.the_amount_of_kyphosis_in_degress = Convert.ToDouble(Data[3][i]);
                                ExaminationP.changing_the_contours_of_the_end_plates = Convert.ToBoolean(Data[4][i]);
                                ExaminationP.wedgeshaped_vertebral_bodies = Convert.ToBoolean(Data[5][i]);
                                ExaminationP.schmorl_hernia = Convert.ToBoolean(Data[6][i]);
                                ExaminationP.osteoporosis_of_the_vertebrae = Convert.ToBoolean(Data[7][i]);
                                ExaminationP.decrease_in_the_height_of_the_intervertebral_disc = Convert.ToBoolean(Data[8][i]);
                                ExaminationP.change_in_the_contours_of_the_apophyses = Convert.ToBoolean(Data[9][i]);
                                ExaminationP.signs_of_osteochondrosis = Convert.ToBoolean(Data[10][i]);
                                ExaminationP.stabilographic_changes = Convert.ToBoolean(Data[11][i]);
                                ExaminationP.enmg = Convert.ToBoolean(Data[12][i]);

                                _context.analyzes.Add(AnalyzesP);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "Неверные данные");
                        break;
                    case "archive_group":
                        if (Data.Count == 2)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                Archive = new Archive_Group();
                                Archive.id_patient= Convert.ToInt32(Data[0][i]);
                                Archive.id_group= Convert.ToInt32(Data[1][i]);

                                _context.archive_group.Add(Archive);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "Неверные данные");
                        break;
                    case "patient_analyzes":
                        if (Data.Count == 2)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                PA = new Patient_Analyzes();
                                PA.id_patient = Convert.ToInt32(Data[0][i]);
                                PA.id_analysis = Convert.ToInt32(Data[1][i]);

                                _context.patient_analyzes.Add(PA);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "Неверные данные");
                        break;
                    case "patient_examinations":
                        if (Data.Count == 2)
                        {
                            for (int i = 0; i < Data[0].Count; i++)
                            {
                                PE = new Patient_Examination();
                                PE.id_patient = Convert.ToInt32(Data[0][i]);
                                PE.id_examination = Convert.ToInt32(Data[1][i]);

                                _context.patient_examinations.Add(PE);
                                await _context.SaveChangesAsync();
                            }
                            break;
                        }
                        ModelState.AddModelError("", "Неверные данные");
                        break;
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Error");
            }
            Data.Clear();
            return Page();
        }
        // подгрузка файла
        DataTableCollection ReadFromExcel(string filePath, ref List<string> sheetNames)
        {
            try
            {
                DataTableCollection tableCollection = null;

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                        });

                        tableCollection = result.Tables;

                        foreach (DataTable table in tableCollection)
                        {
                            sheetNames.Add(table.TableName);
                        }
                    }
                }

                return tableCollection;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // получение данных из Excel файла
        public async Task<IActionResult> OnPostImport(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    Data = new List<List<string>>();

                    List<string> sheetNames = new List<string>();

                    string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string rootPath = _webHostEnvironment.ContentRootPath;
                    string filePath = Path.Combine(rootPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    DataTableCollection tables = ReadFromExcel(filePath, ref sheetNames);

                    foreach (DataTable dt in tables)
                    {
                        var Colum = new List<string>();
                        for (int k = 0; k < dt.Columns.Count; k++)
                            Colum.Add(dt.Columns[k].ToString());
                        Data.Add(Colum);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var row = new List<string>();
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                row.Add(dt.Rows[i][j].ToString());
                            }
                            Data.Add(row);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Data.Clear();
                    ModelState.AddModelError("", "Файл не подходит");
                    return Page();
                }
            }
            return Page();
        }  
    }
}