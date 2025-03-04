using BusinessLayer.Interface;
using ModelLayer.Model;

public class GreetingBL : IGreetingBL
{
    public string GetGreeting()
    {
        return "Hello, World!";
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
