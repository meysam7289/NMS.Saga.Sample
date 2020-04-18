using System.ComponentModel.DataAnnotations;

namespace NMS.Saga.Sample.Contracts.Models
{
    public class Coding
    {
        [Key]
        public int IdCoding { get; set; }
        public string Title { get; set; }
    }
}
