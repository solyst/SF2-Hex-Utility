using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SF2_Hex_Utility
{
    class HexRenderer
    {
        //Default highlight color.
        public static Color HighlightColor = Color.Yellow;
        public static Font HexFont = new Font("Consolas", 8, FontStyle.Regular);
        public static Color HexFontColor = Color.Black;

        //How many bytes are grouped together before a space separater is inserted.
        //Can't be zero. Use a negative number to exclude the space separater.
        private static int groupSize = 1;
        public static int GroupSize
        {
            get { return groupSize; }
            set
            {
                if (value == 0)
                    groupSize = -1;
                else
                    groupSize = value;
            }
        }

        //How many bytes are shown before a new line, if multiline is selected.
        //Bounded by 1 to 64.
        private static int bytesPerLine = 16;
        public static int BytesPerLine
        {
            get { return bytesPerLine; }
            set
            {
                if (value < 1)
                    bytesPerLine = 1;
                else if (value > 64)
                    bytesPerLine = 64;
                else
                    bytesPerLine = value;
            }
        }


        //---------------------------------------------------------
        // Object to Hex String Functions
        //---------------------------------------------------------
        #region ObjectToHex_Functions


        /// <summary>
        /// Return a hex string representation of the provided integer.
        /// </summary>
        /// <param name="value">Signed integer.</param>
        /// <param name="length">Number of bytes to show in the resulting hex. Valid entries are 1, 2, 4, 8.</param>
        /// <returns>String of hex of size length.</returns>
        public static string IntToHexString(Int64 value, int length)
        {
            byte[] bytes = ByteConversion.ToBytes(value, length);
            return BytesToHexString(bytes, false);
        }


        /// <summary>
        /// Return a hex string representation of the provided unsigned integer.
        /// </summary>
        /// <param name="value">Unsigned integer.</param>
        /// <param name="length">Number of bytes to show in the resulting hex. Valid entries are 1, 2, 4, 8.</param>
        /// <returns>String of hex of size length.</returns>
        public static string UIntToHexString(UInt64 value, int length)
        {
            byte[] bytes = ByteConversion.ToBytes(value, length);
            return BytesToHexString(bytes, false);
        }


        //Overload for function below
        public static string BytesToHexString(byte[] bytes, bool multiline = false, int firstByteOffset = 0)
        {
            List<int> byteLocationsInString = new List<int>();
            return BytesToHexString(bytes, multiline, firstByteOffset, ref byteLocationsInString);
        }


        /// <summary>
        /// Convert bytes into human readable hex with options for how to handle multiline, spacing, and alignment.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="multiline"></param>
        /// <param name="firstByteOffset"> </param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bytes, bool multiline, int firstByteOffset, ref List<int> byteLocationsInString)
        {
            //No reason to add blank lines, so prevent the firstByteOffset from exceeding the BytesPerLine.
            //If not multiline, don't add any padding.
            if (multiline == true && firstByteOffset > 0)
            {
                firstByteOffset %= BytesPerLine;
            }
            else
            {
                firstByteOffset = 0;
            }

            //Construct the string representation of the hex.
            string s = BitConverter.ToString(bytes);
            StringBuilder sb = new StringBuilder();
            //Padding before the hex on the first line.
            for (int i = 0; i < firstByteOffset; i++)
            {
                sb.Append("  ");
                if ((i + 1) % GroupSize == 0)
                {
                    sb.Append(" ");
                }
            }
            //Add spaces between the bytes.
            for (int i = 0; i < bytes.Length; i++)
            {
                byteLocationsInString.Add(sb.Length); //record the location in the resulting string for a given byte

                sb.Append(s.Substring(i * 3, 2));   //append the string with the new byte converted into hex
                if ((i + 1) % GroupSize == 0)
                {
                    sb.Append(" ");
                }
                if ((i + 1 + firstByteOffset) % BytesPerLine == 0)
                {
                    sb.AppendLine();
                }
            }
            //Return the result.
            return sb.ToString();
        }


        /// <summary>
        /// Convert bytes into human readable hex. Wildcards (which are NULL bytes) are represented by a double underline "__".
        /// </summary>
        /// <param name="bytes">Array of nullable bytes to convert into hex.</param>
        public static string BytesToHexStringWildcard(byte?[] bytes)
        {
            if (bytes == null)
            {
                return String.Empty;
            }
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (byte? b in bytes)
            {
                if (b == null)
                {
                    sb.Append("__");
                }
                else
                {
                    sb.Append(BitConverter.ToString(new byte[] { (byte)b }));
                }
                if ((i + 1) % GroupSize == 0)
                {
                    sb.Append(" ");
                }
                i++;
            }
            return sb.ToString();
        }


        /// <summary>
        /// Get a string of hex suitable for a header.
        /// </summary>
        /// <returns>Hex from 00 to bytesPerLine.</returns>
        public static string GetHeaderString()
        {
            List<byte> bytes = new List<byte>();
            for (byte i = 0; i < bytesPerLine; i++)
            {
                bytes.Add(i);
            }
            return BytesToHexString(bytes.ToArray(), false, 0);
        }


        /// <summary>
        /// Get a string of lines of offset values, starting at location.
        /// </summary>
        /// <param name="location">First offset.</param>
        /// <param name="rows">Number of rows of offsets.</param>
        /// <returns>Rows of offsets.</returns>
        public static string GetOffsetString(int location, int rows)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                sb.AppendLine(ByteConversion.ToHex(ByteConversion.ToBytes(location, 4)));
                location += bytesPerLine;
            }
            return sb.ToString();
        }


        #endregion



        //---------------------------------------------------------
        // Draw Hex String
        //---------------------------------------------------------
        #region DrawHexString_Functions

        /// <summary>
        /// Convert bytes into hex and draw the result with the appropriate highlights.
        /// </summary>
        /// <param name="g">Graphics object on which to draw the text.</param>
        /// <param name="bytes"></param>
        /// <param name="multiline"></param>
        /// <param name="firstByteOffset"></param>
        /// <param name="highlightHexIndex"></param>
        /// <param name="highlightHexLength"></param>
        /// <param name="drawLocation"></param>
        /// <param name="highlightColor"></param>
        /// <returns></returns>
        public static PointF DrawHexWithHighlight(Graphics g, byte[] bytes,
             bool multiline = false, int firstByteOffset = 0, int highlightHexIndex = 0, int highlightHexLength = 0, Point drawLocation = default(Point), Color highlightColor = default(Color))
        {
            //The optional parameters are handled in the function calls.

            //Error check
            if (bytes == null || bytes.Length == 0)
                return new Point(0, 0);

            //Create the hex string to draw
            List<int> byteLocationsInString = new List<int>();
            string hexString = BytesToHexString(bytes, multiline, firstByteOffset, ref byteLocationsInString);

            //Calculate the highlight range and handle invalid entries
            if (highlightHexIndex >= bytes.Length || highlightHexIndex == -1)
            {
                highlightHexIndex = 0;
                highlightHexLength = 0;
            }
            else if (highlightHexLength == -1)
            {
                highlightHexLength = bytes.Length - highlightHexIndex;
            }
            int charactersToHighlight;
            if (highlightHexIndex + highlightHexLength >= bytes.Length)
                charactersToHighlight = hexString.Length - byteLocationsInString[highlightHexIndex];
            else
                charactersToHighlight = byteLocationsInString[highlightHexIndex + highlightHexLength] - byteLocationsInString[highlightHexIndex];

            //Draw the string
            PointF p = DrawStringWithHighlight(g, hexString, byteLocationsInString[highlightHexIndex], charactersToHighlight, drawLocation, highlightColor);

            //Return the final location of the text (useful for appending additional text).
            return p;
        }



        /// <summary>
        /// Draw the multiline string, text, at point drawLocation, highlighting a portion of the text with color highlightcolor.
        /// </summary>
        /// <param name="g">Graphics object of the object on which to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="highlightIndex">Zero-based location of the character index on which to begin highlighting. If -1 then no text is highlighted.</param>
        /// <param name="highlightLength">How many characters to highlight.</param>
        /// <param name="drawLocation">Point to begin drawing the text.</param>
        /// <param name="highlightColor">Background color behind the text.</param>
        public static PointF DrawStringWithHighlight(Graphics g, string text, int highlightIndex = 0, int highlightLength = 0, PointF drawLocation = default(PointF), Color highlightColor = default(Color))
        {
            //Set to the default color if color is not specified.
            if (highlightColor == default(Color))
            {
                highlightColor = HighlightColor;
            }

            //Set default point if point is not specified.
            if (drawLocation == default(Point))
            {
                drawLocation = new Point(0, 0);
            }

            //If highlightLength == -1 then highlight the entire text starting from the highlightIndex
            if (highlightLength == -1)
            {
                highlightLength = text.Length - highlightIndex;
            }

            //Identify and draw the highlights
            if (highlightLength > 0 && highlightIndex > -1)
            {
                PointF drawLocation2 = drawLocation;
                if (highlightIndex > 0)
                {
                    string startString = text.Substring(0, highlightIndex);
                    drawLocation2 = pointEndOfText(g, startString, HexFont, drawLocation);
                }
                string stringToHighlight = text.Substring(highlightIndex, highlightLength);
                HighlightText(g, stringToHighlight, drawLocation2, drawLocation.X, highlightColor);
            }

            //Draw the text
            TextRenderer.DrawText(g, text, HexFont, Point.Round(drawLocation), HexFontColor);

            //Return the end location of the text draw
            return pointEndOfText(g, text, HexFont, drawLocation); ;
        }


        /// <summary>
        /// Highlight text in the middle of a line and have the highlight correctly carry over to the beginning of the new line.
        /// </summary>
        /// <param name="g">Graphics objects on which to draw the highlight.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="pointAtWhichToAppend">Where to start drawing the new text.</param>
        /// <param name="X">The X location of the start of a new line.</param>
        /// <param name="highlightText">If TRUE, then highlight the drawn text with the highlightColor.</param>
        /// /// <param name="highlightColor">If the text is highlighted, the color of that highlight.</param>
        /// <returns></returns>
        public static PointF HighlightText(Graphics g, string text, PointF pointAtWhichToHighlight, float X, Color highlightColor = default(Color))
        {
            //Use the default highlight color (if requested).
            if (highlightColor == default(Color))
            {
                highlightColor = HighlightColor; //default highlight color
            }

            //Check if more than one line is being drawn
            PointF returnPoint;
            int indexOfNewLine = text.IndexOf(Environment.NewLine);
            int lineHeight = TextRenderer.MeasureText("A", HexFont).Height;
            int indexOfLastLine = text.LastIndexOf(Environment.NewLine);
            int newLineLength = Environment.NewLine.Length;
            SolidBrush highlightBrush = new SolidBrush(highlightColor);
            if (indexOfNewLine == -1)
            {
                //Draw a single line
                float textWidth = MeasureDisplayString(g, text, HexFont).Width;
                g.FillRectangle(highlightBrush, new RectangleF(pointAtWhichToHighlight, new SizeF(textWidth, lineHeight)));
                returnPoint = new PointF(pointAtWhichToHighlight.X + textWidth, pointAtWhichToHighlight.Y);
            }
            else
            {
                //Calculate the first and last lines (same in all cases)
                string firstLine = text.Substring(0, indexOfNewLine + newLineLength - 1);
                string middleLines = "";
                string lastLine = text.Substring(indexOfLastLine + newLineLength);
                //Check if there is more than two lines and if so, calculate the middle lines
                if (indexOfNewLine < indexOfLastLine)
                {
                    middleLines = text.Substring(indexOfNewLine + newLineLength, indexOfLastLine - indexOfNewLine - newLineLength);
                }

                //Draw the highlights
                RectangleF textRectangle = MeasureDisplayString(g, firstLine, HexFont, pointAtWhichToHighlight);
                g.FillRectangle(highlightBrush, textRectangle);
                PointF p = new PointF(X, pointAtWhichToHighlight.Y + lineHeight);
                if (middleLines != "")
                {
                    textRectangle = MeasureDisplayString(g, middleLines, HexFont, p);
                    g.FillRectangle(highlightBrush, textRectangle);
                    p = new PointF(X, textRectangle.Bottom);
                }
                textRectangle = MeasureDisplayString(g, lastLine, HexFont, p);
                g.FillRectangle(highlightBrush, textRectangle);
                returnPoint = new PointF(textRectangle.Right, textRectangle.Top);
            }

            //Return the new location
            return returnPoint;
        }


        /// <summary>
        /// Draw the header for a hex grid.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="drawLocation">Point at which to draw the header.</param>
        public static void DrawHexHeader(Graphics g, Point drawLocation = default(Point))
        {
            TextRenderer.DrawText(g, GetHeaderString(), HexFont, Point.Round(drawLocation), HexFontColor);
        }


        /// <summary>
        /// Draw rows of offset values, starting with location.
        /// </summary>
        /// <param name="g">Graphics object on which to draw the offsets.</param>
        /// <param name="location">First offset to draw.</param>
        /// <param name="rows">Number of offsets to draw.</param>
        /// <param name="drawLocation">Point at which to draw the offsets.</param>
        /// <returns></returns>
        public static PointF DrawHexOffsets(Graphics g, int location, int rows, Point drawLocation = default(Point))
        {
            string textToDraw = GetOffsetString(location, rows);
            TextRenderer.DrawText(g, textToDraw, HexFont, Point.Round(drawLocation), HexFontColor);
            return new PointF(drawLocation.X, MeasureDisplayString(g, textToDraw, HexFont, drawLocation).Bottom);
        }


        #endregion



        //---------------------------------------------------------
        // Public Helper Functions
        //---------------------------------------------------------
        #region PublicHelper_Functions

        /// <summary>
        /// Find the byte at the start of a line for the location.
        /// </summary>
        /// <param name="location">The location of the byte in the binary.</param>
        /// <returns>Return the start of a line for the location.</returns>
        static public int FindFirstByte(int location)
        {
            if (location < 0)
                location += int.MaxValue / bytesPerLine * bytesPerLine;
            return (int)((location / bytesPerLine) * bytesPerLine);
        }


        /// <summary>
        /// Find how many bytes from the start of a line is the start of the location, and return that value.
        /// For example, if a line is 16 bytes long and location = 0A E8, then this would return 8.
        /// </summary>
        /// <param name="location">The location of the data in the binary.</param>
        /// <returns></returns>
        static public int FindFirstByteOffset(int location)
        {
            if (location < 0)
                location += int.MaxValue / bytesPerLine * bytesPerLine;
            return (int)(location % bytesPerLine);
        }


        //adapted from https://www.codeproject.com/Articles/2118/Bypass-Graphics-MeasureString-limitations#premain1
        static public RectangleF MeasureDisplayString(Graphics graphics, string text, Font font, PointF drawPoint = default(PointF))
        {
            //Handle default point
            if (drawPoint == default(PointF))
                drawPoint = new PointF(0, 0);

            //Leading and trailing returns may count as spaces and change the size of the text measurement
            text = text.Trim(new char[] { '\r', '\n' });

            //MeasureCharacterRanges errors if there is no text 
            if (text == "")
                return new RectangleF(drawPoint.X, drawPoint.Y, 0, 0);

            //Measure the text rectangle
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF(drawPoint.X, drawPoint.Y, 1000, 1000);
            System.Drawing.CharacterRange[] ranges = { new System.Drawing.CharacterRange(0, text.Length) };

            format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            format.SetMeasurableCharacterRanges(ranges);

            System.Drawing.Region[] regions = graphics.MeasureCharacterRanges(text, font, rect, format);
            rect = regions[0].GetBounds(graphics);

            //Return value
            return rect;
        }


        //Unused measure example that maybe sorta works?
        static public int MeasureDisplayStringWidth(string text, Font font)
        {
            Size s1 = TextRenderer.MeasureText(text, font);
            Size s2 = TextRenderer.MeasureText(text + text, font);
            return s2.Width - s1.Width;
        }


        /// <summary>
        /// Find the location at the end of the drawn text at which more text can be drawn.
        /// </summary>
        /// <param name="text">Text that was drawn.</param>
        /// <param name="pointStartOfText">Starting point for the original text.</param>
        /// <returns></returns>
        private static PointF pointEndOfText(Graphics graphics, string text, Font font, PointF pointStartOfText)
        {
            int indexOfNewLine = text.LastIndexOf(Environment.NewLine);
            if (indexOfNewLine == -1)
            {
                RectangleF textRectangle = MeasureDisplayString(graphics, text, HexFont, pointStartOfText);
                //return new PointF(textRectangle.Right, textRectangle.Top);
                return new PointF(pointStartOfText.X + textRectangle.Width, textRectangle.Top);
            }
            else
            {
                RectangleF textSizeOnlyLastLine = MeasureDisplayString(graphics, text.Substring(indexOfNewLine), HexFont, pointStartOfText);
                RectangleF textSizeExcludeLastLine = MeasureDisplayString(graphics, text.Substring(0, indexOfNewLine - 1), HexFont, pointStartOfText);
                return new PointF(textSizeOnlyLastLine.Right, textSizeExcludeLastLine.Bottom);
            }
        }

        #endregion



        //---------------------------------------------------------
        // Old Rendering
        //---------------------------------------------------------
        #region OldRendering_DeleteMeLater
        /*

        /// <summary>
        /// Draw the multiline string, text, at point drawLocation, highlighting a portion of the text with color highlightcolor.
        /// </summary>
        /// <param name="g">Graphics object of the object on which to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="highlightIndex">Zero-based location of the character index on which to begin highlighting. If -1 then no text is highlighted.</param>
        /// <param name="highlightLength">How many characters to highlight.</param>
        /// <param name="drawLocation">Point to begin drawing the text.</param>
        /// <param name="highlightColor">Background color behind the text.</param>
        public static Point DrawStringWithHighlightLegacy(Graphics g, string text, int highlightIndex = 0, int highlightLength = 0, Point drawLocation = default(Point), Color highlightColor = default(Color))
        {
            //Set to the default color if color is not specified.
            if (highlightColor == default(Color))
            {
                highlightColor = HighlightColor;
            }

            //Set default point if point is not specified.
            if (drawLocation == default(Point))
            {
                drawLocation = new Point(0, 0);
            }

            //If highlightLength == -1 then highlight the entire text starting from the highlightIndex
            if (highlightLength == -1)
            {
                highlightLength = text.Length - highlightIndex;
            }

            //Identify and draw the text components
            Point returnLocation;
            if (highlightLength > 0 && highlightIndex > -1)
            {
                Point drawLocation2 = drawLocation;
                if (highlightIndex > 0)
                {
                    string startString = text.Substring(0, highlightIndex);            //not highlighted
                    drawLocation2 = AppendDrawString(g, startString, drawLocation, drawLocation.X, false);
                    g.DrawLine(Pens.Blue, drawLocation2, drawLocation);
                }
                string midString = text.Substring(highlightIndex, highlightLength);    //highlighted
                Point drawLocation3 = AppendDrawString(g, midString, drawLocation2, drawLocation.X, true, highlightColor);
                g.DrawLine(Pens.Red, drawLocation3, drawLocation2);
                string endString = text.Substring(highlightIndex + highlightLength);   //not highlighted
                returnLocation = AppendDrawString(g, endString, drawLocation3, drawLocation.X, false);
            }
            else
            {
                //Draw the text
                returnLocation = AppendDrawString(g, text, drawLocation, drawLocation.X, false);
            }

            //Return the end location of the text draw
            return returnLocation;
        }


        /// <summary>
        /// Draw text in the middle of a line and have the new line correctly carry over to the beginning of the new line (not the middle of the line where the prior text ended).
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <param name="pointAtWhichToAppend">Where to start drawing the new text.</param>
        /// <param name="X">The X location of the start of a new line.</param>
        /// <param name="highlightText">If TRUE, then highlight the drawn text with the highlightColor.</param>
        /// /// <param name="highlightColor">If the text is highlighted, the color of that highlight.</param>
        /// <returns></returns>
        public static Point AppendDrawString(Graphics g, string text, Point pointAtWhichToAppend, int X, bool highlightText, Color highlightColor = default(Color))
        {
            //Identify the correct highlight color (if one is used)
            if (highlightText == false)
            {
                highlightColor = Color.Transparent;
            }
            else if (highlightColor == default(Color))
            {
                highlightColor = HighlightColor; //default highlight color
            }

            //Check if more than one line is being drawn
            Point returnPoint;
            int indexOfNewLine = text.IndexOf(Environment.NewLine);
            if (indexOfNewLine == -1)
            {
                //Draw a single line
                //TextRenderer.DrawText(g, text, HexFont, pointAtWhichToAppend, HexFontColor, highlightColor, TextFormatFlags.NoPadding);
                //returnPoint = new Point(pointAtWhichToAppend.X + TextRenderer.MeasureText(text, HexFont).Width, pointAtWhichToAppend.Y);
                g.DrawString(text, HexFont, Brushes.Black, pointAtWhichToAppend);
                returnPoint = new Point(pointAtWhichToAppend.X + (int)MeasureDisplayString(g, text, HexFont).Width, pointAtWhichToAppend.Y);
            }
            else
            {
                int indexOfLastLine = text.LastIndexOf(Environment.NewLine);
                int newLineLength = Environment.NewLine.Length;
                int lineHeight = TextRenderer.MeasureText("A", HexFont).Height;

                //Calculate the first and last lines (same in all cases)
                string firstLine = text.Substring(0, indexOfNewLine - 1);
                string middleLines = "";
                string lastLine = text.Substring(indexOfLastLine + newLineLength);
                //Check if there is more than two lines and if so, calculate the middle lines
                if (indexOfNewLine < indexOfLastLine)
                {
                    middleLines = text.Substring(indexOfNewLine + newLineLength, indexOfLastLine - indexOfNewLine - newLineLength);
                }

                //Draw the text
                g.FillRectangle(Brushes.Yellow, new RectangleF(pointAtWhichToAppend, new Size(200, lineHeight)));
                TextRenderer.DrawText(g, firstLine, HexFont, pointAtWhichToAppend, HexFontColor, highlightColor, TextFormatFlags.NoPadding);
                g.DrawString(firstLine, HexFont, Brushes.Black, new Point(0,80), StringFormat.GenericTypographic);
                Point p = new Point(X, pointAtWhichToAppend.Y + lineHeight);
                if (middleLines != "")
                {
                    //TextRenderer.DrawText(g, middleLines, HexFont, p, HexFontColor, highlightColor, TextFormatFlags.NoPadding);
                    g.DrawString(middleLines, HexFont, Brushes.Black, p);
                    p = new Point(X, p.Y + TextRenderer.MeasureText(middleLines, HexFont).Height); //always the beginning of a new line
                }
                //TextRenderer.DrawText(g, lastLine, HexFont, p, HexFontColor, highlightColor, TextFormatFlags.NoPadding);
                g.DrawString(lastLine, HexFont, Brushes.Black, p);
                returnPoint = new Point(X + TextRenderer.MeasureText(lastLine, HexFont, new Size(1000,1000), TextFormatFlags.TextBoxControl).Width, p.Y);
                returnPoint = new Point(X + (int)MeasureDisplayString(g,lastLine, HexFont).Width, p.Y);
            }

            //Return the new location
            return returnPoint;
        }

        */
        #endregion


    }
}
