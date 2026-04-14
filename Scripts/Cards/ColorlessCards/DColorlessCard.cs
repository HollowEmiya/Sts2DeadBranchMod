using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace DeadBranch.Scripts.Cards;

public abstract class DColorlessCard : CustomCardModel
{
    public override string PortraitPath => $"res://DeadBranch/images/Cards/ColorlessCards/{GetType().Name}.png";

    public DColorlessCard(int energyCost, CardType type,
        CardRarity rarity, TargetType targetType, bool shouldShowInCardLibrary = true) :
        base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
}