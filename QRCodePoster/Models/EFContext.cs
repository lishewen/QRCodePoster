using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodePoster.Models
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options) { }
        /// <summary>
        /// 超级海报
        /// </summary>
        public DbSet<Poster> Poster { get; set; }
        public DbSet<PosterQR> PosterQRs { get; set; }
    }
}
