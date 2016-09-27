using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace demoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("User Name:");
            var userName = Console.ReadLine();
            Console.Write("Password:");
            var password = Console.ReadLine();


            Console.Write("Action : ");
            var action = Console.ReadLine();

            using (var api = new System.Net.Http.HttpClient())
            {
                api.BaseAddress = new Uri("https://0ytestr3xdotrezapi.navitaire.com/api/");

                switch (action)
                {
                    case "create":
                        CreateUser(api, userName, password);
                        break;
                    case "login":
                        Login(api, userName, password);
                    break;
                    default:
                        break;
                }

                var sessionDetails = api.GetAsync("nsk/session/details").Result;

                dynamic result = JsonConvert.DeserializeObject<dynamic>(sessionDetails.Content.ReadAsStringAsync().Result);
            }
        }

        private static void Login(HttpClient api, string userName, string password)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new {
                userName,
                password
            }));
            var result = api.PostAsync("nsk/user/login", content).Result;
        }

        private static void CreateUser(HttpClient api, string userName, string password)
        {
            var user = new User
            {
                username = userName,
                password = password,

                person = new Person
                {
                    name = new Name
                    {
                        first = "Grzegorz",
                        last = "Test"
                    },
                    status = 1,
                    addresses = new List<Address>
                            {
                                new Address
                                {
                                    lineOne = "Some Street",
                                    city = "Barcelona",
                                    countryCode = "ES",
                                    @default = true,
                                    typeCode = "H"
                                }
                            },
                    emailAddresses = new List<EmailAddress>
                            {
                                new EmailAddress
                                {
                                    email = "goronowicz@mailinator.com",
                                    @default = true,
                                    type = "P"


                                }
                            }
                }
            };

            var ctText = JsonConvert.SerializeObject(user);
            var content = new StringContent(ctText);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            var result = api.PostAsync("nsk/user", content).Result;
        }
    }
    public class Name
    {
        public string first { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        public string title { get; set; }
        public string suffix { get; set; }
    }

    public class Alias
    {
        public int aliasType { get; set; }
        public string first { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        public string title { get; set; }
        public string suffix { get; set; }
    }

    public class Details
    {
        public List<Alias> aliases { get; set; }
        public string currencyCode { get; set; }
        public int gender { get; set; }
        public string nationality { get; set; }
        public string residentCountry { get; set; }
        public string dateOfBirth { get; set; }
        public string nationalIdNumber { get; set; }
        public string passengerType { get; set; }
        public string cultureCode { get; set; }
    }

    public class EmailAddress
    {
        public string key { get; set; }
        public string type { get; set; }
        public string email { get; set; }
        public bool @default { get; set; }
    }

    public class PhoneNumber
    {
        public string key { get; set; }
        public bool @default { get; set; }
        public int type { get; set; }
        public string number { get; set; }
    }

    public class Address
    {
        public string key { get; set; }
        public string typeCode { get; set; }
        public string lineOne { get; set; }
        public bool @default { get; set; }
        public string lineTwo { get; set; }
        public string lineThree { get; set; }
        public string countryCode { get; set; }
        public string provinceState { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
    }

    public class Person
    {
        public Name name { get; set; }
        public string customerNumber { get; set; }
        public Details details { get; set; }
        public List<EmailAddress> emailAddresses { get; set; }
        public List<PhoneNumber> phoneNumbers { get; set; }
        public List<Address> addresses { get; set; }
        public int status { get; set; }
    }

    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public Person person { get; set; }
    }

}
