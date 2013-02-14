using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	class NoOpDebugger : IDebugger
	{

		public void Enter(int handle) { }

		public void Exit(int handle) { }
	}
}
