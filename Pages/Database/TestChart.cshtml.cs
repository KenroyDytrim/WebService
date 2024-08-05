using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Web2.Data;
using Web2.Models;

namespace Web2.Pages.Database
{
    public class TestChartModel : PageModel
    {
        public int? Gp = 0;
        public int? Gr = 0;
        public int? B = 1;

        public string? col = "#DC143C";

        public string NameA = "Кальций сыворотки крови (ммоль/л)";
        // выбор группы
        public List<SelectListItem>? GetGroup()
        {
            List<SelectListItem> group = new List<SelectListItem>();
            group.Add(new SelectListItem() { Text = "Контрольная группа", Value = "1" });
            group.Add(new SelectListItem() { Text = "Основная группа", Value = "2" });
            group.Add(new SelectListItem() { Text = "Группа сравнения", Value = "3" });
            return group;
        }
        // выбор показателя
        public List<SelectListItem>? GetAnalysis()
        {
            List<SelectListItem> analysis = new List<SelectListItem>();
            analysis.Add(new SelectListItem() { Text = "Кальций сыворотки крови (ммоль/л)", Value = "1" });
            analysis.Add(new SelectListItem() { Text = "Фосфор сыворотки крови (ммоль/л)", Value = "2" });
            analysis.Add(new SelectListItem() { Text = "Оксипролин сыворотки крови (ммоль/л)", Value = "3" });
            analysis.Add(new SelectListItem() { Text = "Экскреция кальция (мМ/сутки)", Value = "4" });
            analysis.Add(new SelectListItem() { Text = "Экскреция фосфора (ммоль/л)", Value = "5" });
            analysis.Add(new SelectListItem() { Text = "Экскреция оксипролина (мкм/мг креатинина)", Value = "6" });
            analysis.Add(new SelectListItem() { Text = "Степень выраженности ДСТ по Т.Милковской-Димитровой (в баллах)", Value = "7" });
            return analysis;
        }

        private readonly AppDbContext _context;

        public int[]? nums = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public string[] name = { "IzmenKontr", "KlinForm", "GrajaSmorl", "Osteoporoz", "UmenVisoti", "IzmenOpof", "Osteohondroz", "StabIzmen", "ENMG" };

        public List<double> data = new List<double>();
        public TestChartModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Examination> examination { get; set; } = default!;
        public IList<Web2.Models.Analyzes> analyzes { get; set; } = default!;

        public void OnGet()
        {

        }

        public void OnPost(int? G1, int? G2, int? K, string? bg)
        {
            Gp = G1;
            Gr = G2;
            B = K;

            if (bg != null) 
            { 
                col = bg;
            }
            

            switch (B)
            {
                case 1:
                    NameA = "Кальций сыворотки крови (ммоль/л)";
                    break;
                case 2:
                    NameA = "Фосфор сыворотки крови (ммоль/л)";
                    break;
                case 3:
                    NameA = "Оксипролин сыворотки крови (ммоль/л)";
                    break;
                case 4:
                    NameA = "Экскреция кальция (мМ/сутки)";
                    break;
                case 5:
                    NameA = "Экскреция фосфора (ммоль/л)";
                    break;
                case 6:
                    NameA = "Экскреция оксипролина (мкм/мг креатинина)";
                    break;
                case 7:
                    NameA = "Степень выраженности ДСТ по Т.Милковской-Димитровой (в баллах)";
                    break;
            }
        }

        // получение данных для графика : "кол-во патологий"
        public List<PathologCount> GetData1(int? G1)
        {
            List<PathologCount> pathologies = new List<PathologCount>();
            int[]? nums = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int? group = G1;

            if (group == 0 || group == null)
                examination = _context.examination.FromSqlRaw($"SELECT * FROM examination ORDER BY id_examination").ToList();
            else
                examination = _context.examination.FromSqlRaw($"SELECT * FROM examination WHERE id_examination IN(SELECT id_examination FROM patient_examinations WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM archive_group WHERE id_group={group}) ORDER BY id_patient)) ORDER BY id_examination").ToList();

            foreach (var item in examination)
            {
                nums[0] += Convert.ToInt32(item.changing_the_contours_of_the_end_plates);
                nums[1] += Convert.ToInt32(item.wedgeshaped_vertebral_bodies);
                nums[2] += Convert.ToInt32(item.schmorl_hernia);
                nums[3] += Convert.ToInt32(item.osteoporosis_of_the_vertebrae);
                nums[4] += Convert.ToInt32(item.decrease_in_the_height_of_the_intervertebral_disc);
                nums[5] += Convert.ToInt32(item.change_in_the_contours_of_the_apophyses);
                nums[6] += Convert.ToInt32(item.signs_of_osteochondrosis);
                nums[7] += Convert.ToInt32(item.stabilographic_changes);
                nums[8] += Convert.ToInt32(item.enmg);
            }

            for (int i = 0; i < nums.Count(); i++)
            {
                pathologies.Add(new PathologCount()
                {
                    Name = name[i],
                    Count = nums[i]
                });
            }

            return pathologies;
        }

        // получение данных для графика: "Распределение показателей"
        public List<Distribution> GetData2(int? K, int? G2)
        {
            List<Distribution> distribution = new List<Distribution>();
            data.Clear();
            int? n = 1;
            
            if ( K != null)
            { 
                n = K;
            }
            
            int? group = G2;

            if (group == 0 || group == null)
                analyzes = _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes ORDER BY id_analysis").ToList();
            else
                analyzes = _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes WHERE id_analysis IN(SELECT id_analysis FROM patient_analyzes WHERE id_patient IN(SELECT id_patient FROM patient_archive WHERE id_patient IN(SELECT id_patient FROM archive_group WHERE id_group={group}) ORDER BY id_patient)) ORDER BY id_analysis").ToList();

            foreach (var item in analyzes)
            {
                switch (n)
                {
                    case 1:
                        data.Add(item.serum_calcium);
                        break;
                    case 2:
                        data.Add(item.serum_phosphorus);
                        break;
                    case 3:
                        data.Add(item.serum_oxyproline);
                        break;
                    case 4:
                        data.Add(item.calcium_excretion);
                        break;
                    case 5:
                        data.Add(item.phosphorus_excretion);
                        break;
                    case 6:
                        data.Add(item.oxyproline_excretion);
                        break;
                    case 7:
                        data.Add(item.severity_of_dst);
                        break;
                }
            }

            if (data.Count > 0)
            {
                int pocket = (int)Math.Sqrt(data.Count());
                double min = data.Min();
                double max = data.Max();
                double sizeP = (max - min) / pocket;

                List<double> Pockets = new List<double>();
                Pockets.Add(min);
                for (int i = 0; i < pocket; i++)
                {
                    Pockets.Add(Pockets[i] + sizeP);
                }
                int[] dataP = new int[pocket + 1];
                Array.Clear(dataP, 0, dataP.Length);

                for (int i = 0; i < Pockets.Count - 1; i++)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (data[j] >= Pockets[i] && data[j] <= Pockets[i + 1])
                            dataP[i]++;
                    }
                }

                for (int i = 0; i < dataP.Count(); i++)
                {
                    distribution.Add(new Distribution()
                    {
                        Pocket = Pockets[i].ToString("0.00"),
                        Count = dataP[i]
                    });
                }
            }

            return distribution;
        }
    }
}