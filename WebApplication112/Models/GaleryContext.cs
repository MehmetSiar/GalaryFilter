using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication112.Entities;

namespace WebApplication112.Models
{
    public class GaleryContext : DbContext
    {
        public GaleryContext(DbContextOptions<GaleryContext> options) : base(options)
        {

        }


       public DbSet<ArabaGaleri> Arabalar { get; set; }
    }
}