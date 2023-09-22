using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ModelRender.Helpers
{
    public class JSONHelper
    {
        public static string MergeJsonObjects(params string[] jsonStrings)
        {
            Dictionary<string, object> mergedDict = new Dictionary<string, object>();

            foreach (var jsonString in jsonStrings)
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                foreach (var pair in dict)
                {
                    mergedDict[pair.Key] = pair.Value;
                }
            }

            return JsonSerializer.Serialize(mergedDict);
        }
    }
}
