using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodePoster.Models
{
    public static class EFContextExtensions
    {
        public static void EnsureDbInitialized(this EFContext context)
        {
            if (!context.PosterQRs.Any())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.PosterQRs.AddRange(
                    new PosterQR
                    {
                        Name = "张三"
                    },
                    new PosterQR
                    {
                        Name = "李四"
                    },
                    new PosterQR
                    {
                        Name = "王五"
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
