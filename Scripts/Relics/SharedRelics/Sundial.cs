using BaseLib.Utils;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Commands;

namespace DeadBranch.Scripts.Relics;

[Pool(typeof(SharedRelicPool))]
public sealed class Sundial : DRSharedRelicModel
{
    private int _shuffleCount;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("ShuffledAmount", 3),new EnergyVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
       [HoverTipFactory.ForEnergy(this)];

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    [SavedProperty(SerializationCondition.SaveIfNotTypeDefault)]
    public int ShuffleCount
    {
        get
        {
            return _shuffleCount;
        }
        set
        {
            AssertMutable();
            _shuffleCount = value;
            base.Status = (((decimal)_shuffleCount == base.DynamicVars["ShuffledAmount"].BaseValue -1m) ?
                RelicStatus.Active : RelicStatus.Normal);
            InvokeDisplayAmountChanged();
        }
    }

    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player player)
    {
        if(player == base.Owner)
        {
            Flash();
            ShuffleCount++;
            await GetEnergyIfThresholdMet(choiceContext);
        }
    }

    private async Task GetEnergyIfThresholdMet(PlayerChoiceContext choiceContext)
    {
        if(!((decimal)ShuffleCount < base.DynamicVars["ShuffledAmount"].BaseValue))
        {
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
            ShuffleCount %= base.DynamicVars["ShuffledAmount"].IntValue;
        }
    }
}