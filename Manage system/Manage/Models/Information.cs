using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Manage.Models
{

        public class InformationContext : DbContext
        {
            public InformationContext(DbContextOptions<InformationContext> options) : base(options)
            {}

            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<Book> Books { get; set; }
            public DbSet<Video> Videos { get; set; }
            public DbSet<SearchLog> SearchLogs { get; set; }
       
    }
        public class Book
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Author { get; set; }
            public string Publisher { get; set; }

            public string ISBN { get; set; }
            public string Details { get; set; }
            public double? Rank { get; set; }
            public double? Price { get; set; }

            public string ImageUrl { get; set; }
            
    }
        public class Blog
        {   
            public int BlogId { get; set; }

            public string BlogUrl { get; set; }
            public string BlogAuthor { get; set; }
           
        }

        public class Post
        {   
            public int PostId { get; set; }

            public string Title { get; set; }
            public string Content { get; set; }
            public string Time { get; set; }

        }
        
        public class Video
        {   
            public int VideoID { get; set; }

            public string VideoName { get; set; }
            public int VideoNum { get; set; }
            public string VideoURL { get; set; }

        }
}
