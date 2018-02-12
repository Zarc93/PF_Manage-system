using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage.Models;

namespace Manage.Data
{
    public class DbInitializer
    {
        public static void Initialize(InformationContext context)
        {
            context.Database.EnsureCreated();

            if (context.Posts.Any())
            { return; }
            var posts = new Post[]
            { new Post { Title="1",Content="1",Time="2018-2-1" },
              new Post { Title="2",Content="2",Time="2018-2-2" },
              new Post { Title="3",Content="3",Time="2018-2-3"},
            };
            foreach (Post t in posts)
            {
                context.Posts.Add(t);
            }
            context.SaveChanges();

            if (context.Books.Any())
            { return;   }
            var books = new Book[]
            { new Book { Name = "神经计算原理", Author ="1",  Publisher="机械工业出版社" },
              new Book { Name = "Windows程序设计", Author = "2", Publisher = "北京大学出版社" },
              new Book { Name = "java 5 游戏编程", Author = "3", Publisher = "人民邮电出版社" },
            };      
            foreach (Book s in books)
            {
                context.Books.Add(s);
            }
            context.SaveChanges();


            if (context.Blogs.Any())
            { return; }
            var blogs = new Blog[]
            { new Blog {  BlogAuthor ="User A",  BlogUrl="https://www.liaoxuefeng.com/" },
              new Blog {  BlogAuthor ="User B",  BlogUrl="https://www.liaoxuefeng.com/" },
              new Blog {  BlogAuthor ="User C",  BlogUrl="https://www.liaoxuefeng.com/" },
            };
            foreach (Blog u in blogs)
            {
                context.Blogs.Add(u);
            }
            context.SaveChanges();

            


            if (context.Videos.Any())
            { return; }
            var videos = new Video[]
            { new Video { VideoName = "1",VideoNum=1,VideoURL="1" },
              new Video { VideoName = "2",VideoNum=2,VideoURL="2"  },
              new Video { VideoName = "3",VideoNum=3,VideoURL="3"  },
            };
            foreach (Video o in videos )
            {
                context.Videos.Add(o);
            }
            context.SaveChanges();

        }
    }
}
