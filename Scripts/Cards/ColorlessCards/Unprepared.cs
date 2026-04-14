
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DeadBranch.Scripts.Cards;

[Pool(typeof(ColorlessCardPool))]
public sealed class Unprepared : DColorlessCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new []{CardKeyword.Exhaust};

    protected override IEnumerable<DynamicVar> CanonicalVars => new []{new PowerVar<WeakPower>(2m)};

	protected override IEnumerable<IHoverTip> ExtraHoverTips => new []{HoverTipFactory.FromPower<WeakPower>()};
    
	public Unprepared()
		: base(1, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy)
	{
	}

    
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await PowerCmd.Apply<WeakPower>(cardPlay.Target, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
		await CreatureCmd.Stun(cardPlay.Target);
        // cardPlay.Target.Monster.RollMove(new[]{base.Owner.Creature});
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Weak.UpgradeValueBy(1m);
	}
}