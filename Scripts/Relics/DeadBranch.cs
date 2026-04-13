using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Commands;

namespace DeadBranch.Scripts.Relics;

[Pool(typeof(IroncladRelicPool))]
public class DeadBranch : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    
    // 小图标
    public override string PackedIconPath => $"res://DeadBranch/images/Relics/IroncladRelics/DeadBranch.png";
    // 轮廓图标
    protected override string PackedIconOutlinePath => $"res://DeadBranch/images/Relics/IroncladRelics/Outline/DeadBranch.png";
    // 大图标
    protected override string BigIconPath => $"res://DeadBranch/images/Relics/IroncladRelics/DeadBranch.png";

    /// InfernalBlade Code
	// protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	// {
	// 	CardModel cardModel = CardFactory.GetDistinctForCombat(base.Owner,
    //      from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
	// 		    where c.Type == CardType.Attack
	// 		    select c,
    //      1,
    //      base.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
	// 	if (cardModel != null)
	// 	{
	// 		cardModel.SetToFreeThisTurn();
	// 		await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
	// 	}
	// }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
    {
        if(card.Owner == base.Owner)
        {
            CardModel cardModel = CardFactory.GetDistinctForCombat(base.Owner, from c in base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
			where (c.Type == CardType.Attack || c.Type == CardType.Skill || c.Type == CardType.Power)
			select c, 1, base.Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
            if (cardModel != null)
            {
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
            }
        }
    }
}