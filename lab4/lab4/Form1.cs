using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace lab4
{
    public partial class Form1 : Form
    {
        private bool _isAsync = false;
        const string Url = "https://www.cbr-xml-daily.ru/daily_json.js";

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "input.txt";
            textBox2.Text = "output.txt";
        }
     
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isAsync)
                {
                    await ProccessDataAsync();
                    return;
                }
                ProccessData();
            }
            catch (Exception ex)
            {
                richTextBox1.Text = ex.Message;
            }
           
        }

        private string[] GetContent(string fileName)
        {
            var reader = File.OpenText(fileName);
            return reader.ReadLine().Split(' ');
        }

        private async Task<string> GetContentAsync(string fileName)
        {
            var reader = File.OpenText(fileName);
            return await reader.ReadLineAsync();
        }

        public string GetResponse()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return GetProcessedResponse(response);
        }

        public Task<string> GetResponseAsync()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.Accept = "application/json";
            var response = request.GetResponseAsync();
            return response.ContinueWith(task => GetProcessedResponse(task.Result));
        }


        public string GetProcessedResponse(WebResponse response)
        {
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private async Task ProccessDataAsync()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var fileName = textBox1.Text;
            string content = await GetContentAsync(fileName);
            HashSet<string> data = new HashSet<string>(content.Split(' '));

            List<Info> resultList = new List<Info>();
            JObject response = JObject.Parse(await GetResponseAsync());

            foreach (var valute in response["Valute"])
            {
                var val = valute.First();
                var money = new Info();
                money.Name = val["Name"].ToString();
                money.Nominal = int.Parse(val["Nominal"].ToString());
                money.Value = double.Parse(val["Value"].ToString());
                money.CharCode = val["CharCode"].ToString();

                if (data.Contains(money.CharCode))
                {
                    resultList.Add(money);
                }
            }

            var resultString = "";

            foreach (var item in resultList)
            {
                resultString += $"{item.Nominal} {item.Name} по курсу {item.Value}\n";
            }

            var stream = new FileStream(textBox2.Text, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            var writer = new StreamWriter(stream);
            await writer.WriteLineAsync(resultString);
            writer.Close();
      
            resultString += $"Time:{sw.ElapsedMilliseconds.ToString()}\n"; ;
            richTextBox1.Text = resultString;
            sw.Stop();
        }

        private void ProccessData()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var fileName = textBox1.Text;
            HashSet<string> data = new HashSet<string>(GetContent(fileName));

            List<Info> resultList = new List<Info>();
            JObject response = JObject.Parse(GetResponse());

            foreach (var valute in response["Valute"])
            {
                var val = valute.First();
                var money = new Info();
                money.Name = val["Name"].ToString();
                money.Nominal = int.Parse(val["Nominal"].ToString());
                money.Value = double.Parse(val["Value"].ToString());
                money.CharCode = val["CharCode"].ToString();

                if (data.Contains(money.CharCode))
                {
                    resultList.Add(money);
                }
            }

            var resultString = "";
            foreach (var item in resultList)
            {
                resultString += $"{item.Nominal} {item.Name} по курсу {item.Value}\n";
            }

            File.WriteAllText(textBox2.Text, resultString);
            resultString += $"Time:{sw.ElapsedMilliseconds.ToString()}\n";;
            richTextBox1.Text = resultString;
            sw.Stop();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _isAsync = !_isAsync;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
