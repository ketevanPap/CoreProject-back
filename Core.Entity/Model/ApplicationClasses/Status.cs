using System.ComponentModel.DataAnnotations;

namespace ProjectCore.Entity.Model.ApplicationClasses
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
