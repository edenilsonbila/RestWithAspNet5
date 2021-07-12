using CalculadoraRest.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalculadoraRest.Model
{
    [Table("person")]
    public class Person : BaseEntity
    {
        [Column("first_name")]
        public string FirtName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("gender")]
        public string Gender { get; set; } 
        
        [Column("enabled")]
        public bool Enabled { get; set; }
    }
}
