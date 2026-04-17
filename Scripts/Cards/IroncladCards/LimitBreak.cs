using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DeadBranch.Scripts.Cards;

[Pool(typeof(IroncladCardPool))]
public sealed class LimitBreak : DRModelIroncladCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new []{CardKeyword.Exhaust};

    public LimitBreak()
        : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<StrengthPower>(base.Owner.Creature,
            base.Owner.Creature.GetPowerAmount<StrengthPower>(),
            base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
	{
		RemoveKeyword(CardKeyword.Exhaust);
	}
}