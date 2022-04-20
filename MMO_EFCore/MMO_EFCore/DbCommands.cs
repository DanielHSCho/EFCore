using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMO_EFCore
{
    public class DbCommands
    {
        public static void InitializeDB(bool forceReset = false)
        {
            // DB 사용 후 날려주는 경우가 일반적이므로 using 방식으로 사용
            using (AppDbContext db = new AppDbContext()) {
                if (!forceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists()) {
                    return;
                }

                db.Database.EnsureDeleted();
                // DB가 존재하지 않으면 삭제하고 생성
                db.Database.EnsureCreated();

                CreateTestData(db);
                Console.WriteLine("DB Initialized");
            };
        }

        public static void CreateTestData(AppDbContext db)
        {
            var player = new Player() {
                Name = "Daniel"
            };

            List<Item> items = new List<Item>() {
                new Item() {
                    TemplateId = 101,
                    CreateDate = DateTime.Now,
                    Owner = player
                },
                new Item() {
                    TemplateId = 102,
                    CreateDate = DateTime.Now,
                    Owner = player
                },
                new Item() {
                    TemplateId = 103,
                    CreateDate = DateTime.Now,
                    Owner = new Player() {Name = "Hellena"}
                }
            };

            // 여러개를 한번에 추가할때 (AddRange)
            db.Items.AddRange(items);
            db.SaveChanges();
        }

        public static void ReadAll()
        {
            using(var db = new AppDbContext()) {
                // AsNoTraking : ReadOnly << Tracking Snapshot - 데이터 변경 탐지 기능
                // Include : Eager Loading (즉시 로딩)
                foreach(Item item in db.Items.AsNoTracking()
                    .Include(i=>i.Owner)) {
                    Console.WriteLine($"TemplateId({item.TemplateId}) Owner({item.Owner.PlayerId}) Created({item.CreateDate})");
                }
            }
        }
    }
}
