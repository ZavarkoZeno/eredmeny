using System;
namespace eredmeny.Models.DTOs
{
    public class AddEredmenyDTO
    {
        public string? Competition { get; set; }
        public string? Description { get; set; }
        public DateTime ResultTime { get; set; }
        public int SportoloId { get; set; }
    }
}
