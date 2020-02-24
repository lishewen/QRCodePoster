using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodePoster.Models
{
	public class UploadFileResult
	{
		public string name { get; set; }
		public string ext { get; set; }
		public string filename { get; set; }
		public string attachment { get; set; }
		public string url { get; set; }
		public int is_image { get; set; }
		public int filesize { get; set; }
		public int width { get; set; }
		public int height { get; set; }
	}
}
