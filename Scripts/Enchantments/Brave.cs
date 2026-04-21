using DeadBranch.Scripts.Cards.Keyword;
using DeadBranch.Scripts.EnchantmentModel;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace DeadBranch.Scripts.EnchantmentModel;

public sealed class Brave : DREnchantmentModel
{
    public override bool ShowAmount => false;

    public override bool HasExtraCardText => true;

    // protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    //     [HoverTipFactory.FromKeyword(DRCardKeyword.Brave)];

    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card);
    }

    // protected override void OnEnchant()
    // {
    //     Card.AddKeyword(DRCardKeyword.Brave);
    // }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if(side == base.Card.Owner.Creature.Side)
        {
            CardPile pile = PileType.Draw.GetPile(base.Card.Owner);
            if(pile.Cards.FirstOrDefault() == base.Card)
            {
                await CardPileCmd.AutoPlayFromDrawPile(choiceContext,
                    base.Card.Owner, 1, CardPilePosition.Top, forceExhaust: false);
            }
        }
    }
}