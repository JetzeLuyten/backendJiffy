using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiffyBackend.DAL.Entity
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
