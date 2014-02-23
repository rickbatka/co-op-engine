using co_op_engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.ServiceProviders
{
    public interface IActorInformationProvider
    {
        List<GameObject> GetPlayers();
    }
}
