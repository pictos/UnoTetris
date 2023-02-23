using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoTetris.Services;
using Windows.System;
using static UnoTetrisOld.Platforms.Windows.WinAPI;

namespace UnoTetris;
public sealed class InputService : IInputService
{
	static int m_hHook = 0;
	HookProc m_HookProcedure;

	public void ProcessKeyDown(MainPage page, Action<VirtualKey> action)
	{
		page.Loaded += OnPageLoaded;


		void OnPageLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			m_HookProcedure = new HookProc(HookProcedure);
			m_hHook = SetWindowsHookEx(WH_KEYBOARD, m_HookProcedure, (IntPtr)0, (int)GetCurrentThreadId());


			void HookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
			{
				if (nCode < 0) return;

				bool shift = (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Down).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down));

				action(GetVirtualKey());

				static VirtualKey GetVirtualKey()
				{
					if (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Down).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
						return VirtualKey.Down;
					if (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Up).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
						return VirtualKey.Up;
					if (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Left).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
						return VirtualKey.Left;
					if (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Right).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
						return VirtualKey.Right;
					if (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Z).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
						return VirtualKey.Z;
					if (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.C).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
						return VirtualKey.C;
					if (Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Space).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
						return VirtualKey.Space;
					return VirtualKey.None;
				}
			}
		}
	}
}
