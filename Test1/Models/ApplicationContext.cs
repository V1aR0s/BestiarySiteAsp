using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Test1.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {

        public DbSet<СreatureArticle> СreatureArticles { get; set; }
        public DbSet<Sex> Sexes { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Mythology> Mythologies { get; set; }
        public DbSet<DangerType> DangerTypes { get; set; }
        public DbSet<ArticleState> ArticleStates { get; set; }
        public DbSet<CreatureType> CreatureTypes { get; set; }

        public DbSet<Comment> Commetns { get; set; }
        public DbSet<UserLikes> UserLikes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> option) : base(option)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Sex s1 = new Sex { Id = 1, Name = "Мужской"};
            Sex s2 = new Sex { Id = 2, Name = "Женский" };
            Sex s3 = new Sex { Id = 3, Name = "Не определенно" };
            builder.Entity<Sex>().HasData(s1, s2, s3);


            CreatureType t1 = new CreatureType { Id = 1, Name = "Человек" };
            CreatureType t2 = new CreatureType { Id = 2, Name = "Драконоид" };
            CreatureType t3 = new CreatureType { Id = 3, Name = "Хз что" };
            builder.Entity<CreatureType>().HasData(t1, t2, t3);


            DangerType d1 = new DangerType { Id = 1, Name = "Опасный" };
            DangerType d2 = new DangerType { Id = 2, Name = "Нейтральный" };
            DangerType d3 = new DangerType { Id = 3, Name = "Дружелюбный" };
            DangerType d4 = new DangerType { Id = 4, Name = "Неизвестный" };
            builder.Entity<DangerType>().HasData(d1, d2, d3, d4);



            ArticleState a1 = new ArticleState { Id = 1, Name = "Нет" };
            ArticleState a2 = new ArticleState { Id = 2, Name = "В разработке" };
            ArticleState a3 = new ArticleState { Id = 3, Name = "Опубликованно" };
            builder.Entity<ArticleState>().HasData(a1, a2, a3);


            base.OnModelCreating(builder);
        }
    }
}
