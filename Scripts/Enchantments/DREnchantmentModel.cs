using BaseLib.Abstracts;

namespace DeadBranch.Scripts.EnchantmentModel;

public abstract class DREnchantmentModel : CustomEnchantmentModel
{
    protected override string? CustomIconPath => $"res://DeadBranch/images/Enchantments/{GetType().Name}.png";
}