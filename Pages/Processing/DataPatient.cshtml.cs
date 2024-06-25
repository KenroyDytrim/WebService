using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Pages
{
    public class DataPatientModel : PageModel
    {
        private readonly Web2.Data.AppDbContext _context;

        public DataPatientModel(Web2.Data.AppDbContext context)
        {
            _context = context;
        }
        // модель для получения анализов пациента
        public class Analyzes2
        {
            public int id_analysis { get; set; }
            public int id_examination { get; set; }
            public double serum_calcium { set; get; }
            public double serum_phosphorus { set; get; }
            public double serum_oxyproline { set; get; }
            public double calcium_excretion { set; get; }
            public double phosphorus_excretion { set; get; }
            public double oxyproline_excretion { set; get; }
            public int severity_of_dst { set; get; }
            public double posture_holding_time { set; get; }
            public double the_amount_of_kyphosis_in_degress { set; get; }
            public bool stabilographic_changes { set; get; }
            public Analyzes2 Clone()
            {
                return (Analyzes2)this.MemberwiseClone();
            }

        }

        public IList<Analyzes> analyzes { get; set; } = default!;
        public List<Analyzes2> analyzes2 = new List<Analyzes2>();
        public IList<Examination> examinations { get; set; } = default!;
        // получение анализов пациента
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            analyzes =  await _context.analyzes.FromSqlRaw($"SELECT * FROM analyzes WHERE id_analysis IN(SELECT id_analysis FROM patient_analyzes WHERE id_patient={id})").ToListAsync();
            examinations = await _context.examination.FromSqlRaw($"SELECT * FROM examination WHERE id_examination IN(SELECT id_examination FROM patient_examinations WHERE id_patient={id})").ToListAsync();


            for (int i = 0; i < analyzes.Count; i++)
            {
                Analyzes2 analyzesT = new Analyzes2();

                analyzesT.id_analysis = analyzes[i].id_analysis;
                analyzesT.serum_calcium= analyzes[i].serum_calcium;
                analyzesT.serum_phosphorus = analyzes[i].serum_phosphorus;
                analyzesT.serum_oxyproline = analyzes[i].serum_oxyproline;
                analyzesT.calcium_excretion = analyzes[i].calcium_excretion;
                analyzesT.phosphorus_excretion = analyzes[i].phosphorus_excretion;
                analyzesT.oxyproline_excretion = analyzes[i].oxyproline_excretion;
                analyzesT.severity_of_dst = analyzes[i].severity_of_dst;

                for (int j = 0; j < examinations.Count; j++)
                {
                    analyzesT.id_examination = examinations[j].id_examination;
                    analyzesT.posture_holding_time = examinations[j].posture_holding_time;
                    analyzesT.the_amount_of_kyphosis_in_degress = examinations[j].the_amount_of_kyphosis_in_degress;
                    analyzesT.stabilographic_changes = examinations[j].stabilographic_changes;
                    analyzes2.Add(analyzesT.Clone());
                }

            }

            return Page();
        }
    }
}