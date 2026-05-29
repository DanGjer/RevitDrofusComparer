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

        [JsonPropertyName("is_sub_occurrence")]
        public bool IsSubItem {get; set;} = false;

        [JsonPropertyName("parent_occurrence_id_id")]
        public int? HostId {get; set;} = null;

        [JsonPropertyName("parent_occurrence_id_article_id_name")]
        public string? HostItemName {get; set;} = "";

        [JsonPropertyName("occurrence_classification_156_classification_entry_id_code")]
        public string? OmegaStatus {get; set;} = "";

        public DrofusStatus Status {get; set;} = DrofusStatus.Unknown;

        public enum DrofusStatus        {
            Unknown = 0,
            OK = 1,
            MissingInRevit = 2,

        }
    }
}