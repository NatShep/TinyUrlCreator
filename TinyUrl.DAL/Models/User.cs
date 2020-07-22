using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TinyUrl.DAL.Models
{
    public class User:EntityBase
    {
        [StringLength(20, ErrorMessage = "User Name must be 20 characters or less"), Required]
        public string UserName { get; set; }

        [StringLength(20), Required]
        public string Password { get; set; }

        public string HistoryString { get; set; }
        public List<Url> TinyUrls { get; set; } = new List<Url>();

   //     [NotMapped] public int[] History => GetHistory(HistoryString);

   /*     private int[] GetHistory(string history)
        {
            //plug
            return new int[0];
        }*/
    }
}