using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCodePoster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QRCodePoster.Controllers
{
    public class PosterController : Controller
    {
        private readonly EFContext db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PosterController(EFContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            _hostEnvironment = hostEnvironment;
        }
        /// <summary>
        /// 全局提示信息方法
        /// </summary>
        /// <param name="msg"></param>
        private void ShowCommMessage(string msg)
        {
            ViewData["CommMessage"] = msg;
        }

        /// <summary>
        /// 全局错误提示方法
        /// </summary>
        /// <param name="msg"></param>
        private void ShowErrMessage(string msg)
        {
            ViewData["ErrMessage"] = msg;
        }

        /// <summary>
        /// 抛出Http异常
        /// </summary>
        /// <param name="msg"></param>
        protected void ThrowHttpResponseException(string msg)
        {
            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //{
            //	Content = new StringContent(msg)
            //});
        }

        public IActionResult Create()
        {
            return View(new Poster());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Poster model)
        {
            if (ModelState.IsValid)
            {
                await db.AddAsync(model);
                await db.SaveChangesAsync();
                ShowCommMessage("添加成功");
                return View(new Poster());
            }
            else
            {
                ShowErrMessage("添加失败");
                return View(model);
            }
        }
        [HttpPost]
        public async Task<UploadFileResult> Upload(IFormFile file)
        {
            string[] allowExtName = { ".jpg", ".gif", ".png" };
            if (file == null)
                ThrowHttpResponseException("请选择上传文件");

            var fullName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
            int i = fullName.LastIndexOf('.');
            var extName = fullName.Substring(i);

            if (!allowExtName.Contains(extName))
                ThrowHttpResponseException("该文件类型不允许上传");

            string fileName = Guid.NewGuid().ToString() + extName;
            var filePath = _hostEnvironment.WebRootPath + @"\attachment\images\" + fileName;
            //保存文件
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var inputStream = file.OpenReadStream();
                await inputStream.CopyToAsync(fileStream);
            }
            //await file.SaveAsAsync(filePath);

            return new UploadFileResult
            {
                name = fullName,
                ext = extName.Replace(".", ""),
                filename = "images/" + fileName,
                attachment = "images/" + fileName,
                url = "http://" + Request.Host + "/attachment/images/" + fileName,
                is_image = 1,
                filesize = (int)file.Length
            };
        }
    }
}
