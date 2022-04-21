using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var daniel = new Player() { Name = "Daniel" };
            var hellena = new Player() { Name = "Hellena" };
            var newt = new Player() { Name = "Newt" };


            List<Item> items = new List<Item>() {
                new Item() {
                    TemplateId = 101,
                    CreateDate = DateTime.Now,
                    Owner = daniel
                },
                new Item() {
                    TemplateId = 102,
                    CreateDate = DateTime.Now,
                    Owner = hellena
                },
                new Item() {
                    TemplateId = 103,
                    CreateDate = DateTime.Now,
                    Owner = newt
                }
            };

            Guild guild = new Guild() {
                GuildName = "Wizards",
                Members = new List<Player>() {
                    daniel, hellena, newt
                }
            };

            // 여러개를 한번에 추가할때 (AddRange)
            db.Items.AddRange(items);
            db.Guilds.Add(guild);
            db.SaveChanges();
        }

        public static void ShowItems()
        {
            using (AppDbContext db = new AppDbContext()) {
                foreach (var item in db.Items.Include(i => i.Owner).IgnoreQueryFilters().ToList()) {
                    if (item.Owner == null) {
                        Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner(0)");
                    } else {
                        Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner({item.Owner.Name}) OwnerName({item.Owner.Name}))");
                    }
                }
            }
        }

        public static void ShowGuild()
        {
            using(AppDbContext db = new AppDbContext()) {
                foreach(var guild in db.Guilds.Include(g => g.Members).ToList()) {
                    Console.WriteLine($"GuildId({guild.GuildId}) GuildName({guild.GuildName}) MemberCount({guild.Members.Count})");
                }
            }
        }

        public static void TestDelete()
        {
            ShowItems();

            Console.WriteLine("Select Delete ItemId");
            Console.WriteLine("> ");
            int id = int.Parse(Console.ReadLine());

            using (AppDbContext db = new AppDbContext()) {
                Item item = db.Items.Find(id);
                item.SoftDeleted = true;
                db.SaveChanges();
            }

            Console.WriteLine("-- Test Delete Complete --");
            ShowItems();
        }


        //public static void Update_1vM()
        //{
        //    ShowGuild();

        //    Console.WriteLine("Input GuildId");
        //    Console.WriteLine("> ");
        //    int id = int.Parse(Console.ReadLine());

        //    using (AppDbContext db = new AppDbContext()) {
        //        Guild guild = db.Guilds
        //            .Include(p => p.Members)
        //            .Single(p => p.GuildId == id);

        //        guild.Members.Add(new Player() {
        //            Name = "Dopa"
        //        });

        //        db.SaveChanges();
        //    }

        //    Console.WriteLine("-- Test Complete --");
        //    ShowGuild();
        //}

        //public static void Update_1v1()
        //{
        //    ShowItems();

        //    Console.WriteLine("Input ItemSwitch PlayerId");
        //    Console.WriteLine("> ");
        //    int id = int.Parse(Console.ReadLine());

        //    using (AppDbContext db = new AppDbContext()) {
        //        Player player = db.Players
        //            .Include(p => p.Item)
        //            .Single(p => p.PlayerId == id);

        //        if(player.Item != null) {
        //            player.Item.TemplateId = 888;
        //            player.Item.CreateDate = DateTime.Now;
        //        }

        //        db.SaveChanges();
        //    }

        //    Console.WriteLine("-- Test Complete --");
        //    ShowItems();
        //}

        //public static void Test()
        //{
        //    ShowItems();

        //    Console.WriteLine("Input Delete PlayerId");
        //    Console.WriteLine("> ");
        //    int id = int.Parse(Console.ReadLine());

        //    using(AppDbContext db = new AppDbContext()) {
        //        Player player = db.Players
        //            .Include(p => p.Item)
        //            .Single(p => p.PlayerId == id);

        //        db.Players.Remove(player);
        //        db.SaveChanges();
        //    }

        //    Console.WriteLine("-- Test Complete --");
        //    ShowItems();
        //}

        //public static void ShowGuilds()
        //{
        //    using(AppDbContext db = new AppDbContext()) {
        //        foreach(var guild in db.Guilds.MapGuildToDto()) {
        //            Console.WriteLine($"GuildId({guild.GuildId}) GuildName({guild.Name})) MemberCount({guild.MemberCount})");
        //        }
        //    }
        //}

        //public static void UpdateByReload()
        //{
        //    ShowGuilds();

        //    // 외부에서 수정 원하는 데이터의 Id와 정보를 넘겨줬다고 가정
        //    Console.WriteLine("Input GuildId");
        //    Console.WriteLine("> ");
        //    int id = int.Parse(Console.ReadLine());

        //    Console.WriteLine("Input GuildName");
        //    Console.WriteLine("> ");
        //    string name = Console.ReadLine();

        //    using (AppDbContext db = new AppDbContext()) {
        //        // 가장 빠른 방법 - Find의 id가 프라이머리키이기 때문에
        //        Guild guild = db.Find<Guild>(id);
        //        guild.GuildName = name;
        //        db.SaveChanges();
        //    }

        //    Console.WriteLine("--- Update Complete ---");
        //    ShowGuilds();
        //}

        //public static string MakeUpdateJsonStr()
        //{
        //    // 원래는 클라쪽에서 만들어서 보내줘야 함
        //    var jsonStr = "{\"GuildId\":1, \"GuildName\":\"Hello\", \"Members\":null}";
        //    return jsonStr;
        //}

        //public static void UpdateByFull()
        //{
        //    ShowGuilds();

        //    string jsonStr = MakeUpdateJsonStr();
        //    Guild guild = JsonConvert.DeserializeObject<Guild>(jsonStr);

        //    using (AppDbContext db = new AppDbContext()) {
        //        db.Guilds.Update(guild);
        //        db.SaveChanges();
        //    }

        //    Console.WriteLine("--- Update Complete ---");
        //    ShowGuilds();
        //}

        //public static void UpdateTest()
        //{
        //    using(AppDbContext db = new AppDbContext()) {
        //        var guild = db.Guilds.Single(g => g.GuildName == "Wizards");
        //        guild.GuildName = "Murgles";
        //        db.SaveChanges();
        //    }
        //}

        //// 특정 길드에 있는 길드원의 모든 소지 아이템을 로드
        //public static void EagerLoading()
        //{
        //    Console.WriteLine("길드 이름을 입력하세요");
        //    Console.WriteLine("> ");
        //    string name = Console.ReadLine();

        //    using (var db = new AppDbContext()) {
        //        Guild guild = db.Guilds.AsNoTracking()
        //            .Where(g => g.GuildName == name)
        //                .Include(g => g.Members) //Eager로딩
        //                    .ThenInclude(p => p.Item)
        //            .First();

        //        foreach(Player player in guild.Members) {
        //            Console.WriteLine($"TemplateId({player.Item.TemplateId}) Owner({player.Name})");
        //        }
        //    }
        //}

        //public static void ExplicitLoading()
        //{
        //    Console.WriteLine("길드 이름을 입력하세요");
        //    Console.WriteLine("> ");
        //    string name = Console.ReadLine();

        //    using (var db = new AppDbContext()) {
        //        // Explicit방식은 AsNoTracking 사용불가
        //        Guild guild = db.Guilds
        //            .Where(g => g.GuildName == name)
        //            .First();

        //        // 명시적 - 단계적 추출
        //        db.Entry(guild)
        //            .Collection(g => g.Members)
        //            .Load();

        //        foreach(Player player in guild.Members) {
        //            db.Entry(player)
        //                .Reference(p => p.Item)
        //                .Load();
        //        }

        //        foreach (Player player in guild.Members) {
        //            Console.WriteLine($"TemplateId({player.Item.TemplateId}) Owner({player.Name})");
        //        }
        //    }
        //}

        //// 특정 길드의 길드원의 수 구하기
        //// SELECT COUNT (*)
        //public static void SelectLoading()
        //{
        //    Console.WriteLine("길드 이름을 입력하세요");
        //    Console.WriteLine("> ");
        //    string name = Console.ReadLine();

        //    using(var db = new AppDbContext()) {
        //        var info = db.Guilds
        //            .Where(g => g.GuildName == name)
        //            .MapGuildToDto()
        //            .First();

        //        Console.WriteLine($"GuildName({info.Name}), MemberCount({info.MemberCount})");
        //    }
        //}

        //public static void ReadAll()
        //{
        //    using(var db = new AppDbContext()) {
        //        // AsNoTraking : ReadOnly << Tracking Snapshot - 데이터 변경 탐지 기능
        //        // Include : Eager Loading (즉시 로딩)
        //        foreach(Item item in db.Items.AsNoTracking()
        //            .Include(i=>i.Owner)) {
        //            Console.WriteLine($"TemplateId({item.TemplateId}) Owner({item.Owner.PlayerId}) Created({item.CreateDate})");
        //        }
        //    }
        //}

        //public static void ShowItems()
        //{
        //    Console.WriteLine("플레이어 이름 입력하세요");
        //    Console.WriteLine("> ");
        //    string name = Console.ReadLine();

        //    using(var db = new AppDbContext()) {
        //        foreach(Player player in db.Players.AsNoTracking()
        //            .Where(p => p.Name == name)
        //            .Include(p => p.Items)) {
        //            foreach(Item item in player.Items) {
        //                Console.WriteLine($"{item.TemplateId}");
        //            }
        //        }
        //    }
        //}

        //public static void UpdateDate()
        //{
        //    Console.WriteLine("Input Player Name");
        //    Console.WriteLine("> ");

        //    string name = Console.ReadLine();

        //    // 링큐 - Where
        //    using(var db = new AppDbContext()) {
        //        var items = db.Items.Include(i => i.Owner)
        //            .Where(i => i.Owner.Name == name);

        //        foreach(Item item in items) {
        //            item.CreateDate = DateTime.Now;
        //        }

        //        db.SaveChanges();
        //    }

        //    ReadAll();
        //}

        //public static void DeleteItem()
        //{
        //    Console.WriteLine("Input Player Name");
        //    Console.WriteLine("> ");

        //    string name = Console.ReadLine();

        //    using (var db = new AppDbContext()) {
        //        var items = db.Items.Include(i => i.Owner)
        //            .Where(i => i.Owner.Name == name);


        //        db.Items.RemoveRange(items);
        //        db.SaveChanges();
        //    }

        //    ReadAll();
        //}
    }
}
