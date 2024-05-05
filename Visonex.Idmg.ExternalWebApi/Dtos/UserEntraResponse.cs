using Newtonsoft.Json;

namespace Visonex.Idmg.ExternalWebApi.Dtos
{
    public class UserEntraResponse
    {
        [JsonProperty("@odata.context")]
        public string? ODataContext { get; set; }

        public UserInfo[]? value { get; set; }
    }
}
