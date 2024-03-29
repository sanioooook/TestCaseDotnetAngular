﻿using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;

namespace Entities.Models
{
  [Table("Transaction")]
  public class Transaction
  {
    public int Id { get; set; }

    public EnumStatusTransaction Status { get; set; }

    public EnumTypeTransaction Type { get; set; }

    public string ClientName { get; set; }

    public double Amount { get; set; }
  }
}