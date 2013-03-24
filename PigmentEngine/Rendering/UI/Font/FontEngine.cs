using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using D3D=SlimDX.Direct3D11;
using System.Windows;
using Pigment.WPF;
using Pigment.Engine.Rendering.Matter;
using Pigment.Engine.Rendering.Textures;
using Pigment.Engine.Rendering.Matter.Vertices;

namespace Pigment.Engine.Rendering.UI.Font
{
    public class FontEngine : RenderableIndexed<VertexPosTexCol>
    {
        private Font font;
        private List<TextQuad> quads;
        private List<FontString> strings;
        private string fontFile;
        private bool changed = true;
        private D3D.Device device;
        public Texture Texture {get; private set;}

        public int ScreenWidth {get; set;}
        public int ScreenHeight {get; set;}

        private const int maxVertices = 4096;
        private int nextChar;

        public FontEngine(D3D.Device device, string fontFile, string textureFile, int screenWidth, int screenHeight)
            : base(D3D.PrimitiveTopology.TriangleList)
        {
            this.fontFile = fontFile;
            this.device = device;
            this.ScreenWidth = screenWidth;
            this.ScreenHeight = screenHeight;
            
            quads = new List<TextQuad>();
            
            strings = new List<FontString>();
            font = new Font(fontFile);
            Texture = new Texture(device, textureFile);
        }

        public void AddString(FontString text)
        {
            strings.Add(text);
            quads.AddRange(BuildQuads(text));
            changed = true;
        }

        public void RemoveString(FontString text)
        {
            strings.Remove(text);
            changed = true;
        }

        public void ClearStrings()
        {
            strings.Clear();
            quads.Clear();
            changed = true;
        }

        public void DrawIndexed(D3D.DeviceContext context)
        {
            if (changed)
            {
                List<VertexPosTexCol> vertices = new List<VertexPosTexCol>();
                List<ushort> indices = new List<ushort>();
                ushort indexOffset = 0;
                foreach (TextQuad quad in quads)
                {
                    vertices.AddRange(quad.Vertices);
                    ushort newIndex = 0;
                    foreach (ushort index in quad.Indices)
                    {
                        newIndex = (ushort)(index + indexOffset);
                        indices.Add(newIndex);
                    }
                    indexOffset+=6;
                }
                indexCount = indices.Count;

                vertexBuffer = createVertexBuffer(device, vertices);
                indexBuffer = createIndexBuffer(device, indices.ToArray());

                changed = false;
            }
            base.Draw(context);
        }

        /// <summary>Search predicate used to find nodes in kerningList</summary> 
        /// <param name=”node”>Current node.</param> 
        /// <returns>true if the node’s name matches the desired node name, false otherwise.</returns> 
        public bool FindKerningNode(Kerning node)
        {
            return (node.Second == nextChar);
        }

