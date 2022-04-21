using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMO_EFCore
{
    [Table("Item")]
    public class Item
    {
        public bool SoftDeleted { get; set; }
        // 이름Id -> PK
        public int ItemId { get; set; }
        public int TemplateId { get; set; } // 101번 = 집행검
        public DateTime CreateDate { get; set; }

        // 다른 클래스 참조 -> FK (Navigational Property)
        [ForeignKey("OwnerId")]
        public Player Owner { get; set; }
    }

    [Table("Player")]
    // 클래스 이름 = 테이블 이름 = Player
    public class Player
    {
        // 이름Id -> PK
        public int PlayerId { get; set; }
        public string Name { get; set; }

        //public ICollection<Item> Items { get; set; }
        public Item Item { get; set; } // 1:1 관계형성용
        public Guild Guild { get; set; }
    }

    [Table("Guild")]
    public class Guild
    {
        public int GuildId { get; set; }
        public string GuildName { get; set; }
        public ICollection<Player> Members { get; set; }
    }

    // Data Transfer Object
    // TODO : 다른 파일로 분리해두는 것도 좋음
    public class GuildDto
    {
        public int GuildId { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
    }
}
