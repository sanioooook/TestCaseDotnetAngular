using System;
using System.Globalization;
using FileHelpers;

namespace Entities.Converters
{
  public class MoneyConverter : ConverterBase
  {
    public override object StringToField(string from)
    {
      from = from.Trim(new char[] { '$' });
      return Convert.ToDouble(double.Parse(from, CultureInfo.InvariantCulture));
    }

    public override string FieldToString(object fieldValue)
    {
      return (string)fieldValue;
    }

  }
}