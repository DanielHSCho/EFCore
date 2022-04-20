using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO_EFCore
{
    public static class Extensions
    {
        // IEnumerable (Linq to object / Linq to xml 쿼리)
        // IQueryable (Linq to SQL 쿼리)
        // IEnumerable이 메모리에 다들고있으려는 기질이 있어서 최적화 개념으로 I쿼리어블 사용
        public static IQueryable<GuildDto> MapGuildToDto(this IQueryable<Guild> guild)
        {
            return guild.Select(g => new GuildDto() {
                Name = g.GuildName,
                MemberCount = g.Members.Count
            });
        }
    }
}
