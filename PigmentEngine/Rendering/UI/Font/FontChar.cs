using SlimDX;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Pigment.Engine.Rendering.UI.Font
{
    /// <summary>
    /// A single bitmap character
    /// </summary>
    public class FontChar
    {
        /// <summary>
        /// The position of the character
        /// </summary>
        public Tuple<int,int> Position;
        /// <summary>
        /// The character's width and height
        /// </summary>
        public int Width;

        public int Height;
        /// <summary>
        /// The offset of the character 
        /// </summary>
        public Tuple<int,int> Offset;
        /// <summary>
        /// How much to advance in the X direction when this character is used
        /// </summary>
        public int XAdvance;
        /// <summary>
        /// The kerning list
        /// </summary>
        public List<Kerning> KerningList = new List<Kerning>();

        public FontChar()
        {
            Position = new Tuple<int, int>(0, 0);
            Width = 0;
            Height = 0;
            Offset = new Tuple<int, int>(0, 0);
            XAdvance = 0;
            KerningList = new List<Kerning>();
        }
    }

    /// <summary>
    /// Kerning information for a character
    /// </summary>
    public class Kerning
    {
        /// <summary>
        /// The character coming after this character
        /// </summary>
        public char Second;
        /// <summary>
        /// The amount of kerning
        /// </summary>
        public int Amount;
    }

    /// <summary>
    /// Font alignment
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Left alignment
        /// </summary>
        Left,
        /// <summary>
        /// Center alignment
        /// </summary>
        Center,
        /// <summary>
        /// Right alignment
        /// </summary>
        Right
    }

    /// <summary>
    /// A string to be rendered
    /// </summary>
    public struct FontString
    {
        /// <summary>
        /// The text to render
        /// </summary>
        public string Text;
        /// <summary>
        /// The text box
        /// </summary>
        public Rect TextBox;
        /// <summary>
        /// The alignment of the text
        /// </summary>
        public Alignment Alignment;
        /// <summary>
        /// The font size
        /// </summary>
        public float FontSize;
        /// <summary>
        /// The colour
        /// </summary>
        public Color4 Colour;
        /// <summary>
        /// Whether to use kerning
        /// </summary>
        public bool Kerning;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontString" /> struct.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="textBox">The text box.</param>
        /// <param name="alignment">The font alignment.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="colour">The colour.</param>
        /// <param name="kerning">if set to <c>true</c> [kerning].</param>
        public FontString(string text, Rect textBox, Alignment alignment, float fontSize, Color4 colour, bool kerning)
        {
            Text = text;
            TextBox = textBox;
            Alignment = alignment;
            FontSize = fontSize;
            Colour = colour;
            Kerning = kerning;
        }
    }
}