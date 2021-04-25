namespace Disk.Ui.Services
{
    public interface IObjectSerializer
    {
        string Serialize<T>(T ob);
        T Deserialize<T>(string se);
    }
}