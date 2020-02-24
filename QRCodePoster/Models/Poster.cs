using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace QRCodePoster.Models
{
    public class Poster : EntityBase<int>
    {
        /// <summary>
        /// 背景地址
        /// </summary>
        public string BGUrl { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        [NotMapped]
        public IEnumerable<PosterItem> PosterData
        {
            get
            {
                return JsonSerializer.Deserialize<IEnumerable<PosterItem>>(Data);
            }
            set
            {
                Data = JsonSerializer.Serialize(value);
            }
        }
    }
}
