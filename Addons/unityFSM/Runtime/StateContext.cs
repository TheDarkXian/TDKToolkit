using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koneski.StateMachine
{
    public class StateContext
    {
        public StateContext(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public StateMachine stateMachine { get; set; }


    }
}
