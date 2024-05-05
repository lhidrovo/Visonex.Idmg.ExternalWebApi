namespace Visonex.Idmg.ExternalWebApi.Exceptions
{
    [Serializable]
    public class HttpResponseException : Exception
    {
        public int Status { get; set; }
        public string Msg { get; set; }
    }
}
