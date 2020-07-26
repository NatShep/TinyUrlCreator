namespace TinyURl.MVC.Models.ViewModels
{
    public class UrlModel
    {
        public string OriginalUrl { get; set; } = "";
        public string TinyPath { get; set; } = "";
        public bool UrlExist { get; set; } = false;
        public string FullTinyUrl => "https://localhost:5001/TinyUrl/" + TinyPath;
    }
}