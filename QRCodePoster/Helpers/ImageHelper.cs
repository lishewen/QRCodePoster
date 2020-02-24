using Microsoft.AspNetCore.Hosting;
using QRCodePoster.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace QRCodePoster.Helpers
{
    public class ImageHelper
    {
        private readonly string BasePath;
        private readonly IHttpClientFactory clientFactory;
        public ImageHelper(IWebHostEnvironment hostEnvironment, IHttpClientFactory clientFactory)
        {
            BasePath = hostEnvironment.WebRootPath;
            this.clientFactory = clientFactory;
        }
        /// <summary>
        /// 合成海报方法
        /// </summary>
        /// <param name="poster">海报元素</param>
        /// <param name="qr">填充元素</param>
        /// <returns></returns>
        public async Task<Image> CombinPoster(Poster poster, PosterQR qr)
        {
            //加载背景
            Image imgBack;
            if (!string.IsNullOrEmpty(poster.BGUrl))
                imgBack = Image.FromFile(BasePath + @"/attachment/" + poster.BGUrl);
            else
                imgBack = new Bitmap(640, 1008);
            imgBack = GetThumbnail(imgBack, 640, 1008);
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics        
            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);

            foreach (var m in poster.PosterData)
            {
                PointF p = new PointF(Convert.ToSingle(m.left.Replace("px", "")) * 2, Convert.ToSingle(m.top.Replace("px", "")) * 2);
                if (m.type == "marketprice" || m.type == "productprice")
                {
                    if (string.IsNullOrWhiteSpace(m.src))
                        m.src = m.type;

                    g.DrawString(m.src, new Font("微软雅黑", Convert.ToSingle(m.size.Replace("px", ""))), new SolidBrush(ColorTranslator.FromHtml(m.color)), p);
                }
                else if (m.type == "nickname" || m.type == "title")
                {
                    //名称
                    m.src = qr.Name;
                    g.DrawString(m.src, new Font("微软雅黑", Convert.ToSingle(m.size.Replace("px", ""))), new SolidBrush(ColorTranslator.FromHtml(m.color)), p);
                }
                else
                {
                    Image img;
                    int imgwidth = Convert.ToInt32(m.width.Replace("px", "")) * 2;
                    int imgheight = Convert.ToInt32(m.height.Replace("px", "")) * 2;
                    if (m.type == "qr")
                    {
                        //二维码的Url
                        string url = $"https://bc.lishewen.com/Details/{qr.Id}";
                        img = QRCodeHelper.GetQRCode(url, imgwidth, imgheight);
                    }
                    else if (m.type == "head")
                    {
                        if (string.IsNullOrWhiteSpace(qr.avatarUrl))
                            img = Image.FromFile(BasePath + @"/images/img.jpg");
                        else
                            img = await FromUrl(qr.avatarUrl);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(m.src))
                            img = Image.FromFile(BasePath + @"/images/img.jpg");
                        else
                            img = Image.FromFile(BasePath + @"/attachment/" + m.src);
                    }
                    g.DrawImage(img, p.X, p.Y, imgwidth, imgheight);
                }
            }
            g.Dispose();
            //GC.Collect();
            return imgBack;
        }

        private async Task<Image> FromUrl(string url)
        {
            HttpClient client = clientFactory.CreateClient();
            var s = await client.GetStreamAsync(url);
            return Image.FromStream(s);
        }

        private Image GetThumbnail(Image b, int destWidth, int destHeight)
        {
            Image imgSource = b;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            int sW;
            int sH;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }
    }
}
