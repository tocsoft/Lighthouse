using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lighthouse.Control
{
	public interface IDebugger
	{
		void Enter(int handle);
		void Exit(int handle);
	}
}
