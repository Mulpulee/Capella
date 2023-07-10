using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDeath
{
    void OnDeath(EntityBehaviour pEntity);
    void Destroy();
}