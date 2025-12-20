using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegMan.Backend.DAL.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
