using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2.Models
{
    // Таблица: группы пациентов.
    public class patient_group
    {
        [Key]
		[Required(ErrorMessage = "Не указан номер группы")]
		public int id_group { get; set; }
        public string? title { get; set; }
    }
    // Таблица: архив пациентов.
    public class patient_archive
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Не указан номер пациента")]
        public int id_patient { get; set; }
        public string? surname { get; set; }
        public string? name { get; set; }
        public string? patronymic { get; set; }
        [Required(ErrorMessage = "Не указана дата рождения")]
        public DateOnly birthday { get; set; }
        public string? phone_num { get; set; }
    }
    // Таблица: пациенты и их группа.
    [PrimaryKey(nameof(id_patient), nameof(id_group))] 
    public class Archive_Group
    {
		public int id_patient { get; set; }
        [Required(ErrorMessage = "Не указана группа")]
        public int id_group { get; set; }
		[ForeignKey("id_patient")]
        public patient_archive? patient_archive{ get; set; }
        [ForeignKey("id_group")]
        public patient_group? patient_group { get; set; }
    }
    // Таблица: архив анализов.
    public class Analyzes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Не указан номер анализа")]
        public int id_analysis { get; set; }
        public DateOnly date { get; set; }
        [Required(ErrorMessage = "Не указан кальций сыворотки крови")]
        public double serum_calcium { set; get; }
        [Required(ErrorMessage = "Не указан фосфор сыворотки крови")]
        public double serum_phosphorus { set; get; }
        [Required(ErrorMessage = "Не указан оксипролин сыворотки крови")]
        public double serum_oxyproline { set; get; }
        [Required(ErrorMessage = "Не указана экскреция кальция")]
        public double calcium_excretion { set; get; }
        [Required(ErrorMessage = "Не указана экскреция фосфора")]
        public double phosphorus_excretion { set; get; }
        [Required(ErrorMessage = "Не указана экскреция оксипролина")]
        public double oxyproline_excretion { set; get; }
        [Required(ErrorMessage = "Не указана степень выраженности ДСТ")]
        public int severity_of_dst { set; get; }
    }
    // Таблица: пациенты и их анализы.
    [PrimaryKey(nameof(id_patient), nameof(id_analysis))]
    public class Patient_Analyzes
    {
        public int id_patient { get; set; }
        public int id_analysis { get; set; }
        [ForeignKey("id_patient")]
        public patient_archive? patient_archive { get; set; }
        [ForeignKey("id_analysis")]
        public Analyzes? analyzes { get; set; }
    }
    // Таблица: результаты диспансеризации.
    public class Examination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Не указан номер экспертизы")]
        public int id_examination { get; set; }
        [Required(ErrorMessage = "Не указана дата прохождения")]
        public DateOnly date { get; set; }
        [Required(ErrorMessage = "Не указано время удержания позы")]
        public double posture_holding_time { set; get; }
        [Required(ErrorMessage = "Не указана величина кифоза в градусах в грудном отделе позвоночника")]
        public double the_amount_of_kyphosis_in_degress { set; get; }
        public Boolean changing_the_contours_of_the_end_plates { set; get; }
        public Boolean wedgeshaped_vertebral_bodies { set; get; }
        public Boolean schmorl_hernia { set; get; }
        public Boolean osteoporosis_of_the_vertebrae { set; get; }
        public Boolean decrease_in_the_height_of_the_intervertebral_disc { set; get; }
        public Boolean change_in_the_contours_of_the_apophyses { set; get; }
        public Boolean signs_of_osteochondrosis { set; get; }
        public Boolean stabilographic_changes { set; get; }
        public Boolean enmg { set; get; }
    }
    // Таблица: пациенты и их результаты диспансеризации.
    [PrimaryKey(nameof(id_patient), nameof(id_examination))]
    public class Patient_Examination
    {
        public int id_patient { get; set; }
        public int id_examination { get; set; }
        [ForeignKey("id_patient")]
        public patient_archive? patient_archive { get; set; }
        [ForeignKey("id_examination")]
        public Examination? examination { get; set; }
    }
    // Таблица: роли пользователей.
    public class Role:IdentityRole
    {
    }
    // Таблица: пользователи.
    public class User: IdentityUser
    {
        [Required(ErrorMessage = "Не указан логин")]
		public string Login { get; set; }
		[Required(ErrorMessage = "Не указана фамилия")]
		public string Surname { get; set; }
		[Required(ErrorMessage = "Не указано имя")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Не указан номер телефона")]
        [Phone]
		public string Phone { get; set; }
    }
    // Таблица: доктора и их пациенты.
    [PrimaryKey(nameof(id_user), nameof(id_patient))]
    public class User_Patients
    {
        public string id_user { get; set; }
        public int id_patient { get; set; }
        [ForeignKey("id_user")]
        public User? user { get; set; }
        [ForeignKey("id_patient")]
        public patient_archive? patient_archive { get; set; }
    }
}