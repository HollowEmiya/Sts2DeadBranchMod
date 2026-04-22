using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace DeadBranch.Scripts.Cards;

public abstract class DRIroncladCardModel : CustomCardModel
{
    public override string PortraitPath => $"res://DeadBranch/images/Cards/IroncladCards/{GetType().Name}.png";

    public DRIroncladCardModel(int energyCost, CardType type,
        CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true) :
        base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
}