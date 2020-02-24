using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace QRCodePoster.Helpers
{
    public static class QRCodeHelper
    {
        public static Bitmap GetQRCode(string strContent, int width = 200, int height = 200)
        {
            //二维码生成开始
            BitMatrix bitMatrix;//定义像素矩阵对象
            bitMatrix = new MultiFormatWriter().encode(strContent, BarcodeFormat.QR_CODE /*条码或二维码标准*/, width/*宽度*/, height/*高度*/);
            var bw = new BarcodeWriterPixelData();

            var pixelData = bw.Write(bitMatrix);
            var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);//预绘制32bit标准的位图片

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height)/*绘制矩形区域及偏移量*/,
                System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            try
            {
                //假设位图的 row stride = 4 字节 * 图片的宽度
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);//从内存 Unlock bitmap 对象
            }
            //二维码生成结束
            return bitmap;
        }
    }
}
