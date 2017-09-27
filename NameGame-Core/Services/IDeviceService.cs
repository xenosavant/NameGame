// This interface represents an abstraction for a platform-specific device service
// That will return the current width and height of the screen.

using System;

namespace WillowTree.NameGame.Core.Services
{
    public interface IDeviceService
    {
        int GetScreenWidth();

        int GetScreenHeight();
    }
}
