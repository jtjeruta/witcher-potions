using Verse;

namespace WitcherPotions
{
    public class HediffCompProperties_HealOverTime : HediffCompProperties
    {
        public int ticksInterval = 120;
        public float healAmount = 0.6f;
        public bool stopBleeding = true;

        public HediffCompProperties_HealOverTime()
        {
            compClass = typeof(HediffComp_HealOverTime);
        }
    }
}
