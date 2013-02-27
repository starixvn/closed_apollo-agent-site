/// <summary>
/// Summary description for SelectedUserName
/// </summary>
///
namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Components.SelectedUserName
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Fanex.Age.UI;
    using TplCore.Element;

    public class UserName_Selection : IComponent
    {
        SelectedUserName_Template tpl = new SelectedUserName_Template();

        String[] alphabet = new String[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                   "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                                   "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                                   "U", "V", "W", "X", "Y", "Z" };

        public UserName_Selection(IElement parent, string ID, string word, ref bool NextIndex)
        {
            parent.RegisterComponent("UserName_Selection", this);

            Generate_Control(ID, word, ref NextIndex);
        }

        private void Generate_Control(string ID, string Word)
        {
            bool NextIndex = false;
            Generate_Control(ID, Word, ref NextIndex);
        }

        private void Generate_Control(string ID, string Word, ref bool NextIndex)
        {
            SelectedUserName_Option_Block option = new SelectedUserName_Option_Block();
            tpl.Option.Clear();
            tpl.selectedID = ID;

            if (!NextIndex)
            {
                int index = Array.IndexOf(alphabet, Word);
                if (IsNextIndex(Word))
                {
                    index++;
                    NextIndex = true;
                }
                if (index == alphabet.Length - 1)
                    index = 0;

                for (var i = 0; i < alphabet.Length; i++)
                {
                    option.option_selected = string.Empty;
                    option.option_value = alphabet[i];
                    option.value = alphabet[i];

                    if (index == i)
                        option.option_selected = "selected = true";

                    tpl.Option.Append(option);
                }
            }
            else
            {
                foreach (string w in alphabet)
                {
                    option.option_selected = string.Empty;
                    option.option_value = w;
                    option.value = w;

                    if (w.ToLower() == Word.ToLower())
                        option.option_selected = "selected = true";

                    tpl.Option.Append(option);
                }
            }
        }

        private bool IsNextIndex(string value)
        {
            /*int index = Array.IndexOf(alphabet, value);
            if (index < alphabet.Length - 1)
            {
                return true;
            }*/
            return false;
        }

        public static implicit operator string(UserName_Selection value)
        {
            return value.ToString();
        }

        public void HideLinkedResources()
        {
        }

        public override string ToString()
        {
            return tpl.ToString();
        }
    }
}