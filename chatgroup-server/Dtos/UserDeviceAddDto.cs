namespace chatgroup_server.Dtos
{
    public class UserDeviceAddDto
    {
        public int UserId { get; set; }
        public string? DeviceToken { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? OS { get; set; }
        public string? DeviceName { get; set; }
        public string? Address { get; set; }
    }
}
