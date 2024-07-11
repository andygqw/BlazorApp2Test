namespace BlazorApp2Test.Data
{
    internal class TextData
    {
        private string? text;

        public TextData()
        {
            text = null;
        }

        internal void AssignText(string? txt)
        {
            if (txt != null && txt.Trim().Length > 0)
            {
                if (txt.Length > Helper.TextAreaMaxLength)
                {
                    throw new IOException("Maximum text transfer size is 16000 character");
                }
                else
                {
                    text = txt;
                }
            }
            else
            {
                throw new IOException("Text field is empty when submit");
            }
        }

        internal string? GetText()
        {
            if (text != null && text.Trim().Length > 0)
            {
                return text;
            }
            else
            {
                return null;
            }
        }

        internal void CleanText()
        {
            text = null;
        }
    }
}