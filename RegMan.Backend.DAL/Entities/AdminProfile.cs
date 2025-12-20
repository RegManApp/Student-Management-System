using RegMan.Backend.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegMan.Backend.DAL.Entities
{
    public class AdminProfile
    {
        [Key] public int StaffId { get; set; }

        [Required] public string Title { get; set; } = null!;
        public string UserId { get; set; } = null!; //fk
        public BaseUser User { get; set; } = null!;
    }
}