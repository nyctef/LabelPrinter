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

        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                var str = await Request.Content.ReadAsStringAsync();

                var value = System.Web.Helpers.Json.Decode(str);
                string title = value.title;
                string text = value.text;
                List<string> images = ((object[])value.images).Cast<string>().ToList() ?? new List<string>();
                if (text != null)
                {
                    PrintLabel.Print(title, text, images);
                }
                else
                {
                    PrintLabel.Print(title, images);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            return PlainText("Printing ...");
        }
    }
}
