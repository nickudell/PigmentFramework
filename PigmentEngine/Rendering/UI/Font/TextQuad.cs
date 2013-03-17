using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Windows;
using Pigment.Engine.Rendering.Matter;

namespace Pigment.Engine.Rendering.UI.Font
{
    /// <summary>
    /// A quad to be rendered in batch for performance reasons, tied to a bitmap font character
    /// </summary>
    public class TextQuad : Quad
    {
        /// <summary>
        /// The line number of this character in the text
        /// </summary>
        private int lineNumber;
        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        /// <value>
        /// The line number.
        /// </value>
        public int LineNumber { get; set; }

        /// <summary>
        /// The word number in this line of text
        /// </summary>
        private int wordNumber;
        /// <summary>
        /// Gets or sets the word number.
        /// </summary>
        /// <value>
        /// The word number.
        /// </value>
        public int WordNumber { get; set; }

        /// <summary>
        /// The scale factor for the font size
        /// </summary>
        private float sizeScale;
        /// <summary>
        /// Gets or sets the size scale.
        /// </summary>
        /// <value>
        /// The size scale.
        /// </value>
        public float SizeScale { get; set; }

        /// <summary>
        /// The font character to render
        /// </summary>
        private FontChar fontChar;
        /// <summary>
        /// Gets or sets the font char.
        /// </summary>
        /// <value>
        /// The font char.
        /// </value>
        public FontChar FontChar { get; set; }

        /// <summary>
        /// The character of text this represents
        /// </summary>
        private char character;
        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        /// <value>
        /// The character.
        /// </value>
        public char Character { get; set; }

        /// <summary>
        /// The word width
        /// </summary>
        private float wordWidth;
        /// <summary>
        /// Gets or sets the width of the word.
        /// </summary>
        /// <value>
        /// The width of the word.
        /// </value>
        public float WordWidth { get; set; }

        private float fontWidth;
        private float fontHeight;

        public TextQuad(int x, int y, int screenWidth, int screenHeight, int width, int height, Color4 colour, char character, FontChar fontChar, float fontWidth, float fontHeight, float sizeScale, int lineNumber, int wordNumber, float wordWidth)
            : base(x, y, screenWidth, screenHeight, "NULL", width, height, colour)
        {
            this.fontChar = fontChar;
            this.fontWidth = fontWidth;
            this.fontHeight = fontHeight;
            this.wordWidth = wordWidth;
            this.character = character;
            this.lineNumber = lineNumber;
            this.wordNumber = wordNumber;
            this.sizeScale = sizeScale;

            vertices[0].TexCoords = new Vector2(fontChar.Position.X / fontWidth, fontChar.Position.Y / fontHeight);
            vertices[1].TexCoords = new Vector2((fontChar.Position.X + fontChar.Width) / fontWidth, (fontChar.Position.Y + fontChar.Height) / fontHeight);
            vertices[2].TexCoords = new Vector2(fontChar.Position.X / fontWidth, (fontChar.Position.Y + fontChar.Height) / fontHeight);
            vertices[3].TexCoords = new Vector2(fontChar.Position.X / fontWidth, fontChar.Position.Y / fontHeight);
            vertices[4].TexCoords = new Vector2((fontChar.Position.X + fontChar.Width) / fontWidth, fontChar.Position.Y / fontHeight);
            vertices[5].TexCoords = new Vector2((fontChar.Position.X + fontChar.Width) / fontWidth, (fontChar.Position.Y + fontChar.Height) / fontHeight);
        }
    }
}
