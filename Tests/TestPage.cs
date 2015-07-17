using LabelPrinting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class TestPage
    {
        [Test, Explicit]
        public void PrintTestPage()
        {
            var text = @"It was the best of times, it was the worst of times, it was the age of wisdom, it was the age of foolishness, it was the epoch of belief, it was the epoch of incredulity, it was the season of Light, it was the season of Darkness, it was the spring of hope, it was the winter of despair, we had everything before us, we had nothing before us, we were all going direct to Heaven, we were all going direct the other way – in short, the period was so far like the present period, that some of its noisiest authorities insisted on its being received, for good or for evil, in the superlative degree of comparison only.";
            PrintLabel.Print("Engine#273: Run existing SQL Server tests against SQL Server 2016 test server",
                text,
                new List<string>
                { "https://assets-cdn.github.com/images/modules/logos_page/GitHub-Mark.png",
                    "https://s.gravatar.com/avatar/cc940654ab4278a55f6deb9bad4b687a?s=80"
                });
        }

        [Test, Explicit]
        public void PrintTestPageWithTitleOnly()
        {
            PrintLabel.Print("Engine#273: Run existing SQL Server tests against SQL Server 2016 test server",
                new List<string>
                { "https://assets-cdn.github.com/images/modules/logos_page/GitHub-Mark.png",
                    "https://s.gravatar.com/avatar/cc940654ab4278a55f6deb9bad4b687a?s=80"
                });
        }
    }
}
