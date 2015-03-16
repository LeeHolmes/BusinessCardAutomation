using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace ImageRecognizer
{
	/// <summary>
	/// Summary description for CommandCardRecognition.
	/// </summary>
	public class CommandCardRecognition
	{
		public static Image Clean(Image recognitionImage, bool debugMode, PictureBox imgRecognized)
		{
			Bitmap oldBitmap = new Bitmap(recognitionImage);
			
			double brightnessAvg = GetBrightnessThreshold(oldBitmap);
			Bitmap newBitmap = ConvertToBlackAndWhite(oldBitmap, brightnessAvg);

			#region debugMode
			if(debugMode)
			{
				imgRecognized.Image = new Bitmap(newBitmap);
				imgRecognized.Refresh();
			}
			#endregion

			oldBitmap.Dispose();
			oldBitmap = new Bitmap(newBitmap);
			newBitmap.Dispose();

			// 2. Find the top edge
			// This means: 
			//	a) for each xPos, find the first encounter with a batch of black pixels
			//	b) from that set, find the slope between each xPos and it's neighbor +- (some span)
			//	c) find the average slope
			//	d) rotate image by average slope

			// Calculate the encounter coordinates
			int[] xIntersects = GetXIntersects(oldBitmap);

			#region debugMode
			if(debugMode)
			{
				newBitmap = new Bitmap(oldBitmap);
				for(int intersect = 0; intersect < xIntersects.Length; intersect++)
				{
					for(int yPos = 0; yPos < 3; yPos++)
						if(xIntersects[intersect] > 2)
							newBitmap.SetPixel(intersect, xIntersects[intersect] - yPos, Color.Green);
				}

				if(imgRecognized.Image != null)
					imgRecognized.Image.Dispose();
				imgRecognized.Image = new Bitmap(newBitmap);
				oldBitmap.Dispose();
				oldBitmap = new Bitmap(newBitmap);
				newBitmap.Dispose();

				imgRecognized.Refresh();
			}
			#endregion

			float rotationAngle = GetRotationAngle(xIntersects);

			newBitmap = RotateImage.Utilities.RotateImage(oldBitmap, rotationAngle);
			oldBitmap.Dispose();
			oldBitmap = new Bitmap(newBitmap);
			newBitmap.Dispose();

			#region debugMode
			if(debugMode)
			{
				imgRecognized.Image.Dispose();
				imgRecognized.Image = new Bitmap(oldBitmap);
				imgRecognized.Refresh();
			}
			#endregion

			// Find the darkest row in the top, bottom, left, right
			int topLine = GetTopLine(oldBitmap); 
			int bottomLine = GetBottomLine(oldBitmap); 
			int nextLine = GetSecondLine(topLine + 5, oldBitmap);

			int leftLine = GetLeftLine(oldBitmap, topLine, bottomLine); 
			int rightLine = GetRightLine(oldBitmap, topLine, bottomLine);

			Debug.WriteLine(String.Format("Found lines top: {0}, bottom: {1}, left: {2}, right: {3}", topLine, bottomLine, leftLine, rightLine));

			int squareWidth = (rightLine - leftLine) / 10;
			int topPad = (int) ((double) (bottomLine - topLine) / 2);
			int bottomPad = (bottomLine - topLine) - (topPad + (2 * squareWidth));
			int leftPad = 0; int rightPad = 0;
			
			int width = rightLine - leftLine - (leftPad + rightPad);
			int height = bottomLine - topLine - (topPad + bottomPad);

			#region debugMode
			if(debugMode)
			{
				newBitmap = new Bitmap(imgRecognized.Image);
				for(int xPos = 0; xPos < oldBitmap.Width; xPos++)
				{
					newBitmap.SetPixel(xPos, bottomLine, Color.Red);
					newBitmap.SetPixel(xPos, bottomLine - bottomPad, Color.Purple);
					newBitmap.SetPixel(xPos, topLine, Color.Red);
					newBitmap.SetPixel(xPos, topLine + topPad, Color.Purple);
				}
				for(int yPos = 0; yPos < oldBitmap.Height; yPos++)
				{
					newBitmap.SetPixel(leftLine, yPos, Color.Red);
					newBitmap.SetPixel(rightLine, yPos, Color.Red);
				}
				
				imgRecognized.Image.Dispose();
				imgRecognized.Image = new Bitmap(newBitmap);
				newBitmap.Dispose();

				imgRecognized.Refresh();
			}
			#endregion

			// if the next line isn't within 20 px of expected
			// (expected being 300 px into the full-sized image (2 in. @ 600 DPI),)
			// it's not been recognized properly.
			int expected = topLine + (int) (((double) 300 / (double) (2 * 600) * (bottomLine - topLine)));
			if(Math.Abs(nextLine - expected) > 20)
			{
				System.Diagnostics.Debug.WriteLine(
					"Did not recognize card.  Expected at " + expected + " but found at " + nextLine);
				System.Diagnostics.Debug.WriteLine("Top: " + topLine + ", Bottom: " + bottomLine);

				newBitmap = new Bitmap(352, 288);
			}
			else
			{

				newBitmap = new Bitmap(width, height);
				using(Graphics g = Graphics.FromImage(newBitmap))
				{
					g.Clear(Color.White);
					g.DrawImage(oldBitmap, new Rectangle(0, 0, width, height), 
						leftLine + leftPad,
						topLine + topPad, 
						width, 
						height, 
						GraphicsUnit.Pixel);
				}
			}

			oldBitmap.Dispose();
			return newBitmap;
		}

		private static double GetBrightnessThreshold(Bitmap testImage)
		{
			// 0. Get avg color (as a decision point on what is black vs white)
			double brightnessAvg = 0;
			for(int yPos = 0; yPos < testImage.Height; yPos += 5)
				for(int xPos = 0; xPos < testImage.Width; xPos += 5)
					brightnessAvg += testImage.GetPixel(xPos, yPos).GetBrightness();
			brightnessAvg /= (testImage.Height * testImage.Width / 25);

			return (brightnessAvg * 0.9);
		}

		private static Bitmap ConvertToBlackAndWhite(Bitmap oldBitmap, double brightnessAvg)
		{
			Bitmap newBitmap = new Bitmap(oldBitmap);

			// 1. Convert to black/white
			for(int yPos = 0; yPos < oldBitmap.Height; yPos++)
				for(int xPos = 0; xPos < oldBitmap.Width; xPos++)
					if(oldBitmap.GetPixel(xPos, yPos).GetBrightness() < brightnessAvg)
						newBitmap.SetPixel(xPos, yPos, Color.Black);
					else
						newBitmap.SetPixel(xPos, yPos, Color.White);

			return newBitmap;
		}

		private static int[] GetXIntersects(Bitmap oldBitmap)
		{
			int[] xIntersects = new int[oldBitmap.Width];
			for(int xPos = 0; xPos < oldBitmap.Width; xPos++)
			{
				int yPos = 5;
				while((yPos < oldBitmap.Height) && (GetSpreadSample(xPos, yPos, 2, oldBitmap) > 0.5))
					yPos++;

				if(yPos < (oldBitmap.Height / 2))
					xIntersects[xPos] = yPos;
				else
					xIntersects[xPos] = -1;
			}

			return xIntersects;
		}

		private static float GetRotationAngle(int[] xIntersects)
		{
			// Calculate the slopes and rotate
			int totalRise = 0;
			int totalRun = 0;
			int slopeOffset = 30;
			int left = 0;
			int right = 0;
			for(int xPos = 0; xPos < xIntersects.Length; xPos++)
			{
				if((xPos - slopeOffset) >= 0)
					left = xPos - slopeOffset;
				else
					left = xPos;

				if((xPos + slopeOffset) < xIntersects.Length)
					right = xPos + slopeOffset;
				else
					right = xPos;

				if(
					(xIntersects[right] > 0) && 
					(xIntersects[left] > 0) && 
					(Math.Abs((double) (xIntersects[right] - xIntersects[left]) / (double) (right - left)) < 0.45)
					)
				{
					totalRise += (xIntersects[right] - xIntersects[left]);
					totalRun += (right - left);
				}
				else
				{
				}
			}

			double slopeAverage = ((double) totalRise / (double) totalRun);
			float rotate = (float) - (Math.Atan(slopeAverage) * 180 / Math.PI);

			return rotate;
		}

		private static int GetTopLine(Bitmap oldBitmap)
		{
			double darkest = Double.MaxValue;
			int darkestPos = 0; 
			double rowColor = 0;
			for(int yPos = 0; yPos < (oldBitmap.Height / 3); yPos++)
			{
				rowColor = 0;
				for(int xPos = 0; xPos < oldBitmap.Width; xPos++)
					rowColor += GetSpreadSample(xPos, yPos, 2, oldBitmap);

				if((rowColor / darkest) < 0.7 )
				{
					Debug.WriteLine("New Darkest: " + rowColor + "@" + yPos);
					if(darkest != Double.MaxValue)
						return yPos;

					darkestPos = yPos;
					darkest = rowColor;

				}
			}
			return darkestPos;
		}

		private static int GetSecondLine(int start, Bitmap oldBitmap)
		{
			double darkest = Double.MaxValue;
			int darkestPos = 0; 
			double rowColor = 0;
			for(int yPos = start + 5; yPos < oldBitmap.Height; yPos++)
			{
				rowColor = 0;
				for(int xPos = 0; xPos < oldBitmap.Width; xPos++)
					rowColor += GetSpreadSample(xPos, yPos, 2, oldBitmap);

				if((rowColor / darkest) < 0.7 )
				{
					if(darkest != Double.MaxValue)
					{
						Debug.WriteLine("Got second line @" + yPos);
						return yPos;
					}

					darkestPos = yPos;
					darkest = rowColor;
				}
			}
			return darkestPos;
		}

		private static int GetBottomLine(Bitmap oldBitmap)
		{
			double darkest = Double.MaxValue;
			int darkestPos = 0; 
			double rowColor = 0;
			for(int yPos = oldBitmap.Height - (oldBitmap.Height / 3); yPos < oldBitmap.Height; yPos++)
			{
				rowColor = 0;
				for(int xPos = 0; xPos < oldBitmap.Width; xPos++)
					rowColor += GetSpreadSample(xPos, yPos, 2, oldBitmap);

				if((rowColor / darkest) < 0.7 )
				{
					if(darkest != Double.MaxValue)
						return yPos;

					darkestPos = yPos;
					darkest = rowColor;
				}
			}
			return darkestPos;
		}

		private static int GetLeftLine(Bitmap oldBitmap, int topLine, int bottomLine)
		{
			double darkest = Double.MaxValue;
			int darkestPos = 0; 
			double columnColor = 0;
			for(int xPos = 10; xPos < oldBitmap.Width; xPos++)
			{
				columnColor = 0;
				for(int yPos = topLine; yPos < bottomLine; yPos++)
					columnColor += GetSpreadSample(xPos, yPos, 2, oldBitmap);

				if((columnColor / darkest) < 0.7 )
				{
					if(darkest != Double.MaxValue)
						return xPos;

					darkestPos = xPos;
					darkest = columnColor;
				}
			}
			return darkestPos;
		}

		private static int GetRightLine(Bitmap oldBitmap, int topLine, int bottomLine)
		{
			double darkest = Double.MaxValue;
			int darkestPos = 0; 
			double columnColor = 0;
			for(int xPos = oldBitmap.Width - 10; xPos >= 0; xPos--)
			{
				columnColor = 0;
				for(int yPos = topLine; yPos < bottomLine; yPos++)
					columnColor += GetSpreadSample(xPos, yPos, 2, oldBitmap);

				if((columnColor / darkest) < 0.7 )
				{
					if(darkest != Double.MaxValue)
						return xPos;

					darkestPos = xPos;
					darkest = columnColor;
				}
			}
			return darkestPos;
		}

		public static float GetSpreadSample(double xPos, double yPos, double sampleRadius, Bitmap sampleBitmap)
		{
			// Center-weighted points:
			// 1) 3 * Center
			// 2) 2 * Center += (sampleRadius / 4)
			// 3) 1 * Center += sampleRadius
			float totalBrightness = 0;
			int sampleCount = 0;

			// Center
			totalBrightness += (3 * sampleBitmap.GetPixel((int) xPos, (int) yPos).GetBrightness());
			sampleCount += 3;

			if(sampleRadius > 4)
			{
				// Inner Top Left
				if(((xPos - (sampleRadius / 4)) >= 0) && ((yPos - (sampleRadius / 4)) >= 0))
				{
					totalBrightness += (2 * sampleBitmap.GetPixel((int) (xPos - (sampleRadius / 4)), (int) (yPos - (sampleRadius / 4))).GetBrightness());
					sampleCount += 2;
				}

				// Inner Top Right
				if(((xPos + (sampleRadius / 4)) < sampleBitmap.Width) && ((yPos - (sampleRadius / 4)) >= 0))
				{
					totalBrightness += (2 * sampleBitmap.GetPixel((int) (xPos + (sampleRadius / 4)), (int) (yPos - (sampleRadius / 4))).GetBrightness());
					sampleCount += 2;
				}

				// Inner Bottom Left
				if(((xPos - (sampleRadius / 4)) >= 0) && ((yPos + (sampleRadius / 4)) < sampleBitmap.Height))
				{
					totalBrightness += (2 * sampleBitmap.GetPixel((int) (xPos - (sampleRadius / 4)), (int) (yPos + (sampleRadius / 4))).GetBrightness());
					sampleCount += 2;
				}

				// Inner Bottom Right
				if(((xPos + (sampleRadius / 4)) < sampleBitmap.Width) && ((yPos + (sampleRadius / 4)) < sampleBitmap.Height))
				{
					totalBrightness += (2 * sampleBitmap.GetPixel((int) (xPos + (sampleRadius / 4)), (int) (yPos + (sampleRadius / 4))).GetBrightness());
					sampleCount += 2;
				}
			}

			if(sampleRadius > 0)
			{
				// Outer Top Left
				if(((xPos - sampleRadius) >= 0) && ((yPos - sampleRadius) >= 0))
				{
					totalBrightness += (1 * sampleBitmap.GetPixel((int) (xPos - sampleRadius), (int) (yPos - sampleRadius)).GetBrightness());
					sampleCount += 1;
				}

				// Outer Top Right
				if(((xPos + sampleRadius) < sampleBitmap.Width) && ((yPos - sampleRadius) >= 0))
				{
					totalBrightness += (1 * sampleBitmap.GetPixel((int) (xPos + sampleRadius), (int) (yPos - sampleRadius)).GetBrightness());
					sampleCount += 1;
				}

				// Outer Bottom Left
				if(((xPos - sampleRadius) >= 0) && ((yPos + sampleRadius) < sampleBitmap.Height))
				{
					totalBrightness += (1 * sampleBitmap.GetPixel((int) (xPos - sampleRadius), (int) (yPos + sampleRadius)).GetBrightness());
					sampleCount += 1;
				}

				// Outer Bottom Right
				if(((xPos + sampleRadius) < sampleBitmap.Width) && ((yPos + sampleRadius) < sampleBitmap.Height))
				{
					totalBrightness += (1 * sampleBitmap.GetPixel((int) (xPos + sampleRadius), (int) (yPos + sampleRadius)).GetBrightness());
					sampleCount += 1;
				}
			}

			totalBrightness /= sampleCount;
			return totalBrightness;
		}
	}
}
