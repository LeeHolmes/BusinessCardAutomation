using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Printing;

namespace CatalogManager
{
	public class PrinterBounds
	{
		[DllImport("gdi32.dll")] private static extern Int32 
			GetDeviceCaps(IntPtr hdc, Int32 capindex);

		private const int PHYSICALOFFSETX = 112;
		private const int PHYSICALOFFSETY = 113;

		public readonly Rectangle Bounds;
		public readonly int       HardMarginLeft;
		public readonly int       HardMarginTop;

		public PrinterBounds(PrintPageEventArgs e)
		{
			IntPtr hDC = e.Graphics.GetHdc();

			HardMarginLeft = GetDeviceCaps(hDC , PHYSICALOFFSETX);
			HardMarginTop  = GetDeviceCaps(hDC , PHYSICALOFFSETY);

			e.Graphics.ReleaseHdc(hDC);

			HardMarginLeft = (int)(HardMarginLeft * 100.0 / e.Graphics.DpiX);
			HardMarginTop  = (int)(HardMarginTop  * 100.0 / e.Graphics.DpiY);

			Bounds = e.MarginBounds;

			Bounds.Offset(-HardMarginLeft , -HardMarginTop);
		}
	}
}
