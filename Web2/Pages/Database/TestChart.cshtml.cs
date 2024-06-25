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
        public int? G;

        public int? B = 1;
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

        public void OnGet(string G1)
        {
            G = Convert.ToInt32(G1);
        }

        public async Task<IActionResult> OnPostAbc(string G1)
        { 
            G = Convert.ToInt32(G1);
            return new JsonResult(G);
        }

        // получение данных для графика : "кол-во патологий"
        public List<PathologCount> GetData1()
        {
            List<PathologCount> pathologies = new List<PathologCount>();
            int[]? nums = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int? group = G;

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

    }
}
