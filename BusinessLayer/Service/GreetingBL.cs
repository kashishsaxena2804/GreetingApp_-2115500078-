using BusinessLayer.Interface;
using ModelLayer.Model;
using System.Collections.Generic;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private static readonly Dictionary<string, string> _greetings = new Dictionary<string, string>();

        public string GetGreeting() => "Hello, World!";

        public string AddGreeting(string key, string value)
        {
            _greetings[key] = value;
            return $"Key: {key}, Value: {value}";
        }

        public string UpdateGreeting(string key, string value) =>
            _greetings.ContainsKey(key) ? (_greetings[key] = value, $"Key: {key}, New Value: {value}").Item2 : null;

        public string ModifyGreeting(string key, string value) =>
            _greetings.ContainsKey(key) ? (_greetings[key] += $" {value}", $"Key: {key}, Updated Value: {_greetings[key]}").Item2 : null;

        public string DeleteGreeting(string key) =>
            _greetings.Remove(key) ? $"Key: {key} removed" : null;

        ResponseModel<string> IGreetingBL.GetGreeting()
        {
            throw new NotImplementedException();
        }

        public ResponseModel<string> UpdateGreeting(string message)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<string> PatchGreeting(string message)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<string> ResetGreeting()
        {
            throw new NotImplementedException();
        }
    }
}