        public List<TextQuad> BuildQuads(FontString fontString)
        {
            List<TextQuad> quads = new List<TextQuad>();
            float lineWidth = 0f;
            float sizeScale = fontString.FontSize / (float) font.RenderedSize;
            char lastChar = new char();
            int lineNumber = 1;
            int wordNumber = 1;
            float wordWidth = 0f;
            bool firstCharOfLine = true;

            float x = 0f;
            float y = (float)fontString.TextBox.Y;

            for(int i =0; i<fontString.Text.Length;i++)
            {
                //Get and scale the font character
                FontChar fchar = font[fontString.Text[i]];
                Vector2 offset = new Vector2(fchar.Offset.X * sizeScale, fchar.Offset.Y * sizeScale);
                float xAdvance = fchar.XAdvance * sizeScale;
                float width  = fchar.Width * sizeScale;
                float height = fchar.Height * sizeScale;

                //Check vertical bounds
                if(y + offset.Y + height > fontString.TextBox.Bottom) break;
                #region newline check
                //Newline
                //if(fontString.Text[i] == 'n' || fontString.Text[i] == 'r' || (lineWidth + xAdvance >= fontString.TextBox.Width))
                if((lineWidth + xAdvance >= fontString.TextBox.Width))
                {
                    switch(fontString.Alignment)
                    {
                        case Alignment.Left:
                            //Start at the left
                            x = (float)fontString.TextBox.X;
                            break;
                        case Alignment.Center:
                            //Start in the center
                            x = (float)(fontString.TextBox.X + (fontString.TextBox.Width / 2f));
                            break;
                        case Alignment.Right:
                            //Start at the right
                            x = (float)fontString.TextBox.Right;
                            break;
                    }

                    y+= font.LineHeight * sizeScale;
                    float textOffset = 0f;

                    if((lineWidth + xAdvance >= fontString.TextBox.Width) && (wordNumber != 1))
                    {
                        //Next character extends past text box width
                        //We have to move the last word down one line
                        char newLineLastChar = new char();
                        lineWidth = 0f;
                        for(int j = 0; j<quads.Count; j++)
                        {
                            if((quads[j].LineNumber == lineNumber) && (quads[j].WordNumber == wordNumber))
                            {
                                //First move word down to next line
                                quads[j].LineNumber++;
                                quads[j].WordNumber = 1;
                                quads[j].Position = new Common.Vector2Int((int)(x + ( quads[j].FontChar.Offset.X * sizeScale)),(int)(y + (quads[j].FontChar.Offset.Y * sizeScale)));
                                float scaledAdvance = quads[j].FontChar.XAdvance * sizeScale;
                                x+= scaledAdvance;
                                lineWidth += scaledAdvance;
                                float kerning = 0f;
                                Kerning kern = null;
                                if(fontString.Kerning)
                                {
                                    nextChar = quads[j].Character;
                                    kern = font[newLineLastChar].KerningList.Find(FindKerningNode);
                                }
                                switch (fontString.Alignment)
                                {
                                    case Alignment.Left:
                                        //Move current word to the left side of the text box
                                        if(kern != null)
                                        {
                                            kerning = kern.Amount*sizeScale;
                                            x+= kerning;
                                            lineWidth += kerning;
                                        }
                                        break;
                                    case Alignment.Center:
                                        textOffset += scaledAdvance /2f;
                                        if(kern != null)
                                        {
                                            kerning = kern.Amount * sizeScale;
                                            x+= kerning;
                                            lineWidth +=kerning;
                                            textOffset += kerning / 2f;
                                        }
                                        break;
                                    case Alignment.Right:
                                        textOffset += scaledAdvance;
                                        if(kern != null)
                                        {
                                            kerning = kern.Amount * sizeScale;
                                            x+= kerning;
                                            lineWidth +=kerning;
                                            textOffset += kerning;
                                        }
                                        break;
                                }
                                newLineLastChar = quads[j].Character;
                            }
                            //Make post-newline justifications
                            if(fontString.Alignment == Alignment.Center || fontString.Alignment == Alignment.Right)
                            {
                                //Justify the new line
                                for(int k = 0; k< quads.Count; k++)
                                {
                                    if(quads[k].LineNumber == lineNumber +1)
                                    {
                                        quads[k].Position = new Common.Vector2Int(quads[k].Position.X - (int)textOffset, quads[k].Position.Y);
                                    }
                                }
                                x-= textOffset;
                                //Rejustify the line it was moved from
                                for(int k = 0; k<quads.Count; k++)
                                {
                                    if(quads[k].LineNumber == lineNumber)
                                    {
                                        quads[k].Position = new Common.Vector2Int(quads[k].Position.X + (int)textOffset,quads[k].Position.Y);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //New line without any carry-down word
                        firstCharOfLine = true;
                        lineWidth = 0f;
                    }

                    wordNumber = 1;
                    lineNumber++;
                }
                #endregion

                /*//Don't print these
                if (fontString.Text[i] == 'n' || fontString.Text[i] == 'r' || fontString.Text[i] == 't')
                {
                    continue;
                }*/

                //Set starting cursor for alignment
                if(firstCharOfLine)
                {
                    switch(fontString.Alignment)
                    {
                        case Alignment.Left:
                            //start at left
                            x = (float)fontString.TextBox.Left;
                            break;
                        case Alignment.Center:
                            //start in center
                            x = (float)(fontString.TextBox.Left + (fontString.TextBox.Width / 2f));
                            break;
                        case Alignment.Right:
                            //start at right
                            x = (float)fontString.TextBox.Right;
                            break;
                    }
                }

                //Adjust for kerning
                float kernAmount = 0;
                if(fontString.Kerning && !firstCharOfLine)
                {
                    nextChar = fontString.Text[i];
                    Kerning kern = font[lastChar].KerningList.Find(FindKerningNode);
                    if(kern!=null)
                    {
                        kernAmount = kern.Amount*sizeScale;
                        x+= kernAmount;
                        lineWidth+=kernAmount;
                        wordWidth+=kernAmount;
                    }
                }

                firstCharOfLine = false;

                if (fontString.Text[i] == ' ' && fontString.Alignment == Alignment.Right)
                {
                    wordNumber++;
                    wordWidth = 0f;
                }
                wordWidth += xAdvance;
                //create the quad
                TextQuad quad = new TextQuad((int)(x+offset.X), (int)(y+offset.Y),ScreenWidth, ScreenHeight, (int)width,(int)height,fontString.Colour,fontString.Text[i],fchar,font.Width,font.Height,sizeScale,lineNumber,wordNumber,wordWidth);
                quads.Add(quad);

                if (fontString.Text[i] == ' ' && fontString.Alignment == Alignment.Left)
                {
                    wordNumber++;
                    wordWidth = 0f;
                }
                float movement = xAdvance;
                x += movement;
                lineWidth += movement;
                lastChar = fontString.Text[i];

                //Rejustify text
                if (fontString.Alignment == Alignment.Center)
                {
                    //We have to recenter all Quads since we added a new character
                    float textOffset = xAdvance / 2f;
                    if (fontString.Kerning)
                    {
                        textOffset += kernAmount / 2f;
                    }
                    for (int j = 0; j < quads.Count; j++)
                    {
                        if (quads[j].LineNumber == lineNumber)
                        {
                            quads[j].Position = new Common.Vector2Int(quads[j].Position.X - (int)textOffset, quads[j].Position.Y);
                        }
                    }
                    x -= textOffset;
                }
                else if (fontString.Alignment == Alignment.Right)
                {
                    //We have to rejustify all quads since we added a new character
                    float textOffset = 0f;
                    if (fontString.Kerning)
                    {
                        textOffset += kernAmount;
                    }
                    for (int j = 0; j < quads.Count; j++)
                    {
                        if (quads[j].LineNumber == lineNumber)
                        {
                            textOffset = xAdvance;
                            quads[j].Position = new Common.Vector2Int(quads[j].Position.X - (int)xAdvance,quads[j].Position.Y);
                        }
                    }
                    x -= textOffset;
                }
            }
            return quads;
        }

    }
}
