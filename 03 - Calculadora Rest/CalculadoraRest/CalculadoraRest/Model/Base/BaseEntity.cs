﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CalculadoraRest.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
