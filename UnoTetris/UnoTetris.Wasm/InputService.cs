using System;
using UnoTetris.Services;
using Windows.System;

namespace UnoTetris;

public class InputService : IInputService
{
	public void ProcessKeyDown(MainPage page, Action<VirtualKey> action)
	{
		page.KeyDown += (_, e) => action(e.Key);
	}
}
