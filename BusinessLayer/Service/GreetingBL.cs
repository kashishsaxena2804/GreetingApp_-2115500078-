using BusinessLayer.Interface;
using ModelLayer.Model;

public class GreetingBL : IGreetingBL
{
    public ResponseModel<string> GetGreeting(string firstName = null, string lastName = null)
    {
        string greetingMessage = "Hello";

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            greetingMessage += $" {firstName} {lastName}";
        }
        else if (!string.IsNullOrEmpty(firstName))
        {
            greetingMessage += $" {firstName}";
        }
        else if (!string.IsNullOrEmpty(lastName))
        {
            greetingMessage += $" {lastName}";
        }
        else
        {
            greetingMessage += ", World!";
        }

        return new ResponseModel<string>
        {
            Success = true,
            Message = "Greeting generated successfully",
            Data = greetingMessage
        };
    }

    public ResponseModel<string> AddGreeting(string key, string value)
    {
        return new ResponseModel<string> { Success = true, Message = "Greeting added", Data = $"Key: {key}, Value: {value}" };
    }

    public ResponseModel<string> UpdateGreeting(string key, string value)
    {
        return new ResponseModel<string> { Success = true, Message = "Greeting updated", Data = $"Key: {key}, New Value: {value}" };
    }

    public ResponseModel<string> ModifyGreeting(string key, string value)
    {
        return new ResponseModel<string> { Success = true, Message = "Greeting modified", Data = $"Key: {key}, Updated Value: {value}" };
    }

    public ResponseModel<string> DeleteGreeting(string key)
    {
        return new ResponseModel<string> { Success = true, Message = "Greeting deleted", Data = $"Key: {key} removed" };
    }
}
