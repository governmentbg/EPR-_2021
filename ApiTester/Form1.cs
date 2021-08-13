using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApiTester
{
    public partial class Form1 : Form
    {
        private readonly IConfiguration config;
        private readonly EproCryptoHelper cryptoHelper;
        public Form1(IConfiguration _config, EproCryptoHelper _cryptoHelper)
        {
            config = _config;
            cryptoHelper = _cryptoHelper;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbBaseUrl.Text = config.GetValue<string>("Api:Endpoint");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                var result = sendData(tbRequest.Text, config.GetValue<string>("Api:Endpoint"),
                    tbMethod.Text,
                    config.GetValue<string>("Api:AppKey"),
                    config.GetValue<string>("Api:AppSecret")).Result;

                tbResponse.Text = result;
            }
            catch (Exception ex)
            {
                tbResponse.Text = ex.Message;
            }
        }

        private async Task<string> sendData(string requestBody, string endpoint, string methodName, string key, string secret)
        {
            var requestBytes = System.Text.Encoding.UTF8.GetBytes(requestBody);
            var hass = cryptoHelper.ComputeHash(requestBytes, secret);
            var autorizationToken = $"{key}.{hass}";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", autorizationToken);
            Uri address = new Uri(new Uri(endpoint), methodName);

            HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var responseMessage = client.PostAsync(address, content).ConfigureAwait(true).GetAwaiter().GetResult();
            var response = await responseMessage.Content.ReadAsStringAsync();
            return response;
        }
    }
}
