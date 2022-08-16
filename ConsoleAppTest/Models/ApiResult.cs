namespace ConsoleAppTest.Models
{
    public class ApiResult
    {
        public string value { get; set; }
        public bool success { get; set; }
        public object errors { get; set; }
        public int code { get; set; }
    }
}