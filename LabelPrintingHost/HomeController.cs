using LabelPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LabelPrintingHost
{
    public class HomeController : ApiController
    {
        public class ThingToPrint
        {
            public string title;
            public string text;
            public List<string> images;
        }

        public HttpResponseMessage Get()
        {
            return PlainText("POST here with { title: 'foo', text: 'bar', images: ['http://example.com/baz.png'] }\n\neg:\ncurl --data \"{ title: 'test', text: 'this is some test text', images:['https://assets-cdn.github.com/images/modules/logos_page/GitHub-Mark.png']}\" -H \"content-type:application/json\" http://localhost:9000/");
        }

        private static HttpResponseMessage PlainText(string text)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(text, new UTF8Encoding(false), "text/plain")
            };
        }

        public string Post([FromBody] ThingToPrint value)
        {
            PrintLabel.Print(value.title, value.text, value.images);
            return "Printing ...";
        }
    }
}
