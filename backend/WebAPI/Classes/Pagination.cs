﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Classes
{
  public class Pagination<T>
  {
    public IEnumerable<T> Data { get; set; }

    [Required, Range(0, int.MaxValue)]
    public int PageNumber { get; set; }

    [Required, Range(1, 100)]
    public int PageSize { get; set; }

    public int? TotalCount { get; set; }

    public int? PageCount { get; set; }

  }
}