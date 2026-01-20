using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Response
{
    public class ProductResponse
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        public string CreatedBy { get; set; }
        public int Id { get; set; }
        public string MainImage { get; set; }
      //  public List<string> SubImages { get; set; }
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
