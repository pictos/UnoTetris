using Microsoft.UI.Xaml.Controls;
using System;
using Windows.System;

namespace UnoTetris.Services;

public interface IInputService
{
	void ProcessKeyDown(MainPage page, Action<VirtualKey> action);
}
