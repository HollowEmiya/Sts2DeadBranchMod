using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace DeadBranch.Scripts.Cards;

[Pool(typeof(IroncladCardPool))]
public class SpotWeakness : DRModelIroncladCardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new []{ HoverTipFactory.FromPower<StrengthPower>() };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new []{ new PowerVar<StrengthPower>(3m) };

    public SpotWeakness() :
        base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

	protected override bool ShouldGlowGoldInternal
	{
		get
		{
			if (base.CombatState == null)
			{
				return false;
			}
			return base.CombatState.HittableEnemies.Any((Creature e) => e.Monster?.IntendsToAttack ?? false);
		}
	}
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        
        decimal strengthToApply =  (cardPlay.Target.Monster.IntendsToAttack) ? base.DynamicVars.Strength.BaseValue : 0;
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<StrengthPower>(base.Owner.Creature, strengthToApply, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Strength.UpgradeValueBy(1m);
    }
}