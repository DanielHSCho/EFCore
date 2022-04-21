using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMO_EFCore
{
    public class AppDbContext : DbContext
    {
        // DbSet<Item> -> EFCore한테 알려준다
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Guild> Guilds { get; set; }

        // DB ConString - 어떤 DB를 어떻게 연결해라
        // (각종 설정, Authorization 등 포함)
        // TODO : 보안상 외부 Config파일로 빼야한다
        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EfCoreDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // 처음 DB 연동 부분을 옵션으로 넣는다
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 앞으로 Item Entity에 접근 시 항상 사용되는 모델 수준의 필터링
            // 반대로 필터를 무시하고 싶으면 IgnoreQueryFilters 사용
            builder.Entity<Item>().HasQueryFilter(i => i.SoftDeleted == false);

            builder.Entity<Player>()
                .HasIndex(p => p.Name)
                .HasName("Index_Person_Name")
                .IsUnique();
        }
    }
}
