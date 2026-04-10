using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
public abstract class DRModelIroncladCardModel : CardModel
{
    public override string PortraitPath => $"res://DeaBranchMod/images/Cards/IroncladCards/{GetType().Name}.png";

    public DRModelIroncladCardModel(int energyCost, CardType type,
        CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true) :
        base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
}