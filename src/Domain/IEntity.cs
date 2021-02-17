﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    interface IEntity<TId>
    {
        TId Id { get; set; }
        DateTime? CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
    }
}