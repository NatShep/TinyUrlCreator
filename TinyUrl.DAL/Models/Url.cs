using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyUrl.DAL.Models
{
    public class Url:EntityBase
    {
        [MaxLength(8), MinLength(8), Required] public string TinyPath { get; set; }

        public ulong NumberOfTransitions { get; set; } = 0;
        public string OriginalPath { get; set; }
        
        public int UserId { get; set; } 
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        
        [NotMapped]
        public string FullTinyPath => "https://localhost:5001/TinyUrl/"+ TinyPath;
        
        // TODO Give the User's name for Url

        public void IncreaseNumberOfTransitions()
        {
            NumberOfTransitions++;
        }

    }
}