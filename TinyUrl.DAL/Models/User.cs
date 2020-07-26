using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TinyUrl.DAL.Models
{
    public class User:EntityBase
    {
        [StringLength(20, ErrorMessage = "User Name must be 20 characters or less"), Required]
        public string UserName { get; set; }

        [StringLength(20), Required]
        public string Password { get; set; }

        public string HistoryString { get; set; } = "";
        
        public List<Url> TinyUrls { get; set; } = new List<Url>();

        [NotMapped] public string[] History => GetHistory(HistoryString);

        private string[] GetHistory(string historyString)=>
             historyString.Split(',');
    }
}