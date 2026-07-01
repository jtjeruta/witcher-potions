using Verse;

namespace WitcherPotions.BlackBlood
{
    public class HediffComp_BlackBloodDevourerRelease : HediffComp
    {
        private const int ReleaseCheckIntervalTicks = 30;

        public override void CompPostMake()
        {
            base.CompPostMake();
            BlackBloodDevourerUtility.ReleaseFromDevourers(parent.pawn);
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            Pawn pawn = parent.pawn;
            if (pawn == null || pawn.Dead || !pawn.IsHashIntervalTick(ReleaseCheckIntervalTicks))
            {
                return;
            }

            BlackBloodDevourerUtility.ReleaseFromDevourers(pawn);
        }
    }
}
