using System.Collections.Generic;
using RimWorld;
using Verse;

namespace WitcherPotions.BlackBlood
{
    public static class BlackBloodDevourerUtility
    {
        private static readonly HashSet<CompDevourer> TriggeredComps = new HashSet<CompDevourer>();

        public static void TriggerRegurgitation(CompDevourer comp)
        {
            if (!BlackBloodUtility.AnomalyActive || comp == null || TriggeredComps.Contains(comp))
            {
                return;
            }

            Pawn victim = comp.DigestingPawn;
            Pawn devourer = comp.parent as Pawn;
            if (victim == null || devourer == null || !BlackBloodUtility.HasBlackBlood(victim))
            {
                return;
            }

            if (!devourer.Spawned || devourer.Map == null)
            {
                return;
            }

            TriggeredComps.Add(comp);

            if (!devourer.IsBurning())
            {
                FireUtility.TryAttachFire(devourer, 1f, victim);
            }
        }

        public static void ClearTracking(CompDevourer comp)
        {
            if (comp != null)
            {
                TriggeredComps.Remove(comp);
            }
        }

        public static void ReleaseFromDevourers(Pawn victim)
        {
            if (!BlackBloodUtility.AnomalyActive || victim == null)
            {
                return;
            }

            for (int i = 0; i < Find.Maps.Count; i++)
            {
                Map map = Find.Maps[i];
                if (map == null)
                {
                    continue;
                }

                foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
                {
                    CompDevourer comp = pawn.TryGetComp<CompDevourer>();
                    if (comp?.DigestingPawn == victim)
                    {
                        TriggerRegurgitation(comp);
                    }
                }
            }
        }
    }
}
