using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDBootcamp32.Model
{
    [Table("tb_m_item")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Supplier Supplier { get; set; }

        public Item() { }
        public Item(string name, int stock, int price, Supplier supplier)
        {
            this.Name = name;
            this.Stock = stock;
            this.Price = price;
            this.Supplier = supplier;
            this.CreateDate = DateTimeOffset.Now.LocalDateTime;
            
        }
    }
}
