using System.ComponentModel.DataAnnotations;

namespace TinyUrl.DAL.Models
{
    public class EntityBase
    {
        [Key] 
        public int Id { get; set; }
    }
}