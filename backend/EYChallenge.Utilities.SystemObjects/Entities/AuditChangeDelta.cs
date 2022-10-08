using Newtonsoft.Json;

namespace EYChallenge.Utilities.SystemObjects.Entities
{
    public class AuditChangeDelta
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }
        [JsonProperty("valueBefore")]
        public string ValueBefore { get; set; }
        [JsonProperty("valueAfter")]
        public string ValueAfter { get; set; }

        public AuditChangeDelta(string fieldName, string valueBefore, string valueAfter)
        {

            FieldName = fieldName;
            ValueBefore = valueBefore;
            ValueAfter = valueAfter;
        }
    }
}
