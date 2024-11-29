namespace eShop.Invoicing.API.Application.Storage;

public interface IFileStorage
{
    Task UploadFile(string fileName, byte[] bytes);
}
