using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;

namespace Minitab_Assignment
{
    public class UspsAddressValidator : IAddressValidator
    {
        private const string UserId = "<user_id>";
        private const string Endpoint = @"https://secure.shippingapis.com/ShippingAPI.dll";
        private const string RequestTemplate =
@"API=Verify&XML=<AddressValidateRequest USERID=""{0}"">
    <Address>
        <Address1></Address1>
        <Address2>{1}</Address2>
        <City>{2}</City>
        <State>{3}</State>
        <Zip5>{4}</Zip5>
        <Zip4></Zip4>
    </Address>
</AddressValidateRequest>";

        private readonly IHttpClientFactory _httpClientFactory;

        public UspsAddressValidator(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public bool ValidateAddress(Address address)
        {
            var builder = new UriBuilder(Endpoint)
            {
                Port = -1,
                Query = string.Format(RequestTemplate, UserId, address.Line1, address.City, address.State, address.PostalCode)
            };

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, builder.ToString());
            var httpClient = _httpClientFactory.CreateClient();

            //var httpResponseMessage = httpClient.Send(httpRequestMessage);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return false;
            }

            //var responseContent = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();
            //var responseContent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<AddressValidateResponse><Address><Error><Number>-2147219401</Number><Source>clsAMS</Source><Description>Address Not Found.  </Description><HelpFile/><HelpContext/></Error></Address></AddressValidateResponse>";
            var responseContent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<AddressValidateResponse><Address><Address2>10 S LA SALLE ST</Address2><City>CHICAGO</City><State>IL</State><Zip5>60603</Zip5><Zip4>1002</Zip4><ReturnText>Default address: The address you entered was found but more information is needed (such as an apartment, suite, or box number) to match to a specific address.</ReturnText></Address></AddressValidateResponse>";

            var xmlResponse = XElement.Parse(responseContent);
            var errorPresent = xmlResponse.Descendants("Error").FirstOrDefault() != null;
            return !errorPresent;
        }
    }
}
