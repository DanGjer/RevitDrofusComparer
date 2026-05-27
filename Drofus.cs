using System.Text.Json.Serialization;

namespace RevitDrofusComparer;

public static class DrofusHelper
{
    public class DrofusElement
    {
        [JsonPropertyName("id")]
        public int DrofusId {get; set;} = 0;
        
        
        [JsonPropertyName("classification_number")]
        public string? DrofusTag {get; set;} = "";

        [JsonPropertyName("article_id_name")]
        public string? DrofusArticleName {get; set;} = "";


        [JsonPropertyName("occurrence_data_17_11_11_10")]
        public string? DrofusModelName {get; set;} = "";


        [JsonPropertyName("ifc_guids_text_property")]
        public string? DrofusGuid {get; set;} = "";
    }
}