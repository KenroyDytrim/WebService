namespace Web2.Models
{
    // Подсчёт патологий.
    public class PathologCount
    {
        public string? Name { get; set; }
        public int Count { get; set; }
    }
    // Распределение показателей.
    public class Distribution
    {
        public string? Pocket { get; set; }
        public int Count { get; set; }
    }
}
