namespace ConsoleAppTest.DataAccess
{
    public interface IHttpDataAccess
    {
        Task<OutputType> GetAsync<OutputType>(string url) where OutputType : class;
        Task<OutputType> GetAsync<OutputType>(string url, string token) where OutputType : class;
        Task<OutputType> PostAsync<InputType, OutputType>(string url, InputType input)
            where InputType : class
            where OutputType : class;
        Task<OutputType> PostAsync<InputType, OutputType>(string url, string token, InputType input)
            where InputType : class
            where OutputType : class;
        Task<OutputType> PostAsync<OutputType>(string url, OutputType input) where OutputType : class;
    }
}