using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
interface IEntity<TId>
{
    TId Id {
        get;
        set;
    }
}
}
