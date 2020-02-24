using Microsoft.AspNetCore.Mvc;
using QRCodePoster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodePoster.Controllers
{
    public class PosterController : Controller
    {
        private readonly EFContext db;
        public PosterController(EFContext context)
        {
            db = context;
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
    }
}
