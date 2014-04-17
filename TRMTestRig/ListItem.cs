using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMTestRig
{
    public class ListItem
    {
        protected String Text;
        protected int Value;

        public ListItem(String text, int value)
        {
            this.Text= text;
            this.Value = value;
        }

        public override string ToString()
        {
            return Text;
        }

        public int SelectedValue()
        {
            return Value;
        }
    }
}
