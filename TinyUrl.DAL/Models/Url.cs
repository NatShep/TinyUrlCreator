using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TinyUrl.DAL.Models
{
    public class Url:EntityBase
    {
        
        [MaxLength(8), MinLength(8), Required] public string TinyPath { get; set; }
        
        public string NumberOfTransitions { get; set; }
        public string OriginalPath { get; set; }
        public int UserId { get; set; } 
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

  /*      public void IncreaseNumberOfTransitions()
        {
            //dump
        }
    */   
        
    }
}