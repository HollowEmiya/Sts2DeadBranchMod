using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DeadBranch.Scripts.Cards;

public abstract class DRModelIroncladCardModel : CustomCardModel
{
    public override string PortraitPath => $"res://DeadBranch/images/Cards/IroncladCards/{GetType().Name}.png";

    public DRModelIroncladCardModel(int energyCost, CardType type,
        CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true) :
        base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
}