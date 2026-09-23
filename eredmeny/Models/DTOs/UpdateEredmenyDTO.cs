namespace eredmeny.Models.DTOs
{
    public class UpdateEredmenyDTO
    {
        public int Id { get; set; }
        public string? Competition { get; set; }
        public string? Description { get; set; }
        public DateTime ResultTime { get; set; }
        public int SportoloId { get; set; }
    }
}
