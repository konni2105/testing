using System;
using System.Collections.Generic;
using System.Text;

namespace EduTek.Infrastructure.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
