
namespace GameZone.Models
{
    public class Game : BaseEntity
    {
        
        [MaxLength(2500)]
        public string Description { get; set; } = null!;
        public string Cover { get; set; } = null!;
        //Foreign Key
        public int CategoryId { get; set; }
        //Navigation Property
        public Category Category { get; set; } = default!;
        public ICollection<GameDevice> Devices { get; set; } = new List<GameDevice>();
    }
}
