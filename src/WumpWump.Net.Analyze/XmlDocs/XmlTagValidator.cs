using System;
using System.Text;

namespace WumpWump.Net.Analyze
{
    public class RoslynXmlTools
    {
        public static string RemoveDocComments(string text)
        {
            // Remove prepended ///
            StringBuilder result = new();
            foreach (string line in text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string value = line.Trim();
                if (value.StartsWith("///"))
                {
                    value = value.Substring(3).Trim();
                }

                // Test if the line is just an XML tag
                if (RoslynXmlTools.CheckTagType(value) == XmlTagType.NotATag)
                {
                    result.AppendLine(value);
                }
            }

            return result.ToString().Trim();
        }

        public static XmlTagType CheckTagType(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return XmlTagType.NotATag;
            }

            input = input.Trim();

            // Basic checks for tag structure
            if (input.Length < 3 || !input.StartsWith("<") || !input.EndsWith(">"))
            {
                return XmlTagType.NotATag;
            }

            // Check for end tag
            if (input.StartsWith("</"))
            {
                // Verify it has a proper name after </
                return input.Length > 3 && IsValidTagName(input.Substring(2, input.Length - 3)) ? XmlTagType.EndTag : XmlTagType.NotATag;
            }

            // Check for self-closing tag
            if (input.EndsWith("/>"))
            {
                // Verify it has a proper name before />
                int slashPos = input.LastIndexOf('/');
                return slashPos > 1 && IsValidTagName(input.Substring(1, slashPos - 1)) ? XmlTagType.SelfClosingTag : XmlTagType.NotATag;
            }

            // Check for start tag
            return input.Length > 2 && IsValidTagName(input.Substring(1, input.Length - 2)) ? XmlTagType.StartTag : XmlTagType.NotATag;
        }

        private static bool IsValidTagName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            name = name.Trim();

            // Check first character is letter or underscore
            if (!(char.IsLetter(name[0]) || name[0] == '_'))
            {
                return false;
            }

            // Check remaining characters
            for (int i = 1; i < name.Length; i++)
            {
                if (!(char.IsLetterOrDigit(name[i]) || name[i] == '_' || name[i] == '-' || name[i] == '.' || name[i] == ':'))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public enum XmlTagType
    {
        NotATag,
        StartTag,
        EndTag,
        SelfClosingTag
    }
}
