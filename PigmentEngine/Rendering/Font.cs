using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using System.IO;
using System.Xml.Serialization;

namespace Pigment.Engine.Rendering.UI.Font
{
    public class Font
    {
        private Dictionary<char, FontChar> characters;
        public int LineHeight { get; private set; }
        public int Base { get; private set; }
        public int RenderedSize { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public FontChar this[char character]
        {
            get
            {
                if (characters.ContainsKey(character)) return characters[character];
                else return null;
            }
        }

        public Font(string filepath)
        {
            characters = new Dictionary<char, FontChar>(256);
            parseFNTFile(filepath);
        }

        private void parseFNTFile(string path)
        {
            StreamReader stream = new StreamReader(path);
            string line;
            char[] separators = new char[] { ' ', '=' };
            while ((line = stream.ReadLine()) != null)
            {
                string[] tokens = line.Split(separators);
                if (tokens[0] == "info")
                {
                    //get rendering size
                    for (int i = 1; i < tokens.Length; i++)
                    {
                        if (tokens[i] == "size")
                        {
                            RenderedSize = int.Parse(tokens[i + 1]);
                        }
                    }
                }
                else if (tokens[0] == "common")
                {
                    //Fill out fields
                    for (int i = 1; i < tokens.Length; i++)
                    {
                        if (tokens[i] == "lineHeight")
                        {
                            LineHeight = int.Parse(tokens[i + 1]);
                        }
                        else if (tokens[i] == "base")
                        {
                            Base = int.Parse(tokens[i + 1]);
                        }
                        else if (tokens[i] == "scaleW")
                        {
                            Width = int.Parse(tokens[i + 1]);
                        }
                        else if (tokens[i] == "scaleH")
                        {
                            Height = int.Parse(tokens[i + 1]);
                        }
                    }
                }
                else if (tokens[0] == "char")
                {
                    //New FontChar
                    char index = Convert.ToChar(0);
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (tokens[i] == "id")
                        {
                            index = Convert.ToChar(int.Parse(tokens[i + 1]));
                            characters.Add(index,new FontChar());
                        }
                        else if (tokens[i] == "x")
                        {
                            characters[index].Position.X = int.Parse(tokens[i + 1]);
                        }
                        else if(tokens[i] == "y")
                        {
                            characters[index].Position.Y = int.Parse(tokens[i + 1]);
                        }
                        else if(tokens[i] == "width")
                        {
                            characters[index].Width = int.Parse(tokens[i + 1]);
                        }
                        else if (tokens[i] == "height")
                        {
                            characters[index].Height = int.Parse(tokens[i + 1]);
                        }
                        else if (tokens[i] == "xoffset")
                        {
                            characters[index].Offset.X = int.Parse(tokens[i + 1]);
                        }
                        else if (tokens[i] == "yoffset")
                        {
                            characters[index].Offset.Y = int.Parse(tokens[i + 1]);
                        }
                        else if (tokens[i] == "xadvance")
                        {
                            characters[index].XAdvance = int.Parse(tokens[i + 1]);
                        }
                    }
                }
                else if (tokens[0] == "Kerning")
                {
                    //Build kerning list
                    char index = Convert.ToChar(0);
                    Kerning k = new Kerning();
                    for (int i = 1; i < tokens.Length; i++)
                    {
                        if (tokens[i] == "first")
                        {
                            index = Convert.ToChar(int.Parse(tokens[i + 1]));
                        }
                        else if (tokens[i] == "second")
                        {
                            k.Second = Convert.ToChar(int.Parse(tokens[i + 1]));
                        }
                        else if (tokens[i] == "amount")
                        {
                            k.Amount = int.Parse(tokens[i + 1]);
                        }
                    }
                    characters[index].KerningList.Add(k);
                }
            }
            stream.Close();
        }
    }
}
