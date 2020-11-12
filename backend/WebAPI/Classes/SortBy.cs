using Entities.Enums;

namespace WebAPI.Classes
{
  public class SortBy
  {
    public EnumStatusTransaction? SortStatusBy { get; set; }

    public EnumTypeTransaction? SortTypeBy { get; set; }
  }
}