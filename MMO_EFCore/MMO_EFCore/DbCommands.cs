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

                Console.WriteLine("DB Initialized");
            };
        }

    }
}
