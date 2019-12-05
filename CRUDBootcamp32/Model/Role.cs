using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDBootcamp32.Model
{
    [Table ("tb_m_role")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public Role() { }
        public Role(string name)
        {
            this.Name = name;
        }
    }
}
