using Terraria;
using Terraria.ModLoader;

namespace BagajeMod.Content.Buffs
{
    public class ElClamorBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.ElClamorMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000; // Persiste mientras esté convocado
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
