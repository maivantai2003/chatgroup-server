namespace chatgroup_server.Interfaces.IServices
{
    public interface IRecaptchaService
    {
        Task<bool> Verify(string token);
    }
}
