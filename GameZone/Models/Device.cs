
namespace GameZone.Models
{
    public class Device : BaseEntity
    {
        [MaxLength(50)]
        public string Icon { get; set; } = null!;
       // public ICollection<GameDevice>  Games { get; set; } = new List<GameDevice>();
    }
}
