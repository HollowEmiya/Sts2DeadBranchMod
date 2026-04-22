using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace DeadBranch.Scripts.Cards;

public sealed class FinalFlash : DRIroncladCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public FinalFlash()
        : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner, (int)base.DynamicVars.Cards.BaseValue,
            CardPilePosition.Random, forceExhaust: true);
    }
    
	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}